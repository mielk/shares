USE [shares];

GO

--Wyznaczanie Extremum Group:
--jeżeli pomiędzy dwoma wierzchołkami tego samego typu nie ma wierzchołka przeciwnego typu, to są one grupowane do jednego ExtremumGroup

BEGIN TRANSACTION

DECLARE @shareId INT, @stepPrecision FLOAT, @stepFactor FLOAT;
DECLARE @maxDistanceInTrendlinePoints INT, @minDistanceInTrendlinePoints INT;
SET @shareId = 1;
SET @stepPrecision = 1;
SET @stepFactor = POWER(1, @stepPrecision);
SET @maxDistanceInTrendlinePoints = 150; --;
SET @minDistanceInTrendlinePoints = 3;


--Temporary tables.
SELECT * INTO #Extrema FROM [dbo].[extrema] WHERE [ShareId] = @shareId;
SELECT [Id], [DateIndex], [Value] INTO #PeaksByClose FROM #Extrema WHERE [ExtremumType] = 1;
SELECT [Id], [DateIndex], [Value] INTO #PeaksByHigh FROM #Extrema WHERE [ExtremumType] = 2;
SELECT [Id], [DateIndex], [Value] INTO #TroughsByClose FROM #Extrema WHERE [ExtremumType] = 3;
SELECT [Id], [DateIndex], [Value] INTO #TroughsByLow FROM #Extrema WHERE [ExtremumType] = 4;
SELECT [DateIndex], [Date], [Open], [Low], [High], [Close] INTO #Quotes FROM [dbo].[quotes] WHERE [ShareId] = @shareId;


--Create combined sets of extrema.
SELECT
	*
INTO
	#CombinedPeaks
