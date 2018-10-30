USE [fx];

BEGIN TRANSACTION;

GO

--Drop procedures
IF OBJECT_ID('findNewTrendlines','P') IS NOT NULL DROP PROC [dbo].[findNewTrendlines];
IF OBJECT_ID('processTrendlines','P') IS NOT NULL DROP PROC [dbo].[processTrendlines];
IF OBJECT_ID('analyzeTrendlinesLeftSide','P') IS NOT NULL DROP PROC [dbo].[analyzeTrendlinesLeftSide];
--IF OBJECT_ID('analyzeTrendlinesRightSide','P') IS NOT NULL DROP PROC [dbo].[analyzeTrendlinesRightSide];
IF OBJECT_ID('updateTrendEvaluations','P') IS NOT NULL DROP PROC [dbo].[updateTrendEvaluations];

GO

--Drop functions
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendlinesAnalysisLastQuotationIndex]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendlinesAnalysisLastQuotationIndex]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendlinesAnalysisLastExtremumIndex]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendlinesAnalysisLastExtremumIndex]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendlinesAnalysisLastExtremumGroupId]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendlinesAnalysisLastExtremumGroupId]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStepPrecision]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetStepPrecision]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAssetCalculatingTrendlineStepFactor]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetAssetCalculatingTrendlineStepFactor]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendlineExtremaPairingPriceLevels]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendlineExtremaPairingPriceLevels]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendlineCheckDistance]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendlineCheckDistance]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendRangesCrossDetails]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendRangesCrossDetails]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendRangesVariations]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendRangesVariations]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExtremumCheckDistance]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetExtremumCheckDistance]

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendRangesStats]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendRangesStats]

--Types
--IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'DateIndexPrice') DROP TYPE [dbo].[DateIndexPrice];
--IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'IdAndDateIndex') DROP TYPE [dbo].[IdAndDateIndex];

GO


----Creating types.
--CREATE TYPE [dbo].[TrendRangeBasicData] AS TABLE(
--	[TrendRangeId] [int] NOT NULL PRIMARY KEY CLUSTERED,
--	[TrendlineStartDateIndex] [int] NOT NULL,
--	[TrendlineStartLevel] [float] NOT NULL,
--	[TrendlineAngle] [float] NOT NULL,
--	[StartIndex] [int] NOT NULL,
--	[EndIndex] [int] NOT NULL,
--	[IsPeak] [int] NOT NULL
--);


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

CREATE FUNCTION [dbo].[GetTrendlinesAnalysisLastExtremumGroupId](@assetId AS INT, @timeframeId AS INT)
RETURNS INT
AS
BEGIN
	DECLARE @index AS INT;
	SELECT @index = [TrendlinesAnalysisLastExtremumGroupId] FROM [dbo].[timestamps] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;
	RETURN IIF(@index IS NULL, 0, @index);
END

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

CREATE FUNCTION [dbo].[GetTrendlineCheckDistance]()
RETURNS INT
AS
BEGIN
	DECLARE @value AS INT;
	SELECT @value = [SettingValue] FROM [dbo].[settingsNumeric] WHERE [SettingName] = 'TrendlineAnalysisCheckDistance';
	RETURN IIF(@value IS NULL, 0, @value);
END

GO



CREATE FUNCTION [dbo].[GetTrendRangesVariations](
		@assetId AS INT,
		@timeframeId AS INT, 
		@basicData AS [dbo].[TrendRangeBasicData] READONLY
	)
RETURNS TABLE
AS
RETURN
(

		SELECT
			b.[TrendRangeId],
			COUNT(b.[HLVariation]) AS [TotalCandles],
			SUM(b.[HLVariation]) AS [TotalVariation],
			MAX(b.[HLVariation]) AS [ExtremumVariation],
			MAX(b.[OCVariation]) AS [OCVariation]
		FROM	
			(SELECT
				ptc.*,
				ABS(ptc.[ModifiedTrendlineLevel] - ptc.[ModifiedExtremumPrice]) AS [HLVariation],
				ABS(ptc.[ModifiedTrendlineLevel] - ptc.[ModifiedOpenClosePrice]) AS [OCVariation]
			FROM
				(SELECT
					a.*,
					a.[TrendlineLevel] * CAST(a.[IsPeak] AS FLOAT) AS [ModifiedTrendlineLevel],
					a.[ExtremumPrice] * CAST(a.[IsPeak] AS FLOAT) AS [ModifiedExtremumPrice],
					a.[OpenClosePrice] * CAST(a.[IsPeak] AS FLOAT) AS [ModifiedOpenClosePrice]
				FROM
					(SELECT
						bd.[TrendRangeId],
						CAST((tq.[DateIndex] - bd.[TrendlineStartDateIndex]) AS FLOAT) * bd.[TrendlineAngle] + bd.[TrendlineStartLevel] AS [TrendlineLevel],
						IIF(bd.[IsPeak] = 1, tq.[High], tq.[Low]) AS [ExtremumPrice],
						IIF(bd.[IsPeak] = tq.[IsBullish], tq.[Close], tq.[Open]) AS [OpenClosePrice],
						bd.[IsPeak]
					FROM
						@basicData bd
						LEFT JOIN (	SELECT *, IIF(q.[Close] > q.[Open], 1, -1) AS [IsBullish]
									FROM
										(SELECT * FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) q
										LEFT JOIN (SELECT MIN([StartIndex]) AS [Min], MAX([EndIndex]) AS [Max] FROM @basicData) qr
										ON q.[DateIndex] BETWEEN qr.[Min] AND qr.[Max]
									WHERE
										[AssetId] = @assetId AND [TimeframeId] = @timeframeId) tq
						ON tq.[DateIndex] BETWEEN bd.[StartIndex] AND bd.[EndIndex]) a) ptc) b
		GROUP BY 
			b.[TrendRangeId]

);


GO


CREATE FUNCTION [dbo].[GetTrendRangesCrossDetails](
		@assetId AS INT,
		@timeframeId AS INT, 
		@basicData AS [dbo].[TrendRangeBasicData] READONLY
	)
RETURNS TABLE
AS
RETURN
(

	SELECT
		d.[TrendRangeId],
		d.[ExCrossRangeSum] + d.[ExCrossRangeAverage] + [ExCrossRangeStDeviation] AS [ExtremumPriceCrossPenaltyPoints],
		d.[ExCrossRangeCounter] AS [ExtremumPriceCrossCounter],
		d.[OcCrossRangeSum] + d.[OcCrossRangeAverage] + [OcCrossRangeStDeviation] AS [OCPriceCrossPenaltyPoints],
		d.[OcCrossRangeCounter] AS [OCPriceCrossCounter]
	FROM
		(SELECT
			c.[TrendRangeId],
			SUM([ExCrossRange]) AS ExCrossRangeSum,
			AVG([ExCrossRange]) AS ExCrossRangeAverage,
			STDEVP([ExCrossRange]) AS ExCrossRangeStDeviation,
			COUNT([ExCrossRange]) AS ExCrossRangeCounter,
			SUM([OcCrossRange]) AS OcCrossRangeSum,
			AVG([OcCrossRange]) AS OcCrossRangeAverage,
			STDEVP([OcCrossRange]) AS OcCrossRangeStDeviation,
			COUNT([OcCrossRange]) AS OcCrossRangeCounter
		FROM
			(SELECT
				b.[TrendRangeId],
				b.[DateIndex],
				IIF(b.[ModifiedExtremumPrice] > b.[ModifiedTrendlineLevel], b.[ModifiedExtremumPrice] - b.[ModifiedTrendlineLevel], NULL) AS [ExCrossRange],
				IIF(b.[ModifiedOpenClosePrice] > b.[ModifiedTrendlineLevel], b.[ModifiedOpenClosePrice] - b.[ModifiedTrendlineLevel], NULL) AS [OcCrossRange]
			FROM
				(SELECT
						a.*,
						a.[TrendlineLevel] * CAST(a.[IsPeak] AS FLOAT) AS [ModifiedTrendlineLevel],
						a.[ExtremumPrice] * CAST(a.[IsPeak] AS FLOAT) AS [ModifiedExtremumPrice],
						a.[OpenClosePrice] * CAST(a.[IsPeak] AS FLOAT) AS [ModifiedOpenClosePrice]
					FROM
						(SELECT
							bd.[TrendRangeId],
							tq.[DateIndex],
							CAST((tq.[DateIndex] - bd.[TrendlineStartDateIndex]) AS FLOAT) * bd.[TrendlineAngle] + bd.[TrendlineStartLevel] AS [TrendlineLevel],
							IIF(bd.[IsPeak] = 1, tq.[High], tq.[Low]) AS [ExtremumPrice],
							IIF(bd.[IsPeak] = tq.[IsBullish], tq.[Close], tq.[Open]) AS [OpenClosePrice],
							bd.[IsPeak]
						FROM
							@basicData bd
							LEFT JOIN (	SELECT *, IIF(q.[Close] > q.[Open], 1, -1) AS [IsBullish]
										FROM
											(SELECT * FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) q
											LEFT JOIN (SELECT MIN([StartIndex]) AS [Min], MAX([EndIndex]) AS [Max] FROM @basicData) qr
											ON q.[DateIndex] BETWEEN qr.[Min] AND qr.[Max]
										WHERE	
											[AssetId] = @assetId AND [TimeframeId] = @timeframeId) tq
							ON tq.[DateIndex] BETWEEN bd.[StartIndex] AND bd.[EndIndex]) a) b) c
			GROUP BY c.[TrendRangeId]) d
);

GO














