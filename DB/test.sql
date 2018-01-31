USE [fx];

--DELETE FROM [dbo].[quotes];
--DELETE FROM [dbo].[quotesOutOfDate];
--DELETE FROM [dbo].[extrema];
--UPDATE [dbo].[timestamps] SET [ExtremaLastAnalyzedIndex] = NULL;



BEGIN TRANSACTION

EXEC [dbo].[test_addQuoteFromRawH1] @counter = 5
EXEC [dbo].[processExtrema] @assetId = 1, @timeframeId = 4;

SELECT * FROM [dbo].[timestamps];
SELECT * FROM [dbo].[quotes] WHERE [TimeframeId] = 4;
SELECT * FROM [dbo].[extrema] ORDER BY [DateIndex] ASC;


COMMIT TRANSACTION;