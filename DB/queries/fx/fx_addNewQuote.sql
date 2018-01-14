USE [fx];

BEGIN TRANSACTION

GO
IF OBJECT_ID('addNewQuote','P') IS NOT NULL DROP PROC [dbo].[addNewQuote];
IF OBJECT_ID('test_addQuoteFromRawH1','P') IS NOT NULL DROP PROC [dbo].[test_addQuoteFromRawH1];
IF OBJECT_ID('insertMissingQuotations','P') IS NOT NULL DROP PROC [dbo].[insertMissingQuotations];
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTimeframesIndices]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTimeframesIndices]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLowerLevelIndices]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetLowerLevelIndices]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDateForDateIndex]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetDateForDateIndex]
--IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'QuotesTransferTable') DROP TYPE [dbo].[QuotesTransferTable];
--IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'DatetimesTransferTable') DROP TYPE [dbo].[DatetimesTransferTable];
--IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'TimeframeDateIndexTransferTable') DROP TYPE [dbo].[TimeframeDateIndexTransferTable];
--IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'ParentChildTimeframesTransferTable') DROP TYPE [dbo].[ParentChildTimeframesTransferTable];
GO

--CREATE TYPE [dbo].[QuotesTransferTable] AS TABLE([Date] DATETIME, [Open] FLOAT, [High] FLOAT, [Low] FLOAT, [Close] FLOAT, [Volume] INT);
--CREATE TYPE [dbo].[DatetimesTransferTable] AS TABLE([Date] DATETIME);
--CREATE TYPE [dbo].[TimeframeDateIndexTransferTable] AS TABLE([TimeframeId] INT, [DateIndex] INT);
--CREATE TYPE [dbo].[ParentChildTimeframesTransferTable] AS TABLE([ParentTimeframe] INT, [DateIndex] INT, [ChildDateIndex] INT);


GO

CREATE FUNCTION [dbo].[GetDateForDateIndex](@timeframeId AS INT, @dateIndex AS INT)
RETURNS DATETIME
AS
BEGIN
	DECLARE @dt AS DATETIME;
	SELECT @dt = [Date] FROM [dbo].[dates] WHERE [TimeframeId] = @timeframeId AND [DateIndex] = @dateIndex;
	RETURN @dt;
END
GO

CREATE FUNCTION [dbo].[GetTimeframesIndices](@dates AS [dbo].[DatetimesTransferTable] READONLY)
RETURNS TABLE
AS
RETURN 
(SELECT DISTINCT
	b.[TimeframeId], b.[DateIndex]
FROM
	@dates a
	LEFT JOIN
		(SELECT
			d1.[TimeframeId], d1.[DateIndex], d1.[Date] AS [StartDate], d2.[Date] AS [EndDate]
		FROM
			[dbo].[dates] d1
			LEFT JOIN [dbo].[dates] d2
			ON d1.[TimeframeId] = d2.[TimeframeId] AND d1.[DateIndex] = d2.[DateIndex] - 1) b
	ON a.[Date] >= [StartDate] AND a.[Date] < [EndDate]);

GO

CREATE FUNCTION [dbo].[GetLowerLevelIndices](@baseTimeframe AS INT, @timeframeIdIndices AS [dbo].[TimeframeDateIndexTransferTable] READONLY)
RETURNS TABLE
AS
RETURN 
(SELECT
	c.[TimeframeId],
	c.[DateIndex],
	d3.[DateIndex] AS [BaseTimeframeIndex]
FROM
	(SELECT DISTINCT
		b.*
	FROM
		@timeframeIdIndices a
		LEFT JOIN
			(SELECT
				d1.[TimeframeId], d1.[DateIndex], d1.[Date] AS [StartDate], d2.[Date] AS [EndDate]
			FROM
				[dbo].[dates] d1
				LEFT JOIN [dbo].[dates] d2
				ON d1.[TimeframeId] = d2.[TimeframeId] AND d1.[DateIndex] = d2.[DateIndex] - 1) b
		ON a.[TimeframeId] = b.[TimeframeId] AND a.[DateIndex] = b.[DateIndex]) c
	LEFT JOIN (SELECT * FROM [dbo].[dates] WHERE [TimeframeId] = @baseTimeframe) d3
	ON d3.[Date] >= c.[StartDate] AND d3.[Date] < c.[EndDate]);

GO



