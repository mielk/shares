use [fx];

begin transaction

	SELECT
		q.*,
		DATEPART(MINUTE, q.[Date]) AS [OriginalMinute]
	INTO #QuotesWithMinutes
	FROM
		[dbo].[tempData] q;

	SELECT
		q.*,
		DATEADD(MINUTE, -q.[OriginalMinute] % 5, q.[Date]) AS [M5]
	INTO
		#QuotesWith5MPeriod
	FROM
		#QuotesWithMinutes q;
	DROP TABLE #QuotesWithMinutes;

	SELECT 
		q.[M5],
		MIN(q.[Date]) AS [MinDate],
		MAX(q.[Date]) AS [MaxDate],
		MAX(q.[High]) AS [High],
		MIN(q.[Low]) AS [Low]
	INTO
		#GroupedByM5
	FROM
		#QuotesWith5MPeriod q
	GROUP BY q.[M5];
	DROP TABLE #QuotesWith5MPeriod;

	SELECT
		a.[M5] AS [Date],
		q.[Open] AS [Open],
		a.[High] AS [High],
		a.[Low] AS [Low],
		q2.[Close] AS [Close]
	INTO #5M
	FROM
		#GroupedByM5 a
		LEFT JOIN [dbo].[tempData] q
		ON a.[MinDate] = q.[Date]
		LEFT JOIN [dbo].[tempData] q2
		ON a.[MaxDate] = q2.[Date];
	DROP TABLE #GroupedByM5;

	SELECT
		* 
	FROM 
		#5M
	ORDER BY
		[Date] ASC;

rollback transaction