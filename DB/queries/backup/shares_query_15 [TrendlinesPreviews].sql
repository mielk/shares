USE [shares];

EXEC [dbo].[DisplayTrendlineAllData] @trendlineId = 1350568;
EXEC [dbo].[evaluateTrendlines] @shareId = 1, @baseExtremumId = 9361, @counterExtremumId = 9244


BEGIN TRANSACTION;

SELECT *
FROM [shares].[dbo].[TrendRanges]
order by [OCPriceCrossPenaltyPoints] desc;

SELECT
	*
INTO	
	#ManyHits
FROM
	(SELECT 
		t.[Id],
		COUNT(th.[Id]) AS Counter
	FROM 
		[dbo].[trendlines] t
		LEFT JOIN [dbo].[TrendlinesHits] th
		ON t.[Id] =  th.[TrendlineId]
	GROUP BY
		t.[Id]) a
WHERE [Counter] > 10;


UPDATE a
SET [ShowOnChart] = IIF(he.[TrendlineId] IS NULL, 0, 1)
FROM
	[dbo].[trendlines] a
	LEFT JOIN #HighEpcpps he
	ON a.[Id] = he.[TrendlineId]

update trendlines set ShowOnChart = iif(Id = 1350568, 1, 0);
select * from trendRanges where trendlineid = 1334904 order by basedateindex asc;

SELECT * FROM #HighEpcpps;
select * from trendlines where ShowOnChart = 1;

--Clean up
BEGIN

	DROP TABLE #ManyHits;
	DROP TABLE #HighEpcpps;

END

--COMMIT TRANSACTION;
ROLLBACK TRANSACTION;



go

begin transaction

select
	*
FROM
	trendlines t
	left join trendlinesByHits tbh
	on t.[Id] = tbh.[trendlineId]
where
	tbh.[hits] = '9206,9209,9212'

rollback transaction