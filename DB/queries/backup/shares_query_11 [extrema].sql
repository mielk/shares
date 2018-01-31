USE [shares];

GO

BEGIN TRANSACTION;

DECLARE @shareId INT, @minWorse AS INT, @maxChecked AS INT, @firstQuote AS INT, @lastQuote AS INT, @minValue AS INT;
SET @shareId = 1;
SET @minWorse = 5;
SET @maxChecked = 260;
SET @minValue = 55;

--Temporary tables.
SELECT [DateIndex], IIF([Close] > [Open], [Close], [Open]) AS [Close] INTO #MaxOpenClosePrices FROM [quotes] WHERE [ShareId] = @shareId;
SELECT [DateIndex], IIF([Close] > [Open], [Open], [Close]) AS [Close] INTO #MinOpenClosePrices FROM [quotes] WHERE [ShareId] = @shareId;
SELECT [DateIndex], [Low] INTO #LowPrices FROM [quotes] WHERE [ShareId] = @shareId;
SELECT [DateIndex], [High] INTO #HighPrices FROM [quotes] WHERE [ShareId] = @shareId;
SET @firstQuote = (SELECT MIN([DateIndex]) FROM #MaxOpenClosePrices);
SET @lastQuote = (SELECT MAX([DateIndex]) FROM #MaxOpenClosePrices);


--PEAK-BY-CLOSE

--Select extrema for further evaluation.
SELECT
	a.*
INTO
	#PeakByClose_InitialFiltered
FROM
	(SELECT
		c1.*
	FROM 
		#MaxOpenClosePrices c1
		LEFT JOIN
		#MaxOpenClosePrices c2
		ON c1.[DateIndex] = (c2.[DateIndex] + 1)
	WHERE
		c2.[Close] IS NULL OR c2.[Close] < c1.[Close]) a
	LEFT JOIN
	#MaxOpenClosePrices c3
	ON a.[DateIndex] = (c3.[DateIndex] - 1)
WHERE
	c3.[Close] IS NULL OR c3.[Close] <= a.[Close]

SELECT
	a.*
INTO
	#PeaksByCloseExtremaIndices
FROM
	(SELECT
		tif.*
	FROM
		#PeakByClose_InitialFiltered tif
		LEFT JOIN
		#MaxOpenClosePrices c
		ON tif.[DateIndex] > c.[DateIndex] AND tif.[DateIndex] <= (c.[DateIndex] + @minWorse) AND tif.[Close] <= c.[Close]
	WHERE
		c.[DateIndex] IS NULL) a

	LEFT JOIN
	#MaxOpenClosePrices c2
	ON a.[DateIndex] < c2.[DateIndex] AND a.[DateIndex] >= (c2.[DateIndex] - @minWorse) AND a.[Close] < c2.[Close]

	WHERE
		c2.[DateIndex] IS NULL

DROP TABLE #PeakByClose_InitialFiltered;



--Evaluate extrema.
	--Calculate earlier counters.
	SELECT
		fph.[DateIndex],
		IIF(fph.[FirstPrevHigher] IS NULL, 
			IIF(fph.[DateIndex] - @firstQuote > @maxChecked, @maxChecked, fph.[DateIndex] - @firstQuote),
			fph.[DateIndex] - fph.[FirstPrevHigher] - 1) AS [earlierCounter]
	INTO
		#PeakByClose_EarlierCounters
	FROM
		(SELECT 
			ex.[DateIndex], MAX(c.[DateIndex]) AS [FirstPrevHigher]
		FROM 
			#PeaksByCloseExtremaIndices ex
			LEFT JOIN
			#MaxOpenClosePrices c
			ON ex.[DateIndex] > c.[DateIndex] AND ex.[DateIndex] <= (c.[DateIndex] + @maxChecked) AND ex.[Close] < c.[Close]
		GROUP BY
			ex.[DateIndex]) fph
	
	--Calculate later counters.
	SELECT
		fnh.[DateIndex],
		IIF(fnh.[FirstNextHigher] IS NULL, 
			IIF(@lastQuote - fnh.[DateIndex] > @maxChecked, @maxChecked, @lastQuote - fnh.[DateIndex]),
			fnh.[FirstNextHigher] - fnh.[DateIndex] - 1) AS [laterCounter]
	INTO
		#PeakByClose_LaterCounters
	FROM
		(SELECT 
			ex.[DateIndex], MIN(c.[DateIndex]) AS [FirstNextHigher]
		FROM 
			#PeaksByCloseExtremaIndices ex
			LEFT JOIN
			#MaxOpenClosePrices c
			ON ex.[DateIndex] < c.[DateIndex] AND ex.[DateIndex] >= (c.[DateIndex] - @maxChecked) AND ex.[Close] < c.[Close]
		GROUP BY
			ex.[DateIndex]) fnh

	--Combine counters.
	SELECT
		ex.[DateIndex],
		ex.[Close],
		ec.[earlierCounter],
		lc.[laterCounter]
	INTO
		#PeakByClose_Counters
	FROM
		#PeaksByCloseExtremaIndices ex
		INNER JOIN #PeakByClose_EarlierCounters ec ON ex.[DateIndex] = ec.[DateIndex]
		INNER JOIN #PeakByClose_LaterCounters lc ON ex.[DateIndex] = lc.[DateIndex]

	DROP TABLE #PeakByClose_EarlierCounters;
	DROP TABLE #PeakByClose_LaterCounters;
	


	--Calculate earlier amplitudes.
	SELECT 
		counters.[DateIndex], 
		counters.[Close] - MIN(l.[Low]) AS [EarlierAmplitude],
		SUM(counters.[Close] - l.[Low]) AS [TotalArea]
	INTO
		#PeakByClose_EarlierAmplitudes
	FROM 
		#PeakByClose_Counters counters
		LEFT JOIN
		#LowPrices l
		ON counters.[DateIndex] > l.[DateIndex] AND counters.[DateIndex] <= (l.[DateIndex] + counters.[earlierCounter])
	GROUP BY
		counters.[DateIndex], counters.[Close]

	--Calculate later amplitudes.
	SELECT
		counters.[DateIndex], 
		counters.[Close] - MIN(l.[Low]) AS [LaterAmplitude],
		SUM(counters.[Close] - l.[Low]) AS [TotalArea]
	INTO
		#PeakByClose_LaterAmplitudes
	FROM 
		#PeakByClose_Counters counters
		LEFT JOIN
		#LowPrices l
		ON counters.[DateIndex] < l.[DateIndex] AND counters.[DateIndex] >= (l.[DateIndex] - counters.[laterCounter])
	GROUP BY
		counters.[DateIndex], counters.[Close]

	--Calculate earlier changes
	SELECT
		d.*, d.[Close] - c10.[Close] AS [EarlierChange10]
	INTO
		#PeakByClose_EarlierChanges
	FROM
		(SELECT
			c.*, c.[Close] - c5.[Close] AS [EarlierChange5]
		FROM
			(SELECT 
				b.*, b.[Close] - c3.[Close] AS [EarlierChange3]
			FROM
				(SELECT
					a.*, a.[Close] - c2.[Close] AS [EarlierChange2]
				FROM
					(SELECT 
						ex.[DateIndex], ex.[Close], ex.[Close] - c.[Close] AS [EarlierChange1]
					FROM 
						#PeaksByCloseExtremaIndices ex LEFT JOIN #MaxOpenClosePrices c ON ex.[DateIndex] - c.[DateIndex] = 1) a
					LEFT JOIN #MaxOpenClosePrices c2 ON a.[DateIndex] - c2.[DateIndex] = 2) b
				LEFT JOIN #MaxOpenClosePrices c3 ON b.[DateIndex] - c3.[DateIndex] = 3) c
			LEFT JOIN #MaxOpenClosePrices c5 ON c.[DateIndex] - c5.[DateIndex] = 5) d
		LEFT JOIN #MaxOpenClosePrices c10 ON d.[DateIndex] - c10.[DateIndex] = 10

	--Calculate later changes
	SELECT
		d.*, d.[Close] - c10.[Close] AS [LaterChange10]
	INTO
		#PeakByClose_LaterChanges
	FROM
		(SELECT
			c.*, c.[Close] - c5.[Close] AS [LaterChange5]
		FROM
			(SELECT 
				b.*, b.[Close] - c3.[Close] AS [LaterChange3]
			FROM
				(SELECT
					a.*, a.[Close] - c2.[Close] AS [LaterChange2]
				FROM
					(SELECT 
						ex.[DateIndex], ex.[Close], ex.[Close] - c.[Close] AS [LaterChange1]
					FROM 
						#PeaksByCloseExtremaIndices ex LEFT JOIN #MaxOpenClosePrices c ON ex.[DateIndex] - c.[DateIndex] = -1) a
					LEFT JOIN #MaxOpenClosePrices c2 ON a.[DateIndex] - c2.[DateIndex] = -2) b
				LEFT JOIN #MaxOpenClosePrices c3 ON b.[DateIndex] - c3.[DateIndex] = -3) c
			LEFT JOIN #MaxOpenClosePrices c5 ON c.[DateIndex] - c5.[DateIndex] = -5) d
		LEFT JOIN #MaxOpenClosePrices c10 ON d.[DateIndex] - c10.[DateIndex] = -10
	
	--Combine all extremum measurements all together.
	SELECT
		ex.[DateIndex],
		@shareId AS [ShareId],
		1 AS [ExtremumType],
		c.[earlierCounter],
		c.[laterCounter],
		ea.[EarlierAmplitude],
		ea.[TotalArea] AS [EarlierTotalArea],
		ea.[TotalArea] / c.[earlierCounter] AS [EarlierAverageArea],
		la.[LaterAmplitude],
		la.[TotalArea] AS [LaterTotalArea],
		la.[TotalArea] / c.[laterCounter] AS [LaterAverageArea],
		ec.[EarlierChange1],
		ec.[EarlierChange2],
		ec.[EarlierChange3],
		ec.[EarlierChange5],
		ec.[EarlierChange10],
		lc.[LaterChange1],
		lc.[LaterChange2],
		lc.[LaterChange3],
		lc.[LaterChange5],
		lc.[LaterChange10]
	INTO
		#PeaksByClose
	FROM
		#PeaksByCloseExtremaIndices ex
		INNER JOIN #PeakByClose_Counters c ON ex.[DateIndex] = c.[DateIndex]
		INNER JOIN #PeakByClose_EarlierAmplitudes ea ON ex.[DateIndex] = ea.[DateIndex]
		INNER JOIN #PeakByClose_LaterAmplitudes la ON ex.[DateIndex] = la.[DateIndex]
		INNER JOIN #PeakByClose_EarlierChanges ec ON ex.[DateIndex] = ec.[DateIndex]
		INNER JOIN #PeakByClose_LaterChanges lc ON ex.[DateIndex] = lc.[DateIndex]



--Clean up.
DROP TABLE #PeakByClose_Counters;
DROP TABLE #PeakByClose_EarlierAmplitudes;
DROP TABLE #PeakByClose_LaterAmplitudes;
DROP TABLE #PeakByClose_EarlierChanges;
DROP TABLE #PeakByClose_LaterChanges;
DROP TABLE #PeaksByCloseExtremaIndices;












--PEAK-BY-HIGH
--Select extrema for further evaluation.
SELECT
	a.*
INTO
	#PeakByHigh_InitialFiltered
FROM
	(SELECT
		h1.*
	FROM 
		#HighPrices h1
		LEFT JOIN
		#HighPrices h2
		ON h1.[DateIndex] = (h2.[DateIndex] + 1)
	WHERE
		h2.[High] IS NULL OR h2.[High] < h1.[High]) a
	LEFT JOIN
	#HighPrices h3
	ON a.[DateIndex] = (h3.[DateIndex] - 1)
WHERE
	h3.[High] IS NULL OR h3.[High] <= a.[High]

SELECT
	a.*
INTO
	#PeaksByHighExtremaIndices
FROM
	(SELECT
		tif.*
	FROM
		#PeakByHigh_InitialFiltered tif
		LEFT JOIN
		#HighPrices h
		ON tif.[DateIndex] > h.[DateIndex] AND tif.[DateIndex] <= (h.[DateIndex] + @minWorse) AND tif.[High] <= h.[High]
	WHERE
		h.[DateIndex] IS NULL) a

	LEFT JOIN
	#HighPrices h2
	ON a.[DateIndex] < h2.[DateIndex] AND a.[DateIndex] >= (h2.[DateIndex] - @minWorse) AND a.[High] < h2.[High]

	WHERE
		h2.[DateIndex] IS NULL

DROP TABLE #PeakByHigh_InitialFiltered;



--Evaluate extrema.
	--Calculate earlier counters.
	SELECT
		fph.[DateIndex],
		IIF(fph.[FirstPrevHigher] IS NULL, 
			IIF(fph.[DateIndex] - @firstQuote > @maxChecked, @maxChecked, fph.[DateIndex] - @firstQuote),
			fph.[DateIndex] - fph.[FirstPrevHigher] - 1) AS [earlierCounter]
	INTO
		#PeakByHigh_EarlierCounters
	FROM
		(SELECT 
			ex.[DateIndex], MAX(h.[DateIndex]) AS [FirstPrevHigher]
		FROM 
			#PeaksByHighExtremaIndices ex
			LEFT JOIN
			#HighPrices h
			ON ex.[DateIndex] > h.[DateIndex] AND ex.[DateIndex] <= (h.[DateIndex] + @maxChecked) AND ex.[High] < h.[High]
		GROUP BY
			ex.[DateIndex]) fph

	--Calculate later counters.
	SELECT
		fnh.[DateIndex],
		IIF(fnh.[FirstNextHigher] IS NULL, 
			IIF(@lastQuote - fnh.[DateIndex] > @maxChecked, @maxChecked, @lastQuote - fnh.[DateIndex]),
			fnh.[FirstNextHigher] - fnh.[DateIndex] - 1) AS [laterCounter]
	INTO
		#PeakByHigh_LaterCounters
	FROM
		(SELECT 
			ex.[DateIndex], MIN(h.[DateIndex]) AS [FirstNextHigher]
		FROM 
			#PeaksByHighExtremaIndices ex
			LEFT JOIN
			#HighPrices h
			ON ex.[DateIndex] < h.[DateIndex] AND ex.[DateIndex] >= (h.[DateIndex] - @maxChecked) AND ex.[High] < h.[High]
		GROUP BY
			ex.[DateIndex]) fnh

	--Combine counters.
	SELECT
		ex.[DateIndex],
		ex.[High],
		ec.[earlierCounter],
		lc.[laterCounter]
	INTO
		#PeakByHigh_Counters
	FROM
		#PeaksByHighExtremaIndices ex
		INNER JOIN #PeakByHigh_EarlierCounters ec ON ex.[DateIndex] = ec.[DateIndex]
		INNER JOIN #PeakByHigh_LaterCounters lc ON ex.[DateIndex] = lc.[DateIndex]

	DROP TABLE #PeakByHigh_EarlierCounters;
	DROP TABLE #PeakByHigh_LaterCounters;
	

	--Calculate earlier amplitudes.
	SELECT
		counters.[DateIndex], 
		counters.[High] - MIN(l.[Low]) AS [EarlierAmplitude],
		SUM(counters.[High] - l.[Low]) AS [TotalArea]
	INTO
		#PeakByHigh_EarlierAmplitudes
	FROM 
		#PeakByHigh_Counters counters
		LEFT JOIN
		#LowPrices l
		ON counters.[DateIndex] > l.[DateIndex] AND counters.[DateIndex] <= (l.[DateIndex] + counters.[earlierCounter])
	GROUP BY
		counters.[DateIndex], counters.[High]

	--Calculate later amplitudes.
	SELECT
		counters.[DateIndex], 
		counters.[High] - MIN(l.[Low]) AS [LaterAmplitude],
		SUM(counters.[High] - l.[Low]) AS [TotalArea]
	INTO
		#PeakByHigh_LaterAmplitudes
	FROM 
		#PeakByHigh_Counters counters
		LEFT JOIN
		#LowPrices l
		ON counters.[DateIndex] < l.[DateIndex] AND counters.[DateIndex] >= (l.[DateIndex] - counters.[laterCounter])
	GROUP BY
		counters.[DateIndex], counters.[High]



	--Calculate earlier changes
	SELECT
		d.*, d.[Close] - c10.[Close] AS [EarlierChange10]
	INTO
		#PeakByHigh_EarlierChanges
	FROM
		(SELECT
			c.*, c.[Close] - c5.[Close] AS [EarlierChange5]
		FROM
			(SELECT 
				b.*, b.[Close] - c3.[Close] AS [EarlierChange3]
			FROM
				(SELECT
					a.*, a.[Close] - c2.[Close] AS [EarlierChange2]
				FROM
					(SELECT 
						base.[DateIndex], base.[Close], base.[Close] - c.[Close] AS [EarlierChange1]
					FROM 
						(SELECT ex.[DateIndex], c.[Close] FROM #PeaksByHighExtremaIndices ex INNER JOIN #MaxOpenClosePrices c ON ex.[DateIndex] = c.[DateIndex]) base
						LEFT JOIN #MaxOpenClosePrices c ON base.[DateIndex] - c.[DateIndex] = 1) a
					LEFT JOIN #MaxOpenClosePrices c2 ON a.[DateIndex] - c2.[DateIndex] = 2) b
				LEFT JOIN #MaxOpenClosePrices c3 ON b.[DateIndex] - c3.[DateIndex] = 3) c
			LEFT JOIN #MaxOpenClosePrices c5 ON c.[DateIndex] - c5.[DateIndex] = 5) d
		LEFT JOIN #MaxOpenClosePrices c10 ON d.[DateIndex] - c10.[DateIndex] = 10

	--Calculate later changes
	SELECT
		d.*, d.[Close] - c10.[Close] AS [LaterChange10]
	INTO
		#PeakByHigh_LaterChanges
	FROM
		(SELECT
			c.*, c.[Close] - c5.[Close] AS [LaterChange5]
		FROM
			(SELECT 
				b.*, b.[Close] - c3.[Close] AS [LaterChange3]
			FROM
				(SELECT
					a.*, a.[Close] - c2.[Close] AS [LaterChange2]
				FROM
					(SELECT 
						base.[DateIndex], base.[Close], base.[Close] - c.[Close] AS [LaterChange1]
					FROM 
						(SELECT ex.[DateIndex], c.[Close] FROM #PeaksByHighExtremaIndices ex INNER JOIN #MaxOpenClosePrices c ON ex.[DateIndex] = c.[DateIndex]) base
						LEFT JOIN #MaxOpenClosePrices c ON base.[DateIndex] - c.[DateIndex] = -1) a
					LEFT JOIN #MaxOpenClosePrices c2 ON a.[DateIndex] - c2.[DateIndex] = -2) b
				LEFT JOIN #MaxOpenClosePrices c3 ON b.[DateIndex] - c3.[DateIndex] = -3) c
			LEFT JOIN #MaxOpenClosePrices c5 ON c.[DateIndex] - c5.[DateIndex] = -5) d
		LEFT JOIN #MaxOpenClosePrices c10 ON d.[DateIndex] - c10.[DateIndex] = -10
	


	--Combine all extremum measurements all together.
	SELECT
		ex.[DateIndex],
		@shareId AS [ShareId],
		2 AS [ExtremumType],
		c.[earlierCounter],
		c.[laterCounter],
		ea.[EarlierAmplitude],
		ea.[TotalArea] AS [EarlierTotalArea],
		ea.[TotalArea] / c.[earlierCounter] AS [EarlierAverageArea],
		la.[LaterAmplitude],
		la.[TotalArea] AS [LaterTotalArea],
		la.[TotalArea] / c.[laterCounter] AS [LaterAverageArea],
		ec.[EarlierChange1],
		ec.[EarlierChange2],
		ec.[EarlierChange3],
		ec.[EarlierChange5],
		ec.[EarlierChange10],
		lc.[LaterChange1],
		lc.[LaterChange2],
		lc.[LaterChange3],
		lc.[LaterChange5],
		lc.[LaterChange10]
	INTO
		#PeaksByHigh
	FROM
		#PeaksByHighExtremaIndices ex
		INNER JOIN #PeakByHigh_Counters c ON ex.[DateIndex] = c.[DateIndex]
		INNER JOIN #PeakByHigh_EarlierAmplitudes ea ON ex.[DateIndex] = ea.[DateIndex]
		INNER JOIN #PeakByHigh_LaterAmplitudes la ON ex.[DateIndex] = la.[DateIndex]
		INNER JOIN #PeakByHigh_EarlierChanges ec ON ex.[DateIndex] = ec.[DateIndex]
		INNER JOIN #PeakByHigh_LaterChanges lc ON ex.[DateIndex] = lc.[DateIndex]

--Clean up.
DROP TABLE #PeakByHigh_Counters;
DROP TABLE #PeakByHigh_EarlierAmplitudes;
DROP TABLE #PeakByHigh_LaterAmplitudes;
DROP TABLE #PeakByHigh_EarlierChanges;
DROP TABLE #PeakByHigh_LaterChanges;
DROP TABLE #PeaksByHighExtremaIndices;
























--TROUGH-BY-CLOSE
--Select extrema for further evaluation.
SELECT
	a.*
INTO
	#TroughByClose_InitialFiltered
FROM
	(SELECT
		c1.*
	FROM 
		#MinOpenClosePrices c1
		LEFT JOIN
		#MinOpenClosePrices c2
		ON c1.[DateIndex] = (c2.[DateIndex] + 1)
	WHERE
		c2.[Close] IS NULL OR c2.[Close] > c1.[Close]) a
	LEFT JOIN
	#MinOpenClosePrices c3
	ON a.[DateIndex] = (c3.[DateIndex] - 1)
WHERE
	c3.[Close] IS NULL OR c3.[Close] >= a.[Close]

SELECT
	a.*
INTO
	#TroughsByCloseExtremaIndices
FROM
	(SELECT
		tif.*
	FROM
		#TroughByClose_InitialFiltered tif
		LEFT JOIN
		#MinOpenClosePrices c
		ON tif.[DateIndex] > c.[DateIndex] AND tif.[DateIndex] <= (c.[DateIndex] + @minWorse) AND tif.[Close] >= c.[Close]
	WHERE
		c.[DateIndex] IS NULL) a

	LEFT JOIN
	#MinOpenClosePrices c2
	ON a.[DateIndex] < c2.[DateIndex] AND a.[DateIndex] >= (c2.[DateIndex] - @minWorse) AND a.[Close] > c2.[Close]

	WHERE
		c2.[DateIndex] IS NULL

DROP TABLE #TroughByClose_InitialFiltered;

--Evaluate extrema.
	--Calculate earlier counters.
	SELECT
		fph.[DateIndex],
		IIF(fph.[FirstPrevLower] IS NULL, 
			IIF(fph.[DateIndex] - @firstQuote > @maxChecked, @maxChecked, fph.[DateIndex] - @firstQuote),
			fph.[DateIndex] - fph.[FirstPrevLower] - 1) AS [earlierCounter]
	INTO
		#TroughByClose_EarlierCounters
	FROM
		(SELECT 
			ex.[DateIndex], MAX(c.[DateIndex]) AS [FirstPrevLower]
		FROM 
			#TroughsByCloseExtremaIndices ex
			LEFT JOIN
			#MinOpenClosePrices c
			ON ex.[DateIndex] > c.[DateIndex] AND ex.[DateIndex] <= (c.[DateIndex] + @maxChecked) AND ex.[Close] > c.[Close]
		GROUP BY
			ex.[DateIndex]) fph
	
	--Calculate later counters.
	SELECT
		fnh.[DateIndex],
		IIF(fnh.[FirstNextLower] IS NULL, 
			IIF(@lastQuote - fnh.[DateIndex] > @maxChecked, @maxChecked, @lastQuote - fnh.[DateIndex]),
			fnh.[FirstNextLower] - fnh.[DateIndex] - 1) AS [laterCounter]
	INTO
		#TroughByClose_LaterCounters
	FROM
		(SELECT 
			ex.[DateIndex], MIN(c.[DateIndex]) AS [FirstNextLower]
		FROM 
			#TroughsByCloseExtremaIndices ex
			LEFT JOIN
			#MinOpenClosePrices c
			ON ex.[DateIndex] < c.[DateIndex] AND ex.[DateIndex] >= (c.[DateIndex] - @maxChecked) AND ex.[Close] > c.[Close]
		GROUP BY
			ex.[DateIndex]) fnh

	--Combine counters.
	SELECT
		ex.[DateIndex],
		ex.[Close],
		ec.[earlierCounter],
		lc.[laterCounter]
	INTO
		#TroughByClose_Counters
	FROM
		#TroughsByCloseExtremaIndices ex
		INNER JOIN #TroughByClose_EarlierCounters ec ON ex.[DateIndex] = ec.[DateIndex]
		INNER JOIN #TroughByClose_LaterCounters lc ON ex.[DateIndex] = lc.[DateIndex]

	DROP TABLE #TroughByClose_EarlierCounters;
	DROP TABLE #TroughByClose_LaterCounters;

	
	--Calculate earlier amplitudes.
	SELECT 
		counters.[DateIndex], 
		MAX(h.[High]) - counters.[Close] AS [EarlierAmplitude],
		SUM(h.[High] - counters.[Close]) AS [TotalArea]
	INTO
		#TroughByClose_EarlierAmplitudes
	FROM 
		#TroughByClose_Counters counters
		LEFT JOIN
		#HighPrices h
		ON counters.[DateIndex] > h.[DateIndex] AND counters.[DateIndex] <= (h.[DateIndex] + counters.[earlierCounter])
	GROUP BY
		counters.[DateIndex], counters.[Close]

	--Calculate later amplitudes.
	SELECT
		counters.[DateIndex], 
		MAX(h.[High]) - counters.[Close] AS [LaterAmplitude],
		SUM(h.[High] - counters.[Close]) AS [TotalArea]
	INTO
		#TroughByClose_LaterAmplitudes
	FROM 
		#TroughByClose_Counters counters
		LEFT JOIN
		#HighPrices h
		ON counters.[DateIndex] < h.[DateIndex] AND counters.[DateIndex] >= (h.[DateIndex] - counters.[laterCounter])
	GROUP BY
		counters.[DateIndex], counters.[Close]
	
	--Calculate earlier changes
	SELECT
		d.*, d.[Close] - c10.[Close] AS [EarlierChange10]
	INTO
		#TroughByClose_EarlierChanges
	FROM
		(SELECT
			c.*, c.[Close] - c5.[Close] AS [EarlierChange5]
		FROM
			(SELECT 
				b.*, b.[Close] - c3.[Close] AS [EarlierChange3]
			FROM
				(SELECT
					a.*, a.[Close] - c2.[Close] AS [EarlierChange2]
				FROM
					(SELECT 
						ex.[DateIndex], ex.[Close], ex.[Close] - c.[Close] AS [EarlierChange1]
					FROM 
						#TroughsByCloseExtremaIndices ex LEFT JOIN #MinOpenClosePrices c ON ex.[DateIndex] - c.[DateIndex] = 1) a
					LEFT JOIN #MinOpenClosePrices c2 ON a.[DateIndex] - c2.[DateIndex] = 2) b
				LEFT JOIN #MinOpenClosePrices c3 ON b.[DateIndex] - c3.[DateIndex] = 3) c
			LEFT JOIN #MinOpenClosePrices c5 ON c.[DateIndex] - c5.[DateIndex] = 5) d
		LEFT JOIN #MinOpenClosePrices c10 ON d.[DateIndex] - c10.[DateIndex] = 10

	--Calculate later changes
	SELECT
		d.*, d.[Close] - c10.[Close] AS [LaterChange10]
	INTO
		#TroughByClose_LaterChanges
	FROM
		(SELECT
			c.*, c.[Close] - c5.[Close] AS [LaterChange5]
		FROM
			(SELECT 
				b.*, b.[Close] - c3.[Close] AS [LaterChange3]
			FROM
				(SELECT
					a.*, a.[Close] - c2.[Close] AS [LaterChange2]
				FROM
					(SELECT 
						ex.[DateIndex], ex.[Close], ex.[Close] - c.[Close] AS [LaterChange1]
					FROM 
						#TroughsByCloseExtremaIndices ex LEFT JOIN #MinOpenClosePrices c ON ex.[DateIndex] - c.[DateIndex] = -1) a
					LEFT JOIN #MinOpenClosePrices c2 ON a.[DateIndex] - c2.[DateIndex] = -2) b
				LEFT JOIN #MinOpenClosePrices c3 ON b.[DateIndex] - c3.[DateIndex] = -3) c
			LEFT JOIN #MinOpenClosePrices c5 ON c.[DateIndex] - c5.[DateIndex] = -5) d
		LEFT JOIN #MinOpenClosePrices c10 ON d.[DateIndex] - c10.[DateIndex] = -10


	
	--Combine all extremum measurements all together.
	SELECT
		ex.[DateIndex],
		@shareId AS [ShareId],
		3 AS [ExtremumType],
		c.[earlierCounter],
		c.[laterCounter],
		ea.[EarlierAmplitude],
		ea.[TotalArea] AS [EarlierTotalArea],
		ea.[TotalArea] / c.[earlierCounter] AS [EarlierAverageArea],
		la.[LaterAmplitude],
		la.[TotalArea] AS [LaterTotalArea],
		la.[TotalArea] / c.[laterCounter] AS [LaterAverageArea],
		ec.[EarlierChange1],
		ec.[EarlierChange2],
		ec.[EarlierChange3],
		ec.[EarlierChange5],
		ec.[EarlierChange10],
		lc.[LaterChange1],
		lc.[LaterChange2],
		lc.[LaterChange3],
		lc.[LaterChange5],
		lc.[LaterChange10]
	INTO
		#TroughsByClose
	FROM
		#TroughsByCloseExtremaIndices ex
		INNER JOIN #TroughByClose_Counters c ON ex.[DateIndex] = c.[DateIndex]
		INNER JOIN #TroughByClose_EarlierAmplitudes ea ON ex.[DateIndex] = ea.[DateIndex]
		INNER JOIN #TroughByClose_LaterAmplitudes la ON ex.[DateIndex] = la.[DateIndex]
		INNER JOIN #TroughByClose_EarlierChanges ec ON ex.[DateIndex] = ec.[DateIndex]
		INNER JOIN #TroughByClose_LaterChanges lc ON ex.[DateIndex] = lc.[DateIndex]


--Clean up.
DROP TABLE #TroughByClose_Counters;
DROP TABLE #TroughByClose_EarlierAmplitudes;
DROP TABLE #TroughByClose_LaterAmplitudes;
DROP TABLE #TroughByClose_EarlierChanges;
DROP TABLE #TroughByClose_LaterChanges;
DROP TABLE #TroughsByCloseExtremaIndices;


















--TROUGH-BY-LOW
--Select extrema for further evaluation.
SELECT
	a.*
INTO
	#TroughByLow_InitialFiltered
FROM
	(SELECT
		l1.*
	FROM 
		#LowPrices l1
		LEFT JOIN
		#LowPrices l2
		ON l1.[DateIndex] = (l2.[DateIndex] + 1)
	WHERE
		l2.[Low] IS NULL OR l2.[Low] > l1.[Low]) a
	LEFT JOIN
	#LowPrices l3
	ON a.[DateIndex] = (l3.[DateIndex] - 1)
WHERE
	l3.[Low] IS NULL OR l3.[Low] >= a.[Low]

SELECT
	a.*
INTO
	#TroughsByLowExtremaIndices
FROM
	(SELECT
		tif.*
	FROM
		#TroughByLow_InitialFiltered tif
		LEFT JOIN
		#LowPrices l
		ON tif.[DateIndex] > l.[DateIndex] AND tif.[DateIndex] <= (l.[DateIndex] + @minWorse) AND tif.[Low] >= l.[Low]
	WHERE
		l.[DateIndex] IS NULL) a

	LEFT JOIN
	#LowPrices l2
	ON a.[DateIndex] < l2.[DateIndex] AND a.[DateIndex] >= (l2.[DateIndex] - @minWorse) AND a.[Low] > l2.[Low]

	WHERE
		l2.[DateIndex] IS NULL

DROP TABLE #TroughByLow_InitialFiltered;

--Evaluate extrema.
	--Calculate earlier counters.
	SELECT
		fph.[DateIndex],
		IIF(fph.[FirstPrevLower] IS NULL, 
			IIF(fph.[DateIndex] - @firstQuote > @maxChecked, @maxChecked, fph.[DateIndex] - @firstQuote),
			fph.[DateIndex] - fph.[FirstPrevLower] - 1) AS [earlierCounter]
	INTO
		#TroughByLow_EarlierCounters
	FROM
		(SELECT 
			ex.[DateIndex], MAX(l.[DateIndex]) AS [FirstPrevLower]
		FROM 
			#TroughsByLowExtremaIndices ex
			LEFT JOIN
			#LowPrices l
			ON ex.[DateIndex] > l.[DateIndex] AND ex.[DateIndex] <= (l.[DateIndex] + @maxChecked) AND ex.[Low] > l.[Low]
		GROUP BY
			ex.[DateIndex]) fph
	
	--Calculate later counters.
	SELECT
		fnh.[DateIndex],
		IIF(fnh.[FirstNextLower] IS NULL, 
			IIF(@lastQuote - fnh.[DateIndex] > @maxChecked, @maxChecked, @lastQuote - fnh.[DateIndex]),
			fnh.[FirstNextLower] - fnh.[DateIndex] - 1) AS [laterCounter]
	INTO
		#TroughByLow_LaterCounters
	FROM
		(SELECT
			ex.[DateIndex], MIN(l.[DateIndex]) AS [FirstNextLower]
		FROM 
			#TroughsByLowExtremaIndices ex
			LEFT JOIN
			#LowPrices l
			ON ex.[DateIndex] < l.[DateIndex] AND ex.[DateIndex] >= (l.[DateIndex] - @maxChecked) AND ex.[Low] > l.[Low]
		GROUP BY
			ex.[DateIndex]) fnh

	--Combine counters.
	SELECT
		ex.[DateIndex],
		ex.[Low],
		ec.[earlierCounter],
		lc.[laterCounter]
	INTO
		#TroughByLow_Counters
	FROM
		#TroughsByLowExtremaIndices ex
		INNER JOIN #TroughByLow_EarlierCounters ec ON ex.[DateIndex] = ec.[DateIndex]
		INNER JOIN #TroughByLow_LaterCounters lc ON ex.[DateIndex] = lc.[DateIndex]

	DROP TABLE #TroughByLow_EarlierCounters;
	DROP TABLE #TroughByLow_LaterCounters;


	--Calculate earlier amplitudes.
	SELECT 
		counters.[DateIndex], 
		MAX(h.[High]) - counters.[Low] AS [EarlierAmplitude],
		SUM(h.[High] - counters.[Low]) AS [TotalArea]
	INTO
		#TroughByLow_EarlierAmplitudes
	FROM 
		#TroughByLow_Counters counters
		LEFT JOIN
		#HighPrices h
		ON counters.[DateIndex] > h.[DateIndex] AND counters.[DateIndex] <= (h.[DateIndex] + counters.[earlierCounter])
	GROUP BY
		counters.[DateIndex], counters.[Low]

	--Calculate later amplitudes.
	SELECT
		counters.[DateIndex], 
		MAX(h.[High]) - counters.[Low] AS [LaterAmplitude],
		SUM(h.[High] - counters.[Low]) AS [TotalArea]
	INTO
		#TroughByLow_LaterAmplitudes
	FROM 
		#TroughByLow_Counters counters
		LEFT JOIN
		#HighPrices h
		ON counters.[DateIndex] < h.[DateIndex] AND counters.[DateIndex] >= (h.[DateIndex] - counters.[laterCounter])
	GROUP BY
		counters.[DateIndex], counters.[Low]
	
	--Calculate earlier changes
	SELECT
		d.*, d.[Close] - c10.[Close] AS [EarlierChange10]
	INTO
		#TroughByLow_EarlierChanges
	FROM
		(SELECT
			c.*, c.[Close] - c5.[Close] AS [EarlierChange5]
		FROM
			(SELECT 
				b.*, b.[Close] - c3.[Close] AS [EarlierChange3]
			FROM
				(SELECT
					a.*, a.[Close] - c2.[Close] AS [EarlierChange2]
				FROM
					(SELECT 
						base.[DateIndex], base.[Close], base.[Close] - c.[Close] AS [EarlierChange1]
					FROM 
						(SELECT ex.[DateIndex], c.[Close] FROM #TroughsByLowExtremaIndices ex INNER JOIN #MinOpenClosePrices c ON ex.[DateIndex] = c.[DateIndex]) base
						LEFT JOIN #MinOpenClosePrices c ON base.[DateIndex] - c.[DateIndex] = 1) a
					LEFT JOIN #MinOpenClosePrices c2 ON a.[DateIndex] - c2.[DateIndex] = 2) b
				LEFT JOIN #MinOpenClosePrices c3 ON b.[DateIndex] - c3.[DateIndex] = 3) c
			LEFT JOIN #MinOpenClosePrices c5 ON c.[DateIndex] - c5.[DateIndex] = 5) d
		LEFT JOIN #MinOpenClosePrices c10 ON d.[DateIndex] - c10.[DateIndex] = 10

	--Calculate later changes
	SELECT
		d.*, d.[Close] - c10.[Close] AS [LaterChange10]
	INTO
		#TroughByLow_LaterChanges
	FROM
		(SELECT
			c.*, c.[Close] - c5.[Close] AS [LaterChange5]
		FROM
			(SELECT
				b.*, b.[Close] - c3.[Close] AS [LaterChange3]
			FROM
				(SELECT
					a.*, a.[Close] - c2.[Close] AS [LaterChange2]
				FROM
					(SELECT
						base.[DateIndex], base.[Close], base.[Close] - c.[Close] AS [LaterChange1]
					FROM 
						(SELECT ex.[DateIndex], c.[Close] FROM #TroughsByLowExtremaIndices ex INNER JOIN #MinOpenClosePrices c ON ex.[DateIndex] = c.[DateIndex]) base
						LEFT JOIN #MinOpenClosePrices c ON base.[DateIndex] - c.[DateIndex] = -1) a
					LEFT JOIN #MinOpenClosePrices c2 ON a.[DateIndex] - c2.[DateIndex] = -2) b
				LEFT JOIN #MinOpenClosePrices c3 ON b.[DateIndex] - c3.[DateIndex] = -3) c
			LEFT JOIN #MinOpenClosePrices c5 ON c.[DateIndex] - c5.[DateIndex] = -5) d
		LEFT JOIN #MinOpenClosePrices c10 ON d.[DateIndex] - c10.[DateIndex] = -10


	
	--Combine all extremum measurements all together.
	SELECT
		ex.[DateIndex],
		@shareId AS [ShareId],
		4 AS [ExtremumType],
		c.[earlierCounter],
		c.[laterCounter],
		ea.[EarlierAmplitude],
		ea.[TotalArea] AS [EarlierTotalArea],
		ea.[TotalArea] / c.[earlierCounter] AS [EarlierAverageArea],
		la.[LaterAmplitude],
		la.[TotalArea] AS [LaterTotalArea],
		la.[TotalArea] / c.[laterCounter] AS [LaterAverageArea],
		ec.[EarlierChange1],
		ec.[EarlierChange2],
		ec.[EarlierChange3],
		ec.[EarlierChange5],
		ec.[EarlierChange10],
		lc.[LaterChange1],
		lc.[LaterChange2],
		lc.[LaterChange3],
		lc.[LaterChange5],
		lc.[LaterChange10]
	INTO
		#TroughsByLow
	FROM
		#TroughsByLowExtremaIndices ex
		INNER JOIN #TroughByLow_Counters c ON ex.[DateIndex] = c.[DateIndex]
		INNER JOIN #TroughByLow_EarlierAmplitudes ea ON ex.[DateIndex] = ea.[DateIndex]
		INNER JOIN #TroughByLow_LaterAmplitudes la ON ex.[DateIndex] = la.[DateIndex]
		INNER JOIN #TroughByLow_EarlierChanges ec ON ex.[DateIndex] = ec.[DateIndex]
		INNER JOIN #TroughByLow_LaterChanges lc ON ex.[DateIndex] = lc.[DateIndex]


--Clean up.
DROP TABLE #TroughByLow_Counters;
DROP TABLE #TroughByLow_EarlierAmplitudes;
DROP TABLE #TroughByLow_LaterAmplitudes;
DROP TABLE #TroughByLow_EarlierChanges;
DROP TABLE #TroughByLow_LaterChanges;
DROP TABLE #TroughsByLowExtremaIndices;







--Combine all extrema all together.
DELETE FROM [dbo].[extrema] WHERE [ShareId] = @shareId;
INSERT INTO [dbo].[extrema] (
		[DateIndex], [ShareId], [ExtremumType], [earlierCounter], [laterCounter], [EarlierAmplitude], [EarlierTotalArea], [EarlierAverageArea], [LaterAmplitude], [LaterTotalArea], [LaterAverageArea],
		[EarlierChange1], [EarlierChange2], [EarlierChange3], [EarlierChange5], [EarlierChange10], [LaterChange1], [LaterChange2], [LaterChange3], [LaterChange5], [LaterChange10])
SELECT
	*
FROM
	(SELECT * FROM #PeaksByClose UNION ALL
	SELECT * FROM #PeaksByHigh UNION ALL
	SELECT * FROM #TroughsByClose UNION ALL
	SELECT * FROM #TroughsByLow) a




----Clean up.
DROP TABLE #MaxOpenClosePrices;
DROP TABLE #MinOpenClosePrices;
DROP TABLE #LowPrices;
DROP TABLE #HighPrices;
DROP TABLE #PeaksByClose;
DROP TABLE #PeaksByHigh;
DROP TABLE #TroughsByClose;
DROP TABLE #TroughsByLow;








-- Update values
BEGIN

	UPDATE e
	SET
		[Value] = p.[EarlierAmplitudePoints] + p.[LaterAmplitudePoints] + p.[EarlierCounterPoints] + p.[LaterCounterPoints] + p.[EarlierVolatilityPoints] + p.[LaterVolatilityPoints]
	FROM
		[dbo].[extrema] e
		JOIN 
		(SELECT
			a.*,
			a.[EarlierAmplitude] / IIF(a.[ExtremumType] > 2, a.[EarlierAmplitude] + a.[price], a.[price]) * 5 AS [EarlierAmplitudePoints],
			a.[LaterAmplitude] / IIF(a.[ExtremumType] > 2, a.[LaterAmplitude] + a.[price], a.[price]) * 5 AS [LaterAmplitudePoints],
			LOG(LOG(IIF(a.[EarlierCounter] < 5.0, 5.0, a.[EarlierCounter]* 1.0), 260) * 10.0, 10) * 35 AS [EarlierCounterPoints],
			LOG(LOG(IIF(a.[LaterCounter] < 5.0, 5.0, a.[LaterCounter]* 1.0), 260) * 10.0, 10) * 35 AS [LaterCounterPoints],
			IIF(a.[EarlierAverageArea] > 100, 10, [EarlierAverageArea] / 10) AS [EarlierVolatilityPoints],
			IIF(a.[LaterAverageArea] > 100, 10, [LaterAverageArea] / 10) AS [LaterVolatilityPoints]
		FROM
			(SELECT
				q.[Date],
				CASE e.[ExtremumType]
					WHEN 1 THEN IIF(q.[Close] > q.[High], q.[Close], q.[High])
					WHEN 2 THEN q.[High]
					WHEN 3 THEN IIF(q.[Close] < q.[High], q.[Close], q.[High])
					WHEN 4 THEN q.[Low]
				END AS [price],
				e.*
			FROM
				[dbo].[extrema] e
				LEFT JOIN (SELECT * FROM [dbo].[quotes] WHERE [ShareId] = 1) q
				ON e.[DateIndex] = q.[DateIndex]) a
				) p
			ON e.[Id] = p.[Id];


	--Filter extrema with too low value.
	BEGIN

		DELETE 
		FROM 
			[dbo].[extrema]
		WHERE
			[Value] < @minValue OR [Value] IS NUll;

		UPDATE
			[dbo].[extrema]
		SET
			[Value] = ([Value] - 50) * 2;

	END

END


SELECT
	q.[Date], e.*
FROM
	[dbo].[extrema] e
	LEFT JOIN (SELECT * FROM [dbo].[quotes] WHERE [ShareId] = 1) q
	ON e.[DateIndex] = q.[DateIndex]
ORDER BY
	[DateIndex] ASC;


COMMIT TRANSACTION;