FROM
	(SELECT *, 1 AS [type] FROM #PeaksByClose
	UNION ALL
	SELECT *, 2 AS [type] FROM #PeaksByHigh) a;

SELECT
	*
INTO
	#CombinedTroughs
FROM
	(SELECT *, 3 AS [type] FROM #TroughsByClose
	UNION ALL
	SELECT *, 4 AS [type] FROM #TroughsByLow) a;


--Get prospective peak groups.
SELECT 
	cp1.[Id] AS [baseId],
	cp1.[DateIndex] AS [baseDateIndex],
	cp1.[Type] AS [baseType],
	cp2.[Id] AS [counterId],
	cp2.[DateIndex] AS [counterDateIndex],
	cp2.[Type] AS [counterType],
	IIF(cp1.[Value] > cp2.[Value], cp1.[Value], cp2.[Value]) AS [value]
INTO
	#PotentialPeakGroups
FROM 
	#CombinedPeaks cp1,
	#CombinedPeaks cp2
WHERE
	cp2.[DateIndex] - cp1.[DateIndex] BETWEEN 0 AND 3
	AND cp1.[Id] <> cp2.[Id]
	AND (cp1.[DateIndex] <> cp2.[DateIndex] OR cp1.[Type] < cp2.[Type]);

SELECT 
	IIF(ppg.[baseType] = 1, ppg.[baseId], ppg.[counterId]) AS [MasterId],
	IIF(ppg.[baseType] = 1, ppg.[baseDateIndex], ppg.[counterDateIndex]) AS [MasterIndex],
	IIF(ppg.[baseType] = 2, ppg.[baseId], ppg.[counterId]) AS [SlaveId],
	IIF(ppg.[baseType] = 2, ppg.[baseDateIndex], ppg.[counterDateIndex]) AS [SlaveIndex],
	IIF(ppg.[baseDateIndex] < ppg.[counterDateIndex], ppg.[baseDateIndex], ppg.[counterDateIndex]) AS [startIndex],
	IIF(ppg.[baseDateIndex] > ppg.[counterDateIndex], ppg.[baseDateIndex], ppg.[counterDateIndex]) AS [endIndex],
	ppg.[value] AS [value]
INTO
	#PeakGroupIds
FROM 
	#PotentialPeakGroups ppg
	LEFT JOIN 
	#CombinedTroughs ct
	ON ct.[DateIndex] BETWEEN ppg.[baseDateIndex] AND ppg.[counterDateIndex]
WHERE
	ct.[DateIndex] IS NULL;

DELETE FROM #PeakGroupIds
WHERE
	[MasterId] IN (SELECT [MasterId] FROM #PeakGroupIds GROUP BY [MasterId] HAVING COUNT(*) > 1)
	AND [MasterIndex] > [SlaveIndex];

DELETE FROM #PeakGroupIds
WHERE
	[SlaveId] IN (SELECT [SlaveId] FROM #PeakGroupIds GROUP BY [SlaveId] HAVING COUNT(*) > 1)
	AND [SlaveIndex] > [MasterIndex];


--Get all extrema that are not part of any group.
SELECT
	IIF(cp.[Type] = 1, cp.[Id], NULL) AS [MasterId],
	IIF(cp.[Type] = 1, cp.[DateIndex], NULL) AS [MasterIndex],
	IIF(cp.[Type] = 2, cp.[Id], NULL) AS [SlaveId],
	IIF(cp.[Type] = 2, cp.[DateIndex], NULL) AS [SlaveIndex],
	cp.[DateIndex] AS [startIndex],
	cp.[DateIndex] AS [endIndex],
	cp.[value] AS [value]
INTO
	#LonePeakGroupIds
FROM 
	#CombinedPeaks cp
WHERE
	cp.[Id] NOT IN (SELECT [MasterId] FROM #PeakGroupIds UNION SELECT [SlaveId] FROM #PeakGroupIds);

DROP TABLE #PotentialPeakGroups;





--Get prospective trough groups.
SELECT 
	ct1.[Id] AS [baseId],
	ct1.[DateIndex] AS [baseDateIndex],
	ct1.[Type] AS [baseType],
	ct2.[Id] AS [counterId],
	ct2.[DateIndex] AS [counterDateIndex],
	ct2.[Type] AS [counterType],
	IIF(ct1.[Value] > ct2.[Value], ct1.[Value], ct2.[Value]) AS [value]
INTO
	#PotentialTroughGroups
FROM 
	#CombinedTroughs ct1,
	#CombinedTroughs ct2
WHERE
	ct2.[DateIndex] - ct1.[DateIndex] BETWEEN 0 AND 3
	AND ct1.[Id] <> ct2.[Id]
	AND (ct1.[DateIndex] <> ct2.[DateIndex] OR ct1.[Type] < ct2.[Type]);

SELECT
	IIF(ptg.[baseType] = 3, ptg.[baseId], ptg.[counterId]) AS [MasterId],
	IIF(ptg.[baseType] = 3, ptg.[baseDateIndex], ptg.[counterDateIndex]) AS [MasterIndex],
	IIF(ptg.[baseType] = 4, ptg.[baseId], ptg.[counterId]) AS [SlaveId],
	IIF(ptg.[baseType] = 4, ptg.[baseDateIndex], ptg.[counterDateIndex]) AS [SlaveIndex],
	IIF(ptg.[baseDateIndex] < ptg.[counterDateIndex], ptg.[baseDateIndex], ptg.[counterDateIndex]) AS [startIndex],
	IIF(ptg.[baseDateIndex] > ptg.[counterDateIndex], ptg.[baseDateIndex], ptg.[counterDateIndex]) AS [endIndex],
	ptg.[value] AS [value]
INTO
	#TroughGroupIds
FROM
	#PotentialTroughGroups ptg
	LEFT JOIN 
	#CombinedPeaks cp
	ON cp.[DateIndex] BETWEEN ptg.[baseDateIndex] AND ptg.[counterDateIndex]
WHERE
	cp.[DateIndex] IS NULL;

DELETE FROM #TroughGroupIds
WHERE
	[MasterId] IN (SELECT [MasterId] FROM #TroughGroupIds GROUP BY [MasterId] HAVING COUNT(*) > 1)
	AND [MasterIndex] > [SlaveIndex];

DELETE FROM #TroughGroupIds
WHERE
	[SlaveId] IN (SELECT [SlaveId] FROM #TroughGroupIds GROUP BY [SlaveId] HAVING COUNT(*) > 1)
	AND [SlaveIndex] > [MasterIndex];

--Get all extrema that are not part of any group.
SELECT
	IIF(ct.[Type] = 3, ct.[Id], NULL) AS [MasterId],
	IIF(ct.[Type] = 3, ct.[DateIndex], NULL) AS [MasterIndex],
	IIF(ct.[Type] = 4, ct.[Id], NULL) AS [SlaveId],
	IIF(ct.[Type] = 4, ct.[DateIndex], NULL) AS [SlaveIndex],
	ct.[DateIndex] AS [startIndex],
	ct.[DateIndex] AS [endIndex],
	ct.[value] AS [value]
INTO
	#LoneTroughGroupIds
FROM 
	#CombinedTroughs ct
WHERE
	ct.[Id] NOT IN (SELECT [MasterId] FROM #TroughGroupIds UNION SELECT [SlaveId] FROM #TroughGroupIds);

DROP TABLE #PotentialTroughGroups;










DELETE FROM [dbo].[extremumGroups] WHERE [ShareId] = @shareId;
INSERT INTO [dbo].[extremumGroups]([ShareId], [IsPeak], [MasterId], [MasterIndex], [SlaveId], [SlaveIndex], [StartIndex], [EndIndex], [Close], [High], [MasterHigh], [Value])
SELECT
	@shareId AS [ShareId],
	1 AS [IsPeak],
	pgi.[MasterId],
	pgi.[MasterIndex],
	pgi.[SlaveId],
	pgi.[SlaveIndex],
	pgi.[StartIndex],
	pgi.[EndIndex],
	IIF(q1.[Close] IS NOT NULL, IIF(q1.[Close] > q1.[Open], q1.[Close], q1.[Open]), IIF(q2.[Close] > q2.[Open], q2.[Close], q2.[Open])) AS [Close],
	COALESCE(q2.[High], q1.[High]) AS [High],
	q1.[High] AS [MasterHigh],
	pgi.[value] AS [Value]
FROM
	(SELECT * FROM #PeakGroupIds UNION SELECT * FROM #LonePeakGroupIds) pgi
	LEFT JOIN #Quotes q1 ON pgi.[MasterIndex] = q1.[DateIndex]
	LEFT JOIN #Quotes q2 ON pgi.[SlaveIndex] = q2.[DateIndex];


INSERT INTO [dbo].[extremumGroups]([ShareId], [IsPeak], [MasterId], [MasterIndex], [SlaveId], [SlaveIndex], [StartIndex], [EndIndex],	[Close], [Low], [MasterLow], [Value])
SELECT
	@shareId AS [ShareId],
	0 AS [IsPeak],
	tgi.[MasterId],
	tgi.[MasterIndex],
	tgi.[SlaveId],
	tgi.[SlaveIndex],
	tgi.[StartIndex],
	tgi.[EndIndex],
	IIF(q1.[Close] IS NOT NULL, IIF(q1.[Close] < q1.[Open], q1.[Close], q1.[Open]), IIF(q2.[Close] < q2.[Open], q2.[Close], q2.[Open])) AS [Close],
	COALESCE(q2.[Low], q1.[Low]) AS [Low],
	q1.[Low] AS [MasterLow],
	tgi.[value] AS [Value]
FROM
 (SELECT * FROM #TroughGroupIds UNION SELECT * FROM #LoneTroughGroupIds) tgi
	LEFT JOIN #Quotes q1 ON tgi.[MasterIndex] = q1.[DateIndex]
	LEFT JOIN #Quotes q2 ON tgi.[SlaveIndex] = q2.[DateIndex];

DROP TABLE #PeakGroupIds;
DROP TABLE #LonePeakGroupIds;
DROP TABLE #TroughGroupIds;
DROP TABLE #LoneTroughGroupIds;
DROP TABLE #CombinedPeaks;
DROP TABLE #CombinedTroughs;


SELECT * INTO #ExtremumGroups FROM [dbo].[extremumGroups] eg WHERE eg.[ShareId] = @shareId;





--Select required numeric values.
BEGIN
	SELECT
		CEILING((SELECT MIN([Low]) FROM #ExtremumGroups) * @stepFactor) / @stepFactor AS [Min],
		FLOOR((SELECT MAX([High]) FROM #ExtremumGroups) * @stepFactor) / @stepFactor AS [Max]
	INTO 
		#PriceRange

	SELECT
		(pn.[number] / @stepFactor) AS [level]
	INTO 
		#PriceLevels
	FROM
		[dbo].[predefinedNumbers] pn,
		#PriceRange pr
	WHERE
		pn.[number] BETWEEN (pr.[Min] * @stepFactor) AND (pr.[Max] * @stepFactor);

	DROP TABLE #PriceRange;

END



--Calculating extrema group levels for trendline pairing.
SELECT
	b.*
INTO 
	#ExtremaCrossPoints
FROM
	(SELECT DISTINCT
		a.*
	FROM
		(SELECT eg.[Id], pl.[Level], COALESCE(IIF(pl.[Level] > eg.[MasterHigh], eg.[SlaveIndex], eg.[MasterIndex]), eg.[StartIndex]) AS [Index]
		FROM 
			(SELECT * FROM #ExtremumGroups WHERE [IsPeak] = 1) eg
			JOIN #PriceLevels pl
			ON pl.[level] BETWEEN eg.[Close] AND eg.[High]

		UNION 

		SELECT  eg.[Id], eg.[Close], COALESCE(eg.[MasterIndex],eg.[StartIndex]) AS [Index]
		FROM  (SELECT * FROM #ExtremumGroups WHERE [IsPeak] = 1) eg

		UNION

		SELECT eg.[Id], eg.[High], COALESCE(eg.[SlaveIndex], eg.[StartIndex]) AS [Index]
		FROM (SELECT * FROM #ExtremumGroups WHERE [IsPeak] = 1) eg) a

	UNION ALL

	SELECT DISTINCT
		a.*
	FROM
		(SELECT eg.[Id], pl.[Level], COALESCE(IIF(pl.[Level] < eg.[MasterLow], eg.[SlaveIndex], eg.[MasterIndex]), eg.[StartIndex]) AS [Index]
		FROM 
			(SELECT * FROM #ExtremumGroups WHERE [IsPeak] = 0) eg
			JOIN #PriceLevels pl
			ON pl.[level] BETWEEN eg.[Low] AND eg.[Close]

		UNION 

		SELECT eg.[Id], eg.[Close], COALESCE(eg.[MasterIndex],eg.[StartIndex]) AS [Index]
		FROM (SELECT * FROM #ExtremumGroups WHERE [IsPeak] = 0) eg

		UNION

		SELECT eg.[Id], eg.[Low], COALESCE(eg.[SlaveIndex], eg.[StartIndex]) AS [Index]
		FROM (SELECT * FROM #ExtremumGroups WHERE [IsPeak] = 0) eg) a) b


--SELECT * FROM #ExtremaCrossPoints;
--SELECT * FROM #ExtremumGroups;



--Selecting possible pairs for trendlines.

	--Prepare data with value for logarithm.
	SELECT 
		eg1.[Id] AS [baseId],
		eg1.[StartIndex] AS [baseStartIndex], 
		eg1.[IsPeak] AS [baseIsPeak], 
		eg1.[Value] AS [baseValue],
		eg2.[Id] AS [counterId],
		eg2.[StartIndex] AS [counterStartIndex],
		eg2.[IsPeak] AS [counterIsPeak] ,
		eg2.[Value] AS [counterValue],
		IIF(eg1.[Value] > eg2.[Value], eg1.[Value], eg2.[Value]) / 40 + 1.05 AS [LogBase],
		eg2.[StartIndex] - eg1.[StartIndex] AS [StartIndicesDifference]
	INTO
		#ExtremumGroupsInitialPairing
	FROM
		#ExtremumGroups eg1
		LEFT JOIN #ExtremumGroups eg2
		ON (eg2.[StartIndex] - eg1.[StartIndex]) >= @minDistanceInTrendlinePoints;



	--Filter out all pairs with too long distance between.
	SELECT 
		*
	INTO
		#PotentialPairs
	FROM
		#ExtremumGroupsInitialPairing egip
	WHERE
		egip.[StartIndicesDifference] <= LOG(egip.[LogBase], 2.5) * @maxDistanceInTrendlinePoints
	
	--AND egip.[baseStartIndex] = 1258 AND egip.[counterStartIndex] = 1297;
		


	--SELECT * FROM #PotentialPairs;


	--Creating all possible trendlines.
	DELETE FROM [dbo].[trendlines] WHERE [ShareId] = @shareId;
	INSERT INTO [dbo].[trendlines](
		[ShareId],
		[BaseId], 
		[BaseStartIndex], 
		[BaseIsPeak], 
		[BaseLevel], 
		[CounterId], 
		[CounterStartIndex], 
		[CounterIsPeak], 
		[CounterLevel], 
		[Slope], 
		[~IsOpenFromLeft], 
		[~IsOpenFromRight],
		[~CandlesDistance]
	)
	SELECT
		@shareId,
		pp.[baseId],
		ecp1.[Index] AS [baseStartIndex],
		pp.[baseIsPeak],
		ecp1.[Level] AS [baseLevel],
		pp.[counterId],
		ecp2.[index] AS [counterStartIndex],
		pp.[counterIsPeak],
		ecp2.[Level] AS [counterLevel],
		(ecp2.[Level] - ecp1.[Level]) / (pp.[counterStartIndex] - pp.[baseStartIndex]) AS [slope],
		1 AS [~IsOpenFromLeft],
		1 AS [~IsOpenFromRight],
		(pp.[counterStartIndex] - pp.[baseStartIndex]) AS [~CandlesDistance]
	FROM
		#PotentialPairs pp
		LEFT JOIN #ExtremaCrossPoints ecp1
		ON pp.[baseId] = ecp1.[Id]
		LEFT JOIN #ExtremaCrossPoints ecp2
		ON pp.[counterId] = ecp2.[Id]


----SELECT
----		COUNT(*)
----	FROM
----		#PotentialPairs pp
----		LEFT JOIN #ExtremaCrossPoints ecp1
----		ON pp.[baseId] = ecp1.[Id]
----		LEFT JOIN #ExtremaCrossPoints ecp2
----		ON pp.[counterId] = ecp2.[Id]



DROP TABLE #Quotes;
DROP TABLE #Extrema;
DROP TABLE #PeaksByClose;
DROP TABLE #PeaksByHigh;
DROP TABLE #TroughsByClose;
DROP TABLE #TroughsByLow;
DROP TABLE #ExtremumGroups;
DROP TABLE #PriceLevels;
DROP TABLE #ExtremaCrossPoints;
DROP TABLE #ExtremumGroupsInitialPairing;
DROP TABLE #PotentialPairs;


ROLLBACK TRANSACTION