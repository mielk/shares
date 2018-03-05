USE [fx];

BEGIN TRANSACTION;

GO

--Drop procedures
IF OBJECT_ID('findNewTrendlines','P') IS NOT NULL DROP PROC [dbo].[findNewTrendlines];
IF OBJECT_ID('processTrendlines','P') IS NOT NULL DROP PROC [dbo].[processTrendlines];

GO

--Drop functions
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendlinesAnalysisLastQuotationIndex]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendlinesAnalysisLastQuotationIndex]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendlinesAnalysisLastExtremumIndex]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendlinesAnalysisLastExtremumIndex]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendlinesAnalysisLastExtremumGroupId]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendlinesAnalysisLastExtremumGroupId]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStepPrecision]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetStepPrecision]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAssetCalculatingTrendlineStepFactor]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetAssetCalculatingTrendlineStepFactor]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendlineExtremaPairingPriceLevels]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendlineExtremaPairingPriceLevels]


GO


CREATE FUNCTION [dbo].[GetTrendlinesAnalysisLastQuotationIndex](@assetId AS INT, @timeframeId AS INT)
RETURNS INT
AS
BEGIN
	DECLARE @index AS INT;
	SELECT @index = [TrendlinesAnalysisLastQuotationIndex] FROM [dbo].[timestamps] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;
	RETURN IIF(@index IS NULL, 0, @index);
END

GO

CREATE FUNCTION [dbo].[GetTrendlinesAnalysisLastExtremumGroupId](@assetId AS INT, @timeframeId AS INT)
RETURNS INT
AS
BEGIN
	DECLARE @index AS INT;
	SELECT @index = MAX([CounterExtremumGroupId]) FROM [dbo].[trendlines] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;
	RETURN IIF(@index IS NULL, 0, @index);
END

GO

CREATE FUNCTION [dbo].[GetAssetCalculatingTrendlineStepFactor](@assetId AS INT)
RETURNS INT
AS
BEGIN
	DECLARE @stepPrecision AS FLOAT = 2;
	RETURN POWER(10, @stepPrecision);
END

GO

CREATE FUNCTION [dbo].[GetTrendlineExtremaPairingPriceLevels](@minPrice AS FLOAT, @maxPrice AS FLOAT, @stepFactor AS FLOAT)
RETURNS TABLE
AS
RETURN
(
	SELECT
		(pn.[number] / @stepFactor) AS [level]
	FROM
		[dbo].[predefinedNumbers] pn,
		(SELECT
			CEILING(@minPrice * @stepFactor) / @stepFactor AS [Min],
			FLOOR(@maxPrice * @stepFactor) / @stepFactor AS [Max]) pr
	WHERE
		pn.[number] BETWEEN (pr.[Min] * @stepFactor) AND (pr.[Max] * @stepFactor)
);

GO



CREATE PROC [dbo].[findNewTrendlines] @assetId AS INT, @timeframeId AS INT
AS
BEGIN

	DECLARE @lastAnalyzedIndex AS INT = [dbo].[GetTrendlinesAnalysisLastQuotationIndex](@assetId, @timeframeId);
	DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);
	DECLARE @lastExtremumGroupId AS INT = [dbo].[GetTrendlinesAnalysisLastExtremumGroupId](@assetId, @timeframeId);

	IF (@lastQuote > @lastAnalyzedIndex)
	BEGIN
	
		

		--Clean-up
		BEGIN
			DROP TABLE #ExtremaForComparing;
			DROP TABLE #NewExtrema;
			DROP TABLE #PossiblePriceLevels;
			DROP TABLE #PriceLevelForExtremaPairing;
		END

	END	

END

GO



CREATE PROC [dbo].[processTrendlines] @assetId AS INT, @timeframeId AS INT
AS
BEGIN
	
	EXEC [dbo].[findNewTrendlines] @assetId = @assetId, @timeframeId = @timeframeId;
	--EXEC [dbo].[analyzeTrendlines] @assetId = @assetId, @timeframeId = @timeframeId;

	----Update timestamp.
	--BEGIN

		DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);

		UPDATE [dbo].[timestamps] 
		SET [TrendlinesAnalysisLastQuotationIndex] = @lastQuote
		WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId
		IF @@ROWCOUNT=0
			INSERT INTO [dbo].[timestamps]([AssetId], [TimeframeId], [TrendlinesAnalysisLastQuotationIndex]) 
			VALUES (@assetId, @timeframeId, @lastQuote);
		
	END

END

GO

--exec [dbo].[processTrendlines] @assetId  = 1, @timeframeId = 4

commit transaction