CREATE PROC [dbo].[findNewTrendlines] @assetId AS INT, @timeframeId AS INT
AS
BEGIN

	DECLARE @lastAnalyzedIndex AS INT = [dbo].[GetTrendlinesAnalysisLastQuotationIndex](@assetId, @timeframeId);
	DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);
	DECLARE @lastExtremumGroupId AS INT = [dbo].[GetTrendlinesAnalysisLastExtremumGroupId](@assetId, @timeframeId);
	DECLARE @trendlineCheckDistance AS INT = [dbo].[GetTrendlineCheckDistance]();
	DECLARE @extremumMinDistance AS INT = [dbo].[GetExtremumMinDistance]();

	IF (@lastQuote > @lastAnalyzedIndex)
	BEGIN


		--Create table with extremum groups for pairing.
		SELECT
			*
		INTO
			#ExtremumGroupsForPairing
		FROM
			[dbo].[extremumGroups] eg
		WHERE
			eg.[ExtremumGroupId] > @lastExtremumGroupId - @trendlineCheckDistance - @extremumMinDistance;
		

		--Select all extrema group without trendlines.
		SELECT
			*
		INTO
			#UnprocessedExtremumGroups
		FROM
			#ExtremumGroupsForPairing eg
		WHERE
			eg.[ExtremumGroupId] > @lastExtremumGroupId;
		

		--Get price levels for extrema pairing.
		DECLARE @minPrice AS FLOAT = (SELECT [dbo].[MinValue](MIN([ExtremumPriceLevel]), MIN([OCPriceLevel])) FROM #ExtremumGroupsForPairing);
		DECLARE @maxPrice AS FLOAT = (SELECT [dbo].[MaxValue](MAX([ExtremumPriceLevel]), MAX([OCPriceLevel])) FROM #ExtremumGroupsForPairing);
		DECLARE @stepFactor AS FLOAT = [dbo].[GetAssetCalculatingTrendlineStepFactor](@assetId);
		SELECT * INTO #PossiblePriceLevels FROM [dbo].[GetTrendlineExtremaPairingPriceLevels](@minPrice, @maxPrice, @stepFactor);



		-- Price levels and DateIndex for specific extrema.
		SELECT
			eg.[ExtremumGroupId],
			pl.[Level],
			IIF((eg.[IsPeak] = IIF(pl.[Level] <= eg.[MiddlePriceLevel], 1, -1)), eg.[MasterDateIndex], eg.[SlaveDateIndex]) AS [DateIndex]
		INTO
			#PriceLevelForExtremumGroups
		FROM
			(SELECT *, IIF([IsPeak] = 1, [OCPriceLevel], [ExtremumPriceLevel]) AS [Min], IIF([IsPeak] = 1, [ExtremumPriceLevel], [OCPriceLevel]) AS [Max] FROM #ExtremumGroupsForPairing) eg
			LEFT JOIN #PossiblePriceLevels pl
			ON pl.[level] BETWEEN [Min] AND [Max];

		-- Select prospective extremum group pairs.
		SELECT
			egfp.[ExtremumGroupId] AS [BaseExtremumGroupId],
			ueg.[ExtremumGroupId] AS [CounterExtremumGroupId]
		INTO
			#ExtremumGroupPairs
		FROM
			#UnprocessedExtremumGroups ueg
			INNER JOIN #ExtremumGroupsForPairing egfp
			ON ueg.[StartDateIndex] - egfp.[EndDateIndex] <= @trendlineCheckDistance AND ueg.[EndDateIndex] - egfp.[StartDateIndex] >= @extremumMinDistance;
			

		-- Create trendlines.
		INSERT INTO [dbo].[trendlines]([AssetId], [TimeframeId], [BaseExtremumGroupId], [BaseDateIndex], [BaseLevel], [CounterExtremumGroupId], [CounterDateIndex], [CounterLevel], [Angle], [CandlesDistance])
		SELECT 
			@assetId,
			@timeframeId,
			eg.[BaseExtremumGroupId],
			pl1.[DateIndex] AS [BaseDateIndex],
			pl1.[level] AS [BaseLevel],
			eg.[CounterExtremumGroupId],
			pl2.[DateIndex] AS [CounterDateIndex],
			pl2.[level] AS [CounterLevel], 
			(pl1.[level] - pl2.[level]) / (pl1.[DateIndex] - pl2.[DateIndex]) AS [Angle],
			pl2.[DateIndex] - pl1.[DateIndex] AS [CandlesDistance]
		FROM
			#ExtremumGroupPairs eg
			LEFT JOIN #PriceLevelForExtremumGroups pl1 ON eg.[BaseExtremumGroupId] = pl1.[ExtremumGroupId]
			LEFT JOIN #PriceLevelForExtremumGroups pl2 ON eg.[CounterExtremumGroupId] = pl2.[ExtremumGroupId]
		--WHERE
		--	pl1.[DateIndex] = 40 AND
		--	pl1.[Level] = 104.70 AND
		--	pl2.[DateIndex] = 53 AND
		--	pl2.[Level] = 104.78;


		--Clean-up
		BEGIN
			DROP TABLE #UnprocessedExtremumGroups;
			DROP TABLE #ExtremumGroupsForPairing;
			DROP TABLE #PossiblePriceLevels;
			DROP TABLE #PriceLevelForExtremumGroups;
			DROP TABLE #ExtremumGroupPairs;
		END

	END

END

GO



CREATE PROC [dbo].[analyzeTrendlinesLeftSide] @assetId AS INT, @timeframeId AS INT
AS
BEGIN
	
	-- Prepare temp tables.
	BEGIN
		
		-- Trend breaks
		BEGIN

			CREATE TABLE #TrendBreaks(
				[TrendBreakId] [int] IDENTITY(1,1) NOT NULL,
				[TrendlineId] [int] NOT NULL,
				[DateIndex] [int] NOT NULL,
				[BreakFromAbove] [int] NOT NULL,
				[ProductionId] [int] NULL,
				CONSTRAINT [PK_temp_trendBreaks] PRIMARY KEY CLUSTERED ([TrendBreakId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY];

			CREATE NONCLUSTERED INDEX [ixTrendlineId_temp_trendlinesBreaks] ON #TrendBreaks
			([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixDateIndex_temp_trendlinesBreaks] ON #TrendBreaks
			([DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		END

		-- Trend hits
		BEGIN

			CREATE TABLE #TrendHits(
				[TrendHitId] [int] IDENTITY(1,1) NOT NULL,
				[TrendlineId] [int] NOT NULL,
				[ExtremumGroupId] [int] NOT NULL,
				[DateIndex] [int] NOT NULL,
				[ProductionId] [int] NULL,
				[Value] [float] NULL,
				CONSTRAINT [PK_temp_trendlinesHits] PRIMARY KEY CLUSTERED ([TrendHitId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY];

			CREATE NONCLUSTERED INDEX [ixTrendlineId_temp_trendlinesHits] ON #TrendHits
			([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixExtremumGroup_temp_trendlinesHits] ON #TrendHits
			([ExtremumGroupId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixDateIndex_temp_trendlinesHits] ON #TrendHits
			([DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		END

		-- Trend ranges
		BEGIN
		
			CREATE TABLE #TrendRanges(
				[TrendRangeId] [int] IDENTITY(1,1) NOT NULL,
				[TrendlineId] [int] NOT NULL,
				[BaseId] [int] NOT NULL,
				[BaseIsHit] [int] NOT NULL,
				[BaseDateIndex] [int] NOT NULL,
				[CounterId] [int] NOT NULL,
				[CounterIsHit] [int] NOT NULL,
				[CounterDateIndex] [int] NOT NULL,
				[ProductionId] [int] NULL,
				[IsPeak] [int] NOT NULL DEFAULT(0),
				[ExtremumPriceCrossPenaltyPoints] [float] NULL,
				[ExtremumPriceCrossCounter] [int] NULL,
				[OCPriceCrossPenaltyPoints] [float] NULL,
				[OCPriceCrossCounter] [int] NULL,
				[TotalCandles] [int] NULL,
				[AverageVariation] [float] NULL,
				[ExtremumVariation] [float] NULL,
				[OpenCloseVariation] [float] NULL,
				[BaseHitValue] [float] NULL,
				[CounterHitValue] [float] NULL,
				CONSTRAINT [PK_temp_trendRanges] PRIMARY KEY CLUSTERED ([TrendRangeId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY];

			CREATE NONCLUSTERED INDEX [ixTrendlineId_temp_trendRanges] ON #TrendRanges
			([TrendlineId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixBaseId_temp_trendRanges] ON #TrendRanges
			([BaseId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixBaseDateIndex_temp_trendRanges] ON #TrendRanges
			([BaseDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixCounterId_temp_trendRanges] ON #TrendRanges
			([CounterId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
			
			CREATE NONCLUSTERED INDEX [ixCounterDateIndex_temp_trendRanges] ON #TrendRanges
			([CounterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
			
			CREATE NONCLUSTERED INDEX [ixIsPeak_temp_trendRanges] ON #TrendRanges
			([IsPeak] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		END

		-- Trendlines
		BEGIN

			-- All trendlines
			BEGIN

				CREATE TABLE #Trendlines(
					[TrendlineId] [int] NOT NULL,	
					[BaseExtremumGroupId] [int] NOT NULL,
					[BaseDateIndex] [int] NOT NULL,
					[BaseLevel] [float] NOT NULL,
					[CounterExtremumGroupId] [int] NOT NULL,
					[CounterDateIndex] [int] NOT NULL,
					[CounterLevel] [float] NOT NULL,
					[Angle] [float] NOT NULL,
					[StartDateIndex] [int] NULL,
					[EndDateIndex] [int] NULL,
					[IsOpenFromLeft] [bit] NOT NULL DEFAULT(1),
					[IsOpenFromRight] [bit] NOT NULL DEFAULT(1),
					[CandlesDistance] [int] NOT NULL,
					[BreakIndex] [int] NULL,
					[PrevBreakIndex] [int] NULL,
					[HitIndex] [int] NULL,
					[PrevHitIndex] [int] NULL,
					[LookForPeaks] [int] NOT NULL,
					[AnalysisStartPoint] [int] NOT NULL
					CONSTRAINT [PK_temp_trendlines] PRIMARY KEY CLUSTERED ([TrendlineId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
				) ON [PRIMARY]
		
				CREATE NONCLUSTERED INDEX [ixBaseDateIndex_temp_trendlines] ON #Trendlines
				([BaseDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
				CREATE NONCLUSTERED INDEX [ixCounterDateIndex_temp_trendlines] ON #Trendlines
				([CounterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixIsOpenFromLeft_temp_trendlines] ON #Trendlines
				([IsOpenFromLeft] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixBreakIndex_temp_trendlines] ON #Trendlines
				(BreakIndex ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixPrevBreakIndex_temp_trendlines] ON #Trendlines
				([PrevBreakIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixHitIndex_temp_trendlines] ON #Trendlines
				([HitIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixPrevHitIndex_temp_trendlines] ON #Trendlines
				([PrevHitIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixLookForPeaks_temp_trendlines] ON #Trendlines
				([LookForPeaks] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixAnalysisStartPoint_temp_trendlines] ON #Trendlines
				([AnalysisStartPoint] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			END

			-- Closed trendlines
			BEGIN

				CREATE TABLE #ClosedTrendlines(
					[TrendlineId] [int] NOT NULL,	
					[BaseExtremumGroupId] [int] NOT NULL,
					[BaseDateIndex] [int] NOT NULL,
					[BaseLevel] [float] NOT NULL,
					[CounterExtremumGroupId] [int] NOT NULL,
					[CounterDateIndex] [int] NOT NULL,
					[CounterLevel] [float] NOT NULL,
					[Angle] [float] NOT NULL,
					[StartDateIndex] [int] NULL,
					[EndDateIndex] [int] NULL,
					[IsOpenFromLeft] [bit] NOT NULL DEFAULT(1),
					[IsOpenFromRight] [bit] NOT NULL DEFAULT(1),
					[CandlesDistance] [int] NOT NULL,
					[BreakIndex] [int] NULL,
					[PrevBreakIndex] [int] NULL,
					[HitIndex] [int] NULL,
					[PrevHitIndex] [int] NULL,
					[LookForPeaks] [int] NOT NULL,
					[AnalysisStartPoint] [int] NOT NULL
					CONSTRAINT [PK_temp_openTrendlines] PRIMARY KEY CLUSTERED ([TrendlineId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
				) ON [PRIMARY]
		
				CREATE NONCLUSTERED INDEX [ixBaseDateIndex_temp_closedTrendlines] ON #ClosedTrendlines
				([BaseDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
				CREATE NONCLUSTERED INDEX [ixCounterDateIndex_temp_closedTrendlines] ON #ClosedTrendlines
				([CounterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixIsOpenFromLeft_temp_closedTrendlines] ON #ClosedTrendlines
				([IsOpenFromLeft] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixBreakIndex_temp_closedTrendlines] ON #ClosedTrendlines
				(BreakIndex ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixPrevBreakIndex_temp_closedTrendlines] ON #ClosedTrendlines
				([PrevBreakIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixHitIndex_temp_closedTrendlines] ON #ClosedTrendlines
				([HitIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixPrevHitIndex_temp_closedTrendlines] ON #ClosedTrendlines
				([PrevHitIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixLookForPeaks_temp_closedTrendlines] ON #ClosedTrendlines
				([LookForPeaks] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixAnalysisStartPoint_temp_closedTrendlines] ON #ClosedTrendlines
				([AnalysisStartPoint] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			END

		END

		-- Quotes
		BEGIN

			CREATE TABLE #Quotes_AssetTimeframe(
				[DateIndex] [int] NOT NULL,
				[Open] [float] NOT NULL,
				[Low] [float] NOT NULL,
				[High] [float] NOT NULL,
				[Close] [float] NOT NULL,
				CONSTRAINT [PK_temp_quotesAssetTimeframe] PRIMARY KEY CLUSTERED ([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]

			CREATE NONCLUSTERED INDEX [ixDateIndex_temp_QuotesAssetTimeframe] ON #Quotes_AssetTimeframe
			([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE TABLE #Quotes_Iteration(
				[DateIndex] [int] NOT NULL,
				[Open] [float] NOT NULL,
				[Low] [float] NOT NULL,
				[High] [float] NOT NULL,
				[Close] [float] NOT NULL,
				CONSTRAINT [PK_temp_quotesIteration] PRIMARY KEY CLUSTERED ([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]

			CREATE NONCLUSTERED INDEX [ixDateIndex_temp_QuotesIteration] ON #Quotes_Iteration
			([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		END

		-- Extremum groups
		BEGIN
			
			CREATE TABLE #ExtremumGroups(
				[ExtremumGroupId] [int] NOT NULL,
				[AssetId] [int] NOT NULL,
				[TimeframeId] [int] NOT NULL,
				[IsPeak] [int] NOT NULL,
				[MasterExtremumId] [int] NOT NULL,
				[SlaveExtremumId] [int] NOT NULL,
				[MasterDateIndex] [int] NOT NULL,
				[SlaveDateIndex] [int] NOT NULL,
				[StartDateIndex] [int] NOT NULL,
				[EndDateIndex] [int] NOT NULL,
				[OCPriceLevel] [float] NOT NULL,
				[ExtremumPriceLevel] [float] NOT NULL,
				[MiddlePriceLevel] [float] NOT NULL,
				CONSTRAINT [PK_extremaGroups] PRIMARY KEY CLUSTERED ([ExtremumGroupId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			);

		
			CREATE NONCLUSTERED INDEX [ixIsPeak_temp_extremumGroups] ON #ExtremumGroups
			([IsPeak] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixMasterExtremumId_temp_extremumGroups] ON #ExtremumGroups
			([MasterExtremumId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
			CREATE NONCLUSTERED INDEX [ixSlaveExtremumId_temp_extremumGroups] ON #ExtremumGroups
			([SlaveExtremumId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixMasterDateIndex_temp_extremumGroups] ON #ExtremumGroups
			([MasterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixSlaveDateIndex_temp_extremumGroups] ON #ExtremumGroups
			([SlaveDateIndex]  ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixStartDateIndex_temp_extremumGroups] ON #ExtremumGroups
			([StartDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixEndDateIndex_temp_extremumGroups] ON #ExtremumGroups
			([EndDateIndex]  ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


		END

	END

	-- Select initial data:
	-- * trendlines for analysis (with [IsLeftOpen] = 1).
	-- * quotes for the given AssetId and TimeframeId
	BEGIN

		INSERT INTO #Trendlines
		SELECT 
			t.[TrendlineId], t.[BaseExtremumGroupId], t.[BaseDateIndex], t.[BaseLevel], t.[CounterExtremumGroupId], t.[CounterDateIndex], 
			t.[CounterLevel], t.[Angle], t.[StartDateIndex], t.[EndDateIndex], t.[IsOpenFromLeft], t.[IsOpenFromRight], t.[CandlesDistance],
			NULL AS [BreakIndex],
			NULL AS [PrevBreakIndex],
			NULL AS [HitIndex],
			NULL AS [PrevHitIndex],
			eg.[IsPeak] AS [LookForPeaks],
			t.[CounterDateIndex] AS [AnalysisStartPoint]
		FROM 
			(SELECT * FROM [dbo].[trendlines] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) t
			LEFT JOIN (SELECT * FROM [dbo].[extremumGroups] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) eg
			ON t.[CounterExtremumGroupId] = eg.[ExtremumGroupId]
		WHERE
			t.[IsOpenFromLeft] = 1;
		
		INSERT INTO #Quotes_AssetTimeframe
		SELECT 
			[DateIndex], [Open], [Low], [High], [Close]
		FROM
			[dbo].[quotes]
		WHERE
			[AssetId] = @assetId AND 
			[TimeframeId] = @timeframeId;

	END

	-- Proper analysis process.
	BEGIN
		
		DECLARE @trendlineStartOffset AS INT = 0;
		DECLARE @maxDeviationFromTrendline AS FLOAT = 0.001;
		DECLARE @minDistanceFromExtremumToBreak AS INT = 5;
		DECLARE @maxCheckRange AS INT = 10; -- as multiplier of distance between extrema.
		DECLARE @remainingTrendlines AS INT = (SELECT COUNT(*) FROM #Trendlines);

		WHILE @remainingTrendlines > 0
		BEGIN
			
			-- [1] Find first breaks to the left of the current point.
			BEGIN

				-- [1.1] Get proper set of quotes required for analysis and insert them into Quotes_Iteration table.
				BEGIN

					-- [1.1.1] Calculate minimal and maximal required quotation.
					SELECT
						MIN(a.[startQuote]) AS [Min],
						MAX(a.[endQuote]) AS [Max]
					INTO
						#BorderPoints
					FROM
						(SELECT 
							t.[AnalysisStartPoint] - (@maxCheckRange * t.[CandlesDistance]) AS [startQuote],
							t.[AnalysisStartPoint] - 1 AS [endQuote]
						FROM 
							#Trendlines t) a;
					
					DECLARE @minQuoteIndex AS INT = (SELECT [Min] FROM #BorderPoints);
					DECLARE @maxQuoteIndex AS INT = (SELECT [Max] FROM #BorderPoints);


					-- [1.1.2] Load proper set of quotes based on [min] and [max] value obtained above.
					DELETE FROM #Quotes_Iteration;
					INSERT INTO #Quotes_Iteration
					SELECT * 
					FROM #Quotes_AssetTimeframe
					WHERE [DateIndex] BETWEEN @minQuoteIndex AND @maxQuoteIndex;


					-- [1.1.3] Drop temporary tables.
					DROP TABLE #BorderPoints;
									
				END

				-- [1.2] Create matching table between trendlines and quotations.
				BEGIN

					SELECT
						t.[TrendlineId],
						q.[DateIndex],
						q.[Close] * t.[LookForPeaks] AS [ModifiedClose],
						q.[Open] * t.[LookForPeaks] AS [ModifiedOpen],
						(t.[baseLevel] + (q.[DateIndex] - t.[BaseDateIndex]) * t.[Angle]) * t.[LookForPeaks] AS [ModifiedTrendlineLevel],
						t.[LookForPeaks]
					INTO
						#TrendlineQuotePairs
					FROM
						#Trendlines t
						LEFT JOIN #Quotes_Iteration q
						ON q.[DateIndex] BETWEEN (t.[AnalysisStartPoint] - (@maxCheckRange * t.[CandlesDistance])) AND (t.[AnalysisStartPoint] - 1);
					
				END

				-- [1.3] Filter only data with Close and Open prices above Resistance line or below Support Line.
				BEGIN

					SELECT
						t.[TrendlineId], 
						t.[DateIndex], 
						t.[LookForPeaks]
					INTO
						#FilteredTrendlineQuotePairs
					FROM
						#TrendlineQuotePairs t
					WHERE
						NOT(t.[ModifiedTrendlineLevel] > t.[ModifiedClose] AND t.[ModifiedTrendlineLevel] > t.[ModifiedOpen])
					
				END

				-- [1.4] Select the latest break for each analyzed trendline.
				BEGIN

					SELECT
						ft.[TrendlineId], 
						ft.[LookForPeaks],
						MAX(ft.[DateIndex]) + 1 AS [DateIndex]
					INTO 
						#TrendlinesFirstBreaks
					FROM
						#FilteredTrendlineQuotePairs ft
					GROUP BY
						ft.[TrendlineId], ft.[LookForPeaks];

				END


				-- [1.5] Insert information obtained above to the proper tables for the next iteration of analysis.
				BEGIN
					
					-- [Trend breaks]
					INSERT INTO #TrendBreaks([TrendlineId], [DateIndex], [BreakFromAbove])
					SELECT tfb.[TrendlineId], tfb.[DateIndex], tfb.[LookForPeaks]
					FROM #TrendlinesFirstBreaks tfb;

					-- [Trendlines]
					UPDATE t
					SET [BreakIndex] = tfb.[DateIndex]
					FROM 
						#Trendlines t
						LEFT JOIN #TrendlinesFirstBreaks tfb
						ON t.[TrendlineId] = tfb.[TrendlineId];

				END

				-- [1.6] Clean up.
				BEGIN
					DROP TABLE #TrendlineQuotePairs;
					DROP TABLE #FilteredTrendlineQuotePairs;
					DROP TABLE #TrendlinesFirstBreaks;
				END

			END

			-- [2] Find all trend hits between 
			BEGIN

				-- [2.1] Select extremum groups required for this analysis.
				BEGIN

					-- [2.1.1] Calculate minimal and maximal required quotation.
					SELECT
						MIN(a.[startDateIndex]) AS [Min],
						MAX(a.[endDateIndex]) AS [Max]
					INTO
						#ExtremumGroupsBorderPoints
					FROM
						(SELECT 
							t.[AnalysisStartPoint] - (@maxCheckRange * t.[CandlesDistance]) AS [startDateIndex],
							t.[AnalysisStartPoint] AS [endDateIndex]
						FROM 
							#Trendlines t) a;
					
					DECLARE @minDateIndex AS INT = (SELECT [Min] FROM #ExtremumGroupsBorderPoints);
					DECLARE @maxDateIndex AS INT = (SELECT [Max] FROM #ExtremumGroupsBorderPoints);


					-- [2.1.2] Load proper set of extremum group based on [min] and [max] value obtained above.
					DELETE FROM #ExtremumGroups;
					INSERT INTO #ExtremumGroups
					SELECT * 
					FROM [dbo].[extremumGroups]
					WHERE 
						[AssetId] = @assetId AND
						[TimeframeId] = @timeframeId AND
						[StartDateIndex] >= @minQuoteIndex AND 
						[EndDateIndex] <= @maxQuoteIndex;


					-- [2.1.3] Drop temporary tables.
					DROP TABLE #ExtremumGroupsBorderPoints;
					
				END
				
				-- [2.2] Find borders for matching for each separate trendline (depending on breaks found before).
				BEGIN

					SELECT
						t.*,
						COALESCE(t.[BreakIndex] + @minDistanceFromExtremumToBreak, t.[AnalysisStartPoint] - (@maxCheckRange * t.[CandlesDistance])) + 1 AS [MatchingLeftBorder],
						t.[AnalysisStartPoint] AS [MatchingRightBorder]
					INTO 
						#TrendlinesHitsSearchBorders
					FROM
						#Trendlines t;

				END

				-- [2.3] Create table with all possible matches Trendline-ExtremumGroup.
				BEGIN
					
					SELECT
						t.[TrendlineId],
						t.[LookForPeaks],
						(t.[BaseLevel] + (eg.[SlaveDateIndex] - t.[BaseDateIndex]) * t.[Angle]) AS [TrendlineLevel],
						eg.[ExtremumGroupId],
						eg.[StartDateIndex] AS [ExtremumStartIndex],
						eg.[ExtremumPriceLevel] AS [ExtremumPrice]
					INTO
						#TrendlineExtremumPossibleMatches
					FROM
						#TrendlinesHitsSearchBorders t
						LEFT JOIN #ExtremumGroups eg
						ON  eg.[IsPeak] = t.[LookForPeaks] AND 
							eg.[StartDateIndex] BETWEEN t.[MatchingLeftBorder] AND t.[MatchingRightBorder];

					SELECT 
						t.[TrendlineId],
						t.[ExtremumGroupId],
						t.[ExtremumStartIndex] AS [ExtremumStartIndex],
						t.[LookForPeaks],
						t.[ExtremumPrice] * t.[LookForPeaks] AS [ModifiedPrice],
						t.[TrendlineLevel] * t.[LookForPeaks] AS [ModifiedTrendlineLevel],
						IIF(t.[TrendlineLevel] > 0, 1, -1) AS [TrendlineAboveZero],
						(t.[TrendlineLevel] - t.[ExtremumPrice]) / t.[TrendlineLevel] AS [PriceTrendlineDistance]
					INTO
						#TrendlineMatchesWithModifiedPrices
					FROM
						#TrendlineExtremumPossibleMatches t;

				END

				-- [2.4] Filter out prices that are not close enough to matched trendline and insert rest of records into TrendHits temporary table.
				BEGIN

					INSERT INTO #TrendHits([TrendlineId], [ExtremumGroupId], [DateIndex])
					SELECT
						t.[TrendlineId],
						t.[ExtremumGroupId],
						t.[ExtremumStartIndex]
					FROM
						#TrendlineMatchesWithModifiedPrices t
					WHERE
						t.[LookForPeaks] * t.[PriceTrendlineDistance] * t.[TrendlineAboveZero]  < @maxDeviationFromTrendline;

				END

				-- [2.5] Remove duplicates from temporary #TrendHits table.
				BEGIN

					WITH CTE AS(
					   SELECT [TrendlineId], [ExtremumGroupId], RN = ROW_NUMBER()
					   OVER(PARTITION BY [TrendlineId], [ExtremumGroupId] ORDER BY [TrendlineId], [ExtremumGroupId])
					   FROM #TrendHits
					)
					DELETE FROM CTE WHERE RN > 1					

				END

				-- [2.6] Append info about trend hits found to temporary #Trendlines table.
				BEGIN

					-- [2.6.1] Create temporary table with the earliest trend hit for each trendline.
					SELECT
						th.[TrendlineId],
						MIN(th.[DateIndex]) AS [FirstHit]
					INTO 
						#EarliestTrendHits
					FROM
						#TrendHits th
					GROUP BY
						th.[TrendlineId];

					-- [2.6.2] Update TrendHit pointers.
					UPDATE t
					SET 
						[HitIndex] = h.[FirstHit]
					FROM
						#Trendlines t
						LEFT JOIN #EarliestTrendHits h
						ON t.[TrendlineId] = h.[TrendlineId]
					WHERE
						t.[IsOpenFromLeft] = 1 AND
						h.[FirstHit] <= t.[AnalysisStartPoint];

					-- [2.6.3] Clean up
					DROP TABLE #EarliestTrendHits;					

				END

				-- [2.7] Remove temporary tables.
				BEGIN
					DROP TABLE #TrendlinesHitsSearchBorders
					DROP TABLE #TrendlineExtremumPossibleMatches;
					DROP TABLE #TrendlineMatchesWithModifiedPrices;
				END

			END

			-- [3] Prepare data for next iteration based on breaks and hits found.
			BEGIN

				-- [3.1] Move all trendlines without break nor hit to ClosedTrendlines table.
				BEGIN

					-- [3.1.1] Select all trendlines without break nor hit.
					SELECT *
					INTO #TrendlinesWithoutEvent
					FROM #Trendlines t
					WHERE 
						(t.[BreakIndex] IS NULL AND t.[HitIndex] IS NULL) OR
						(t.[BreakIndex] IS NOT NULL AND t.[PrevBreakIndex] IS NOT NULL AND t.[HitIndex] IS NULL) OR
						(t.[BreakIndex] IS NOT NULL AND t.[HitIndex] IS NULL AND t.[PrevHitIndex] IS NULL);

					-- [3.1.2] Update their [StartDateIndex] property.
					UPDATE #TrendlinesWithoutEvent
					SET 
						[IsOpenFromLeft] = 0,
						[StartDateIndex] = COALESCE([PrevHitIndex], [AnalysisStartPoint]) - @trendlineStartOffset;

					-- [3.1.3] Insert those records into [ClosedTrendlines] table.
					INSERT INTO #ClosedTrendlines
					SELECT * FROM #TrendlinesWithoutEvent;

					-- [3.1.4] Remove them from table with open trendlines.
					DELETE 
					FROM #Trendlines
					WHERE 
						([BreakIndex] IS NULL AND [HitIndex] IS NULL) OR
						([BreakIndex] IS NOT NULL AND [PrevBreakIndex] IS NOT NULL AND [HitIndex] IS NULL) OR
						([BreakIndex] IS NOT NULL AND [HitIndex] IS NULL AND [PrevHitIndex] IS NULL);

					-- [3.1.5] Drop temporary table.
					DROP TABLE #TrendlinesWithoutEvent;

				END

				-- [3.2] Update status of all remaining records in #Trendlines table.
				BEGIN

					UPDATE
						#Trendlines
					SET 
						[LookForPeaks] = [LookForPeaks] * IIF([BreakIndex] IS NULL, 1, -1),
						[AnalysisStartPoint] = COALESCE(IIF([BreakIndex] IS NOT NULL, [BreakIndex] - 1, [HitIndex] - 1), 0),
						[PrevBreakIndex] = IIF([BreakIndex] IS NULL, [PrevBreakIndex], [BreakIndex]),
						[BreakIndex] = NULL,
						[PrevHitIndex] = IIF([HitIndex] IS NULL, [PrevHitIndex], [HitIndex]),
						[HitIndex] = NULL;

				END

			END


			SET @remainingTrendlines = (SELECT COUNT(*) FROM #Trendlines);

		END

	END


	-- Feed production DB tables with data obtained above.
	BEGIN
		
		-- [1] Update data about validated trendlines into production tables.
		BEGIN
			
			-- [1.0] Create temporary table with trendlines qualified for further analysis.
			BEGIN

				-- [1.0.1] Create table with data about validated trendlines.
				SELECT 
					*
				INTO
					#ValidatedTrendlines
				FROM
					#ClosedTrendlines ct
				WHERE
					ct.[StartDateIndex] <= ct.[BaseDateIndex];

				-- [1.0.2] Create table containing IDs of all trendlines without a single hit to the left side.
				SELECT 
					ct.[TrendlineId]
				INTO
					#InvalidatedTrendlines
				FROM
					#ClosedTrendlines ct
				WHERE
					ct.[StartDateIndex] > ct.[BaseDateIndex];

			END

			-- [1.1] Move data of trend hits into the production TrendHits table.
			BEGIN
				
				-- [1.1.1] Remove info about trend breaks for invalidated trendlines.
				DELETE
				FROM 
					#TrendHits
				WHERE 
					[TrendlineId] IN (SELECT * FROM #InvalidatedTrendlines);

				-- [1.1.2] Remove records with trend hits before trendline start.
				DELETE th
				FROM
					#TrendHits th
					LEFT JOIN #ClosedTrendlines ct
					ON th.[TrendlineId] = ct.[TrendlineId]
				WHERE
					th.[DateIndex] < ct.[StartDateIndex];

				-- [1.1.3] Insert records for hits at Counter Extremum Group date index.
				INSERT INTO #TrendHits([TrendlineId], [ExtremumGroupId], [DateIndex])
				SELECT 
					vt.[TrendlineId],
					vt.[CounterExtremumGroupId],
					vt.[CounterDateIndex]
				FROM 
					#ValidatedTrendlines vt;
					
				-- [1.1.4] Remove duplicates from #TrendHits table.
				WITH CTE AS(
					SELECT [TrendlineId], [ExtremumGroupId], RN = ROW_NUMBER()
					OVER(PARTITION BY [TrendlineId], [ExtremumGroupId] ORDER BY [TrendlineId], [ExtremumGroupId])
					FROM #TrendHits
				)
				DELETE FROM CTE WHERE RN > 1

				-- [1.1.5] #removed

				-- [1.1.6] Create temporary table to store IDs given by DB engine.
				CREATE TABLE #TempTrendHitsForIdentityMatching(
					[TrendHitId] [int] NOT NULL,
					[TrendlineId] [int] NOT NULL,
					[ExtremumGroupId] [int] NOT NULL
				);

				-- [1.1.7] Insert data into DB table.
				INSERT INTO [dbo].[trendHits]([TrendlineId], [ExtremumGroupId])
				OUTPUT Inserted.[TrendHitId], Inserted.[TrendlineId], Inserted.[ExtremumGroupId]
				INTO #TempTrendHitsForIdentityMatching
				SELECT [TrendlineId], [ExtremumGroupId]
				FROM #TrendHits;


				-- [1.1.8] Append IDs given by the DB engine to the records in the temporary table.
				UPDATE th
				SET 
					[ProductionId] = h.[TrendHitId]
				FROM
					#TrendHits th
					LEFT JOIN #TempTrendHitsForIdentityMatching h
					ON  th.[TrendlineId] = h.[TrendlineId] AND
						th.[ExtremumGroupId] = h.[ExtremumGroupId];

				-- [1.1.9] Drop temporary table.
				DROP TABLE #TempTrendHitsForIdentityMatching;

			END

			-- [1.2] Move data of trend breaks into the production TrendBreaks table.
			BEGIN
				
				-- [1.2.1] Remove info about trend breaks for invalidated trendlines.
				DELETE
				FROM 
					#TrendBreaks
				WHERE 
					[TrendlineId] IN (SELECT * FROM #InvalidatedTrendlines);


				-- [1.2.2] Remove records with trend breaks before trendline start.
				DELETE tb
				FROM
					#TrendBreaks tb
					LEFT JOIN #ClosedTrendlines ct
					ON tb.[TrendlineId] = ct.[TrendlineId]
				WHERE
					(tb.[DateIndex] - @minDistanceFromExtremumToBreak) <= ct.[StartDateIndex];


				-- [1.2.3] Create temporary table to store IDs given by DB engine.
				CREATE TABLE #TempTrendBreaksForIdentityMatching(
					[TrendBreakId] [int] NOT NULL,
					[TrendlineId] [int] NOT NULL,
					[DateIndex] [int] NOT NULL
				);

				-- [1.2.4] Insert remaining trendlines into TrendBreaks table.
				INSERT INTO [dbo].[TrendBreaks]([TrendlineId], [DateIndex], [BreakFromAbove])
				OUTPUT Inserted.[TrendBreakId], Inserted.[TrendlineId], Inserted.[DateIndex]
				INTO #TempTrendBreaksForIdentityMatching
				SELECT
					tb.[TrendlineId],
					tb.[DateIndex],
					tb.[BreakFromAbove]
				FROM
					#TrendBreaks tb;

				-- [1.2.5] Append IDs given by the DB engine to the records in the temporary table.
				UPDATE tb
				SET 
					[ProductionId] = b.[TrendBreakId]
				FROM
					#TrendBreaks tb
					LEFT JOIN #TempTrendBreaksForIdentityMatching b
					ON  tb.[TrendlineId] = b.[TrendlineId] AND
						tb.[DateIndex] = b.[DateIndex];

				-- [1.2.6] Drop temporary table.
				DROP TABLE #TempTrendBreaksForIdentityMatching;

			END

			-- [1.3] Create and insert records about trend ranges.
			BEGIN

				-- [1.3.1] Create temporary table containing all breaks and hits from temporary tables.
				SELECT
					*, 
					[number] = ROW_NUMBER() OVER (ORDER BY [TrendlineId], [DateIndex])
				INTO
					#CombinedBreaksAndHits
				FROM
					(SELECT [TrendlineId], [ProductionId], [DateIndex], 0 AS [IsHit]
					FROM #TrendBreaks
					UNION ALL
					SELECT [TrendlineId], [ProductionId], [DateIndex], 1 AS [IsHit]
					FROM #TrendHits) a;

				-- [1.3.2] Create trend range border pairs and insert them into #TrendRanges temporary table.
				INSERT INTO #TrendRanges([TrendlineId], [BaseId], [BaseIsHit], [BaseDateIndex], [CounterId], [CounterIsHit], [CounterDateIndex])
				SELECT 
					cb1.[TrendlineId], cb1.[ProductionId], cb1.[IsHit], cb1.[DateIndex], cb2.[ProductionId], cb2.[IsHit], cb2.[DateIndex]
				FROM 
					#CombinedBreaksAndHits cb1
					INNER JOIN #CombinedBreaksAndHits cb2
					ON  cb1.[TrendlineId] = cb2.[TrendlineId] AND
						cb1.[number] = cb2.[number] - 1;

				-- [1.3.3] Append info if the given trend range is top or bottom.
				UPDATE tr
				SET
					[IsPeak] = eg.[IsPeak]
				FROM
					#TrendRanges tr
					LEFT JOIN #TrendHits th ON (tr.[BaseIsHit] = 1 AND tr.[BaseId] = th.[ProductionId]) OR (tr.[CounterIsHit] = 1 AND tr.[CounterId] = th.[ProductionId])
					LEFT JOIN [dbo].[extremumGroups] eg ON th.[ExtremumGroupId] = eg.[ExtremumGroupId];

				-- [1.3.4] Call function evaluating trend ranges.
				BEGIN
					
					DECLARE @TrendRangeBasicData AS [dbo].[TrendRangeBasicData];
					INSERT INTO @TrendRangeBasicData
					SELECT
						tr.[TrendRangeId],
						--tr.[TrendlineId],
						t.[BaseDateIndex] AS [TrendlineStartDateIndex],
						t.[BaseLevel] AS [TrendlineStartLevel],
						t.[Angle] As [TrendlineAngle],
						IIF(tr.[BaseIsHit] = 1, eg.[EndDateIndex] + 1, tr.[BaseDateIndex]) AS [StartIndex],
						IIF(tr.[CounterIsHit] = 1, eg2.[StartDateIndex] - 1, tr.[CounterDateIndex]) AS [EndIndex],
						tr.[IsPeak]
					FROM
						#TrendRanges tr
						LEFT JOIN #TrendHits th ON tr.[BaseId] = th.[ProductionId]
						LEFT JOIN [dbo].[extremumGroups] eg ON th.[ExtremumGroupId] = eg.[ExtremumGroupId]
						LEFT JOIN #TrendHits th2 ON tr.[CounterId] = th2.[ProductionId]
						LEFT JOIN [dbo].[extremumGroups] eg2 ON th2.[ExtremumGroupId] = eg2.[ExtremumGroupId]
						LEFT JOIN [dbo].[trendlines] t ON tr.[TrendlineId] = t.[TrendlineId];


					-- [1.3.4.1] Update variation data.
					UPDATE tr
					SET
						[TotalCandles] = v.[TotalCandles],
						[AverageVariation] = v.[TotalVariation] / v.[TotalCandles],
						[ExtremumVariation] = v.[ExtremumVariation],
						[OpenCloseVariation] = v.[OCVariation]
					FROM
						#TrendRanges tr
						LEFT JOIN [dbo].[GetTrendRangesVariations](@assetId, @timeframeId, @TrendRangeBasicData) v ON tr.[TrendRangeId] = v.[TrendRangeId]

					-- [1.3.4.2] Update cross data.
					UPDATE tr
					SET
						[ExtremumPriceCrossPenaltyPoints] = v.[ExtremumPriceCrossPenaltyPoints],
						[ExtremumPriceCrossCounter] = v.[ExtremumPriceCrossCounter],
						[OCPriceCrossPenaltyPoints] = v.[OCPriceCrossPenaltyPoints],
						[OCPriceCrossCounter] = v.[OCPriceCrossCounter]
					FROM
						#TrendRanges tr
						LEFT JOIN [dbo].[GetTrendRangesCrossDetails](@assetId, @timeframeId, @TrendRangeBasicData) v ON tr.[TrendRangeId] = v.[TrendRangeId]
							
				END


				-- [1.3.x] Move ranges to the production table.
				INSERT INTO [dbo].[trendRanges]([TrendlineId], [BaseId], [BaseIsHit], [BaseDateIndex], [CounterId], [CounterIsHit], [CounterDateIndex], [IsPeak],
												[ExtremumPriceCrossPenaltyPoints], [ExtremumPriceCrossCounter], [OCPriceCrossPenaltyPoints], [OCPriceCrossCounter],
												[TotalCandles], [AverageVariation], [ExtremumVariation], [OpenCloseVariation], [BaseHitValue], [CounterHitValue])
				SELECT
					[TrendlineId], 
					[BaseId], 
					[BaseIsHit], 
					[BaseDateIndex], 
					[CounterId], 
					[CounterIsHit], 
					[CounterDateIndex], 
					[IsPeak],
					[ExtremumPriceCrossPenaltyPoints], 
					[ExtremumPriceCrossCounter], 
					[OCPriceCrossPenaltyPoints], 
					[OCPriceCrossCounter],
					[TotalCandles], 
					[AverageVariation], 
					[ExtremumVariation], 
					[OpenCloseVariation], 
					[BaseHitValue], 
					[CounterHitValue]
				FROM
					#TrendRanges;

				-- [1.3.y] Clean up
				BEGIN
					DROP TABLE #CombinedBreaksAndHits;
				END

			END

			-- [1.4] Update trendlines with any hit found.
			UPDATE t
			SET
				[IsOpenFromLeft] = 0,
				[StartDateIndex] = ct.[StartDateIndex]
			FROM
				[dbo].[trendlines] t
				LEFT JOIN (SELECT * FROM #ClosedTrendlines WHERE [StartDateIndex] <= [BaseDateIndex]) ct
				ON t.[TrendlineId] = ct.[TrendlineId]
			WHERE
				ct.[TrendlineId] IS NOT NULL;
			
		END

		-- [2] Remove trendlines without a single hit.
		BEGIN

			-- [2.1] Remove trendlines with IDs listed above from production table.
			BEGIN
				
				DELETE t
				FROM
					[dbo].[trendlines] t
					LEFT JOIN #InvalidatedTrendlines it
					ON t.[TrendlineId] = it.[TrendlineId]
				WHERE
					it.[TrendlineId] IS NOT NULL;

			END

		END

		-- [3] Clean up
		BEGIN
			DROP TABLE #InvalidatedTrendlines;
			DROP TABLE #ValidatedTrendlines;
		END

	
	END


	-- Drop temporary tables.
	BEGIN

		DROP TABLE #TrendBreaks;
		DROP TABLE #TrendHits;
		DROP TABLE #TrendRanges;
		DROP TABLE #Trendlines;
		DROP TABLE #ClosedTrendlines;
		DROP TABLE #Quotes_AssetTimeframe;
		DROP TABLE #Quotes_Iteration;
		DROP TABLE #ExtremumGroups;

	END

END
GO

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


IF OBJECT_ID('evaluateTrendBreaks','P') IS NOT NULL DROP PROC [dbo].[evaluateTrendBreaks];
GO

CREATE PROC [dbo].[evaluateTrendBreaks] @assetId AS INT, @timeframeId AS INT
AS
BEGIN
	

	DECLARE @checkDistance AS INT = 5;


	-- [1] Create temporary tables.
	BEGIN

		-- [1.1] Select necessary quotes.
		SELECT
			*
		INTO 
			#Quotes
		FROM
			[dbo].[quotes] 
		WHERE
			[AssetId] = @assetId AND
			[TimeframeId] = @timeframeId

		-- [1.2] Select necessary trendlines.
		SELECT
			*
		INTO
			#Trendlines
		FROM
			[dbo].[trendlines]
		WHERE
			[AssetId] = @assetId AND
			[TimeframeId] = @timeframeId AND
			[IsOpenFromRight] = 1;
	
		-- [1.3] Select necessary trend breaks.
		SELECT
			tb.*
		INTO
			#TrendBreaks
		FROM
			[dbo].[trendBreaks] tb
			INNER JOIN #Trendlines t ON tb.[TrendlineId] = t.[TrendlineId];

	END


	-- [2] Actual calculation.
	BEGIN


		-- [2.1] Create table with temp data.
		BEGIN
			SELECT 
				 tb.[TrendBreakId]
				,tb.[TrendlineId]
				,tb.[DateIndex] AS [BreakDateIndex]
				,tb.[BreakFromAbove]
				,tl.[BaseDateIndex]
				,tl.[BaseLevel]
				,tl.[CounterDateIndex]
				,tl.[CounterLevel]
				,tl.[Angle]
				,tl.[StartDateIndex]
				,tl.[EndDateIndex]
				,q.[DateIndex]
				,q.[Open]
				,q.[Low]
				,q.[High]
				,q.[Close]
				,(q.[DateIndex] - tl.[BaseDateIndex]) * tl.[Angle] + tl.[BaseLevel] AS [TrendlineLevel]
			INTO
				#TempData
			FROM 
				#TrendBreaks tb
				LEFT JOIN #Trendlines tl ON tb.[TrendlineId] = tl.[TrendlineId]
				LEFT JOIN #Quotes q ON q.[DateIndex] BETWEEN tb.[DateIndex] - @checkDistance AND tb.[DateIndex] + @checkDistance
		END -- [2.1]



		-- [2.2] Max i min odchylenie od linii trendu oraz delta OC dla dokładnego notowania
		BEGIN

			SELECT
				a.[TrendBreakId],
				(a.[DailyAmplitude] * 2 + a.[OpenDiff] + a.[CloseDiff] + a.[MinExtremumDiff] + a.[MaxExtremumDiff]) / a.[TrendlineLevel] AS [Points]
			INTO
				#BreakDayAmplitude
			FROM
				(SELECT
					td.[TrendBreakId],
					(td.[TrendlineLevel] - td.[High]) * td.[BreakFromAbove] AS [MaxExtremumDiff],
					(td.[TrendlineLevel] - td.[Low]) * td.[BreakFromAbove] AS [MinExtremumDiff],
					(td.[TrendlineLevel] - td.[Open]) * td.[BreakFromAbove] AS [OpenDiff],
					(td.[TrendlineLevel] - td.[Close]) * td.[BreakFromAbove] AS [CloseDiff],
					(td.[Open] - td.[Close]) * td.[BreakFromAbove] AS [DailyAmplitude],
					td.[TrendlineLevel]
				FROM
					#TempData td
				WHERE
					td.[BreakDateIndex] = td.[DateIndex]) a

		END -- [2.2]



		-- [2.3] Różnica pomiędzy notowaniem z dnia breaku a notowaniem poprzedzającym
		BEGIN

			SELECT
				td.[TrendBreakId],
				td.[TrendlineId],
				td.[BreakDateIndex] - td.[DateIndex] AS [DaysDiff],
				((td.[Open] - td.[TrendlineLevel]) * td.[BreakFromAbove]) / td.[TrendlineLevel] AS [FarOcDiff],
				((IIF(td.[BreakFromAbove] = 1, td.[Low], td.[High]) - td.[TrendlineLevel]) * td.[BreakFromAbove]) / td.[TrendlineLevel] AS [ExtremumDiff],
				((td.[Open] - td.[Close]) * td.[BreakFromAbove]) / td.[TrendlineLevel] AS [DaysAmplitude]
			INTO
				#PrevQuotationTempData
			FROM
				(SELECT * FROM #TempData WHERE ([BreakDateIndex] - [DateIndex]) BETWEEN 1 AND 3) td
				
			SELECT
				a.[TrendBreakId],
				SUM((a.[DaysAmplitude] * 3 + a.[FarOcDiff] * 2 + a.[ExtremumDiff]) * (1 / a.[DaysDiff])) AS [Points]
			INTO
				#PreviousDayPoints
			FROM
				#PrevQuotationTempData a
			GROUP BY
				a.[TrendBreakId];

		END -- [2.3]



		-- [2.4] Zestawienie największych odchyleń od linii trendu w ciągu 5 dni od przebicia
		BEGIN
			
			SELECT
				td.[TrendBreakId],
				td.[TrendlineId],
				td.[DateIndex] - td.[BreakDateIndex] AS [DaysDiff],
				(td.[TrendlineLevel] - (IIF(td.[BreakFromAbove] = 1, td.[Low], td.[High]))) * td.[BreakFromAbove] / td.[TrendlineLevel] AS [ExtremumDiff]
			INTO
				#NextQuotationTempDataForMaxAnalysis
			FROM
				(SELECT * FROM #TempData WHERE ([DateIndex] - [BreakDateIndex]) BETWEEN 1 AND 5) td


			SELECT
				a.[TrendBreakId],
				SUM(a.[ExtremumDiff] * (1 / a.[DaysDiff])) AS [Points]
			INTO
				#NextDaysMaxVariancePoints
			FROM 
				#NextQuotationTempDataForMaxAnalysis a
			GROUP BY 
				a.[TrendBreakId];

		END -- [2.4]



		-- [2.5] Zestawienie minimalnych odległości od linii trendu w ciągu 5 dni od przebicia
		BEGIN
		
			SELECT
				td.[TrendBreakId],
				td.[TrendlineId],
				td.[DateIndex] - td.[BreakDateIndex] AS [DaysDiff],
				(td.[TrendlineLevel] - (IIF(td.[BreakFromAbove] = 1, td.[High], td.[Low]))) * td.[BreakFromAbove] / td.[TrendlineLevel] AS [ExtremumDiff]
			INTO
				#NextQuotationTempDataForMinAnalysis
			FROM
				(SELECT * FROM #TempData WHERE ([DateIndex] - [BreakDateIndex]) BETWEEN 1 AND 5) td


			SELECT
				a.[TrendBreakId],
				SUM(a.[ExtremumDiff] * (1 / a.[DaysDiff])) AS [Points]
			INTO
				#NextDaysMinDistancePoints
			FROM 
				#NextQuotationTempDataForMinAnalysis a
			GROUP BY 
				a.[TrendBreakId];

		END -- [2.5] 



		-- [2.6] Putting all values together into single value.
		BEGIN
			
			SELECT
				tb.[TrendBreakId],
				(bda.[Points] + pdp.[Points] + minPoints.[Points] + maxPoints.[Points]) * 100 AS [Points]
			INTO
				#EvaluationTable
			FROM
				#TrendBreaks tb
				LEFT JOIN #BreakDayAmplitude bda ON tb.[TrendBreakId] = bda.[TrendBreakId]
				LEFT JOIN #PreviousDayPoints pdp ON tb.[TrendBreakId] = pdp.[TrendBreakId]
				LEFT JOIN #NextDaysMaxVariancePoints maxPoints ON tb.[TrendBreakId] = maxPoints.[TrendBreakId]
				LEFT JOIN #NextDaysMinDistancePoints minPoints ON tb.[TrendBreakId] = minPoints.[TrendBreakId];

		END -- [2.6]


		-- [2.7] Update production table.
		BEGIN
			
			UPDATE tb
			SET 
				tb.[Value] = et.[Points]
			FROM
				[dbo].[trendBreaks] tb
				LEFT JOIN #EvaluationTable et ON tb.[TrendBreakId] = et.[TrendBreakId]

		END -- [2.7]

	END


	-- [X] Clean up.
	BEGIN
		DROP TABLE #Quotes;
		DROP TABLE #Trendlines;
		DROP TABLE #TrendBreaks;
		DROP TABLE #TempData;
		DROP TABLE #PrevQuotationTempData;
		DROP TABLE #NextQuotationTempDataForMinAnalysis;
		DROP TABLE #NextQuotationTempDataForMaxAnalysis;
		DROP TABLE #BreakDayAmplitude;
		DROP TABLE #PreviousDayPoints
		DROP TABLE #NextDaysMaxVariancePoints;
		DROP TABLE #NextDaysMinDistancePoints;
		DROP TABLE #EvaluationTable;
	END


END

GO

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


IF OBJECT_ID('evaluateTrendRanges','P') IS NOT NULL DROP PROC [dbo].[evaluateTrendRanges];
GO

CREATE PROC [dbo].[evaluateTrendRanges] @assetId AS INT, @timeframeId AS INT
AS
BEGIN
	

	DECLARE @checkDistance AS INT = 5;


	-- [1] Create temporary tables.
	BEGIN

		-- [1.1] Select necessary trendlines.
		SELECT
			*
		INTO
			#Trendlines
		FROM
			[dbo].[trendlines]
		WHERE
			[AssetId] = @assetId AND
			[TimeframeId] = @timeframeId;
	
		-- [1.2] Select necessary trend breaks.
		SELECT
			tr.*
		INTO
			#TrendRanges
		FROM
			[dbo].[trendRanges] tr
			INNER JOIN #Trendlines t ON tr.[TrendlineId] = t.[TrendlineId];

		-- [1.3] Select necessary trend hits.
		SELECT
			th.*
		INTO
			#TrendHits
		FROM
			[dbo].[trendHits] th
			INNER JOIN #Trendlines t ON th.[TrendlineId] = t.[TrendlineId];

		-- [1.4] Select necessary trend breaks.
		SELECT
			tb.*
		INTO
			#TrendBreaks
		FROM
			[dbo].[trendBreaks] tb
			INNER JOIN #Trendlines t ON tb.[TrendlineId] = t.[TrendlineId];

	END


	-- [2] Actual calculation.
	BEGIN


		-- [2.1] Update trend breaks & trend hits points
		BEGIN
			
			UPDATE
				tr
			SET
				[BaseHitValue] = COALESCE(th1.[Value], tb1.[Value]),
				[CounterHitValue] = COALESCE(th2.[Value], tb2.[Value])
			FROM
				#TrendRanges tr
				LEFT JOIN #TrendHits th1 ON tr.[BaseIsHit] = 1 AND tr.[BaseId] = th1.[TrendHitId]
				LEFT JOIN #TrendHits th2 ON tr.[CounterIsHit] = 1 AND tr.[CounterId] = th2.[TrendHitId]
				LEFT JOIN #TrendBreaks tb1 ON tr.[BaseIsHit] = 0 AND tr.[BaseId] = tb1.[TrendBreakId]
				LEFT JOIN #TrendBreaks tb2 ON tr.[CounterIsHit] = 0 AND tr.[CounterId] = tb2.[TrendBreakId];

		END -- [2.1]

	END


	-- [3] Update production table.
	BEGIN
			
		UPDATE tr
		SET 
			tr.[BaseHitValue] = a.[BaseHitValue],
			tr.[CounterHitValue] = a.[CounterHitValue]
		FROM
			[dbo].[trendRanges] tr
			LEFT JOIN #TrendRanges a ON tr.[TrendRangeId] = a.[TrendRangeId]

	END -- [3]


	-- [4] Remove trend ranges with missing base or counter item.
	BEGIN
		
		DELETE 
		FROM
			[dbo].[trendRanges]
		WHERE
			[TrendRangeId] IN (SELECT [TrendRangeId] FROM #TrendRanges WHERE [BaseHitValue] IS NULL OR [CounterHitValue] IS NULL);

	END -- [4]


	-- [5] Calculate total value for trend ranges.
	BEGIN
		
		SELECT
			c.[TrendRangeId],
			(c.[DistancePoints] * [ExPriceCrossPoints] * [OCPriceCrossPoints] * c.[BaseHitPoints] * c.[CounterHitPoints] * 
			[AverageVariationValue] * [ExtremumVariationValue] * [OpenCloseVariationValue]) / 100000000000000 AS [TotalPoints]
		INTO
			#TrendRangePoints
		FROM
			(SELECT
				b.*,
				[dbo].[MinValue]([dbo].[MaxValue](100 - ABS(100 - b.[TotalCandles]), 0), 100) AS [DistancePoints],
				[dbo].[MinValue]([dbo].[MaxValue](100 - 20 * COALESCE(b.[ExtremumPriceCrossPenaltyPoints], 0), 0), 100) AS [ExPriceCrossPoints],
				[dbo].[MinValue]([dbo].[MaxValue](100 - 20 * COALESCE(b.[OCPriceCrossPenaltyPoints], 0), 0), 100) AS [OCPriceCrossPoints],
				[dbo].[MinValue]([dbo].[MaxValue](b.[BaseHitValue], 0), 100) AS [BaseHitPoints],
				[dbo].[MinValue]([dbo].[MaxValue](b.[CounterHitValue], 0), 100) AS [CounterHitPoints],
				[dbo].[MinValue]([dbo].[MaxValue](10000 *  b.[RelativeAverageVariation] / b.[HoursDiffModified], 0), 100) AS [AverageVariationValue],
				[dbo].[MinValue]([dbo].[MaxValue](5000 * b.[RelativeExtremumVariation] / b.[HoursDiffModified], 0), 100) AS [ExtremumVariationValue],
				[dbo].[MinValue]([dbo].[MaxValue](5000 * b.[RelativeOpenCloseVariation] / b.[HoursDiffModified], 0), 100) AS [OpenCloseVariationValue]
			FROM
				(SELECT	
					a.*,
					[dbo].[MaxValue](50, POWER(CAST(a.[HoursDiff] AS FLOAT), (1 - LOG(IIF(a.[HoursDiff] <= 20, 21, a.[HoursDiff]) / 20, 2) / 50))) AS [HoursDiffModified],
					100 * a.[ExtremumVariation] / a.[MidLevel] AS [RelativeExtremumVariation],
					100 * a.[OpenCloseVariation] / a.[MidLevel] AS [RelativeOpenCloseVariation],
					100 * a.[AverageVariation] / a.[MidLevel] AS [RelativeAverageVariation]
				FROM
					(SELECT
						tr.*,
						((CAST(tr.[BaseDateIndex] AS DECIMAL) + (CAST(tr.[CounterDateIndex] AS DECIMAL) - CAST(tr.[BaseDateIndex] AS DECIMAL)) / 2.0) - t.[BaseDateIndex]) * t.[Angle] + t.[BaseLevel] AS [MidLevel],
						d1.[Date] AS [BaseDate],
						d2.[Date] AS [CounterDate],
						DATEDIFF(HOUR, d1.[Date], d2.[Date]) AS [HoursDiff]
					FROM
						[dbo].[trendRanges] tr
						LEFT JOIN [dbo].[trendlines] t ON tr.[TrendlineId] = t.[TrendlineId]
						LEFT JOIN [dbo].[dates] d1 ON t.[TimeframeId] = d1.[TimeframeId] AND d1.[DateIndex] = tr.[BaseDateIndex]
						LEFT JOIN [dbo].[dates] d2 ON t.[TimeframeId] = d2.[TimeframeId] AND d2.[DateIndex] = tr.[CounterDateIndex]) a
				) b
			) c
			

	END -- [5]


	-- [6] Update production table with evaluation.
	BEGIN
			
		UPDATE tr
		SET 
			tr.[Value] = trp.[TotalPoints]
		FROM
			[dbo].[trendRanges] tr
			LEFT JOIN #TrendRangePoints trp ON tr.[TrendRangeId] = trp.[TrendRangeId]

	END -- [6]


	-- [X] Clean up.
	BEGIN
		DROP TABLE #Trendlines;
		DROP TABLE #TrendRanges;
		DROP TABLE #TrendBreaks;
		DROP TABLE #TrendHits;
		DROP TABLE #TrendRangePoints;
	END


END

GO

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



CREATE PROC [dbo].[updateTrendEvaluations] @assetId AS INT, @timeframeId AS INT
AS
BEGIN

	DECLARE @lastAnalyzedIndex AS INT = [dbo].[GetTrendlinesAnalysisLastQuotationIndex](@assetId, @timeframeId);
	DECLARE @extremumCheckDistance AS INT = [dbo].[GetExtremumCheckDistance]();
	DECLARE @maxDeviationFromTrendline AS FLOAT = 0.001;
	DECLARE @trendHitEvaluation_DistanceShare AS FLOAT = 0.4;

	-- [1] Update TrendHits table.
	BEGIN
		
		-- [1.1] Select trend hits to be updated.
		SELECT
			th.[TrendHitId],
			th.[TrendlineId],
			eg.[ExtremumGroupId],
			e.[ExtremumId],
			e.[DateIndex] AS [ExtremumDateIndex],
			IIF(e.[DateIndex] = eg.[MasterDateIndex], 1, 0) AS [IsMasterExtremum],
			e.[Value] AS [ExtremumValue],
			t.[Angle] * (e.[DateIndex] - t.[BaseDateIndex]) + t.[BaseLevel] AS [TrendlineLevel],
			IIF(eg.[IsPeak] = -1, q.[Low], q.[High]) AS [ExtremumPriceLevel],
			IIF(eg.[IsPeak] = IIF(q.[Close] > q.[Open], 1, 0), q.[Close], q.[Open]) AS [OCPriceLevel]
		INTO
			#FilteredTrendHits
		FROM
			[dbo].[trendHits] th
			LEFT JOIN [dbo].[trendlines] t ON th.[TrendlineId] = t.[TrendlineId]
			LEFT JOIN (SELECT * FROM [dbo].[extremumGroups] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) eg ON th.[ExtremumGroupId] = eg.[ExtremumGroupId]
			LEFT JOIN (SELECT * FROM [dbo].[extrema] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) e ON (eg.[MasterExtremumId] = e.[ExtremumId] AND e.[ExtremumTypeId] IN (1, 3)) OR (eg.[SlaveExtremumId] = e.[ExtremumId] AND e.[ExtremumTypeId] IN (2, 4))
			LEFT JOIN (SELECT * FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) q ON e.[DateIndex] = q.[DateIndex]
		WHERE 
			t.[AssetId] = @assetId AND 
			t.[TimeframeId] = @timeframeId AND 
			eg.[EndDateIndex] >= (@lastAnalyzedIndex - @extremumCheckDistance);

		-- [1.2] Create table with trend hit values.
		SELECT 
			  c.[TrendHitId]
			, c.[PointsForDistance] + c.[PointsForValue] AS [TrendHitValue]
			, [number] = ROW_NUMBER() OVER(PARTITION BY c.[TrendHitId] ORDER BY c.[TrendHitId] ASC, (c.[PointsForDistance] + c.[PointsForValue]) DESC)
		INTO 
			#TrendHitsValues
		FROM
			(SELECT
				b.*,
				b.[Gap] / b.[PriceLevel] AS [RelativeGap],
				(@maxDeviationFromTrendline - (b.[Gap] / b.[PriceLevel])) * 100000 * @trendHitEvaluation_DistanceShare AS [PointsForDistance],
				(1.0 - @trendHitEvaluation_DistanceShare) * b.[ExtremumValue] AS [PointsForValue]
			FROM
				(SELECT
					a.*,
					IIF([IsMasterExtremum] = 1, ABS(a.[TrendlineLevel] - a.[ExtremumPriceLevel]), ABS(a.[TrendlineLevel] - a.[OCPriceLevel])) AS [Gap],
					IIF([IsMasterExtremum] = 1, a.[ExtremumPriceLevel], a.[OCPriceLevel]) AS [PriceLevel]
				FROM #FilteredTrendHits a
				) b
			) c

		-- [1.3] Update destination table with values.
		UPDATE th
		SET th.[Value] = thv.[TrendHitValue]
		FROM
			[dbo].[trendHits] th
			LEFT JOIN (SELECT * FROM #TrendHitsValues WHERE [number] = 1) thv ON th.[TrendHitId] = thv.[TrendHitId]

	END



	-- [2] Update TrendBreaks table.
	BEGIN

		EXEC [dbo].[evaluateTrendBreaks] @assetId = @assetId, @timeframeId = @timeframeId;

	END


	-- [3] Calculate TrendRanges points.
	BEGIN
		EXEC [dbo].[evaluateTrendRanges] @assetId = @assetId, @timeframeId = @timeframeId;
	END

	-- [X] Clean up.
	BEGIN
		DROP TABLE #FilteredTrendHits;
		DROP TABLE #TrendHitsValues;
	END


END


GO





CREATE PROC [dbo].[processTrendlines] @assetId AS INT, @timeframeId AS INT
AS
BEGIN
	
	EXEC [dbo].[findNewTrendlines] @assetId = @assetId, @timeframeId = @timeframeId;
	EXEC [dbo].[analyzeTrendlinesLeftSide] @assetId = @assetId, @timeframeId = @timeframeId;
	EXEC [dbo].[analyzeTrendlinesRightSide] @assetId = @assetId, @timeframeId = @timeframeId;
	EXEC [dbo].[updateTrendEvaluations] @assetId = @assetId, @timeframeId = @timeframeId;



	--Update timestamp.
	BEGIN

		DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);
		DECLARE @lastExtremumGroup AS INT = (SELECT MAX([ExtremumGroupId]) FROM [dbo].[extremumGroups] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId);

		UPDATE [dbo].[timestamps] 
		SET 
			[TrendlinesAnalysisLastQuotationIndex] = @lastQuote,
			[TrendlinesAnalysisLastExtremumGroupId] = @lastExtremumGroup
		WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId
		IF @@ROWCOUNT=0
			INSERT INTO [dbo].[timestamps]([AssetId], [TimeframeId], [TrendlinesAnalysisLastQuotationIndex]) 
			VALUES (@assetId, @timeframeId, @lastQuote);
		
	END

END

GO

--exec [dbo].[processTrendlines] @assetId  = 1, @timeframeId = 4

commit transaction