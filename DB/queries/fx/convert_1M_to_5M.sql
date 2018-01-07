use [fx];

GO

BEGIN TRANSACTION

GO
IF OBJECT_ID('convert1MDataToHourFormat','P') IS NOT NULL DROP PROC [dbo].[convert1MDataToHourFormat];
IF OBJECT_ID('convert1MDataToWeekFormat','P') IS NOT NULL DROP PROC [dbo].[convert1MDataToWeekFormat];
IF OBJECT_ID('convert1MDataToDayFormat','P') IS NOT NULL DROP PROC [dbo].[convert1MDataToDayFormat];
IF OBJECT_ID('convertDataForYear','P') IS NOT NULL DROP PROC [dbo].[convertDataForYear];
IF OBJECT_ID('convert1MDataToOtherTimeframe','P') IS NOT NULL DROP PROC [dbo].[convert1MDataToOtherTimeframe];
GO

CREATE PROC [dbo].[convert1MDataToOtherTimeframe] @minutes AS INT, @year AS INT
AS
BEGIN

	SELECT
		q.*,
		DATEPART(MINUTE, q.[Date]) AS [OriginalMinute]
	INTO #QuotesWithMinutes
	FROM
		[dbo].[tempData] q
	WHERE YEAR([Date]) = @year;

	SELECT
		q.*,
		DATEADD(MINUTE, -q.[OriginalMinute] % @minutes, q.[Date]) AS [ParentPeriod]
	INTO
		#QuotesWithParentPeriods
	FROM
		#QuotesWithMinutes q;
	DROP TABLE #QuotesWithMinutes;

	SELECT 
		q.[ParentPeriod],
		MIN(q.[Date]) AS [MinDate],
		MAX(q.[Date]) AS [MaxDate],
		MAX(q.[High]) AS [High],
		MIN(q.[Low]) AS [Low]
	INTO
		#GroupedByParentPeriod
	FROM
		#QuotesWithParentPeriods q
	GROUP BY q.[ParentPeriod];
	DROP TABLE #QuotesWithParentPeriods;

	SELECT
		a.[ParentPeriod] AS [Date],
		q.[Open] AS [Open],
		a.[High] AS [High],
		a.[Low] AS [Low],
		q2.[Close] AS [Close]
	INTO #ByParentPeriod
	FROM
		#GroupedByParentPeriod a
		LEFT JOIN [dbo].[tempData] q
		ON a.[MinDate] = q.[Date]
		LEFT JOIN [dbo].[tempData] q2
		ON a.[MaxDate] = q2.[Date];
	DROP TABLE #GroupedByParentPeriod;

	IF (@minutes = 5)
	BEGIN
		INSERT INTO
			[dbo].[RawM5Data]
		SELECT
			* 
		FROM 
			#ByParentPeriod
		ORDER BY
			[Date] ASC;
	END

END

GO


CREATE PROC [dbo].[convertDataForYear] @year AS INT
AS
BEGIN

	EXEC [dbo].[convert1MDataToOtherTimeframe] @minutes = 5, @year = @year;
	--EXEC [dbo].[convert1MDataToOtherTimeframe] @minutes = 15, @year = @year;
	--EXEC [dbo].[convert1MDataToOtherTimeframe] @minutes = 30, @year = @year;

END

GO


CREATE PROC [dbo].[convert1MDataToWeekFormat]
AS
BEGIN

	SELECT
		q.*,
		CONVERT(DATE, q.[Date]) AS [OriginalDate]
	INTO #QuotesWithHours
	FROM
		[dbo].[tempData] q;

	SELECT
		q.*,
		DATEADD(D, -(DATEPART(dw, q.[OriginalDate]) + 5) % 7, q.[OriginalDate]) AS [ParentPeriod]
	INTO
		#QuotesWithParentPeriods
	FROM
		#QuotesWithHours q;
	DROP TABLE #QuotesWithHours;

	SELECT 
		q.[ParentPeriod],
		MIN(q.[Date]) AS [MinDate],
		MAX(q.[Date]) AS [MaxDate],
		MAX(q.[High]) AS [High],
		MIN(q.[Low]) AS [Low]
	INTO
		#GroupedByParentPeriod
	FROM
		#QuotesWithParentPeriods q
	GROUP BY q.[ParentPeriod];
	DROP TABLE #QuotesWithParentPeriods;

	SELECT
		a.[ParentPeriod] AS [Date],
		q.[Open] AS [Open],
		a.[High] AS [High],
		a.[Low] AS [Low],
		q2.[Close] AS [Close]
	INTO #ByParentPeriod
	FROM
		#GroupedByParentPeriod a
		LEFT JOIN [dbo].[tempData] q
		ON a.[MinDate] = q.[Date]
		LEFT JOIN [dbo].[tempData] q2
		ON a.[MaxDate] = q2.[Date];
	DROP TABLE #GroupedByParentPeriod;

	SELECT
		* 
	FROM 
		#ByParentPeriod
	ORDER BY
		[Date] ASC;

