USE [shares];

BEGIN TRANSACTION;

GO

CREATE PROCEDURE [dbo].[generateWeeklyPrices]
AS

BEGIN
	
	--Remove previous weekly quotes.
	DELETE FROM [dbo].[quotes] WHERE [Timeframe] = 7;

	--Create temporary table with all daily quotes.
	SELECT * INTO #QuotesTemp FROM [dbo].[quotes] WHERE [Timeframe] = 6;

	--Create temporary table with basic data about weeks.
	SELECT
		d.[ParentLevelDateIndex] AS [WeekIndex],
		q.ShareId AS [ShareId],
		MIN(q.[Id]) AS [FirstQuote],
		MAX(q.[Id]) AS [LastQuote],
		MIN([Low]) AS [Low],
		MAX([High]) AS [High],
		SUM([Volume]) AS [Volume]
	INTO
		#WeekTemp
	FROM
		#QuotesTemp q
		LEFT JOIN [dbo].[dates] d
		ON 
			q.[Timeframe] = d.[Timeframe] AND 
			q.[DateIndex] = d.[DateIndex]
	GROUP BY
		d.[ParentLevelDateIndex], q.ShareId;

	--Create week records.
	SELECT
		wt.[ShareId],
		d.[Date],
		q.[Open],
		wt.[Low],
		wt.[High],
		q2.[Close],
		q2.[Close] AS [AdjClose],
		wt.[Volume],
		wt.[WeekIndex] AS [DateIndex],
		7 As [Timeframe]
	INTO
		#WeeklyQuotes
	FROM
		#WeekTemp wt
		LEFT JOIN #QuotesTemp q ON wt.[FirstQuote] = q.[Id]
		LEFT JOIN #QuotesTemp q2 ON wt.[LastQuote] = q2.[Id]
		LEFT JOIN (SELECT * FROM [dbo].[dates] WHERE [Timeframe] = 7) d ON wt.[WeekIndex] = d.[DateIndex];

	--Insert week records into quotes table.
	INSERT INTO [dbo].[quotes]([ShareId], [Date], [Open], [Low], [High], [Close], [AdjClose], [Volume], [DateIndex], [Timeframe])
	SELECT * FROM #WeeklyQuotes;

	--Clean up temporary tables.
	BEGIN

		DROP TABLE #WeekTemp;
		DROP TABLE #QuotesTemp;
		DROP TABLE #WeeklyQuotes;

	END

END

GO

ROLLBACK TRANSACTION;