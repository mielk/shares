use [stock];

BEGIN TRANSACTION

go

CREATE PROCEDURE [dbo].[evaluateExtrema](@assetId AS INT, @timeframeId AS INT)
AS
BEGIN

	DECLARE @PriceAverageSampleSize AS INT = 200;

	
	-- [0] Select Extrema for update
	BEGIN
		SELECT
			*
		INTO
			#Extrema
		FROM
			[dbo].[extrema] e
		WHERE
			e.[AssetId] = @assetId AND
			e.[TimeframeId] = @timeframeId;
	END


	-- [1] For each quotation calculate average price and StDev for previous 150 prices.
	BEGIN

		SELECT
			*
		INTO
			#Quotes
		FROM
			[dbo].[quotes] q
		WHERE
			q.[AssetId] = @assetId AND
			q.[TimeframeId] = @timeframeId;

		SELECT
			b.[DateIndex],
			b.[Ma],
			b.[StDev],
			(b.[StDev] / b.[Ma]) AS [CoV]
		INTO
			#QuotesStats
		FROM
			(SELECT
				a.[DateIndex],
				AVG((a.[ComparedOpen] + a.[ComparedClose]) / 2) AS [Ma],
				STDEV((a.[ComparedOpen] + a.[ComparedClose]) / 2) AS [StDev]
			FROM
				(SELECT
					q1.[DateIndex],
					q2.[Open] AS [ComparedOpen],
					q2.[Close] AS [ComparedClose]
				FROM
					#Quotes q1
					LEFT JOIN #Quotes q2 ON q1.[DateIndex] - q2.[DateIndex] BETWEEN 0 AND @PriceAverageSampleSize) a
			GROUP BY
				a.[DateIndex]) b

	END


	-- [2] Calculate amplitude points
	BEGIN


		-- [2.1] Extract all amplitudes into single table.
		SELECT
			*
		INTO
			#AllAmplitudes
		FROM
			(SELECT e.[ExtremumId], e.[DateIndex], e.[EarlierAmplitude] AS [Amplitude] FROM #Extrema e
			UNION ALL
			SELECT e.[ExtremumId], e.[DateIndex], e.[LaterAmplitude]  AS [Amplitude] FROM #Extrema e) a


		-- [2.2] Calculate CoV and sum of amplitudes
		SELECT
			a.[ExtremumId],
			a.[DateIndex],
			STDEVP(a.[Amplitude])/AVG(a.[Amplitude]) AS [AmplitudeCoV],
			SUM(a.[Amplitude]) AS [AmplitudeSum]
		INTO
			#AmplitudesStDevPAndSum
		FROM	
			#AllAmplitudes a
		GROUP BY
			a.[ExtremumId], 
			a.[DateIndex];


		-- [2.3] Append amplitude sum divided by price.
		SELECT
			a.[ExtremumId],
			a.[AmplitudeCoV],
			a.[AmplitudeSum] / qs.[Ma] AS [WeightedAmplitude]
		INTO
			#AmplitudesStDevPAndWeightedAmplitude
		FROM	
			#AmplitudesStDevPAndSum a
			LEFT JOIN #QuotesStats qs ON a.[DateIndex] = qs.[DateIndex];


		-- [2.4] Create final table.
		BEGIN
			SELECT
				a.[ExtremumId],
				(1 - a.[AmplitudeCoV]) * a.[WeightedAmplitude] AS [AmplitudePoints]
			INTO
				#AmplitudePoints
			FROM
				#AmplitudesStDevPAndWeightedAmplitude a;

		END


		-- [2.Z] Drop temporary tables.
		BEGIN
			DROP TABLE #AllAmplitudes;
			DROP TABLE #AmplitudesStDevPAndSum;
			DROP TABLE #AmplitudesStDevPAndWeightedAmplitude;
		END

	END
	

	-- [3] Calculate distance points
	BEGIN
	
		-- [3.1] Extract all distances into single table.
		SELECT
			*
		INTO
			#AllDistances
		FROM
			(SELECT e.[ExtremumId], e.[DateIndex], SQRT(LOG(e.[EarlierCounter], 260)) AS [CounterLog] FROM #Extrema e
			UNION ALL
			SELECT e.[ExtremumId], e.[DateIndex], SQRT(LOG(e.[LaterCounter], 260)) AS [CounterLog] FROM #Extrema e) a


		-- [3.2] Calculate CoV and sum of distances
		SELECT
			a.[ExtremumId],
			a.[DateIndex],
			STDEVP(a.[CounterLog]) AS [CounterStDev],
			AVG(a.[CounterLog]) AS [CountersAvg]
		INTO
			#DistancesStDevPAndSum
		FROM	
			#AllDistances a
		GROUP BY
			a.[ExtremumId], 
			a.[DateIndex];


		-- [3.3] Create final table with points for distances
		BEGIN

			SELECT
				b.[ExtremumId],
				b.[CountersAvg],
				b.[CountersCoV],
				0.7 * b.[CountersAvg] + 0.3 * (1 - b.[CountersCoV]) AS [DistancePoints]
			INTO
				#DistancePoints
			FROM
				(SELECT
					a.[ExtremumId],
					a.[DateIndex],
					a.[CountersAvg],
					a.[CounterStDev] / a.[CountersAvg] AS [CountersCoV]
				FROM
					#DistancesStDevPAndSum a) b

		END


		-- [3.Z] Drop temporary tables
		BEGIN
			
			DROP TABLE #AllDistances;
			DROP TABLE #DistancesStDevPAndSum;
		END

	END



	-- [4] Calculate averagea area points
	BEGIN
	
		-- [4.1] Extract all avearage areas into single table.
		SELECT
			*
		INTO
			#AllAreas
		FROM
			(SELECT e.[ExtremumId], e.[DateIndex], e.[EarlierAverageArea] AS [AverageArea] FROM #Extrema e
			UNION ALL
			SELECT e.[ExtremumId], e.[DateIndex], e.[LaterAverageArea] AS [AverageArea] FROM #Extrema e) a


		-- [4.2] Calculate CoV and sum of areas
		SELECT
			a.[ExtremumId],
			a.[DateIndex],
			SUM(a.[AverageArea]) AS [AreasSum],
			STDEVP(a.[AverageArea]) AS [AreasStDev],
			AVG(a.[AverageArea]) AS [AreasAvg]
		INTO
			#AreasStDevPAndSum
		FROM	
			#AllAreas a
		GROUP BY
			a.[ExtremumId], 
			a.[DateIndex];


		-- [4.3] Create final table with points for average variation area
		BEGIN

			SELECT
				b.[ExtremumId],
				3 * (b.[WeightedAvgArea] * (1 - [AreasCoV])) AS [AreasPoints]
			INTO
				#AreasPoints
			FROM
				(SELECT
					a.[ExtremumId],
					a.[DateIndex],
					a.[AreasSum] / qs.[Ma] AS [WeightedAvgArea],
					a.[AreasStDev] / a.[AreasAvg] AS [AreasCoV]
				FROM
					#AreasStDevPAndSum a
					LEFT JOIN #QuotesStats qs ON a.[DateIndex] = qs.[DateIndex]) b

		END


		-- [4.Z] Drop temporary tables
		BEGIN
			DROP TABLE #AllAreas;
			DROP TABLE #AreasStDevPAndSum;
		END

	END



	-- [5] Calculate previous variations points
	BEGIN

		-- [5.1] Calculate total weighted change
		BEGIN
			
			SELECT
				e.[ExtremumId],
				e.[DateIndex],
				(COALESCE(e.[EarlierChange1], 0) * 30 + COALESCE(e.[EarlierChange2], 0) * 15 + COALESCE(e.[EarlierChange3], 0) * 10 + COALESCE(e.[EarlierChange5], 0) * 6 + COALESCE(e.[EarlierChange10], 0) * 3) / 64 AS [WeightedChange]
			INTO
				#EarlierChanges
			FROM
				#Extrema e;

		END


		-- [5.2] Create final table with points
		BEGIN

			SELECT
				ch.[ExtremumId],
				ch.[DateIndex],
				[dbo].[MinValue](1, LOG(1000 * ch.[WeightedChange] / qs.[Ma], 50)) AS [ChangesPoints]
			INTO
				#EarlierChangesPoints
			FROM
				#EarlierChanges ch
				LEFT JOIN #QuotesStats qs ON ch.[DateIndex] = qs.[DateIndex];

		END


		-- [5.Z] Drop temporary tables
		BEGIN
			DROP TABLE #EarlierChanges;
		END		

	END

	

	-- [6] Calculate previous variations points
	BEGIN

		-- [6.1] Calculate total weighted change
		BEGIN
			
			SELECT
				e.[ExtremumId],
				e.[DateIndex],
				(COALESCE(e.[LaterChange1], 0) * 30 + COALESCE(e.[LaterChange2], 0) * 15 + COALESCE(e.[LaterChange3], 0) * 10 + COALESCE(e.[LaterChange5], 0) * 6 + COALESCE(e.[LaterChange10], 0) * 3) / 64 AS [WeightedChange]
			INTO
				#LaterChanges
			FROM
				#Extrema e;

		END


		-- [6.2] Create final table with points
		BEGIN

			SELECT
				ch.[ExtremumId],
				ch.[DateIndex],
				[dbo].[MinValue](1, LOG(1000 * ch.[WeightedChange] / qs.[Ma], 50)) AS [ChangesPoints]
			INTO
				#LaterChangesPoints
			FROM
				#LaterChanges ch
				LEFT JOIN #QuotesStats qs ON ch.[DateIndex] = qs.[DateIndex];

		END


		-- [6.Z] Drop temporary tables
		BEGIN
			DROP TABLE #LaterChanges;
		END		

	END



	-- [7] Combine all values fetched above into single final value
	BEGIN

		SELECT
			e.[ExtremumId],
			e.[DateIndex],
			15 * ap.[AmplitudePoints] + 
			30 * dp.[DistancePoints] + 
			15 * arp.[AreasPoints] + 
			20 * ecp.[ChangesPoints] + 
			20 * lcp.[ChangesPoints] AS [TotalPoint]
		INTO
			#TotalPoints
		FROM
			#Extrema e
			LEFT JOIN #AmplitudePoints ap ON e.[ExtremumId] = ap.[ExtremumId]
			LEFT JOIN #DistancePoints dp ON e.[ExtremumId] = dp.[ExtremumId]
			LEFT JOIN #AreasPoints arp ON e.[ExtremumId] = arp.[ExtremumId]
			LEFT JOIN #EarlierChangesPoints ecp ON e.[ExtremumId] = ecp.[ExtremumId]
			LEFT JOIN #LaterChangesPoints lcp ON e.[ExtremumId] = lcp.[ExtremumId];

	END


	-- [8] Update production table
	BEGIN

		UPDATE e
		SET
			e.[Value] = tp.[TotalPoint]
		FROM
			[dbo].[extrema] e 
			LEFT JOIN #TotalPoints tp ON e.[ExtremumId] = tp.[ExtremumId];

	END

	select * from [dbo].[extrema];

	-- [Z] Drop temporary tables.
	BEGIN
		DROP TABLE #Quotes;
		DROP TABLE #QuotesStats;
		DROP TABLE #AmplitudePoints;
		DROP TABLE #DistancePoints;
		DROP TABLE #AreasPoints;
		DROP TABLE #EarlierChangesPoints;
		DROP TABLE #LaterChangesPoints;
		DROP TABLE #TotalPoints;
	END

END

go

exec [dbo].[evaluateExtrema] @assetId = 1, @timeframeId = 6;

COMMIT TRANSACTION