CREATE PROC [dbo].[insertMissingQuotations] @assetId AS INT, @timeframeId AS INT
AS
BEGIN

	DECLARE @minIndex AS INT = (SELECT MIN([DateIndex]) FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId);
	DECLARE @maxIndex AS INT = (SELECT MAX([DateIndex]) FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId);
	DECLARE @firstMissing AS INT;

	SELECT * INTO #TempQuotes FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;

	SELECT 
		d.[DateIndex], d.[TimeframeId]
	INTO
		#MissingQuotations
	FROM 
		#TempQuotes q
		RIGHT JOIN (SELECT * FROM [dbo].[dates] WHERE [TimeframeId] = 4 AND [DateIndex] BETWEEN @minIndex AND @maxIndex) d
		ON q.[DateIndex] = d.[DateIndex]
	WHERE
		q.[DateIndex] IS NULL

	SET @firstMissing = (SELECT MIN([DateIndex]) FROM #MissingQuotations);

	SELECT
		m.[DateIndex],
		MAX(q.[DateIndex]) AS [PreviousExisting]
	INTO
		#MissingExistingPairs
	FROM
		#MissingQuotations m
		LEFT JOIN (SELECT * FROM #TempQuotes WHERE [DateIndex] >= (@firstMissing - 1)) q
		ON m.[DateIndex] > q.[DateIndex]
	GROUP BY
		m.[DateIndex]

	INSERT INTO [dbo].[quotes]([AssetId], [TimeframeId], [DateIndex], [Open], [High], [Low], [Close], [Volume])
	SELECT
		@assetId AS [AssetId],
		@timeframeId AS [TimeframeId],
		mep.[DateIndex] AS [DateIndex],
		q.[Close] AS [Open],
		q.[Close] AS [High],
		q.[Close] AS [Low],
		q.[Close] AS [Close],
		0 AS [Volume]
	FROM
		#MissingExistingPairs mep
		LEFT JOIN #TempQuotes q
		ON mep.[PreviousExisting] = q.[DateIndex];


	--Clean up
	BEGIN

		DROP TABLE #TempQuotes;
		DROP TABLE #MissingQuotations;
		DROP TABLE #MissingExistingPairs;

	END

END

GO

CREATE PROC [dbo].[addNewQuote] @assetId AS INT, @timeframeId AS INT, @quotes AS [dbo].[QuotesTransferTable] READONLY
AS
BEGIN
	
	--Insert data into [dbo].[quotes]
	BEGIN
		
		--Create temporary table with DateIndex instead of date.
		SELECT
			@assetId AS [AssetId],
			@timeframeId AS [TimeframeId],
			d.[DateIndex],
			q.[Date],
			q.[Open],
			q.[High],
			q.[Low],
			q.[Close],
			q.[Volume],
			1 AS [IsComplete]
		INTO
			#Quotes
		FROM
			@quotes q
			LEFT JOIN (SELECT * FROM [dbo].[dates]) d
			ON @timeframeId = d.[TimeframeId] AND q.[Date] = d.[Date];

		--Insert data from the table above into [dbo].[quotes]
		INSERT INTO [dbo].[quotes]([AssetId], [TimeframeId], [DateIndex], [Open], [High], [Low], [Close], [Volume], [IsComplete])
		SELECT [AssetId], [TimeframeId], [DateIndex], [Open], [High], [Low], [Close], [Volume], [IsComplete] FROM #Quotes WHERE [DateIndex] IS NOT NULL;
			
		--Insert data with missing DateIndex into [dbo].[quotesOutOfDate]
		INSERT INTO [dbo].[quotesOutOfDate]([AssetId], [TimeframeId], [Date], [Open], [High], [Low], [Close], [Volume])
		SELECT [AssetId], [TimeframeId], [Date], [Open], [High], [Low], [Close], [Volume] FROM #Quotes WHERE [DateIndex] IS NULL;

	END

	--Insert missing quotations.
	EXEC [dbo].[insertMissingQuotations] @assetId = @assetId, @timeframeId = @timeframeId;

	--Insert quotations for higher timeframe levels.
	BEGIN
		
		DECLARE @dates AS [dbo].[DatetimesTransferTable];
		DECLARE @timeframeIdIndices AS [dbo].[TimeframeDateIndexTransferTable];
		DECLARE @timeframeIdsMapping AS [dbo].[ParentChildTimeframesTransferTable];

		INSERT INTO @dates SELECT [Date] FROM @quotes;
		INSERT INTO @timeframeIdIndices SELECT * FROM [dbo].[GetTimeframesIndices](@dates) WHERE [TimeframeId] > @timeframeId;	
		INSERT INTO @timeframeIdsMapping SELECT * FROM [dbo].[GetLowerLevelIndices](@timeframeId, @timeframeIdIndices);

		--Join timeframe mapping fetched in previous step with quotes for base timeframe.
		SELECT
			tm.*, q.[Open], q.[High], q.[Low], q.[Close], q.[Volume]
		INTO 
			#JoinTimeframeMappingWithBaseTimeframeQuotes
		FROM
			@timeframeIdsMapping tm
			LEFT JOIN (SELECT * FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) q
			ON tm.[ChildDateIndex] = q.[DateIndex]
		WHERE
			q.[Close] IS NOT NULL;

		--Calculate candles for higher level timeframes.
		BEGIN
			SELECT 
				a.[ParentTimeframe], a.[DateIndex], b.[Open], a.[High], a.[Low], c.[Close], a.[Volume]
			INTO
				#HigherLevelQuotes
			FROM
				(	SELECT q.[ParentTimeframe], q.[DateIndex], MIN(q.[ChildDateIndex]) AS [MinIndex], MAX(q.[ChildDateIndex]) AS [MaxIndex], MIN(q.[Low]) AS [Low], MAX(q.[High]) AS [High], SUM(q.[Volume]) AS [Volume]
					FROM #JoinTimeframeMappingWithBaseTimeframeQuotes q
					GROUP BY q.[ParentTimeframe], q.[DateIndex]) a
				LEFT JOIN #JoinTimeframeMappingWithBaseTimeframeQuotes b ON a.[ParentTimeframe] = b.[ParentTimeframe] AND a.[DateIndex] = b.[DateIndex] AND a.[MinIndex] = b.[ChildDateIndex]
				LEFT JOIN #JoinTimeframeMappingWithBaseTimeframeQuotes c ON a.[ParentTimeframe] = c.[ParentTimeframe] AND a.[DateIndex] = c.[DateIndex] AND a.[MaxIndex] = c.[ChildDateIndex];

			DROP TABLE #JoinTimeframeMappingWithBaseTimeframeQuotes;

		END

		--Insert high-level candles fetched in the previous step to the [quotes] table (if they already exists, override them).
		BEGIN

			DELETE q 
			FROM [dbo].[quotes] q 
			WHERE EXISTS
			(
			   SELECT 1 FROM #HigherLevelQuotes hlq 
			   WHERE q.[AssetId] = @assetId AND q.[TimeframeId] = hlq.[ParentTimeframe] AND q.[DateIndex] = hlq.[DateIndex]
			);

			INSERT INTO [dbo].[quotes]([AssetId], [TimeframeId], [DateIndex], [Open], [High], [Low], [Close], [Volume])
			SELECT @assetId AS [AssetId], hlv.* FROM  #HigherLevelQuotes hlv;

		END

		--Updating [IsComplete] field.
		BEGIN

			-- Temporary table for better performance.
			SELECT * INTO #TempQuotes FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;

			-- Select the last base timeframe quotation.
			DECLARE @lastIndex AS INT = (SELECT MAX([DateIndex]) FROM #TempQuotes);
			DECLARE @dt AS DATETIME = [dbo].[GetDateForDateIndex](@timeframeId, @lastIndex);
			
			--Fetch the higher level date index for the highest quote from the base timeframe.
			DELETE FROM @dates;
			DELETE FROM @timeframeIdIndices;

			INSERT INTO @dates SELECT @dt;
			INSERT INTO @timeframeIdIndices SELECT * FROM [dbo].[GetTimeframesIndices](@dates) WHERE [TimeframeId] > @timeframeId;

			--Create temporary table with all higher level quotes match with the proper record from @timeframeIdIndices fetched above.
			SELECT
				q.*,
				IIF(ti.[TimeframeId] IS NULL, 0, 1) AS [IsCovered]
			INTO
				#HigherLevelTimeframesWithCoverageJoined
			FROM
				(SELECT * FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] > @timeframeId) q
				LEFT JOIN @timeframeIdIndices ti
				ON q.[AssetId] = @assetId AND q.[TimeframeId] = ti.[TimeframeId] AND q.[DateIndex] < ti.[DateIndex];

			--Update [IsComplete] field in [quotes] table.
			UPDATE q
			SET [IsComplete] = hlt.[IsCovered]
			FROM
				[dbo].[quotes] q
				LEFT JOIN #HigherLevelTimeframesWithCoverageJoined hlt
				ON q.[AssetId] = hlt.[AssetId] AND q.[TimeframeId] = hlt.[TimeframeId] AND q.[DateIndex] = hlt.[DateIndex]
			WHERE
				q.[AssetId] = @assetId AND q.[TimeframeId] > @timeframeId;

			DROP TABLE #HigherLevelTimeframesWithCoverageJoined;

		END

	END

	--Clean up temporary tables
	BEGIN
		DROP TABLE #Quotes;
		DROP TABLE #HigherLevelQuotes;
	END

END

	

GO

CREATE PROC [dbo].[test_addQuoteFromRawH1] @counter AS INTEGER
AS
BEGIN

	DECLARE @items AS [dbo].[QuotesTransferTable];
	DECLARE @assetId AS INT = 1;
	DECLARE @timeframeId AS INT = 4;
	DECLARE @lastDate AS DATETIME;
	
	SET @lastDate = (SELECT
					MAX(d.[Date]) AS [LastQuoteDate]
				FROM 
					(SELECT * FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) q
					LEFT JOIN (SELECT * FROM [dbo].[dates] WHERE [TimeframeId] = @timeframeId) d
					ON q.[DateIndex] = d.[DateIndex]);
	
	INSERT INTO @items
	SELECT TOP (@counter)
		d.*,
		0 AS [Volume]
	FROM 
		[dbo].[RawH1Data] d
	WHERE
		d.[Date] > COALESCE(@lastDate, CAST('1970-01-01' AS DATETIME))
	ORDER BY
		d.[Date] ASC;

	EXEC [dbo].[addNewQuote] @assetId = @assetId, @timeframeId = @timeframeId, @quotes = @items;

END

GO




EXEC [dbo].[test_addQuoteFromRawH1] @counter = 6


--ROLLBACK TRANSACTION
COMMIT TRANSACTION;