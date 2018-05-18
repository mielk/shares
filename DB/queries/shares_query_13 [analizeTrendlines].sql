
			NULL AS [BreakIndex],
			NULL AS [PrevBreakIndex],
			NULL AS [HitIndex],
			NULL AS [PrevHitIndex],











USE [shares];

GO

DROP PROCEDURE [dbo].[evaluateTrendlines];

GO

CREATE PROCEDURE [dbo].[evaluateTrendlines] @shareId INT, @baseExtremumId INT, @counterExtremumId INT

AS

BEGIN TRANSACTION

SELECT @shareId, @baseExtremumId, @counterExtremumId;



DECLARE @trendlineStartOffset INT;
DECLARE @displayLeftSidePreviewTables BIT, @displayRightSidePreviewTables BIT;
DECLARE @i INT, @j INT;

DECLARE @minDistanceFromExtremumToBreak INT, @quotesAnalyzedForBreakEvaluation INT;
----------------------------------------------------------------------------------------------------------------
SET @minDistanceFromExtremumToBreak = 5;
SET @quotesAnalyzedForBreakEvaluation = 5;

SET @trendlineStartOffset = 0;
----------------------------------------------------------------------------------------------------------------
SET @displayLeftSidePreviewTables = 0;
SET @displayRightSidePreviewTables = 0;
----------------------------------------------------------------------------------------------------------------


--Temporary tables.
--SELECT * INTO #Trendlines FROM [dbo].[trendlines] WHERE [ShareId] = @shareId AND [BaseId] = @baseExtremumId AND [CounterId] = @counterExtremumId AND [~IsOpenFromRight] = 1;
SELECT * INTO #Trendlines FROM [dbo].[trendlines] WHERE [Id] = 1350568;
SELECT * INTO #ExtremumGroups FROM [dbo].[extremumGroups] WHERE [ShareId] = @shareId;
SELECT [DateIndex], [Date], [Open], [Low], [High], [Close], [Volume] INTO #Quotes FROM [dbo].[quotes] WHERE [ShareId] = @shareId;





