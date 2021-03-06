USE [fx];

BEGIN TRANSACTION

IF OBJECT_ID('calculateTrendBreaks','P') IS NOT NULL DROP PROC [dbo].[calculateTrendBreaks];

GO

CREATE PROC [dbo].[calculateTrendBreaks] @assetId AS INT, @timeframeId AS INT
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



		-- [2.3] Różnica pomiędzy notowanie z dnia breaku a notowaniem poprzedzającym
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
			FROM
				#TrendBreaks tb
				LEFT JOIN #BreakDayAmplitude bda ON tb.[TrendBreakId] = bda.[TrendBreakId]
				LEFT JOIN #PreviousDayPoints pdp ON tb.[TrendBreakId] = pdp.[TrendBreakId]
				LEFT JOIN #NextDaysMaxVariancePoints maxPoints ON tb.[TrendBreakId] = maxPoints.[TrendBreakId]
				LEFT JOIN #NextDaysMinDistancePoints minPoints ON tb.[TrendBreakId] = minPoints.[TrendBreakId];

		END -- [2.6]


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
	END


END

GO

EXEC [dbo].[calculateTrendBreaks] @assetId = 1, @timeframeId = 4

ROLLBACK TRANSACTION