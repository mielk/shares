USE [fx];

--DELETE FROM [dbo].[quotes];
--DELETE FROM [dbo].[quotesoutofdate];
--DELETE FROM [dbo].[extrema];
--UPDATE [dbo].[timestamps] SET [extremalastanalyzedindex] = NULL, [trendlinesanalysislastquotationindex] = NULL;


BEGIN TRANSACTION

--EXEC [dbo].[test_addQuoteFromRawH1] @counter = 5
--EXEC [dbo].[processExtrema] @assetId = 1, @timeframeId = 4;
EXEC [dbo].[processTrendlines] @assetId = 1, @timeframeId = 4;

--SELECT 'Timestamps after', * FROM [dbo].[timestamps];
--SELECT * FROM [dbo].[quotes] WHERE [TimeframeId] = 4;
SELECT [ExtremumId], [DateIndex], IIF([ExtremumTypeId] < 3, 'Peak', 'Trough') AS [Peak/Trough], IIF([ExtremumTypeId] % 2 = 0, 'By Extremum', 'By OC'), [Value] FROM [dbo].[extrema] ORDER BY [DateIndex] ASC;
SELECT * FROM [dbo].[trendlines];
--SELECT * FROM [dbo].[ExtremumGroups];
--select * from [dbo].[quotes];


COMMIT TRANSACTION;