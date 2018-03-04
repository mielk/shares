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

CREATE FUNCTION [dbo].[GetTrendlinesAnalysisLastExtremumIndex](@assetId AS INT, @timeframeId AS INT)
RETURNS INT
AS
BEGIN
	DECLARE @index AS INT;
	SELECT @index = MAX([CounterStartIndex]) FROM [dbo].[trendlines] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;
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
	DECLARE @lastExtremum AS INT = 1 --[dbo].[GetTrendlinesAnalysisLastExtremumIndex](@assetId, @timeframeId);

	IF (@lastQuote > @lastAnalyzedIndex)
	BEGIN
	
		DECLARE @minDistance AS INT = [dbo].[GetExtremumMinDistance]();
		DECLARE @maxDistance AS INT = [dbo].[GetExtremumCheckDistance]();
		DECLARE @startIndex AS INT = [dbo].[MaxValue](@lastAnalyzedIndex - @minDistance + 1, 0);
		DECLARE @endIndex AS INT = @lastQuote - @minDistance;


		--Get all extrema required for process of calculating new trendlines.
		SELECT
			e.[ExtremumId],
			e.[DateIndex],
			e.[ExtremumTypeId],
			e.[MasterExtremumDateIndex],
			e.[Value],
			IIF(q.[Open] > q.[Close], q.[Open], q.[Close]) AS [MaxOC],
			IIF(q.[Open] < q.[Close], q.[Open], q.[Close]) AS [MinOC],
			q.[High] AS [High],
			q.[Low] AS [Low]
		INTO 
			#ExtremaForComparing
		FROM
				(SELECT *
				FROM [dbo].[extrema]
				WHERE
					[AssetId] = @assetId AND
					[TimeframeId] = @timeframeId AND
					[DateIndex] BETWEEN (@startIndex - @maxDistance) AND @lastQuote AND
					[Value] > 50) e
			LEFT JOIN 
				(SELECT * 
				 FROM [dbo].[quotes]
				 WHERE
					[AssetId] = @assetId AND
					[TimeframeId] = @timeframeId AND
					[DateIndex] BETWEEN (@startIndex - @maxDistance) AND @lastQuote) q
			ON e.[DateIndex] = q.[DateIndex];
		CREATE NONCLUSTERED INDEX [ixDateIndex_ExtremaForComparing] ON #ExtremaForComparing ([DateIndex] ASC);


		--Get all extrema to be checked as the base for new trendlines.
		SELECT
			*
		INTO
			#NewExtrema
		FROM
			#ExtremaForComparing e
		WHERE
			e.[DateIndex] >= @lastExtremum;
		

		--Get price levels for extrema pairing.
		DECLARE @minPrice AS FLOAT = (SELECT MIN([Low]) FROM #NewExtrema);
		DECLARE @maxPrice AS FLOAT = (SELECT MAX([High]) FROM #NewExtrema);
		DECLARE @stepFactor AS FLOAT = [dbo].[GetAssetCalculatingTrendlineStepFactor](@assetId);
		SELECT * INTO #PossiblePriceLevels FROM [dbo].[GetTrendlineExtremaPairingPriceLevels](@minPrice, @maxPrice, @stepFactor);


		-- Price levels for specific extrema.
		SELECT
			ne.[ExtremumId],
			IIF(ne.[ExtremumTypeId] < 3, ne.[High], ec.[MinOC]) AS [High],
			IIF(ne.[ExtremumTypeId] < 3, ec.[MaxOC], ne.[Low]) AS [Low]
		INTO
			#PriceLevelForExtremaPairing
		FROM
			#NewExtrema ne
			LEFT JOIN #ExtremaForComparing ec
			ON ne.[MasterExtremumDateIndex] = ec.[DateIndex]


		--Possible extrema pairs.
		select 
			'Possible extrema pairs', * 
		from 
			#NewExtrema ne
			LEFT JOIN #ExtremaForComparing ec
			ON ne.[DateIndex] - ec.[DateIndex] BETWEEN @minDistance AND @maxDistance


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

	--	DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);

	--	UPDATE [dbo].[timestamps] 
	--	SET [TrendlinesAnalysisLastQuotationIndex] = @lastQuote
	--	WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId
	--	IF @@ROWCOUNT=0
	--		INSERT INTO [dbo].[timestamps]([AssetId], [TimeframeId], [TrendlinesAnalysisLastQuotationIndex]) 
	--		VALUES (@assetId, @timeframeId, @lastQuote);
		
	--END

END

GO

--exec [dbo].[processTrendlines] @assetId  = 1, @timeframeId = 4

commit transaction