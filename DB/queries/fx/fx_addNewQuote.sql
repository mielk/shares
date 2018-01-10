USE [fx];

BEGIN TRANSACTION

GO
--IF TYPE_ID(N'ParentChildTimeframesTransferTable') IS NOT NULL DROP TYPE [dbo].[ParentChildTimeframesTransferTable];
IF OBJECT_ID('addNewQuote','P') IS NOT NULL DROP PROC [dbo].[addNewQuote];
IF OBJECT_ID('test_addQuoteFromRawH1','P') IS NOT NULL DROP PROC [dbo].[test_addQuoteFromRawH1];
IF OBJECT_ID(N'GetTimeframesIndices', N'FN') IS NOT NULL DROP FUNCTION [dbo].[GetTimeframesIndices]
IF OBJECT_ID(N'GetLowerLevelIndices', N'FN') IS NOT NULL DROP FUNCTION [dbo].[GetLowerLevelIndices]
GO

--CREATE TYPE [dbo].[QuotesTransferTable] AS TABLE([Date] DATETIME, [Open] FLOAT, [High] FLOAT, [Low] FLOAT, [Close] FLOAT, [Volume] INT);
--CREATE TYPE [dbo].[DatetimesTransferTable] AS TABLE([dt] DATETIME);
--CREATE TYPE [dbo].[TimeframeDateIndexTransferTable] AS TABLE([Timeframe] INT, [DateIndex] INT);
--CREATE TYPE [dbo].[ParentChildTimeframesTransferTable] AS TABLE([ParentTimeframe] INT, [DateIndex] INT, [ChildDateIndex] INT);

--GO

CREATE PROC [dbo].[addNewQuote] @assetId AS INT, @timeframe AS INT, @quotes AS [dbo].[QuotesTransferTable] READONLY
AS
BEGIN
	
	--Insert data into [dbo].[quotes]
	BEGIN

		--Create temporary table with DateIndex instead of date.
		SELECT
			@assetId AS [AssetId],
			@timeframe AS [Timeframe],
			d.[DateIndex],
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
			ON @timeframe = d.[Timeframe] AND q.[Date] = d.[Date];

		--Insert data from the table above into [dbo].[quotes]
		INSERT INTO [dbo].[quotes]([AssetId], [Timeframe], [DateIndex], [Open], [High], [Low], [Close], [Volume], [IsComplete])
		SELECT * FROM #Quotes;

	END

	--Get date indices.
	BEGIN
		
		DECLARE @dates AS [dbo].[DatetimesTransferTable];
		DECLARE @timeframeIndices AS [dbo].[TimeframeDateIndexTransferTable];
		DECLARE @timeframesMapping AS [dbo].[ParentChildTimeframesTransferTable];

		INSERT INTO @dates SELECT [Date] FROM @quotes;
		INSERT INTO @timeframeIndices SELECT * FROM [dbo].[GetTimeframesIndices](@dates) WHERE [Timeframe] > @timeframe;	
		INSERT INTO @timeframesMapping SELECT * FROM [dbo].[GetLowerLevelIndices](@timeframe, @timeframeIndices);

		--Get cummulative quotations of higher level.
		SELECT
			tm.*, q.[Open], q.[High], q.[Low], q.[Close], q.[Volume]
		INTO 
			#QuotesForHigherLevel
		FROM
			@timeframesMapping tm
			LEFT JOIN (SELECT * FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [Timeframe] = @timeframe) q
			ON tm.[ChildDateIndex] = q.[DateIndex]
		WHERE
			q.[Close] IS NOT NULL;

		SELECT 
			a.[ParentTimeframe], a.[DateIndex], b.[Open], a.[High], a.[Low], c.[Close], a.[Volume]
		FROM
			(	SELECT q.[ParentTimeframe], q.[DateIndex], MIN(q.[ChildDateIndex]) AS [MinIndex], MAX(q.[ChildDateIndex]) AS [MaxIndex], MIN(q.[Low]) AS [Low], MAX(q.[High]) AS [High], SUM(q.[Volume]) AS [Volume]
				FROM #QuotesForHigherLevel q
				GROUP BY q.[ParentTimeframe], q.[DateIndex]) a
			LEFT JOIN #QuotesForHigherLevel b ON a.[ParentTimeframe] = b.[ParentTimeframe] AND a.[DateIndex] = b.[DateIndex] AND a.[MinIndex] = b.[ChildDateIndex]
			LEFT JOIN #QuotesForHigherLevel c ON a.[ParentTimeframe] = c.[ParentTimeframe] AND a.[DateIndex] = c.[DateIndex] AND a.[MaxIndex] = c.[ChildDateIndex];


	END

	--Clean up temporary tables
	BEGIN
		DROP TABLE #Quotes;
		DROP TABLE #QuotesForHigherLevel;
	END

