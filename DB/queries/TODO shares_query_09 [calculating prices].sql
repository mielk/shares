USE [shares];

GO

BEGIN TRANSACTION;

SELECT [DateIndex], [Close] INTO #TempClosePrices FROM [quotes] WHERE [ShareId] = 1 ORDER BY [DateIndex] ASC;

--Update Delta quotes
SELECT 
	tcp1.*,
	(tcp1.[Close] - tcp2.[Close]) AS [Delta]
FROM 
	#TempClosePrices tcp1
	LEFT JOIN
	#TempClosePrices tcp2
	ON tcp1.[DateIndex] = (tcp2.[DateIndex] + 1)


--Calculate price directions


DROP TABLE #TempClosePrices;

ROLLBACK TRANSACTION;