USE [fx];

BEGIN TRANSACTION

GO

--Drop procedures
IF OBJECT_ID('analyzeExtrema','P') IS NOT NULL DROP PROC [dbo].[analyzeExtrema];
IF OBJECT_ID('findNewExtrema','P') IS NOT NULL DROP PROC [dbo].[findNewExtrema];

--Drop functions
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLastExtremumAnalysisDate]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetLastExtremumAnalysisDate]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLastQuote]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetLastQuote]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExtremumCheckDistance]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetExtremumCheckDistance]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExtremumMinDistance]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetExtremumMinDistance]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FindExtremaForSingleExtremumType]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[FindExtremaForSingleExtremumType]

--Types
--IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'DateIndexPrice') DROP TYPE [dbo].[DateIndexPrice];

GO



--Creating types.
--CREATE TYPE [dbo].[DateIndexPrice] AS TABLE(
--	[DateIndex] [int] NOT NULL PRIMARY KEY CLUSTERED,
--	[Price] [float] NOT NULL
--);

--GO



--Creating functions.
CREATE FUNCTION [dbo].[GetLastExtremumAnalysisDate](@assetId AS INT, @timeframeId AS INT)
RETURNS INT
AS
BEGIN
	DECLARE @index AS INT;
	SELECT @index = [ExtremaLastAnalyzedIndex] FROM [dbo].[timestamps] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;
	RETURN IIF(@index IS NULL, 0, @index);
END

GO

CREATE FUNCTION [dbo].[GetLastQuote](@assetId AS INT, @timeframeId AS INT)
RETURNS INT
AS
BEGIN
	DECLARE @index AS INT;
	SELECT @index = MAX([DateIndex]) FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;
	RETURN IIF(@index IS NULL, 0, @index);
END

GO

CREATE FUNCTION [dbo].[GetExtremumCheckDistance]()
RETURNS INT
AS
BEGIN
	DECLARE @value AS INT;
	SELECT @value = [SettingValue] FROM [dbo].[settingsNumeric] WHERE [SettingName] = 'ExtremumAnalysisCheckDistance';
	RETURN IIF(@value IS NULL, 0, @value);
END

GO

CREATE FUNCTION [dbo].[GetExtremumMinDistance]()
RETURNS INT
AS
BEGIN
	DECLARE @value AS INT;
	SELECT @value = [SettingValue] FROM [dbo].[settingsNumeric] WHERE [SettingName] = 'ExtremumAnalysisMinDistance';
	RETURN IIF(@value IS NULL, 0, @value);
END

GO

CREATE FUNCTION [dbo].[FindExtremaForSingleExtremumType](@isPeak AS INT, @minDistance AS INT, @basePrices AS [dbo].[DateIndexPrice] READONLY, @comparedPrices AS [dbo].[DateIndexPrice] READONLY)
RETURNS TABLE
AS
RETURN 
(
	SELECT 
		leftFiltered.*
	FROM
		(SELECT
				bp.*
			FROM
				@basePrices bp
				LEFT JOIN @comparedPrices cp
				ON (bp.[DateIndex] - cp.[DateIndex] BETWEEN 1 AND @minDistance) AND ((bp.[Price] * @isPeak) <= (cp.[Price] * @isPeak))
			WHERE cp.[DateIndex] IS NULL) leftFiltered
		LEFT JOIN @comparedPrices cp2
		ON (cp2.[DateIndex] - leftFiltered.[DateIndex] BETWEEN 1 AND @minDistance) AND ((leftFiltered.[Price] * @isPeak) < (cp2.[Price] * @isPeak))
		WHERE cp2.[DateIndex] IS NULL);

GO