-- RIGHT-SIDE ANALYSIS
BEGIN
	
	--PREPARE TRENDLINES TABLE
	BEGIN

		UPDATE 
			#TrendlinesTemp
		SET 
			[AnalysisStartPoint] = [BaseStartIndex],
			[LookForPeaks] = IIF([BaseIsPeak] = 1, 1, -1)
	
		IF (@displayRightSidePreviewTables = 1) SELECT '#TrendlinesBreaks.BeforeStart', * FROM #TrendlinesTemp; --WHERE [TrendlineId] = 24019795 ORDER BY DateIndex ASC;

	END

	SET @j = 1;
	WHILE @j <= 10
	BEGIN

		--FIND FIRST BREAK TO THE RIGHT OF THE CURRENT POINT [1]
		BEGIN

			--Create temporary tables.
			BEGIN

				SELECT * INTO #TrendlinesBreaks_RightSide FROM #TrendlinesBreaks WHERE 0 = 1;

				SELECT 
					* 
				INTO
					#OpenTrendlinesRs
				FROM
					#TrendlinesTemp
				WHERE
					[~IsOpenFromRight] = 1;

			END

			--Get proper set of quotes required for analysis.			
			BEGIN

				SELECT
					MIN(a.[startQuote]) AS [Min],
					MAX(a.[endQuote]) AS [Max]
				INTO #BorderPointsRs
				FROM
					(SELECT 
						tt.[AnalysisStartPoint] + 1 AS [startQuote],
						tt.[AnalysisStartPoint] + (@distanceIterations * tt.[~CandlesDistance]) AS [endQuote]
					FROM 
						#OpenTrendlinesRs tt) a;

				SET @minQuoteIndex = (SELECT [Min] FROM #BorderPointsRs);
				SET @maxQuoteIndex = (SELECT [Max] FROM #BorderPointsRs);
	
				SELECT *
				INTO #Quotes_RightSide
				FROM #Quotes q
				WHERE q.[DateIndex] BETWEEN @minQuoteIndex AND @maxQuoteIndex;

				DROP TABLE #BorderPointsRs;

			END

			--Look for break by searching batches (each batch contains as many elements as given trendline [~CandlesDistance]).
			BEGIN

				SET @i = 0;
				WHILE @i < @distanceIterations
				BEGIN
				
					--Create temporary table containing only trendlines without break already found.
					SELECT
						t.* 
					INTO
						#RsTrendlinesWithoutBreak
					FROM 
						#OpenTrendlinesRs t
						LEFT JOIN #TrendlinesBreaks_RightSide tb
						ON t.[Id] = tb.[TrendlineId]
					WHERE 
						tb.[TrendlineId] IS NULL;

					--Leave loop if there is no trendlines left without break.
					IF (SELECT COUNT(*) FROM #RsTrendlinesWithoutBreak) = 0 
					BEGIN
						DROP TABLE #RsTrendlinesWithoutBreak;
						BREAK;
					END

					--Append current batch dates range to the table containing all trendlines without break.
					SELECT
						t.[Id] AS [TrendlineId], 
						t.[BaseLevel],
						t.[BaseStartIndex],
						t.[LookForPeaks],
						t.[Slope],
						t.[AnalysisStartPoint] + t.[~CandlesDistance] * @i AS [BatchStartIndex],
						t.[AnalysisStartPoint] + t.[~CandlesDistance] * (@i + 1) AS [BatchEndIndex]
					INTO
						#RsTrendlinesWithoutBreakWithDatesRange
					FROM
						#RsTrendlinesWithoutBreak t;

					--Create table with pairs.
					SELECT
						t.[TrendlineId], 
						t.[Slope],
						q.[Close] * t.[LookForPeaks] AS [ModifiedClose],
						q.[Open] * t.[LookForPeaks] AS [ModifiedOpen],
						q.[DateIndex],
						q.[Date],
						(t.[baseLevel] + (q.[DateIndex] - t.[BaseStartIndex]) * t.[Slope]) * t.[LookForPeaks] AS [ModifiedTrendlineLevel],
						t.[LookForPeaks]
					INTO
						#RsTrendlineQuotePairs
					FROM
						#RsTrendlinesWithoutBreakWithDatesRange t
						INNER JOIN #Quotes_RightSide q
						ON q.[DateIndex] BETWEEN t.[BatchStartIndex] AND t.[BatchEndIndex]

					--Filter only those pairs where Close and Open prices are above Resistance line or below Support line.
					SELECT
						t.[TrendlineId], 
						t.[DateIndex], 
						t.[LookForPeaks]
					INTO
						#RsFilteredTrendlineQuotePairs
					FROM
						#RsTrendlineQuotePairs t
					WHERE
						t.[ModifiedTrendlineLevel] < t.[ModifiedClose] AND t.[ModifiedTrendlineLevel] < t.[ModifiedOpen]

					--Select the latest break found before.
					SELECT
						ft.[TrendlineId], 
						MIN(ft.[DateIndex]) AS [DateIndex], 
						ft.[LookForPeaks] * (-1) AS [BreakFromAbove]
					INTO 
						#RsTrendlineFirstBreak
					FROM
						#RsFilteredTrendlineQuotePairs ft
					GROUP BY
						ft.[TrendlineId], ft.[LookForPeaks];

					--Insert result into temporary breaks table.
					BEGIN 

						INSERT INTO #TrendlinesBreaks_RightSide
						SELECT * FROM #RsTrendlineFirstBreak;

					END

					--Clean up
					BEGIN

						DROP TABLE #RsTrendlinesWithoutBreak;
						DROP TABLE #RsTrendlinesWithoutBreakWithDatesRange;
						DROP TABLE #RsTrendlineQuotePairs;
						DROP TABLE #RsFilteredTrendlineQuotePairs;
						DROP TABLE #RsTrendlineFirstBreak;

					END

					SET @i = @i + 1;

				END;

			END
			
			--Append breaks into #TrendlinesBreaks table.
			INSERT INTO #TrendlinesBreaks([TrendlineId], [DateIndex], [BreakFromAbove])
			SELECT [TrendlineId], [DateIndex], [BreakFromAbove] FROM #TrendlinesBreaks_RightSide;

			--Clean up.
			BEGIN

				DROP TABLE #OpenTrendlinesRs;
				DROP TABLE #Quotes_RightSide;
				DROP TABLE #TrendlinesBreaks_RightSide;

			END

		END
		
		IF (@displayRightSidePreviewTables = 1) SELECT '#TrendlinesBreaks', @j AS [iteration], * FROM #TrendlinesBreaks ORDER BY [DateIndex] DESC;


		--MATCH BREAKS INFO TO TRENDLINES TABLE
		BEGIN

			--Update TrendBreaks pointers.
			UPDATE 
				tt
			SET 
				[BreakIndex] = tb.[DateIndex]
			FROM
				#TrendlinesTemp tt
				LEFT JOIN #TrendlinesBreaks tb
				ON tt.[Id] = tb.[TrendlineId]
			WHERE
				tt.[~IsOpenFromRight] = 1 AND
				tb.[DateIndex] >= tt.[AnalysisStartPoint];

		END

		IF (@displayRightSidePreviewTables = 1) SELECT '#TrendlinesTemp.AfterBreaks', @j AS [iteration], * FROM #TrendlinesTemp;


		--FIND HITS BETWEEN LEFT-SIDE ELEMENT AND LAST ANALYZED.
		BEGIN

			--Create temporary tables.
			BEGIN

				SELECT 
					* 
				INTO
					#RsOpenTrendlinesForHits
				FROM
					#TrendlinesTemp
				WHERE
					[~IsOpenFromRight] = 1;

			END

			--Predefine [ExtremumGroups] table for better performance.
			BEGIN
		
				DECLARE @minRsExtremumIndex INT, @maxRsExtremumIndex INT;

				SELECT
					MIN(a.[startIndex]) AS [Min],
					MAX(a.[endIndex]) AS [Max]
				INTO #RsBorderExtremaPoints
				FROM
					(SELECT 
						MIN([AnalysisStartPoint]) AS [startIndex],
						MAX([AnalysisStartPoint] + (tt.[~CandlesDistance] * @distanceIterations) - 1) AS [endIndex]
					FROM
						#RsOpenTrendlinesForHits tt) a;

				SET @minRsExtremumIndex  = (SELECT [Min] FROM #RsBorderExtremaPoints);
				SET @maxRsExtremumIndex  = (SELECT [Max] FROM #RsBorderExtremaPoints);
		
				SELECT
					*
				INTO 
					#RsExtremumGroups_Temp
				FROM
					#ExtremumGroups eg
				WHERE
					eg.[EndIndex] >= @minRsExtremumIndex AND eg.[StartIndex] <= @maxRsExtremumIndex;

				DROP TABLE #RsBorderExtremaPoints;

			END

			--Create temporary table with all possible combinations of hits (only distance is taken into account at this time).
			BEGIN
			
				--Append calculating fields to #OpenTrendlinesForHits
				SELECT
					*,
					[AnalysisStartPoint] + (@distanceIterations * [~CandlesDistance]) - 1 AS [CheckingEndPointIfNoBreakIndex],
					IIF([LookForPeaks] = 1, 1, 0) AS [LookForPeaksAsBit]
				INTO
					#RsTrendlinesWithCalculatingFields
				FROM
					#RsOpenTrendlinesForHits

				--Append match borders
				SELECT
					tt.*,
					tt.[AnalysisStartPoint] AS [MatchingLeftBorder],
					COALESCE(tt.[BreakIndex] - + @minDistanceFromExtremumToBreak, tt.[CheckingEndPointIfNoBreakIndex]) - 1 AS [MatchingRightBorder]
				INTO
					#RsTrendlinesForHitsMatching
				FROM
					#RsTrendlinesWithCalculatingFields tt


				--Get table with possible matches Trendline-ExtremumGroup.
				SELECT
					-- Trendline properties
					tfhm.[Id] AS [TrendlineId],
					tfhm.[LookForPeaks] AS [SupResFactor],
					tfhm.[BaseLevel],
					tfhm.[BaseStartIndex],
					tfhm.[Slope],
					-- Extremum properties
					eg.[Id] AS [ExtremumGroupId],
					eg.[StartIndex] AS [ExtremumStartIndex],
					COALESCE(eg.[SlaveIndex], eg.[MasterIndex]) AS [ExtremumIndex],
					IIF(tfhm.[LookForPeaks] = 1, eg.[High], eg.[Low]) AS [ExtremumPrice]
				INTO
					#RsTrendlineExtremumPossibleMatches
				FROM
					#RsTrendlinesForHitsMatching tfhm
					LEFT JOIN
					#RsExtremumGroups_Temp eg
					ON
							eg.[IsPeak] = tfhm.[LookForPeaksAsBit]
						AND eg.[EndIndex] >= tfhm.[MatchingLeftBorder] AND eg.[StartIndex] <= tfhm.[MatchingRightBorder]
		

				--Append trendline level.
				SELECT
					[TrendlineId],
					[SupResFactor],
					[ExtremumGroupId],
					[ExtremumStartIndex],
					[ExtremumPrice],
					([baseLevel] + ([ExtremumIndex] - [BaseStartIndex]) * [Slope]) AS [TrendlineLevel]
				INTO
					#RsTrendlineMatchesWithPriceLevels
				FROM
					#RsTrendlineExtremumPossibleMatches

				--Append prices modified by SupResFactor.
				SELECT
					[TrendlineId],
					[ExtremumGroupId],
					[ExtremumStartIndex],
					[SupResFactor],
					[ExtremumPrice] * [SupResFactor] AS [ModifiedPrice],
					[TrendlineLevel] * [SupResFactor] AS [ModifiedTrendlineLevel],
					IIF([TrendlineLevel] > 0, 1, -1) AS [TrendlineAboveZero],
					([TrendlineLevel] - [ExtremumPrice]) / [TrendlineLevel] AS [PriceTrendlineDistance]
				INTO
					#RsTrendlineMatchesWithModifiedPrices
				FROM
					#RsTrendlineMatchesWithPriceLevels

			END

			--Filter out all matches with price too far away from trendline.
			--Insert the rest of them into temporary #TrendlinesHits table.
			BEGIN

				INSERT INTO #TrendlinesHits([TrendlineId], [ExtremumGroupId], [DateIndex])
				SELECT DISTINCT
					[TrendlineId],
					[ExtremumGroupId],
					[ExtremumStartIndex]
				FROM
					#RsTrendlineMatchesWithModifiedPrices
				WHERE
					[SupResFactor] * [PriceTrendlineDistance] * [TrendlineAboveZero] < @maxDeviationFromTrendline;

			END

			--Remove duplicates from #TrendlinesHits table.
			BEGIN
				
				WITH CTE AS(
				   SELECT [TrendlineId], [ExtremumGroupId], [DateIndex], RN = ROW_NUMBER()
				   OVER(PARTITION BY [TrendlineId], [ExtremumGroupId], [DateIndex] ORDER BY [TrendlineId], [ExtremumGroupId], [DateIndex])
				   FROM #TrendlinesHits
				)
				DELETE FROM CTE WHERE RN > 1

			END


			--Clean up after looking for trend hits.
			BEGIN

				DROP TABLE #RsOpenTrendlinesForHits;
				DROP TABLE #RsExtremumGroups_Temp;
				DROP TABLE #RsTrendlinesWithCalculatingFields;
				DROP TABLE #RsTrendlinesForHitsMatching;
				DROP TABLE #RsTrendlineExtremumPossibleMatches;
				DROP TABLE #RsTrendlineMatchesWithPriceLevels;
				DROP TABLE #RsTrendlineMatchesWithModifiedPrices;

			END

		END

		IF (@displayRightSidePreviewTables = 1) SELECT '#TrendlinesHits', @j AS [iteration], * FROM #TrendlinesHits ORDER BY DateIndex DESC;


		--MATCH HITS INFO TO TRENDLINES TABLE
		BEGIN

			--Create temporary table with the latest trend hit for each trendline.
			SELECT
				th.[TrendlineId],
				MAX(th.[DateIndex]) AS [LastHit]
			INTO 
				#LatestTrendHits
			FROM
				#TrendlinesHits th
			GROUP BY
				th.[TrendlineId]

			--Update TrendHit pointers.
			UPDATE 
				tt
			SET 
				[HitIndex] = h.[LastHit]
			FROM
				#TrendlinesTemp tt
				LEFT JOIN #LatestTrendHits h
				ON tt.[Id] = h.[TrendlineId]
			WHERE
				tt.[~IsOpenFromRight] = 1 AND
				h.[LastHit] >= tt.[AnalysisStartPoint];

			--Clean up
			DROP TABLE #LatestTrendHits;

		END
	
		IF (@displayRightSidePreviewTables = 1) SELECT '#TrendlinesTemp.AfterHits', @j AS [iteration], * FROM #TrendlinesTemp --WHERE [Id] = 24019795;

			
		--PREPARE TRENDLINES TABLE FOR NEXT ANALYSIS ITERATION
		BEGIN

			--Inactivate trendlines with no Break and no Hit found
			UPDATE
				#TrendlinesTemp
			SET
				[~IsOpenFromRight] = 0,
				[EndDateIndex] = COALESCE([PrevHitIndex] + @trendlineStartOffset, [BaseStartIndex])
			WHERE
				[~IsOpenFromRight] = 1 AND
				[BreakIndex] IS NULL AND [HitIndex] IS NULL;

			--Inactivate trendlines with two breaks with no hit between
			UPDATE
				#TrendlinesTemp
			SET
				[~IsOpenFromRight] = 0,
				[EndDateIndex] = COALESCE([PrevHitIndex] + @trendlineStartOffset, [BaseStartIndex])
			WHERE
				[~IsOpenFromRight] = 1 AND
				[BreakIndex] IS NOT NULL AND [PrevBreakIndex] IS NOT NULL AND [HitIndex] IS NULL;

			--Inactivate if distance between hits is too large


			--Update indices of remaining trendlines for next analysis iteration.
			UPDATE
				#TrendlinesTemp
			SET 
				[LookForPeaks] = [LookForPeaks] * IIF([BreakIndex] IS NULL, 1, -1),
				[AnalysisStartPoint] = COALESCE(IIF([BreakIndex] IS NOT NULL, [BreakIndex] + 1, [HitIndex] + 1), 0),
				[PrevBreakIndex] = IIF([BreakIndex] IS NULL, [PrevBreakIndex], [BreakIndex]),
				[BreakIndex] = NULL,
				[PrevHitIndex] = IIF([HitIndex] IS NULL, [PrevHitIndex], [HitIndex]),
				[HitIndex] = NULL
			WHERE
				[~IsOpenFromRight] = 1;

		END
	
		IF (@displayRightSidePreviewTables = 1) SELECT '#TrendlinesTemp.BeforeNextIteration', @j AS [iteration], * FROM #TrendlinesTemp --WHERE [Id] = 24019795;
	


		--TODO: Check if not end of data.

		--CHECK IF THERE ARE STILL SOME OPEN TRENDLINES
		IF (SELECT COUNT(*) FROM #TrendlinesTemp WHERE [~IsOpenFromRight] = 1) = 0 BREAK;

		--INCREMENT ITERATION COUNTER
		SET @j = @j + 1;

	END
	

END



-- CLEAR RESULTS
BEGIN

	--Remove all breaks out of trendlines range.
	DELETE tb
	FROM
		#TrendlinesBreaks tb
		INNER JOIN #TrendlinesTemp tt
		ON tb.[TrendlineId] = tt.[Id]
	WHERE
		tb.[DateIndex] < tt.[StartDateIndex] OR
		tb.[DateIndex] > tt.[EndDateIndex];

	--Remove all hits out of trendlines range.
	DELETE th
	FROM
		#TrendlinesHits th
		INNER JOIN #TrendlinesTemp tt
		ON th.[TrendlineId] = tt.[Id]
	WHERE
		th.[DateIndex] < tt.[StartDateIndex] OR
		th.[DateIndex] > tt.[EndDateIndex];

END



-- EVALUTE TRENDLINES
BEGIN

	
	--Append trendline type.	
	BEGIN
		
		--Append HitId
		SELECT
			tr.*, 
			IIF(tr.[BaseIsHit] = 1, tr.[BaseId], tr.[CounterId]) AS [HitId]
		INTO
			#TrendRangesWithHitId
		FROM
			#TrendRanges tr
			LEFT JOIN #TrendlinesHits th
			ON tr.[BaseId] = th.[Id];

		--Append ExtremumId
		SELECT
			tr.*, th.[ExtremumGroupId]
		INTO
			#TrendRangesWithExtremumGroupId
		FROM
			#TrendRangesWithHitId tr
			LEFT JOIN #TrendlinesHits th
			ON tr.[HitId] = th.[Id];

		--Append IsPeak info.
		SELECT
			trw.*, 
			IIF(eg.[IsPeak] = 1, 1, -1) AS [ExtremumIsPeak]
		INTO 
			#TrendRangesWithIsPeak
		FROM
			#TrendRangesWithExtremumGroupId trw
			LEFT JOIN #ExtremumGroups eg
			ON trw.[ExtremumGroupId] = eg.[Id];

		--Update base table.
		UPDATE tr
		SET [IsPeak] = trwip.[ExtremumIsPeak]
		FROM
			#TrendRanges tr
			LEFT JOIN #TrendRangesWithIsPeak trwip
			ON tr.[Id] = trwip.[Id];

		--Clean up
		BEGIN

			DROP TABLE #TrendRangesWithHitId;
			DROP TABLE #TrendRangesWithExtremumGroupId;
			DROP TABLE #TrendRangesWithIsPeak
		END

	END
	
	--Get proper set of quotes and trendline levels required for analysis.			
	BEGIN

		SELECT
			MIN(tr.[BaseDateIndex]) AS [Min],
			MAX(tr.[CounterDateIndex]) AS [Max]
		INTO #TrendRangesEvaluation_QuotesBorderPointsRs
		FROM
			#TrendRanges tr;

		SET @minQuoteIndex = (SELECT [Min] FROM #TrendRangesEvaluation_QuotesBorderPointsRs);
		SET @maxQuoteIndex = (SELECT [Max] FROM #TrendRangesEvaluation_QuotesBorderPointsRs);
		
		--Create table of quotes.
		SELECT 
			q.*, 
			IIF(q.[Close] > q.[Open], 1, 0) AS [IsBullish],
			IIF(q.[Close] < q.[Open], 1, 0) AS [IsBearish]
		INTO 
			#Quotes_TrendRangesEvaluation
		FROM #Quotes q
		WHERE q.[DateIndex] BETWEEN @minQuoteIndex AND @maxQuoteIndex;

		--Create table of trendline levels.
		SELECT
			t.[Id] AS [TrendlineId],
			q.[DateIndex],
			(t.[BaseLevel] + (q.[DateIndex] - t.[BaseStartIndex]) * t.[Slope]) AS [TrendlineLevel]
		INTO
			#TrendlinesLevels_TrendRangesEvaluation
		FROM
			#TrendlinesTemp t,
			#Quotes_TrendRangesEvaluation q
		WHERE
				q.[DateIndex] >= t.[StartDateIndex]
			AND q.[DateIndex] <= t.[EndDateIndex];

		DROP TABLE #TrendRangesEvaluation_QuotesBorderPointsRs;

	END

	--Append partial evaluations.
	BEGIN
		
		--Create table with all quotes matched to ranges.
		SELECT
			tr.*,
			q.[DateIndex],
			IIF(tr.[IsPeak] = 1, q.[High], q.[Low]) AS [ExtremumPrice],
			IIF(tr.[IsPeak] = q.[IsBullish], q.[Close], q.[Open]) AS [OpenClosePrice]
		INTO
			#TrendRangesQuotesMatched
		FROM
			#TrendRanges tr
			INNER JOIN #Quotes_TrendRangesEvaluation q
			ON q.[DateIndex] BETWEEN tr.[BaseDateIndex] + 1 AND tr.[CounterDateIndex] - IIF([CounterIsHit] = 1, 1, 2)
		ORDER BY [TrendlineId];

		--Create table above with trendline level appended.
		SELECT
			tr.*,
			tl.[TrendlineLevel],
			tr.[ExtremumPrice] * tr.[IsPeak] AS [ModifiedExtremumPrice],
			tr.[OpenClosePrice] * tr.[IsPeak] AS [ModifiedOpenClosePrice],
			tl.[TrendlineLevel] * tr.[IsPeak] AS [ModifiedTrendline]
		INTO 
			#TrendRangesQuotesTrendlineLevels
		FROM
			#TrendRangesQuotesMatched tr
			LEFT JOIN #TrendlinesLevels_TrendRangesEvaluation tl
			ON 
					tr.[TrendlineId] = tl.[TrendlineId] 
				AND tr.[DateIndex] = tl.[DateIndex];
			
		--[AVERAGE VARIATION]
		BEGIN

			--Calculate variation area for each range.
			SELECT
				tr.[Id],
				ABS(tr.[TrendlineLevel] - tr.[ExtremumPrice]) AS [Variation]
			INTO
				#TrendRangeVariations
			FROM
				#TrendRangesQuotesTrendlineLevels tr

			--Calculate variation area for each range.
			SELECT
				tv.[Id],
				COUNT(tv.[Variation]) AS [TotalCandles],
				SUM(tv.[Variation]) AS [TotalVariation]
			INTO
				#TrendRangesTotalVariations
			FROM
				#TrendRangeVariations tv
			GROUP BY
				tv.[Id];
			
			--Append average variation.
			SELECT
				trtv.[Id],
				trtv.[TotalCandles],
				trtv.[TotalVariation],
				trtv.[TotalVariation] / trtv.[TotalCandles] AS [AverageVariation]
			INTO
				#TrendRangesWithAverageVariation
			FROM
				#TrendRangesTotalVariations trtv

			--Calculate maximum variation by extremum price.
			SELECT
				tr.[Id],
				MAX(tr.[ModifiedTrendline] - tr.[ModifiedExtremumPrice]) AS [ExtremumVariation],
				MAX(tr.[ModifiedTrendline] - tr.[ModifiedOpenClosePrice]) AS [OpenCloseVariation]
			INTO
				#TrendRangesMaxVariations
			FROM
				#TrendRangesQuotesTrendlineLevels tr
			GROUP BY
				tr.[Id];

			--Clean up
			BEGIN

				DROP TABLE #TrendRangeVariations;
				DROP TABLE #TrendRangesTotalVariations

			END

		END

		--[TRENDLINE CROSSING]
		BEGIN
			
			SELECT
				tr.[Id],
				tr.[DateIndex],
				tr.[ExtremumPrice] * tr.[IsPeak] AS [ModifiedExtremumPrice],
				tr.[OpenClosePrice] * tr.[IsPeak] AS [ModifiedOpenClosePrice],
				IIF(tl.[TrendlineLevel] > 0, 1, -1) AS [TrendlineAboveZero],
				tl.[TrendlineLevel] * tr.[IsPeak] AS [ModifiedTrendlineLevel]
			INTO
				#TrendlinePriceComparison
			FROM
				#TrendRangesQuotesMatched tr
				LEFT JOIN #TrendlinesLevels_TrendRangesEvaluation tl
				ON tr.[TrendlineId] = tl.[TrendlineId] 
					AND tr.[DateIndex] = tl.[DateIndex]
			
			--Extremum crossing penalty points.
			BEGIN

				--Select quotes where trendline is broken by extremum price.
				SELECT
					tpc.[Id],
					tpc.[DateIndex],
					ABS(tpc.[ModifiedExtremumPrice] - tpc.[ModifiedTrendlineLevel]) AS [CrossRange]
				INTO
					#ExtremumPriceCrossingTrendline
				FROM
					#TrendlinePriceComparison tpc
				WHERE
					tpc.[ModifiedExtremumPrice] > tpc.[ModifiedTrendlineLevel]

				--Get statistics for extremum price crossing trendline.
				SELECT
					e.[Id],
					SUM([CrossRange]) AS CrossRangeSum,
					AVG([CrossRange]) AS CrossRangeAverage,
					STDEVP([CrossRange]) AS CrossRangeStandardDeviation,
					COUNT([CrossRange]) AS CrossRangeCounter
				INTO
					#ExtremumPriceCrossingTrendline_Stats
				FROM
					#ExtremumPriceCrossingTrendline e
				GROUP BY
					e.[Id];

				--Calculate penalty points for extremum price crossing trendline.
				SELECT
					e.[Id],
					e.[CrossRangeSum] + e.[CrossRangeAverage] + e.[CrossRangeStandardDeviation] AS [ExtremumPriceCrossPenaltyPoints],
					e.[CrossRangeCounter] AS [ExtremumPriceCrossCounter]
				INTO
					#ExtremumPriceCrossingPenaltyPoints
				FROM
					#ExtremumPriceCrossingTrendline_Stats e;


				--Clean up
				BEGIN
				
					DROP TABLE #ExtremumPriceCrossingTrendline;
					DROP TABLE #ExtremumPriceCrossingTrendline_Stats;

				END


			END

			--Close/Open crossing penalty points.
			BEGIN

				--Select quotes where trendline is broken by open/close price.
				SELECT
					tpc.[Id],
					tpc.[DateIndex],
					ABS(tpc.[ModifiedOpenClosePrice] - tpc.[ModifiedTrendlineLevel]) AS [CrossRange]
				INTO
					#OCPriceCrossingTrendline
				FROM
					#TrendlinePriceComparison tpc
				WHERE
					tpc.[ModifiedOpenClosePrice] > tpc.[ModifiedTrendlineLevel]

--select '#OCPriceCrossingTrendline', * from #OCPriceCrossingTrendline;

				--Get statistics for extremum price crossing trendline.
				SELECT
					e.[Id],
					SUM([CrossRange]) AS CrossRangeSum,
					AVG([CrossRange]) AS CrossRangeAverage,
					STDEVP([CrossRange]) AS CrossRangeStandardDeviation,
					COUNT([CrossRange]) AS CrossRangeCounter
				INTO
					#OCPriceCrossingTrendline_Stats
				FROM
					#OCPriceCrossingTrendline e
				GROUP BY
					e.[Id];

				--Calculate penalty points for extremum price crossing trendline.
				SELECT
					e.[Id],
					e.[CrossRangeSum] + e.[CrossRangeAverage] + e.[CrossRangeStandardDeviation] AS [OCPriceCrossPenaltyPoints],
					e.[CrossRangeCounter] AS [OCPriceCrossCounter]
				INTO
					#OCPriceCrossingPenaltyPoints
				FROM
					#OCPriceCrossingTrendline_Stats e;

				--Clean up
				BEGIN
				
					DROP TABLE #OCPriceCrossingTrendline;
					DROP TABLE #OCPriceCrossingTrendline_Stats;

				END
				

			END

		END

	END



	--[BREAK VALUES]
	BEGIN

		--Get proper set of quotes required for breaks evaluation.
		BEGIN

			SELECT
				MIN(tb.[DateIndex] - @quotesAnalyzedForBreakEvaluation) AS [Min],
				MAX(tb.[DateIndex]) AS [Max]
			INTO #BorderPointsForBreaksEvaluation
			FROM
				#TrendlinesBreaks tb;

			SET @minQuoteIndex = (SELECT [Min] FROM #BorderPointsForBreaksEvaluation);
			SET @maxQuoteIndex = (SELECT [Max] FROM #BorderPointsForBreaksEvaluation);
	
			SELECT
				*
			INTO 
				#Quotes_BreaksEvaluation
			FROM
				#Quotes q
			WHERE
				q.[DateIndex] BETWEEN @minQuoteIndex AND @maxQuoteIndex;

			DROP TABLE #BorderPointsForBreaksEvaluation;

		END
	
		--Get all required quotes matched with break.
		BEGIN

			SELECT
				tb.[Id] AS [BreakId],
				tb.[BreakFromAbove],
				tb.[DateIndex] AS [BreakIndex],
				q.[DateIndex] AS [PriceIndex],
				q.[Open],
				q.[Low],
				q.[High],
				q.[Close],
				q.[Volume],
				tl.[TrendlineLevel]
			INTO 
				#BreakEvaluationQuotesMatch
			FROM
				#TrendlinesBreaks tb
				LEFT JOIN #Quotes_BreaksEvaluation q
				ON tb.[DateIndex] - q.[DateIndex] BETWEEN 0 AND @quotesAnalyzedForBreakEvaluation
				LEFT JOIN #TrendlinesLevels_TrendRangesEvaluation tl
				ON tb.[TrendlineId] = tl.[TrendlineId] AND q.[DateIndex] = tl.[DateIndex];

		END

		
		--Check if there is price hole.
		BEGIN

			SELECT
				b.[BreakId],
				b.[Open] * b.[BreakFromAbove] AS [ModifiedOpen],
				b.[Low] * b.[BreakFromAbove] AS [ModifiedLow],
				b.[High] * b.[BreakFromAbove] AS [ModifiedHigh],
				b.[Close] * b.[BreakFromAbove] AS [ModifiedClose],
				IIF(b.[BreakFromAbove] = 1, b.[Low] * b.[BreakFromAbove], b.[High] * b.[BreakFromAbove]) AS [ModifiedProperExtremum],
				b.[TrendlineLevel] * b.[BreakFromAbove] AS [ModifiedTrendline]
			INTO
				#BreakEveModifiedPrices
			FROM
				#BreakEvaluationQuotesMatch b
			WHERE
				b.[BreakIndex] - b.[PriceIndex] = 1;

			SELECT
				b.*,
				IIF(b.[ModifiedProperExtremum] > b.[ModifiedTrendline], 1, 0) AS [FullHole],
				IIF(b.[ModifiedClose] > b.[ModifiedTrendline] AND b.[ModifiedOpen] > b.[ModifiedTrendline], 1, 0) AS [CandleHole],
				IIF(b.[ModifiedClose] > b.[ModifiedTrendline], 1, 0) AS [PartialHole]
			INTO
				#BreakPriceHolesInfo
			FROM
				#BreakEveModifiedPrices b;

			--Evaluates price holes.
			--SELECT
			--	b.*,
			--	b.[ModifiedProperExtremum] - b.[ModifiedTrendline] AS [ExtremumTrendlineDistance],
			--	b.[ModifiedOpen] - b.[ModifiedTrendline] AS [OpenTrendlineDistance],
			--	b.[ModifiedClose] - b.[ModifiedTrendline] AS [CloseTrendlineDistance]
			--FROM
			--	#BreakEveModifiedPrices b;

			--Clean up
			DROP TABLE #BreakPriceHolesInfo;
			DROP TABLE #BreakEveModifiedPrices;

		END


		--Calculate points for volume.
		BEGIN

			--Table with average volumes.
			SELECT
				b.[BreakId],
				AVG(b.[Volume]) AS [PreviousAverageVolume]
			INTO
				#QuotesBeforeBreakAverageVolume
			FROM
				#BreakEvaluationQuotesMatch b
			WHERE b.[BreakIndex] - b.[PriceIndex] BETWEEN 2 AND 5
			GROUP BY
				b.[BreakId];

			--Table with break moment volumes.
			SELECT
				b.[BreakId],
				b.[Volume] AS [BreakCandleVolume]
			INTO
				#VolumesAtBreakTime
			FROM
				#BreakEvaluationQuotesMatch b
			WHERE b.[BreakIndex] - b.[PriceIndex] = 1;
			
			--Table with average volumes.
			SELECT
				v.[BreakId],
				v.[BreakCandleVolume],
				a.[PreviousAverageVolume]
			INTO
				#BreakVolumesComparing
			FROM
				#VolumesAtBreakTime v
				LEFT JOIN #QuotesBeforeBreakAverageVolume a
				ON v.[BreakId] = a.[BreakId];

			--Clean up
			DROP TABLE #BreakEvaluationQuotesMatch;
			DROP TABLE #VolumesAtBreakTime;


		END


		SELECT
			tb.[Id],
			0.5 AS [Value]
		INTO
			#TrendlineBreaksEvaluation
		FROM
			#TrendlinesBreaks tb


		--Clean up
		BEGIN

			DROP TABLE #QuotesBeforeBreakAverageVolume;
			DROP TABLE #Quotes_BreaksEvaluation;
			DROP TABLE #BreakVolumesComparing;

		END

	END

	
	--[HITS VALUES]
	SELECT
		th.*,
		eg.[Value] AS [Value]
	INTO
		#TrendlineHitsValues
	FROM
		#TrendlinesHits th
		LEFT JOIN #ExtremumGroups eg
		ON th.[ExtremumGroupId] = eg.[Id];


	--Combine all evaluation data for trend ranges.
	SELECT
		tr.*,
		epcpp.[ExtremumPriceCrossPenaltyPoints],
		epcpp.[ExtremumPriceCrossCounter],
		tbe.[Value],
		ocpp.[OCPriceCrossPenaltyPoints],
		ocpp.[OCPriceCrossCounter],
		trwav.[TotalCandles],
		trwav.[AverageVariation],
		trmv.[ExtremumVariation],
		trmv.[OpenCloseVariation],
		COALESCE(thv1.[Value], -0.5) AS [BaseHitValue],
		COALESCE(thv2.[Value], -0.5) AS [CounterHitValue]
	INTO 
		#TrendRangesPartValues
	FROM
		#TrendRanges tr
		LEFT JOIN #ExtremumPriceCrossingPenaltyPoints epcpp ON tr.[Id] = epcpp.[Id]
		LEFT JOIN #TrendlineBreaksEvaluation tbe ON tr.[Id] = tbe.[Id]
		LEFT JOIN #OCPriceCrossingPenaltyPoints ocpp ON tr.[Id] = ocpp.[Id]
		LEFT JOIN #TrendRangesWithAverageVariation trwav ON tr.[Id] = trwav.[Id]
		LEFT JOIN #TrendRangesMaxVariations trmv ON tr.[Id] = trmv.[Id]
		LEFT JOIN #TrendlineHitsValues thv1 ON (tr.[BaseIsHit] = 1 AND tr.[BaseId] = thv1.[Id])
		LEFT JOIN #TrendlineHitsValues thv2 ON (tr.[CounterIsHit] = 1 AND tr.[CounterId] = thv2.[Id])


	--SELECT 'TrendlinesTemp', * FROM #TrendlinesTemp;
	--SELECT 'TrendlinesHits', * FROM #TrendlinesHits ORDER BY [TrendlineId] ASC;
	--SELECT * FROM #TrendRangesPartValues ORDER BY [BaseDateIndex] ASC;


	--Clean up
	BEGIN
		
		DROP TABLE #Quotes_TrendRangesEvaluation;
		DROP TABLE #TrendlinesLevels_TrendRangesEvaluation;
		DROP TABLE #TrendRangesQuotesMatched;
		DROP TABLE #TrendlinePriceComparison;
		DROP TABLE #TrendRangesQuotesTrendlineLevels
		--Trendline evaluation tables.
		DROP TABLE #ExtremumPriceCrossingPenaltyPoints;
		DROP TABLE #TrendlineBreaksEvaluation;
		DROP TABLE #OCPriceCrossingPenaltyPoints;
		DROP TABLE #TrendRangesWithAverageVariation;
		DROP TABLE #TrendRangesMaxVariations;
		--DROP TABLE #TrendRangesPartValues;
		DROP TABLE #TrendlineHitsValues;

	END

END




-----------------


-- POST-ANALYSIS QUERIES
BEGIN

	--Create table with counter of hits for each trendline.
	SELECT
		tt.[Id],
		COUNT(th.[TrendlineId]) AS [Counter]
	INTO
		#HitCounters
	FROM	
		#TrendlinesTemp tt
		LEFT JOIN #TrendlinesHits th
		ON tt.[Id] = th.[TrendlineId]
	GROUP BY
		tt.[Id];

	--Create table with IDs of trendlines to be removed (less than 3 hits, closed from right side)
	SELECT
		t.[Id]
	INTO
		#TrendlinesToBeRemoved
	FROM
		#TrendlinesTemp t
		LEFT JOIN #HitCounters hc
		ON t.[Id] = hc.[Id]
	WHERE
		hc.[Counter] <= 2 AND
		t.[~IsOpenFromRight] = 0;

	--Delete trendlines mentioned above.
	DELETE
	FROM
		[dbo].[trendlines]
	WHERE
		[Id] IN (SELECT [Id] FROM #TrendlinesToBeRemoved);

	DELETE
	FROM
		#TrendlinesTemp
	WHERE
		[Id] IN (SELECT [Id] FROM #TrendlinesToBeRemoved);
		
	-----------------

	UPDATE t 
	SET
		[StartDateIndex] = tt.[StartDateIndex],
		[EndDateIndex] = tt.[EndDateIndex],
		[~IsOpenFromLeft] = tt.[~IsOpenFromLeft],
		[~IsOpenFromRight] = tt.[~IsOpenFromRight],
		[ShowOnChart] = 1
	FROM	
		[dbo].[trendlines] t
		INNER JOIN #TrendlinesTemp tt
		ON t.[Id] = tt.[Id]

	-----------------

	--Save info about hits/breaks/ranges.

	INSERT INTO [dbo].[trendlinesBreaks]([TrendlineId], [DateIndex], [BreakFromAbove])
	SELECT
		[TrendlineId], [DateIndex], [BreakFromAbove]
	FROM
		#TrendlinesBreaks
	WHERE
		[TrendlineId] IN (SELECT [Id] FROM #TrendlinesTemp);

	INSERT INTO [dbo].[TrendlinesHits]([TrendlineId], [ExtremumGroupId], [DateIndex])
	SELECT
		[TrendlineId], [ExtremumGroupId], [DateIndex]
	FROM
		#TrendlinesHits
	WHERE
		[TrendlineId] IN (SELECT [Id] FROM #TrendlinesTemp);

	INSERT INTO [dbo].[TrendRanges](	
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
		[CounterHitValue], 
		[Value])
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
		[CounterHitValue], 
		[Value]
	FROM
		#TrendRangesPartValues
	WHERE
		[TrendlineId] IN (SELECT [Id] FROM #TrendlinesTemp);

	--SELECT 
	--	* 
	--FROM 
	--	[dbo].[trendlines]
	--WHERE
	--	[ShareId] = @shareId AND
	--	[BaseId] = @baseExtremumId AND
	--	[CounterId] = @counterExtremumId

END




-- CLEAN-UP
BEGIN

	DROP TABLE #TrendlinesBreaks;
	DROP TABLE #TrendlinesHits;
	DROP TABLE #Trendlines;
	DROP TABLE #TrendRanges;
	DROP TABLE #ExtremumGroups;
	DROP TABLE #Quotes;
	DROP TABLE #TrendlinesTemp;
	DROP TABLE #HitCounters;
	DROP TABLE #TrendlinesToBeRemoved;
	DROP TABLE #TrendRangesPartValues;

END


COMMIT TRANSACTION



-- [1] Break - each quote that have Close price and Open price above resistance line or below support line.