USE [fx];

--DELETE FROM [dbo].[quotes];
--DELETE FROM [dbo].[quotesoutofdate];
--DELETE FROM [dbo].[extrema];
--UPDATE [dbo].[timestamps] SET [extremalastanalyzedindex] = NULL, [trendlinesanalysislastquotationindex] = NULL;


BEGIN TRANSACTION

EXEC [dbo].[test_addQuoteFromRawH1] @counter = 10
EXEC [dbo].[processAdx] @assetId = 1, @timeframeId = 4;
--EXEC [dbo].[processExtrema] @assetId = 1, @timeframeId = 4;
--EXEC [dbo].[processTrendlines] @assetId = 1, @timeframeId = 4;

SELECT 'Timestamps after', * FROM [dbo].[timestamps];
--SELECT * FROM [dbo].[quotes] WHERE [TimeframeId] = 4;
--SELECT [ExtremumId], [DateIndex], IIF([ExtremumTypeId] < 3, 'Peak', 'Trough') AS [Peak/Trough], IIF([ExtremumTypeId] % 2 = 0, 'By Extremum', 'By OC'), [Value] FROM [dbo].[extrema] ORDER BY [DateIndex] ASC;
--SELECT * FROM [dbo].[ExtremumGroups];
--SELECT * FROM [dbo].[trendlines];
--SELECT * FROM [dbo].[trendBreaks];
--SELECT * FROM [dbo].[trendHits];
--SELECT * FROM [dbo].[trendRanges];

--SELECT
--	t.[TrendlineId],
--	b.[Counter] AS [Breaks],
--	h.[Counter] AS [Hits]
--FROM
--	[dbo].[trendlines] t
--	LEFT JOIN (SELECT [TrendlineId], COUNT(*) AS [Counter] FROM [dbo].[trendBreaks] GROUP BY [TrendlineId]) b ON t.[TrendlineId] = b.[TrendlineId]
--	LEFT JOIN (SELECT [TrendlineId], COUNT(*) AS [Counter] FROM [dbo].[trendHits] GROUP BY [TrendlineId]) h ON t.[TrendlineId] = h.[TrendlineId]
--ORDER BY
--	b.[Counter] * h.[Counter] DESC,
--	h.[Counter] DESC;

select * from [adx];


COMMIT TRANSACTION;