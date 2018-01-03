USE [shares];

BEGIN TRANSACTION;


SELECT [TrendlineId], hits = STUFF(
             (SELECT ',' + CAST([ExtremumGroupId] AS NVARCHAR(5))
              FROM [dbo].[TrendlinesHits] th
              WHERE th.[TrendlineId] = th2.[TrendlineId]
			  ORDER BY [ExtremumGroupId] ASC
              FOR XML PATH (''))
             , 1, 1, '') 
INTO [dbo].[trendlinesByHits]
FROM [dbo].[TrendlinesHits] th2
GROUP BY [TrendlineId];


SELECT
	hits, COUNT(TrendlineId)
FROM
	[dbo].[trendlinesByHits]
GROUP BY 
	[Hits]

commit TRANSACTION;