END


GO


CREATE PROC [dbo].[convert1MDataToDayFormat]
AS
BEGIN

	SELECT
		q.*,
		CONVERT(DATE, q.[Date]) AS [OriginalDate]
	INTO #QuotesWithHours
	FROM
		[dbo].[tempData] q;

	SELECT
		q.*,
		q.[OriginalDate] AS [ParentPeriod]
	INTO
		#QuotesWithParentPeriods
	FROM
		#QuotesWithHours q;
	DROP TABLE #QuotesWithHours;

	SELECT 
		q.[ParentPeriod],
		MIN(q.[Date]) AS [MinDate],
		MAX(q.[Date]) AS [MaxDate],
		MAX(q.[High]) AS [High],
		MIN(q.[Low]) AS [Low]
	INTO
		#GroupedByParentPeriod
	FROM
		#QuotesWithParentPeriods q
	GROUP BY q.[ParentPeriod];
	DROP TABLE #QuotesWithParentPeriods;

	SELECT
		a.[ParentPeriod] AS [Date],
		q.[Open] AS [Open],
		a.[High] AS [High],
		a.[Low] AS [Low],
		q2.[Close] AS [Close]
	INTO #ByParentPeriod
	FROM
		#GroupedByParentPeriod a
		LEFT JOIN [dbo].[tempData] q
		ON a.[MinDate] = q.[Date]
		LEFT JOIN [dbo].[tempData] q2
		ON a.[MaxDate] = q2.[Date];
	DROP TABLE #GroupedByParentPeriod;

	SELECT
		* 
	FROM 
		#ByParentPeriod
	ORDER BY
		[Date] ASC;

END


GO


CREATE PROC [dbo].[convert1MDataToHourFormat] @hours AS INT
AS
BEGIN

	SELECT
		q.*,
		DATEPART(MINUTE, q.[Date]) AS [OriginalMinute],
		DATEPART(HOUR, q.[Date]) AS [OriginalHour]
	INTO #QuotesWithHours
	FROM
		[dbo].[tempData] q;

	SELECT
		q.*,
		DATEADD(HOUR, -q.[OriginalHour] % @hours, DATEADD(MINUTE, -q.[OriginalMinute], q.[Date])) AS [ParentPeriod]
	INTO
		#QuotesWithParentPeriods
	FROM
		#QuotesWithHours q;
	DROP TABLE #QuotesWithHours;

	SELECT 
		q.[ParentPeriod],
		MIN(q.[Date]) AS [MinDate],
		MAX(q.[Date]) AS [MaxDate],
		MAX(q.[High]) AS [High],
		MIN(q.[Low]) AS [Low]
	INTO
		#GroupedByParentPeriod
	FROM
		#QuotesWithParentPeriods q
	GROUP BY q.[ParentPeriod];
	DROP TABLE #QuotesWithParentPeriods;

	SELECT
		a.[ParentPeriod] AS [Date],
		q.[Open] AS [Open],
		a.[High] AS [High],
		a.[Low] AS [Low],
		q2.[Close] AS [Close]
	INTO #ByParentPeriod
	FROM
		#GroupedByParentPeriod a
		LEFT JOIN [dbo].[tempData] q
		ON a.[MinDate] = q.[Date]
		LEFT JOIN [dbo].[tempData] q2
		ON a.[MaxDate] = q2.[Date];
	DROP TABLE #GroupedByParentPeriod;

	SELECT
		* 
	INTO 
		[dbo].[RawH1Data]
	FROM 
		#ByParentPeriod
	ORDER BY
		[Date] ASC;

END


GO

EXEC [dbo].[convertDataForYear] @year = 2007;
EXEC [dbo].[convertDataForYear] @year = 2008;
EXEC [dbo].[convertDataForYear] @year = 2009;
EXEC [dbo].[convertDataForYear] @year = 2010;
EXEC [dbo].[convertDataForYear] @year = 2011;
EXEC [dbo].[convertDataForYear] @year = 2012;
EXEC [dbo].[convertDataForYear] @year = 2013;
EXEC [dbo].[convertDataForYear] @year = 2014;
EXEC [dbo].[convertDataForYear] @year = 2015;
EXEC [dbo].[convertDataForYear] @year = 2016;
EXEC [dbo].[convertDataForYear] @year = 2017;

COMMIT TRANSACTION