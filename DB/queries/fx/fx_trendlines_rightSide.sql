USE [fx];

BEGIN TRANSACTION

-- USER DEFINED DATA TYPES ----------------------------------------------------------------------------------

-- Commented to avoid deadlock --

--GO

-------------------------------------------------------------------------------------------------------------



-------------------------------------------------------------------------------------------------------------

IF OBJECT_ID('analyzeTrendlinesRightSide','P') IS NOT NULL DROP PROC [dbo].[analyzeTrendlinesRightSide];

GO


CREATE PROC [dbo].[analyzeTrendlinesRightSide] @assetId AS INT, @timeframeId AS INT
AS
BEGIN


	-- [1] Preparing temporary tables.
	BEGIN
		
		-- [1.1] Trend breaks
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

			--CREATE UNIQUE NONCLUSTERED INDEX [ixTrendlineIdDateIndex_temp_trendlinesBreaks] ON #TrendBreaks
			--([TrendlineId] ASC, [DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


		END

		-- [1.2] Trend hits
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

			CREATE UNIQUE NONCLUSTERED INDEX [ixTrendlineIdDateIndex_temp_trendlinesHits] ON #TrendHits
			([TrendlineId] ASC, [DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


		END

		-- [1.3] Trend ranges
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

			CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_temp_trendlinesHits] ON #TrendRanges
			([TrendlineId] ASC, [BaseDateIndex] ASC, [CounterDateIndex] ASC, [IsPeak] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


		END

		-- [1.4] Trendlines
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

		-- [1.5] Quotes
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

		-- [1.6] Extremum groups
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


	-- [2] Select initial data:
	BEGIN

		-- [2.1] Select trendlines open for right-side analysis.
		BEGIN

			-- [2.1.1] Filter out irrelevant trendlines.
			SELECT
				*
			INTO
				#FilteredTrendlines
			FROM 
				[dbo].[trendlines] 
			WHERE 
				[AssetId] = @assetId 
				AND [TimeframeId] = @timeframeId
				AND [IsOpenFromRight] = 1;

			-- [2.1.2] Filter out irrelevant extremum groups.
			SELECT
				*
			INTO
				#FilteredExtremumGroups
			FROM 
				[dbo].[extremumGroups] 
			WHERE 
				[AssetId] = @assetId AND [TimeframeId] = @timeframeId;

			-- [2.1.3] For each filtered trendline get its last trend-ranges.
			SELECT
				*
			INTO
				#FilteredTrendlinesLastRanges
			FROM
				(SELECT
					ft.[TrendlineId],
					tr.[BaseIsHit],
					tr.[BaseDateIndex],
					tr.[CounterIsHit],
					tr.[CounterDateIndex],
					tr.[IsPeak],
					number = ROW_NUMBER() OVER(PARTITION BY tr.[TrendlineId] ORDER BY tr.[TrendlineId] ASC, tr.[CounterDateIndex] DESC)
				FROM
					#FilteredTrendlines ft 
					LEFT JOIN [dbo].[trendRanges] tr ON ft.[TrendlineId] = tr.[TrendlineId]) a 
			WHERE
				a.[number] = 1;

			-- [2.1.4] Append pointers for analysis and insert data into table with Trendlines for analysis.
			INSERT INTO #Trendlines([TrendlineId], [BaseExtremumGroupId], [BaseDateIndex], [BaseLevel], [CounterExtremumGroupId], [CounterDateIndex], [CounterLevel], [Angle], [StartDateIndex], [EndDateIndex], 
									[IsOpenFromLeft], [IsOpenFromRight], [CandlesDistance], [BreakIndex], [PrevBreakIndex], [HitIndex], [PrevHitIndex], [LookForPeaks], [AnalysisStartPoint])
			SELECT
				ft.[TrendlineId],
				ft.[BaseExtremumGroupId],
				ft.[BaseDateIndex], 
				ft.[BaseLevel], 
				ft.[CounterExtremumGroupId], 
				ft.[CounterDateIndex], 
				ft.[CounterLevel], 
				ft.[Angle], 
				ft.[StartDateIndex], 
				ft.[EndDateIndex], 
				ft.[IsOpenFromLeft], 
				ft.[IsOpenFromRight], 
				ft.[CandlesDistance],
				NULL AS [BreakIndex],
				IIF(ftlr.[CounterIsHit] = 1, NULL, ftlr.[CounterDateIndex]) AS [PrevBreakIndex],
				NULL AS [HitIndex],
				IIF(ftlr.[CounterIsHit] = 1, ftlr.[CounterDateIndex], ftlr.[BaseDateIndex]) AS [PrevHitIndex],
				IIF(ftlr.[CounterIsHit] = 1, ftlr.[IsPeak], ftlr.[IsPeak] * (-1)) AS [LookForPeaks],
				ftlr.[CounterDateIndex] + 1 AS [AnalysisStartPoint]
			FROM
				#FilteredTrendlines ft
				LEFT JOIN #FilteredTrendlinesLastRanges ftlr ON ft.[TrendlineId] = ftlr.[TrendlineId];

			-- [2.1.5] Drop temporary tables.
			BEGIN
				DROP TABLE #FilteredTrendlines;
				DROP TABLE #FilteredExtremumGroups;
				DROP TABLE #FilteredTrendlinesLastRanges;
			END

		END
		
		-- [2.2] Select quotes.
		BEGIN

			INSERT INTO #Quotes_AssetTimeframe
			SELECT 
				[DateIndex], [Open], [Low], [High], [Close]
			FROM
				[dbo].[quotes]
			WHERE
				[AssetId] = @assetId AND 
				[TimeframeId] = @timeframeId AND 
				[DateIndex] >= (SELECT MIN([CounterDateIndex]) FROM #Trendlines);

		END


--SELECT 'DEBUG', '#Trendlines [2.2]' AS [2.2. #Trendlines], * FROM #Trendlines;


	END


	-- [3] Proper analysis.
	BEGIN


		DECLARE @trendlineStartOffset AS INT = 0;
		DECLARE @maxDeviationFromTrendline AS FLOAT = 0.001;
		DECLARE @minDistanceFromExtremumToBreak AS INT = 3;
		DECLARE @maxCheckRange AS INT = 10; -- as multiplier of distance between extrema.
		DECLARE @remainingTrendlines AS INT = (SELECT COUNT(*) FROM #Trendlines);
		DECLARE @maxQuoteIndex AS INT = (SELECT MAX([DateIndex]) FROM #Quotes_AssetTimeframe);
		
PRINT 'Max quote index:  ' + CAST(@maxQuoteIndex AS NVARCHAR(255));
--SELECT 'DEBUG', 'Pointer', @maxQuoteIndex;

		WHILE @remainingTrendlines > 0
		BEGIN
			
			-- [3.1] Find first breaks to the right of the current point.
			BEGIN
			
				
				-- [3.1.1] Get proper set of quotes required for analysis and insert them into Quotes_Iteration table.
				BEGIN

					-- [3.1.1.1] Calculate minimal and maximal required quotation.
					BEGIN

						SELECT
							MIN(a.[startQuote]) AS [Min],
							MAX(a.[endQuote]) AS [Max]
						INTO
							#BorderPoints
						FROM
							(SELECT 
								t.[AnalysisStartPoint] AS [startQuote],
								t.[AnalysisStartPoint] + (@maxCheckRange * t.[CandlesDistance]) AS [endQuote]
							FROM
								#Trendlines t) a;

					END -- [3.1.1.1]

					-- [3.1.1.2] Load proper set of quotes based on [min] and [max] value obtained above.
					BEGIN

						DELETE FROM #Quotes_Iteration;

						INSERT INTO #Quotes_Iteration
						SELECT 
							qat.* 
						FROM 
							#Quotes_AssetTimeframe qat
							LEFT JOIN #BorderPoints bp ON qat.[DateIndex] BETWEEN bp.[Min] AND bp.[Max]
						WHERE
							bp.[Min] IS NOT NULL;

					END -- [3.1.1.2]
					
					-- [3.1.1.X] Drop temporary tables.
					BEGIN
						DROP TABLE #BorderPoints;
					END

				END --[3.1.1]

----SELECT 'DEBUG', '[3.1.1]', '#Quotes_Iteration', * FROM #Quotes_Iteration;



				-- [3.1.2] Create matching table between trendlines and quotations.
				BEGIN

					SELECT
						t.[TrendlineId],
						q.[DateIndex],
						q.[Close] * IIF(t.[LookForPeaks] = 1, 1, -1) AS [ModifiedClose],
						q.[Open] * IIF(t.[LookForPeaks] = 1, 1, -1) AS [ModifiedOpen],
						(t.[baseLevel] + (q.[DateIndex] - t.[BaseDateIndex]) * t.[Angle]) * IIF(t.[LookForPeaks] = 1, 1, -1) AS [ModifiedTrendlineLevel],
						IIF(t.[LookForPeaks] = 1, 1, -1) AS [LookForPeaks]
					INTO
						#TrendlineQuotePairs
					FROM
						#Trendlines t
						LEFT JOIN #Quotes_Iteration q
						ON q.[DateIndex] BETWEEN t.[AnalysisStartPoint] AND (t.[AnalysisStartPoint] + (@maxCheckRange * t.[CandlesDistance]));
					
				END

--SELECT 'DEBUG', '[3.1.2]', '#TrendlineQuotePairs', * FROM #TrendlineQuotePairs;



				-- [3.1.3] Filter only data with Close and Open prices above Resistance line or below Support Line.
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
						t.[ModifiedTrendlineLevel] < t.[ModifiedClose] AND t.[ModifiedTrendlineLevel] < t.[ModifiedOpen];
					
				END

--SELECT 'DEBUG', '[3.1.3]', '#FilteredTrendlineQuotePairs', * FROM #FilteredTrendlineQuotePairs;



				-- [3.1.4] Select the first break for each analyzed trendline.
				BEGIN

					SELECT
						ft.[TrendlineId], 
						ft.[LookForPeaks] AS [LookForPeaks],
						MIN(ft.[DateIndex]) AS [DateIndex]
					INTO 
						#TrendlinesFirstBreaks
					FROM
						#FilteredTrendlineQuotePairs ft
					GROUP BY
						ft.[TrendlineId], ft.[LookForPeaks];

				END

--SELECT 'DEBUG', '[3.1.4]', '#TrendlinesFirstBreaks', * FROM #TrendlinesFirstBreaks;



				-- [3.1.5] Insert information obtained above to the proper tables for the next iteration of analysis.
				BEGIN
					
					-- [Trend breaks]
					INSERT INTO #TrendBreaks([TrendlineId], [DateIndex], [BreakFromAbove])
					SELECT tfb.[TrendlineId], tfb.[DateIndex], tfb.[LookForPeaks] * (-1)
					FROM #TrendlinesFirstBreaks tfb;

					-- [Trendlines]
					UPDATE t
					SET [BreakIndex] = tfb.[DateIndex]
					FROM 
						#Trendlines t
						LEFT JOIN #TrendlinesFirstBreaks tfb
						ON t.[TrendlineId] = tfb.[TrendlineId];

				END

--SELECT 'DEBUG', '[3.1.5]' AS [3.1.5 - TrendBreaks], '#TrendBreaks', * FROM #TrendBreaks;
--SELECT 'DEBUG', '[3.1.5]' AS [3.1.5 - Trendlines], '#Trendlines', * FROM #Trendlines;



				-- [3.1.X] Clean-up.
				BEGIN
					DROP TABLE #TrendlineQuotePairs
					DROP TABLE #FilteredTrendlineQuotePairs;
					DROP TABLE #TrendlinesFirstBreaks;
				END -- [3.1.X]


			END -- [3.1]



			-- [3.2] Find trend hits between analysis start and break or end of quotations.
			BEGIN


				-- [3.2.1] Select extremum groups required for this analysis.
				BEGIN

					-- [3.2.1.1] Calculate minimal and maximal required quotation.
					BEGIN

						SELECT
							MIN(a.[startDateIndex]) AS [Min],
							MAX(a.[endDateIndex]) AS [Max]
						INTO
							#ExtremumGroupsBorderPoints
						FROM
							(SELECT 
								t.[AnalysisStartPoint] AS [startDateIndex],
								t.[AnalysisStartPoint] + (@maxCheckRange * t.[CandlesDistance]) AS [endDateIndex]
							FROM 
								#Trendlines t) a;

					END -- [3.2.1.1]
					
					-- [3.2.1.2] Load proper set of extremum group based on [min] and [max] value obtained above.
					BEGIN

						DELETE FROM #ExtremumGroups;

						INSERT INTO #ExtremumGroups
						SELECT 
							eg.* 
						FROM 
							[dbo].[extremumGroups] eg
							LEFT JOIN #ExtremumGroupsBorderPoints egbp ON eg.[StartDateIndex] >= egbp.[Min] AND eg.[EndDateIndex] <= egbp.[Max]
						WHERE 
							[AssetId] = @assetId AND
							[TimeframeId] = @timeframeId AND
							egbp.[Min] IS NOT NULL;

					END -- [3.2.1.2] 

					-- [3.2.1.3] Drop temporary tables.
					BEGIN
						DROP TABLE #ExtremumGroupsBorderPoints;
					END
					
				END -- [3.2.1]	

				

				-- [3.2.2] Calculate borders for matching for each separate trendline (depending on breaks found before).
				BEGIN

					SELECT
						t.*,
						t.[AnalysisStartPoint] AS [MatchingLeftBorder],
						IIF(t.[BreakIndex] IS NULL, t.[AnalysisStartPoint] + (@maxCheckRange * t.[CandlesDistance]), t.[BreakIndex] - @minDistanceFromExtremumToBreak) AS [MatchingRightBorder]
					INTO 
						#TrendlinesHitsSearchBorders
					FROM
						#Trendlines t;

				END -- [3.2.2]



				-- [3.2.3] Create table with all possible matches Trendline-ExtremumGroup.
				BEGIN
				
					-- [3.2.3.1] Get all combination based on [Peaks/Bottom] + [Extremum is within searching bounds]
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

					END

					-- [3.2.3.2] Add modified price to the table created above (for comparison purposes).
					BEGIN

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


				END -- [3.2.3]

----SELECT 'DEBUG', '[3.2.3]', '#TrendlineMatchesWithModifiedPrices', * FROM TrendlineMatchesWithModifiedPrices;



				-- [3.2.4] Filter out prices that are not close enough to matched trendline and insert rest of records into TrendHits temporary table.
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

				END -- [3.2.4]

----SELECT 'DEBUG', '[3.2.4]', '#TrendHits', * FROM #TrendHits;



				-- [3.2.5] Remove duplicates from temporary #TrendHits table.
				BEGIN

					WITH CTE AS(
					   SELECT [TrendlineId], [ExtremumGroupId], RN = ROW_NUMBER()
					   OVER(PARTITION BY [TrendlineId], [ExtremumGroupId] ORDER BY [TrendlineId], [ExtremumGroupId])
					   FROM #TrendHits
					)
					DELETE FROM CTE WHERE RN > 1					

				END -- [3.2.5]

----SELECT 'DEBUG', '[3.2.5]', '#TrendHits: after removing duplicates', * FROM #TrendHits;



				-- [3.2.6] Append info about trend hits found to temporary #Trendlines table.
				BEGIN


					-- [3.2.6.1] Create temporary table with the latest trend hit for each trendline.
					BEGIN

						SELECT
							th.[TrendlineId],
							MAX(th.[DateIndex]) AS [LastHit]
						INTO 
							#LatestTrendHits
						FROM
							#TrendHits th
						GROUP BY
							th.[TrendlineId];

					END -- [3.2.6.1] 


					-- [3.2.6.2] Update TrendHit pointers.
					BEGIN

						UPDATE t
						SET 
							[HitIndex] = h.[LastHit]
						FROM
							#Trendlines t
							LEFT JOIN #LatestTrendHits h ON t.[TrendlineId] = h.[TrendlineId]
						WHERE
							t.[IsOpenFromRight] = 1 AND
							h.[LastHit] >= t.[AnalysisStartPoint];
					
					END -- [3.2.6.2] 


					-- [2.6.3] Clean up
					BEGIN
						DROP TABLE #LatestTrendHits;	
					END


				END -- [3.2.6]

----SELECT 'DEBUG', '[3.2.6]', '#Trendlines: after trendhit update', * FROM #Trendlines;



				-- [3.2.7] Remove temporary tables.
				BEGIN
					DROP TABLE #TrendlinesHitsSearchBorders
					DROP TABLE #TrendlineExtremumPossibleMatches;
					DROP TABLE #TrendlineMatchesWithModifiedPrices;
				END


			END -- [3.2]



			-- [3.3] Prepare data for next iteration based on breaks and hits found.
			BEGIN
				

				-- [3.3.1] Move all trendlines without break nor hit to ClosedTrendlines table (unless too less quotations have been checked).
				BEGIN


					-- [3.3.1.1] Select all trendlines without break nor hit.
					BEGIN

						SELECT 
							*
						INTO 
							#TrendlinesToBeSkippedInThisIteration
						FROM 
							#Trendlines t
						WHERE 
							t.[HitIndex] IS NULL AND (t.[BreakIndex] IS NULL OR (t.[BreakIndex] IS NOT NULL AND t.[PrevBreakIndex] IS NOT NULL));

					END -- [3.3.1.1]

--SELECT 'DEBUG', '[3.3.1.1]' AS [3.3.1.1], '#TrendlinesToBeSkippedInThisIteration', * FROM #TrendlinesToBeSkippedInThisIteration;


					-- [3.3.1.2] Find trendlines to be marked as Right-Side closed and update their [EndDateIndex] and [IsOpenFromRight] properties.
					BEGIN

						UPDATE 
							twe
						SET
							twe.[IsOpenFromRight] = 0,
							twe.[EndDateIndex] = COALESCE(twe.[PrevHitIndex], twe.[AnalysisStartPoint]) + @trendlineStartOffset
						FROM 
							#TrendlinesToBeSkippedInThisIteration twe
						WHERE
							(twe.[BreakIndex] IS NOT NULL AND twe.[PrevBreakIndex] IS NOT NULL) OR
							(@maxQuoteIndex - twe.[AnalysisStartPoint] > @maxCheckRange * twe.[CandlesDistance]);
							
					END -- [3.3.1.2] 

--SELECT 'DEBUG', '[3.3.1.2]' AS [3.3.1.2], '#TrendlinesToBeSkippedInThisIteration', * FROM #TrendlinesToBeSkippedInThisIteration;


					-- [3.3.1.3] Move all trendlines without break nor hit to ClosedTrendlines table.
					BEGIN
						
						INSERT INTO #ClosedTrendlines
						SELECT 
							* 
						FROM 
							#TrendlinesToBeSkippedInThisIteration;

						DELETE
						FROM
							#Trendlines
						WHERE
							[TrendlineId] IN (SELECT [TrendlineId] FROM #ClosedTrendlines);

					END -- [3.3.1.3]

----SELECT 'DEBUG', '[3.3.1.3]' AS [3.3.1.3], '#ClosedTrendlines', * FROM #ClosedTrendlines;



				END -- [3.3.1] 


				-- [3.3.2] Update status of all remaining trendlines.
				BEGIN
					
					UPDATE
						#Trendlines
					SET 
						[LookForPeaks] = [LookForPeaks] * IIF([BreakIndex] IS NULL, 1, -1),
						[AnalysisStartPoint] = COALESCE(IIF([BreakIndex] IS NOT NULL, [BreakIndex] + 1, [HitIndex] + 1), 0),
						[PrevBreakIndex] = [BreakIndex],
						[BreakIndex] = NULL,
						[PrevHitIndex] = IIF([HitIndex] IS NULL, [PrevHitIndex], [HitIndex]),
						[HitIndex] = NULL;

				END

--SELECT 'DEBUG', '[3.3.2]' AS [3.3.2 Trendlines before next iteration], '#Trendlines', * FROM #Trendlines;


				-- [3.3.X] Clean-up
				BEGIN
					DROP TABLE #TrendlinesToBeSkippedInThisIteration;
				END



			END -- [3.3] 


			SET @remainingTrendlines = (SELECT COUNT(*) FROM #Trendlines);


		END -- [WHILE]

	END -- [3]


--SELECT 'DEBUG', '[3.X]' AS [3.X ClosedTrendlines], '#ClosedTrendlines', * FROM #ClosedTrendlines;




	-- [4] Feed production tables with data calculated above
	BEGIN


		-- [4.0] Trendlines
		BEGIN

			UPDATE t
			SET
				t.[IsOpenFromRight] = ct.[IsOpenFromRight],
				t.[EndDateIndex] = ct.[EndDateIndex]
			FROM
				[dbo].[trendlines] t
				LEFT JOIN #ClosedTrendlines ct ON t.[TrendlineId] = ct.[TrendlineId]
			WHERE
				ct.[TrendlineId] IS NOT NULL;

		END -- [4.0]


		-- [4.1] Trend hits
		BEGIN

			
			-- [4.1.0] Remove trend hits 
			BEGIN
				
				DELETE th
				FROM 
					#TrendHits th
					LEFT JOIN [dbo].[trendlines] t ON th.[TrendlineId] = t.[TrendlineId]
				WHERE
					t.[IsOpenFromRight] = 0 AND th.[DateIndex] > t.[EndDateIndex];

			END -- [4.1.0]


			-- [4.1.1] Remove duplicates from #TrendHits table.
			BEGIN

				WITH CTE AS(
					SELECT [TrendlineId], [ExtremumGroupId], RN = ROW_NUMBER()
					OVER(PARTITION BY [TrendlineId], [ExtremumGroupId] ORDER BY [TrendlineId], [ExtremumGroupId])
					FROM #TrendHits
				)
				DELETE FROM CTE WHERE RN > 1

			END -- [4.1.1]


			-- [4.1.2] Create temporary table to store IDs given by DB engine.
			BEGIN
				
				CREATE TABLE #TempTrendHitsForIdentityMatching(
					[TrendHitId] [int] NOT NULL,
					[TrendlineId] [int] NOT NULL,
					[ExtremumGroupId] [int] NOT NULL
				);

			END -- [4.1.2]
			

			-- [4.1.3] Insert data into DB table.
			BEGIN

				INSERT INTO [dbo].[trendHits]([TrendlineId], [ExtremumGroupId])
				OUTPUT Inserted.[TrendHitId], Inserted.[TrendlineId], Inserted.[ExtremumGroupId] INTO #TempTrendHitsForIdentityMatching
				SELECT 
					[TrendlineId], 
					[ExtremumGroupId]
				FROM 
					#TrendHits;

			END -- [4.1.3]


			-- [4.1.4] Append IDs given by the DB engine to the records in the temporary table.
			BEGIN

				UPDATE th
				SET 
					[ProductionId] = h.[TrendHitId]
				FROM
					#TrendHits th
					LEFT JOIN #TempTrendHitsForIdentityMatching h 
									ON  th.[TrendlineId] = h.[TrendlineId] AND
										th.[ExtremumGroupId] = h.[ExtremumGroupId];

			END -- [4.1.4]


			-- [4.1.5] Drop temporary table.
			BEGIN
				DROP TABLE #TempTrendHitsForIdentityMatching;
			END -- [4.1.5]

		END -- [4.1]


		-- [4.2] Trend breaks
		BEGIN


			-- [4.2.0] Remove trend hits 
			BEGIN

				DELETE tb
				FROM 
					#TrendBreaks tb
					LEFT JOIN #ClosedTrendlines ct ON tb.[TrendlineId] = ct.[TrendlineId]
				WHERE
					ct.[EndDateIndex] IS NOT NULL AND tb.[DateIndex] > ct.[EndDateIndex];

			END -- [4.2.0]


			-- [4.2.1] Create temporary table to store IDs given by DB engine.
			BEGIN
				CREATE TABLE #TempTrendBreaksForIdentityMatching(
					[TrendBreakId] [int] NOT NULL,
					[TrendlineId] [int] NOT NULL,
					[DateIndex] [int] NOT NULL
				);
			END -- [4.2.1]


			-- [4.2.2] Insert remaining trendlines into TrendBreaks table.
			BEGIN

--SELECT 'DEBUG', '[4.2.2]' AS [4.2.2 #CurrentTrendBreaks], '[dbo].[TrendBreaks]', * FROM [dbo].[TrendBreaks];
--SELECT 'DEBUG', '[4.2.2]' AS [4.2.2 #TrendBreaksToBeAdded], '#TrendBreaksToBeAdded', * FROM #TrendBreaks;

				INSERT INTO [dbo].[TrendBreaks]([TrendlineId], [DateIndex], [BreakFromAbove])
				OUTPUT Inserted.[TrendBreakId], Inserted.[TrendlineId], Inserted.[DateIndex]
				INTO #TempTrendBreaksForIdentityMatching
				SELECT
					tb.[TrendlineId],
					tb.[DateIndex],
					tb.[BreakFromAbove]
				FROM
					#TrendBreaks tb;

			END -- [4.2.2]

				
			-- [4.2.3] Append IDs given by the DB engine to the records in the temporary table.
			BEGIN

				UPDATE tb
				SET 
					[ProductionId] = b.[TrendBreakId]
				FROM
					#TrendBreaks tb
					LEFT JOIN #TempTrendBreaksForIdentityMatching b
					ON  tb.[TrendlineId] = b.[TrendlineId] AND
						tb.[DateIndex] = b.[DateIndex];

			END -- [4.2.3]


			-- [4.2.4] Drop temporary table.
			BEGIN
				DROP TABLE #TempTrendBreaksForIdentityMatching;
			END -- [4.2.4]


		END -- [4.2] 


		-- [4.3] Trend ranges
		BEGIN

			
			-- [4.3.1] Create temporary table containing all breaks and hits from temporary tables.
			BEGIN

				SELECT
					*
				INTO
					#CurrentAnalysisBreaksAndHits
				FROM
					(SELECT [TrendlineId], [ProductionId], [DateIndex], 0 AS [IsHit]
					FROM #TrendBreaks
					UNION ALL
					SELECT [TrendlineId], [ProductionId], [DateIndex], 1 AS [IsHit]
					FROM #TrendHits) a;

			END -- [4.3.1]

--SELECT 'DEBUG', '[4.3.2]' AS [4.3.2], '#CurrentAnalysisBreaksAndHits', * FROM #CurrentAnalysisBreaksAndHits;


			-- [4.3.2] Create table containing one record for each trendline - with the last break or hit from previous analysis for this trendline.
			BEGIN

				SELECT
					[TrendlineId], [ProductionId], [DateIndex], [IsHit]
				INTO 
					#PrevAnalysisLastBreakOrHit
				FROM
					(SELECT
						*, 
						[number] = ROW_NUMBER() OVER (PARTITION BY [TrendlineId] ORDER BY [TrendlineId], [DateIndex] DESC)
					FROM
						(SELECT 
							tb.[TrendlineId], tb.[TrendBreakId] AS [ProductionId], tb.[DateIndex], 0 AS [IsHit] 
						FROM 
							(SELECT 
								tb.* 
							FROM 
								[dbo].[trendBreaks] tb
								INNER JOIN (SELECT * FROM [dbo].[trendlines] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) t 
												ON tb.[TrendlineId] = t.[TrendlineId]
							) tb

						UNION ALL
					
						SELECT 
							th.[TrendlineId], th.[TrendHitId] AS [ProductionId], eg.[EndDateIndex] AS [DateIndex], 1 AS [IsHit] 
						FROM 
							(SELECT 
								th.*
							FROM 
								[dbo].[trendHits] th
								INNER JOIN (SELECT * FROM [dbo].[trendlines] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) t 
												ON th.[TrendlineId] = t.[TrendlineId]
							) th
							LEFT JOIN (SELECT * FROM [dbo].[extremumGroups] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) eg ON th.[ExtremumGroupId] = eg.[ExtremumGroupId]
							) a
					WHERE
						CONCAT([ProductionId], [IsHit]) NOT IN (SELECT DISTINCT CONCAT([ProductionId], [IsHit]) FROM #CurrentAnalysisBreaksAndHits)) z
				WHERE z.[number] = 1;

			END -- [4.3.2]

--SELECT 'DEBUG', '[4.3.2]' AS [4.3.2], '#PrevAnalysisLastBreakOrHit', * FROM #PrevAnalysisLastBreakOrHit;


			-- [4.3.3] Create table with combined records from 4.3.1 and 4.3.2
			BEGIN

				SELECT
					*,
					[number] = ROW_NUMBER() OVER (ORDER BY [TrendlineId], [DateIndex])
				INTO
					#CombinedBreaksAndHits
				FROM
					(SELECT * FROM #CurrentAnalysisBreaksAndHits
					UNION ALL
					SELECT * FROM #PrevAnalysisLastBreakOrHit) a;

			END -- [4.3.3]

----SELECT 'DEBUG', '[4.3.3]' AS [4.3.3], '#CombinedBreaksAndHits', * FROM #CombinedBreaksAndHits;



			-- [4.3.4] Create pairs between two hits or a hit and a break from table #CombinedBreaksAndHits
			BEGIN
			
				DELETE FROM #TrendRanges;

				SELECT 
					cb1.[TrendlineId], cb1.[ProductionId] AS [BaseProductionId], cb1.[IsHit] AS [BaseIsHit], cb1.[DateIndex] AS [BaseDateIndex], cb2.[ProductionId] AS [CounterProductionId], cb2.[IsHit] AS [CounterIsHit], cb2.[DateIndex] AS [CounterDateIndex]
				INTO 
					#TrendRangeSourceData
				FROM 
					#CombinedBreaksAndHits cb1
					INNER JOIN #CombinedBreaksAndHits cb2
					ON  cb1.[TrendlineId] = cb2.[TrendlineId] AND
						cb1.[number] = cb2.[number] - 1;


					
--SELECT 'DEBUG', '[4.3.4]' AS [4.3.4 #CombinedBreaksAndHits], '#CombinedBreaksAndHits', * FROM #CombinedBreaksAndHits;
--SELECT 'DEBUG', '[4.3.4]' AS [4.3.4 #CurrentTrendRanges], '#TrendRanges', * FROM #TrendRanges;
--SELECT 'DEBUG', '[4.3.4]' AS [4.3.4 #TrendRangeSourceData], '#TrendRangeSourceData', * FROM #TrendRangeSourceData;


				INSERT INTO #TrendRanges([TrendlineId], [BaseId], [BaseIsHit], [BaseDateIndex], [CounterId], [CounterIsHit], [CounterDateIndex])
				SELECT
					*
				FROM
					#TrendRangeSourceData


			END -- [4.3.4]

----SELECT 'DEBUG', '[4.3.4]' AS [4.3.4 - Trend ranges after first insert], '#TrendRanges', * FROM #TrendRanges;



			-- [4.3.5] Update info if the given trend range is top or bottom.
			BEGIN

				UPDATE tr
				SET
					[IsPeak] = IIF(eg.[IsPeak] = 1, 1, -1)
				FROM
					#TrendRanges tr
					LEFT JOIN [dbo].[trendHits] th ON (tr.[CounterIsHit] = 0 AND tr.[BaseId] = th.[TrendHitId]) OR (tr.[CounterIsHit] = 1 AND tr.[CounterId] = th.[TrendHitId])
					LEFT JOIN #TrendHits tth ON (tr.[CounterIsHit] = 0 AND tr.[BaseId] = tth.[ProductionId]) OR (tr.[CounterIsHit] = 1 AND tr.[CounterId] = tth.[ProductionId])
					LEFT JOIN [dbo].[extremumGroups] eg ON (th.[ExtremumGroupId] = eg.[ExtremumGroupId]) OR (tth.[ExtremumGroupId] = eg.[ExtremumGroupId])
				WHERE
					eg.[ExtremumGroupId] IS NOT NULL;

			END -- [4.3.5]

----SELECT 'DEBUG', '[4.3.5]' AS [4.3.5 - Trend ranges after IsPeak update], '#TrendRanges', * FROM #TrendRanges;



			-- [4.3.6] Evaluate trend ranges.
			BEGIN


				-- [4.3.6.1] Create data to be used in evaluating function.
				BEGIN

					DECLARE @TrendRangeBasicData AS [dbo].[TrendRangeBasicData];

					SELECT
						tr.[TrendRangeId],
						--tr.[TrendlineId],
						t.[BaseDateIndex] AS [TrendlineStartDateIndex],
						t.[BaseLevel] AS [TrendlineStartLevel],
						t.[Angle] As [TrendlineAngle],
						IIF(tr.[BaseIsHit] = 1, eg.[EndDateIndex] + 1, tr.[BaseDateIndex]) AS [StartIndex],
						IIF(tr.[CounterIsHit] = 1, eg2.[StartDateIndex] - 1, tr.[CounterDateIndex]) AS [EndIndex],
						tr.[IsPeak]
					INTO
						#TrendRangesEvaluationSourceData
					FROM
						#TrendRanges tr
						LEFT JOIN [dbo].[trendHits] th ON tr.[BaseId] = th.[TrendHitId]
						LEFT JOIN [dbo].[extremumGroups] eg ON th.[ExtremumGroupId] = eg.[ExtremumGroupId]
						LEFT JOIN #TrendHits th2 ON tr.[CounterId] = th2.[ProductionId]
						LEFT JOIN [dbo].[extremumGroups] eg2 ON th2.[ExtremumGroupId] = eg2.[ExtremumGroupId]
						LEFT JOIN [dbo].[trendlines] t ON tr.[TrendlineId] = t.[TrendlineId];


----SELECT 'DEBUG', '[4.3.6.0a]' AS [4.3.6.0 - Trend ranges], '#TrendRanges', * FROM #TrendRanges;
----SELECT 'DEBUG', '[4.3.6.0b]' AS [4.3.6.0 - Trend hits], '#TrendHits', * FROM #TrendHits;
----SELECT 'DEBUG', '[4.3.6.0b]' AS [4.3.6.0 - Trend hits PROD], '[dbo].[trendHits]', * FROM [dbo].[trendHits];
----SELECT 'DEBUG', '[4.3.6.0c]' AS [4.3.6.0 - Extremum groups], '[dbo].[extremumGroups]', * FROM [dbo].[extremumGroups];
----SELECT 'DEBUG', '[4.3.6.0d]' AS [4.3.6.0 - Trend lines], '[dbo].[trendlines]', * FROM [dbo].[trendlines];
----SELECT 'DEBUG', '[4.3.6.1]' AS [4.3.6.1 - Trend ranges evaluation source data], '#TrendRangesEvaluationSourceData', * FROM #TrendRangesEvaluationSourceData;


					INSERT INTO @TrendRangeBasicData
					SELECT * FROM #TrendRangesEvaluationSourceData;

				END -- [4.3.6.1]


				-- [4.3.6.2] Update variation data.
				BEGIN

					UPDATE tr
					SET
						[TotalCandles] = v.[TotalCandles],
						[AverageVariation] = v.[TotalVariation] / v.[TotalCandles],
						[ExtremumVariation] = v.[ExtremumVariation],
						[OpenCloseVariation] = v.[OCVariation]
					FROM
						#TrendRanges tr
						LEFT JOIN [dbo].[GetTrendRangesVariations](@assetId, @timeframeId, @TrendRangeBasicData) v ON tr.[TrendRangeId] = v.[TrendRangeId]
				
				END -- [4.3.6.2]


				-- [4.3.6.3] Update cross data.
				BEGIN

					UPDATE tr
					SET
						[ExtremumPriceCrossPenaltyPoints] = v.[ExtremumPriceCrossPenaltyPoints],
						[ExtremumPriceCrossCounter] = v.[ExtremumPriceCrossCounter],
						[OCPriceCrossPenaltyPoints] = v.[OCPriceCrossPenaltyPoints],
						[OCPriceCrossCounter] = v.[OCPriceCrossCounter]
					FROM
						#TrendRanges tr
						LEFT JOIN [dbo].[GetTrendRangesCrossDetails](@assetId, @timeframeId, @TrendRangeBasicData) v ON tr.[TrendRangeId] = v.[TrendRangeId]

				END -- [4.3.6.3]


			END -- [4.3.6]

----SELECT 'DEBUG', '[4.3.6]' AS [4.3.6 - Trend ranges after evaluation], '#TrendRanges', * FROM #TrendRanges;


	
			-- [4.3.7] Move ranges to the production table.
			BEGIN

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

			END -- [4.3.7]

----SELECT 'DEBUG', '[4.3.7]' AS [4.3.7 - Trend ranges after export to production table], '[dbo].[trendRanges]', * FROM [dbo].[trendRanges];



			-- [4.3.X] Clean up.
			BEGIN
				DROP TABLE #CurrentAnalysisBreaksAndHits;
				DROP TABLE #PrevAnalysisLastBreakOrHit;
				DROP TABLE #CombinedBreaksAndHits;
				DROP TABLE #TrendRangesEvaluationSourceData;
				DROP TABLE #TrendRangeSourceData;
			END -- [4.3.X]
		


		END -- [4.3]


		-- [4.4] Remove trend events after trend end.
		BEGIN
			

			-- [4.4.1] Remove expired trend ranges.
			BEGIN
				DELETE tr
				FROM
					[dbo].[trendRanges] tr
					LEFT JOIN [dbo].[trendlines] t ON tr.[TrendlineId] = t.[TrendlineId]
				WHERE
					t.[IsOpenFromRight] = 0 AND t.[EndDateIndex] < tr.[CounterDateIndex];
			END -- [4.4.1]

			
			-- [4.4.2] Remove expired trend ranges.
			BEGIN
				DELETE tb
				FROM
					[dbo].[trendBreaks] tb
					LEFT JOIN [dbo].[trendlines] t ON tb.[TrendlineId] = t.[TrendlineId]
				WHERE
					t.[IsOpenFromRight] = 0 AND t.[EndDateIndex] < tb.[DateIndex];
			END -- [4.4.2]


			-- [4.4.3] Remove expired trend hits.
			BEGIN
				DELETE th
				FROM
					[dbo].[trendHits] th
					LEFT JOIN [dbo].[trendlines] t ON th.[TrendlineId] = t.[TrendlineId]
					LEFT JOIN [dbo].[extremumGroups] eg ON th.[ExtremumGroupId] = eg.[ExtremumGroupId]
				WHERE
					t.[IsOpenFromRight] = 0 AND t.[EndDateIndex] < eg.[StartDateIndex];
			END -- [4.4.3]


		END -- [4.4] 


	END -- [4]




	-- [5] Drop temporary tables.
	BEGIN

		DROP TABLE #TrendBreaks;
		DROP TABLE #TrendHits;
		DROP TABLE #TrendRanges;
		DROP TABLE #Trendlines;
		DROP TABLE #ClosedTrendlines;
		DROP TABLE #Quotes_AssetTimeframe;
		DROP TABLE #Quotes_Iteration;
		DROP TABLE #ExtremumGroups;

	END	-- [5]


--SELECT '[DEBUG]', '[analyzeTrendlinesRightSide]', 'finished';


END
GO




GO

--ROLLBACK TRANSACTION
COMMIT TRANSACTION