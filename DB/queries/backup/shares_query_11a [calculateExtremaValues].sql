USE [shares];

BEGIN TRANSACTION;


UPDATE e
SET
	[Value] = p.[EarlierAmplitudePoints] + p.[LaterAmplitudePoints] + p.[EarlierCounterPoints] + p.[LaterCounterPoints] + p.[EarlierVolatilityPoints] + p.[LaterVolatilityPoints]
FROM
	[dbo].[extrema] e
	JOIN 
	(SELECT
		a.*,
		a.[EarlierAmplitude] / IIF(a.[ExtremumType] > 2, a.[EarlierAmplitude] + a.[price], a.[price]) * 5 AS [EarlierAmplitudePoints],
		a.[LaterAmplitude] / IIF(a.[ExtremumType] > 2, a.[LaterAmplitude] + a.[price], a.[price]) * 5 AS [LaterAmplitudePoints],
		LOG(LOG(IIF(a.[EarlierCounter] < 5.0, 5.0, a.[EarlierCounter]* 1.0), 260) * 10.0, 10) * 35 AS [EarlierCounterPoints],
		LOG(LOG(IIF(a.[LaterCounter] < 5.0, 5.0, a.[LaterCounter]* 1.0), 260) * 10.0, 10) * 35 AS [LaterCounterPoints],
		IIF(a.[EarlierAverageArea] > 100, 10, [EarlierAverageArea] / 10) AS [EarlierVolatilityPoints],
		IIF(a.[LaterAverageArea] > 100, 10, [LaterAverageArea] / 10) AS [LaterVolatilityPoints]
	FROM
		(SELECT
			q.[Date],
			CASE e.[ExtremumType]
				WHEN 1 THEN IIF(q.[Close] > q.[High], q.[Close], q.[High])
				WHEN 2 THEN q.[High]
				WHEN 3 THEN IIF(q.[Close] < q.[High], q.[Close], q.[High])
				WHEN 4 THEN q.[Low]
			END AS [price],
			e.*
		FROM
			[dbo].[extrema] e
			LEFT JOIN (SELECT * FROM [dbo].[quotes] WHERE [ShareId] = 1) q
			ON e.[DateIndex] = q.[DateIndex]) a
			) p
		ON e.[Id] = p.[Id];




SELECT
	q.[Date], e.[Id], e.[DateIndex], (e.[Value] - 50) * 2, e.[ExtremumType]
FROM
	[dbo].[extrema] e
	LEFT JOIN (SELECT * FROM [dbo].[quotes] WHERE [ShareId] = 1) q
	ON e.[DateIndex] = q.[DateIndex]
ORDER BY
	[Value] DESC;


ROLLBACK TRANSACTION;