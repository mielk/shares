use [shares];

exec [dbo].[evaluateTrendlines] @shareId = 1, @baseExtremumId = 7078, @counterExtremumId = 7079

--begin transaction;
--
--UPDATE t
--SET 
--	[ShowOnChart] = 1,
--	[StartDateIndex] = [BaseStartIndex] - 3,
--	[EndDateIndex] = [CounterStartIndex] + 3
--FROM
--	[dbo].[Trendlines] t
--	LEFT JOIN
--	(SELECT 
--		[BaseId], [CounterId], MIN([Id]) AS [MinId]
--	FROM
--		[dbo].[Trendlines]
--	GROUP BY
--		[BaseId], [CounterId]) x
--	ON t.[Id] = x.[MinId]
--WHERE
--	x.[MinId] IS NOT NULL;
--
--commit transaction;