CREATE PROC [dbo].[findNewExtrema] @assetId AS INT, @timeframeId AS INT, @lastAnalyzedIndex AS INT, @lastQuote AS INT
AS
BEGIN

	DECLARE @minDistance AS INT = [dbo].[GetExtremumMinDistance]();
	DECLARE @startIndex AS INT = [dbo].[MaxValue](@lastAnalyzedIndex - @minDistance + 1, 0);
	DECLARE @endIndex AS INT = @lastQuote - @minDistance;


	--Get minimum amount of data required to look for extrema (@startIndex/@endIndex range and @minDistance offset to both sides).
	SELECT
		q.[DateIndex],
		IIF(q.[Close] > q.[Open], q.[Close], q.[Open]) AS [MaxOC],
		IIF(q.[Close] < q.[Open], q.[Close], q.[Open]) AS [MinOC],
		q.[High],
		q.[Low]
	INTO 
		#QuotesForComparing
	FROM
		[dbo].[quotes] q
	WHERE
		q.[AssetId] = @assetId AND
		q.[TimeframeId] = @timeframeId AND
		q.[DateIndex] BETWEEN (@startIndex - @minDistance) AND @lastQuote;
	CREATE NONCLUSTERED INDEX [ixDateIndex_QuotesForComparing] ON #QuotesForComparing ([DateIndex] ASC);

	--Get quotes to be analyzed.
	SELECT
		*
	INTO	
		#AnalyzedQuotes
	FROM
		#QuotesForComparing q
	WHERE
		q.[DateIndex] BETWEEN @startIndex AND @endIndex;
	CREATE NONCLUSTERED INDEX [ixDateIndex_AnalyzedQuotes] ON #AnalyzedQuotes ([DateIndex] ASC);


	--Insert new extrema to the database.
	BEGIN

		DECLARE @basePrices AS [dbo].[DateIndexPrice];
		DECLARE @comparedPrices AS [dbo].[DateIndexPrice];

		--Peak-by-close
		BEGIN
			INSERT INTO @basePrices SELECT [DateIndex], [MaxOC] FROM #AnalyzedQuotes;
			INSERT INTO @comparedPrices SELECT [DateIndex], [MaxOC] FROM #QuotesForComparing;
			INSERT INTO [dbo].[extrema]([AssetId], [TimeframeId], [DateIndex], [ExtremumTypeId]) 
			SELECT @assetId AS [AssetId], @timeframeId AS [TimeframeId], a.[DateIndex], 1 FROM [dbo].[FindExtremaForSingleExtremumType](1, @minDistance, @basePrices, @comparedPrices) a;
			DELETE FROM @basePrices;
			DELETE FROM @comparedPrices;
		END
		
		--Peak-by-high
		BEGIN
			INSERT INTO @basePrices SELECT [DateIndex], [High] FROM #AnalyzedQuotes;
			INSERT INTO @comparedPrices SELECT [DateIndex], [High] FROM #QuotesForComparing;
			INSERT INTO [dbo].[extrema]([AssetId], [TimeframeId], [DateIndex], [ExtremumTypeId]) 
			SELECT @assetId AS [AssetId], @timeframeId AS [TimeframeId], a.[DateIndex], 2 FROM [dbo].[FindExtremaForSingleExtremumType](1, @minDistance, @basePrices, @comparedPrices) a;
			DELETE FROM @basePrices;
			DELETE FROM @comparedPrices;
		END	
		
		--Trough-by-close
		BEGIN
			INSERT INTO @basePrices SELECT [DateIndex], [MinOC] FROM #AnalyzedQuotes;
			INSERT INTO @comparedPrices SELECT [DateIndex], [MinOC] FROM #QuotesForComparing;
			INSERT INTO [dbo].[extrema]([AssetId], [TimeframeId], [DateIndex], [ExtremumTypeId]) 
			SELECT @assetId AS [AssetId], @timeframeId AS [TimeframeId], a.[DateIndex], 3 FROM [dbo].[FindExtremaForSingleExtremumType](-1, @minDistance, @basePrices, @comparedPrices) a;
			DELETE FROM @basePrices;
			DELETE FROM @comparedPrices;
		END	
		
		--Trough-by-low
		BEGIN
			INSERT INTO @basePrices SELECT [DateIndex], [Low] FROM #AnalyzedQuotes;
			INSERT INTO @comparedPrices SELECT [DateIndex], [Low] FROM #QuotesForComparing;
			INSERT INTO [dbo].[extrema]([AssetId], [TimeframeId], [DateIndex], [ExtremumTypeId]) 
			SELECT @assetId AS [AssetId], @timeframeId AS [TimeframeId], a.[DateIndex], 4 FROM [dbo].[FindExtremaForSingleExtremumType](-1, @minDistance, @basePrices, @comparedPrices) a;
			DELETE FROM @basePrices;
			DELETE FROM @comparedPrices;
		END								

	END


	--Clean-up
	BEGIN
		DROP TABLE #AnalyzedQuotes;
		DROP TABLE #QuotesForComparing;
	END

END

GO


CREATE PROC [dbo].[analyzeExtrema] @assetId AS INT, @timeframeId AS INT
AS
BEGIN
	
	DECLARE @lastAnalyzedIndex AS INT = [dbo].[GetLastExtremumAnalysisDate](@assetId, @timeframeId);
	DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);
	
	--DECLARE @checkDistance AS INT = [dbo].[GetExtremumCheckDistance]();

	IF (@lastQuote > @lastAnalyzedIndex)
	BEGIN
		
		EXEC [dbo].[findNewExtrema] @assetId = @assetId, @timeframeId = @timeframeId, @lastAnalyzedIndex = @lastAnalyzedIndex, @lastQuote = @lastQuote;

	END

	--PRINT @lastAnalyzedIndex;
	--PRINT @lastQuote;
	--PRINT @checkDistance;
	--PRINT @minDistance;

END

GO





EXEC [dbo].[analyzeExtrema] @assetId = 1, @timeframeId = 4;

SELECT * FROM [dbo].[extrema];

ROLLBACK TRANSACTION;