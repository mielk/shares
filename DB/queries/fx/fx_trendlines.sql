USE [fx];

BEGIN TRANSACTION;

GO


--Drop functions
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLastTrendlinesAnalysisDate]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetLastTrendlinesAnalysisDate]


GO

CREATE FUNCTION [dbo].[GetLastTrendlinesAnalysisDate](@assetId AS INT, @timeframeId AS INT)
RETURNS INT
AS
BEGIN
	DECLARE @index AS INT;
	SELECT @index = [TrendlinesLastAnalyzedIndex] FROM [dbo].[timestamps] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;
	RETURN IIF(@index IS NULL, 0, @index);
END

GO











CREATE PROC [dbo].[findNewTrendlines] @assetId AS INT, @timeframeId AS INT
AS
BEGIN

	DECLARE @lastAnalyzedIndex AS INT = [dbo].[GetLastTrendlinesAnalysisDate](@assetId, @timeframeId);
	DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);

	IF (@lastQuote > @lastAnalyzedIndex)
	BEGIN
	
		DECLARE @minDistance AS INT = [dbo].[GetExtremumMinDistance]();
		DECLARE @maxDistance AS INT = [dbo].[GetExtremumCheckDistance]();
		DECLARE @startIndex AS INT = [dbo].[MaxValue](@lastAnalyzedIndex - @minDistance + 1, 0);
		DECLARE @endIndex AS INT = @lastQuote - @minDistance;

		--Get all extrema required for process of calculating new trendlines.
		SELECT
			*
		INTO 
			#ExtremaForComparing
		FROM
			[dbo].[extrema] e
		WHERE
			e.[AssetId] = @assetId AND
			e.[TimeframeId] = @timeframeId AND
			e.[DateIndex] BETWEEN (@startIndex - @maxDistance) AND @lastQuote;
		CREATE NONCLUSTERED INDEX [ixDateIndex_ExtremaForComparing] ON #ExtremaForComparing ([DateIndex] ASC);

		--Get all extrema to be checked as the base for new trendlines.
		SELECT
			*
		INTO
			#NewExtrema
		FROM
			#ExtremaForComparing e
		WHERE
			e.[DateIndex] >= @startIndex;
		
		select * from #ExtremaForComparing;
		select * from #NewExtrema;

	--	--Insert new extrema to the database.
	--	BEGIN

	--		DECLARE @ocMaxPrices AS [dbo].[DateIndexPrice];
	--		DECLARE @ocMinPrices AS [dbo].[DateIndexPrice];
	--		DECLARE @lowPrices AS [dbo].[DateIndexPrice];
	--		DECLARE @highPrices AS [dbo].[DateIndexPrice];

	--		INSERT INTO @ocMaxPrices SELECT [DateIndex], [MaxOC] FROM #QuotesForComparing;
	--		INSERT INTO @ocMinPrices SELECT [DateIndex], [MinOC] FROM #QuotesForComparing;
	--		INSERT INTO @lowPrices SELECT [DateIndex], [Low] FROM #QuotesForComparing;
	--		INSERT INTO @highPrices SELECT [DateIndex], [High] FROM #QuotesForComparing;

	--		--Peak-by-close
	--		INSERT INTO [dbo].[extrema]([AssetId], [TimeframeId], [ExtremumTypeId], [DateIndex], [MasterExtremumDateIndex], [EarlierCounter], [EarlierAmplitude], [EarlierTotalArea], [EarlierAverageArea], [EarlierChange1], [EarlierChange2], [EarlierChange3], [EarlierChange5], [EarlierChange10]) 
	--		SELECT 
	--			@assetId, @TimeframeId, 1, a.[DateIndex], a.[DateIndex], COALESCE(a.[EarlierCounter], a.[DateIndex] - @firstQuotation),
	--			a.[EarlierAmplitude], a.[TotalArea], a.[TotalArea] / COALESCE(a.[EarlierCounter], a.[DateIndex] - @firstQuotation), 
	--			a.[Delta1], a.[Delta2], a.[Delta3], a.[Delta5], a.[Delta10]
	--		FROM [dbo].[FindExtremaForSingleExtremumType](@startIndex, @endIndex, 1, @minDistance, @maxDistance, @ocMaxPrices, @lowPrices) a;
		
	--		--Peak-by-high
	--		INSERT INTO [dbo].[extrema]([AssetId], [TimeframeId], [ExtremumTypeId], [DateIndex], [MasterExtremumDateIndex], [EarlierCounter], [EarlierAmplitude], [EarlierTotalArea], [EarlierAverageArea], [EarlierChange1], [EarlierChange2], [EarlierChange3], [EarlierChange5], [EarlierChange10]) 
	--		SELECT 
	--			@assetId, @TimeframeId, 2, a.[DateIndex], a.[DateIndex], COALESCE(a.[EarlierCounter], a.[DateIndex] - @firstQuotation),
	--			a.[EarlierAmplitude], a.[TotalArea], a.[TotalArea] / COALESCE(a.[EarlierCounter], a.[DateIndex] - @firstQuotation), 
	--			a.[Delta1], a.[Delta2], a.[Delta3], a.[Delta5], a.[Delta10]
	--		FROM [dbo].[FindExtremaForSingleExtremumType](@startIndex, @endIndex, 1, @minDistance, @maxDistance, @highPrices, @lowPrices) a;

	--		--Trough-by-close
	--		INSERT INTO [dbo].[extrema]([AssetId], [TimeframeId], [ExtremumTypeId], [DateIndex], [MasterExtremumDateIndex], [EarlierCounter], [EarlierAmplitude], [EarlierTotalArea], [EarlierAverageArea], [EarlierChange1], [EarlierChange2], [EarlierChange3], [EarlierChange5], [EarlierChange10]) 
	--		SELECT 
	--			@assetId, @TimeframeId, 3, a.[DateIndex], a.[DateIndex], COALESCE(a.[EarlierCounter], a.[DateIndex] - @firstQuotation),
	--			a.[EarlierAmplitude], a.[TotalArea], a.[TotalArea] / COALESCE(a.[EarlierCounter], a.[DateIndex] - @firstQuotation), 
	--			a.[Delta1], a.[Delta2], a.[Delta3], a.[Delta5], a.[Delta10]
	--		FROM [dbo].[FindExtremaForSingleExtremumType](@startIndex, @endIndex, -1, @minDistance, @maxDistance, @ocMinPrices, @highPrices) a;

	--		--Trough-by-low
	--		INSERT INTO [dbo].[extrema]([AssetId], [TimeframeId], [ExtremumTypeId], [DateIndex], [MasterExtremumDateIndex], [EarlierCounter], [EarlierAmplitude], [EarlierTotalArea], [EarlierAverageArea], [EarlierChange1], [EarlierChange2], [EarlierChange3], [EarlierChange5], [EarlierChange10]) 
	--		SELECT 
	--			@assetId, @TimeframeId, 4, a.[DateIndex], a.[DateIndex], COALESCE(a.[EarlierCounter], a.[DateIndex] - @firstQuotation),
	--			a.[EarlierAmplitude], a.[TotalArea], a.[TotalArea] / COALESCE(a.[EarlierCounter], a.[DateIndex] - @firstQuotation), 
	--			a.[Delta1], a.[Delta2], a.[Delta3], a.[Delta5], a.[Delta10]
	--		FROM [dbo].[FindExtremaForSingleExtremumType](@startIndex, @endIndex, -1, @minDistance, @maxDistance, @lowPrices, @highPrices) a;

	--	END


	--	--Update new extrema [MasterExtremumDateIndex]
	--	UPDATE e
	--	SET 
	--		[MasterExtremumDateIndex] = e2.[DateIndex]
	--	FROM
	--		[dbo].[extrema] e
	--		INNER JOIN (SELECT * FROM [dbo].[extrema] WHERE [DateIndex] BETWEEN @startIndex - @minDistance AND @endIndex) e2
	--		ON e.[DateIndex] - e2.[DateIndex] BETWEEN 1 AND @minDistance - 1 
	--			AND CEILING(e.[ExtremumTypeId]/2.0) = CEILING(e2.[ExtremumTypeId]/2.0)
	--	WHERE e.[DateIndex] BETWEEN @startIndex AND @endIndex;


		--Clean-up
		BEGIN
			DROP TABLE #ExtremaForComparing;
			DROP TABLE #NewExtrema;
		END

	END	

END

GO




CREATE PROC [dbo].[processTrendlines] @assetId AS INT, @timeframeId AS INT
AS
BEGIN
	
	EXEC [dbo].[findNewTrendlines] @assetId = @assetId, @timeframeId = @timeframeId;
	--EXEC [dbo].[analyzeTrendlines] @assetId = @assetId, @timeframeId = @timeframeId;

	--Update timestamp.
	BEGIN

		DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);

		UPDATE [dbo].[timestamps] 
		SET [TrendlinesLastAnalyzedIndex] = @lastQuote
		WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId
		IF @@ROWCOUNT=0
			INSERT INTO [dbo].[timestamps]([AssetId], [TimeframeId], [TrendlinesLastAnalyzedIndex]) 
			VALUES (@assetId, @timeframeId, @lastQuote);
		
	END

END

GO



EXEC [dbo].[findNewTrendlines] @assetId = 1, @timeframeId = 4;


ROLLBACK TRANSACTION;