END

	

GO

CREATE PROC [dbo].[test_addQuoteFromRawH1] @counter AS INTEGER
AS
BEGIN

	DECLARE @items AS [dbo].[QuotesTransferTable];
	DECLARE @assetId AS INT = 1;
	DECLARE @timeframe AS INT = 4;
	DECLARE @lastDate AS DATETIME;
	
	SET @lastDate = (SELECT
					MAX(d.[Date]) AS [LastQuoteDate]
				FROM 
					(SELECT * FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [Timeframe] = @timeframe) q
					LEFT JOIN (SELECT * FROM [dbo].[dates] WHERE [Timeframe] = @timeframe) d
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

	EXEC [dbo].[addNewQuote] @assetId = @assetId, @timeframe = @timeframe, @quotes = @items;

END

GO


CREATE FUNCTION [dbo].[GetTimeframesIndices](@dates AS [dbo].[DatetimesTransferTable] READONLY)
RETURNS TABLE
AS
RETURN 
(SELECT DISTINCT
	b.[Timeframe], b.[DateIndex]
FROM
	@dates a
	LEFT JOIN
		(SELECT
			d1.[Timeframe], d1.[DateIndex], d1.[Date] AS [StartDate], d2.[Date] AS [EndDate]
		FROM
			[dbo].[dates] d1
			LEFT JOIN [dbo].[dates] d2
			ON d1.[Timeframe] = d2.[Timeframe] AND d1.[DateIndex] = d2.[DateIndex] - 1) b
	ON a.[dt] >= [StartDate] AND a.[dt] < [EndDate]);

GO


CREATE FUNCTION [dbo].[GetLowerLevelIndices](@baseTimeframe AS INT, @timeframeIndices AS [dbo].[TimeframeDateIndexTransferTable] READONLY)
RETURNS TABLE
AS
RETURN 
(SELECT
	c.[Timeframe],
	c.[DateIndex],
	d3.[DateIndex] AS [BaseTimeframeIndex]
FROM
	(SELECT DISTINCT
		b.*
	FROM
		@timeframeIndices a
		LEFT JOIN
			(SELECT
				d1.[Timeframe], d1.[DateIndex], d1.[Date] AS [StartDate], d2.[Date] AS [EndDate]
			FROM
				[dbo].[dates] d1
				LEFT JOIN [dbo].[dates] d2
				ON d1.[Timeframe] = d2.[Timeframe] AND d1.[DateIndex] = d2.[DateIndex] - 1) b
		ON a.[Timeframe] = b.[Timeframe] AND a.[DateIndex] = b.[DateIndex]) c
	LEFT JOIN (SELECT * FROM [dbo].[dates] WHERE [Timeframe] = @baseTimeframe) d3
	ON d3.[Date] >= c.[StartDate] AND d3.[Date] < c.[EndDate]);

GO


EXEC [dbo].[test_addQuoteFromRawH1] @counter = 5


ROLLBACK TRANSACTION;