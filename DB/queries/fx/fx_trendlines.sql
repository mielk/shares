USE [fx];

BEGIN TRANSACTION;

GO

--Drop procedures
IF OBJECT_ID('findNewTrendlines','P') IS NOT NULL DROP PROC [dbo].[findNewTrendlines];
IF OBJECT_ID('processTrendlines','P') IS NOT NULL DROP PROC [dbo].[processTrendlines];
IF OBJECT_ID('analyzeTrendlinesLeftSide','P') IS NOT NULL DROP PROC [dbo].[analyzeTrendlinesLeftSide];

GO

--Drop functions
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendlinesAnalysisLastQuotationIndex]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendlinesAnalysisLastQuotationIndex]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendlinesAnalysisLastExtremumIndex]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendlinesAnalysisLastExtremumIndex]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendlinesAnalysisLastExtremumGroupId]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendlinesAnalysisLastExtremumGroupId]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStepPrecision]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetStepPrecision]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAssetCalculatingTrendlineStepFactor]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetAssetCalculatingTrendlineStepFactor]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendlineExtremaPairingPriceLevels]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendlineExtremaPairingPriceLevels]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTrendlineCheckDistance]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetTrendlineCheckDistance]


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
			LEFT JOIN #PriceLevelForExtremumGroups pl2 ON eg.[CounterExtremumGroupId] = pl2.[ExtremumGroupId];


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
				[BaseDateIndex] [int] NOT NULL,
				[BaseIsHit] [int] NOT NULL,
				[CounterDateIndex] [int] NOT NULL,
				[CounterIsHit] [int] NOT NULL,
				[IsPeak] [int] NOT NULL DEFAULT(0),
				CONSTRAINT [PK_temp_trendRanges] PRIMARY KEY CLUSTERED ([TrendRangeId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY];

			CREATE NONCLUSTERED INDEX [ixTrendlineId_temp_trendRanges] ON #TrendRanges
			([TrendlineId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixBaseDateIndex_temp_trendRanges] ON #TrendRanges
			([BaseDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

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

			-- Open trendlines
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

			CREATE TABLE #Quotes_Iteration(
				[DateIndex] [int] NOT NULL,
				[Open] [float] NOT NULL,
				[Low] [float] NOT NULL,
				[High] [float] NOT NULL,
				[Close] [float] NOT NULL,
				CONSTRAINT [PK_temp_quotesIteration] PRIMARY KEY CLUSTERED ([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]

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
		DECLARE @maxDeviationFromTrendline AS FLOAT = 0.008;
		DECLARE @minDistanceFromExtremumToBreak AS INT = 5;
		DECLARE @maxCheckRange AS INT = 10; -- as multiplier of distance between extrema.
		DECLARE @remainingTrendlines AS BIT = (SELECT COUNT(*) FROM #Trendlines);

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


					-- [1.1.2] Load propert set of quotes based on [min] and [max] value obtained above.
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
						t.[ModifiedTrendlineLevel] < t.[ModifiedClose] AND t.[ModifiedTrendlineLevel] < t.[ModifiedOpen]					
					
				END

				-- [1.4] Select the latest break for each analyzed trendline.
				BEGIN

					SELECT
						ft.[TrendlineId], 
						ft.[LookForPeaks],
						MAX(ft.[DateIndex]) AS [DateIndex]
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


					-- [2.1.2] Load propert set of extremum group based on [min] and [max] value obtained above.
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
					   SELECT [TrendlineId], [ExtremumGroupId], [DateIndex], RN = ROW_NUMBER()
					   OVER(PARTITION BY [TrendlineId], [ExtremumGroupId], [DateIndex] ORDER BY [TrendlineId], [ExtremumGroupId], [DateIndex])
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
		
		
			--SELECT 'ClosedTrendlines', * FROM #ClosedTrendlines;
			--SELECT 'TrendlinesAfter', * FROM #Trendlines;
			--SELECT 'TrendBreaks', * FROM #TrendBreaks;
			--SELECT 'TrendHits', * FROM #TrendHits;



		-- [1] Update data about validated trendlines into production tables.
		BEGIN

			-- [1.1] Move data of trend hits into the production TrendHits table.
			INSERT INTO [dbo].[trendHits]([TrendlineId], [ExtremumGroupId])
			SELECT [TrendlineId], [ExtremumGroupId]
			FROM #TrendHits;

			-- [1.2] Move data of trend breaks into the production TrendBreaks table.
			INSERT INTO [dbo].[TrendBreaks]([TrendlineId], [DateIndex], [BreakFromAbove])
			SELECT
				tb.[TrendlineId],
				tb.[DateIndex],
				tb.[BreakFromAbove]
			FROM
				#TrendBreaks tb
				LEFT JOIN #ClosedTrendlines ct
				ON tb.[TrendlineId] = ct.[TrendlineId]
			WHERE
				tb.[DateIndex] > ct.[StartDateIndex];
				

			-- [1.3] Update trendlines with any hit found.
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

			-- [2.1] Get table containing IDs of all trendlines without a single hit to the left side.
			SELECT 
				ct.[TrendlineId]
			INTO
				#InvalidatedTrendlines
			FROM
				#ClosedTrendlines ct
			WHERE
				ct.[StartDateIndex] > ct.[BaseDateIndex];

			-- [2.2] Remove trendlines with IDs listed above from production table.
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



CREATE PROC [dbo].[processTrendlines] @assetId AS INT, @timeframeId AS INT
AS
BEGIN
	
	EXEC [dbo].[findNewTrendlines] @assetId = @assetId, @timeframeId = @timeframeId;
	EXEC [dbo].[analyzeTrendlinesLeftSide] @assetId = @assetId, @timeframeId = @timeframeId;

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