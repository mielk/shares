use fx;


SELECT
	c.*,
	(c.[DistancePoints] * [ExPriceCrossPoints] * [OCPriceCrossPoints] * c.[BaseHitPoints] * c.[CounterHitPoints] * 
	[AverageVariationValue] * [ExtremumVariationValue] * [OpenCloseVariationValue]) / 100000000000000 AS [TotalPoints]
FROM	
	(SELECT
		b.*,
		[dbo].[MinValue]([dbo].[MaxValue](100 - ABS(100 - b.[TotalCandles]), 0), 100) AS [DistancePoints],
		[dbo].[MinValue]([dbo].[MaxValue](100 - 20 * COALESCE(b.[ExtremumPriceCrossPenaltyPoints], 0), 0), 100) AS [ExPriceCrossPoints],
		[dbo].[MinValue]([dbo].[MaxValue](100 - 20 * COALESCE(b.[OCPriceCrossPenaltyPoints], 0), 0), 100) AS [OCPriceCrossPoints],
		[dbo].[MinValue]([dbo].[MaxValue](b.[BaseHitValue], 0), 100) AS [BaseHitPoints],
		[dbo].[MinValue]([dbo].[MaxValue](b.[CounterHitValue], 0), 100) AS [CounterHitPoints],
		[dbo].[MinValue]([dbo].[MaxValue](10000 *  b.[RelativeAverageVariation] / b.[HoursDiffModified], 0), 100) AS [AverageVariationValue],
		[dbo].[MinValue]([dbo].[MaxValue](5000 * b.[RelativeExtremumVariation] / b.[HoursDiffModified], 0), 100) AS [ExtremumVariationValue],
		[dbo].[MinValue]([dbo].[MaxValue](5000 * b.[RelativeOpenCloseVariation] / b.[HoursDiffModified], 0), 100) AS [OpenCloseVariationValue]
	FROM
		(SELECT	
			a.*,
			[dbo].[MaxValue](50, POWER(CAST(a.[HoursDiff] AS FLOAT), (1 - LOG(IIF(a.[HoursDiff] <= 20, 21, a.[HoursDiff]) / 20, 2) / 50))) AS [HoursDiffModified],
			100 * a.[ExtremumVariation] / a.[MidLevel] AS [RelativeExtremumVariation],
			100 * a.[OpenCloseVariation] / a.[MidLevel] AS [RelativeOpenCloseVariation],
			100 * a.[AverageVariation] / a.[MidLevel] AS [RelativeAverageVariation]
		FROM
			(SELECT
				tr.*,
				((CAST(tr.[BaseDateIndex] AS DECIMAL) + (CAST(tr.[CounterDateIndex] AS DECIMAL) - CAST(tr.[BaseDateIndex] AS DECIMAL)) / 2.0) - t.[BaseDateIndex]) * t.[Angle] + t.[BaseLevel] AS [MidLevel],
				d1.[Date] AS [BaseDate],
				d2.[Date] AS [CounterDate],
				DATEDIFF(HOUR, d1.[Date], d2.[Date]) AS [HoursDiff]
			FROM
				[dbo].[trendRanges] tr
				LEFT JOIN [dbo].[trendlines] t ON tr.[TrendlineId] = t.[TrendlineId]
				LEFT JOIN [dbo].[dates] d1 ON t.[TimeframeId] = d1.[TimeframeId] AND d1.[DateIndex] = tr.[BaseDateIndex]
				LEFT JOIN [dbo].[dates] d2 ON t.[TimeframeId] = d2.[TimeframeId] AND d2.[DateIndex] = tr.[CounterDateIndex]) a
		) b
	) c