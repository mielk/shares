use [fx];
BEGIN TRANSACTION;
INSERT INTO [tempData]
COMMIT TRANSACTION;

SELECT 
	COUNT(*) AS [Counter],
	MIN([Date]) AS [FirstQuote],
	MAX([Date]) AS [LastQuote]
FROM
	[dbo].[tempData]
WHERE
	YEAR([Date]) = 2017