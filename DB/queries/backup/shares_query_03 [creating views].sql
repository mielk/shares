USE [shares];

GO

BEGIN TRANSACTION;

--DROP VIEWS
BEGIN
		
	IF OBJECT_ID('missingData','v') IS NOT NULL DROP VIEW [dbo].[missingData];
	IF OBJECT_ID('sharesWithMissingData','v') IS NOT NULL DROP VIEW [dbo].[sharesWithMissingData];;
	IF OBJECT_ID('firstQuotes','v') IS NOT NULL DROP VIEW [dbo].[firstQuotes];

END

GO

--MISSING DATA
CREATE VIEW [dbo].[missingData] AS
SELECT
	sh.[Name] AS [Share],
	sh.[YahooSymbol],
	sh.[Plus500Symbol],
	m.[Name] AS [Market],
	hul.[dataType]
FROM 
	(SELECT [ShareId], 'quotes' AS [dataType] FROM  [dbo].[historicalUpdatesLogs] WHERE  [QuotesUpdateTimestamp] IS NULL
	UNION ALL
	SELECT [ShareId], 'dividends' AS [dataType] FROM  [dbo].[historicalUpdatesLogs] WHERE  [DividendsUpdateTimestamp] IS NULL
	UNION ALL
	SELECT [ShareId], 'splits' AS [dataType] FROM  [dbo].[historicalUpdatesLogs] WHERE  [SplitsUpdateTimestamp] IS NULL) hul
	INNER JOIN [dbo].[shares] sh ON hul.[ShareId] = sh.[Id]
	INNER JOIN [dbo].[markets] m ON sh.[MarketId] = m.[Id]

GO


--SHARES WITH MISSING DATA
CREATE VIEW [dbo].[sharesWithMissingData] AS
(SELECT
	*
FROM 
	[dbo].[shares] sh
WHERE
	[Id] IN (SELECT [ShareId]
				FROM  [dbo].[historicalUpdatesLogs]
				WHERE
					[QuotesUpdateTimestamp] IS NULL OR
					[DividendsUpdateTimestamp] IS NULL OR
					[SplitsUpdateTimestamp] IS NULL))

GO

--FIRST QUOTES
CREATE VIEW [dbo].[firstQuotes] AS
SELECT
	sh.[Id],
	sh.[Name],
	sh.[YahooSymbol],
	firstQuotes.[fq]

FROM
	[dbo].[shares] sh
	LEFT JOIN (SELECT [ShareId], MIN([Date]) AS fq
				FROM  [dbo].[quotes]
				GROUP BY [ShareId]) firstQuotes
	ON sh.[Id] = firstQuotes.[ShareId]

GO


ROLLBACK TRANSACTION;
--COMMIT TRANSACTION;