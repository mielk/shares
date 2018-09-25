USE [fx];


--SETTINGS
BEGIN

	CREATE TABLE [dbo].[settingsNumeric](
		[SettingName] [nvarchar] (255) NOT NULL,
		[SettingValue] [int] NOT NULL
	);
	CREATE UNIQUE NONCLUSTERED INDEX [ixName_SettingsNumeric] ON [dbo].[settingsNumeric]
	([SettingName] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	INSERT INTO [dbo].[settingsNumeric]
	SELECT 'ExtremumAnalysisCheckDistance', 260 UNION ALL
	SELECT 'ExtremumAnalysisMinDistance', 5;

	CREATE TABLE [dbo].[settingsText](
		[SettingName] [nvarchar] (255) NOT NULL,
		[SettingValue] [nvarchar] (255) NOT NULL
	);
	CREATE UNIQUE NONCLUSTERED INDEX [ixName_SettingText] ON [dbo].[settingsText]
	([SettingName] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

END


--ERROR LOGS
BEGIN

	CREATE TABLE [dbo].[errorLogs](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Timestamp] [datetime] NOT NULL CONSTRAINT [Default_ErrorLogs_Timestamp]  DEFAULT (GETDATE()),
		[Class] [nvarchar](255) NOT NULL,
		[Method] [nvarchar](255) NOT NULL,
		[InputParams] [nvarchar](MAX) NULL,
		[ErrNumber] [int] NOT NULL,
		[ErrDescription] [nvarchar](MAX) NOT NULL,
		[SqlString] [nvarchar](MAX) NULL,
	 CONSTRAINT [PK_errorLogs] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[errorLogs]  WITH CHECK ADD  CONSTRAINT [CH_notEmpty_Class] CHECK  ((LEN(RTRIM(LTRIM([Class])))>(0)))

	ALTER TABLE [dbo].[errorLogs]  WITH CHECK ADD  CONSTRAINT [CH_notEmpty_ErrNumber] CHECK  (([ErrNumber]<>(0)))

	ALTER TABLE [dbo].[errorLogs]  WITH CHECK ADD  CONSTRAINT [CH_notEmpty_ErrorDescription] CHECK  ((LEN(RTRIM(LTRIM([ErrDescription])))>(0)))

	ALTER TABLE [dbo].[errorLogs]  WITH CHECK ADD  CONSTRAINT [CH_notEmpty_Method] CHECK  ((LEN(RTRIM(LTRIM([Method])))>(0)))

END


--PREDEFINED NUMBERS
BEGIN

	SELECT (ones.n + 10*tens.n + 100*hundreds.n + 1000*thousands.n + 10000*tenThousands.n + 100000*hundredThousands.n + 1000000*millions.n) AS [number]
	INTO [dbo].[predefinedNumbers]
	FROM (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) ones(n),
			(VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) tens(n),
			(VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) hundreds(n),
			(VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) thousands(n),
			(VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) tenThousands(n),
			(VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) hundredThousands(n),
			(VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) millions(n)
	ORDER BY [number];

	CREATE NONCLUSTERED INDEX [ixNumber_predefinedNumbers] ON [dbo].[predefinedNumbers]
	([number] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

END


--CURRENCIES
BEGIN

	CREATE TABLE [dbo].[currencies](
		[CurrencyId] [int] NOT NULL,
		[Uuid] [nvarchar](36) NOT NULL CONSTRAINT [Default_Currencies_Uuid] DEFAULT (NEWID()),
		[Name] [nvarchar](255) NOT NULL,
		[Symbol] [nvarchar] (3) NOT NULL,
		[IsActive] [bit] NOT NULL CONSTRAINT [Default_Currencies_IsActive]  DEFAULT ((1)),
		[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Currencies_CreatedDate]  DEFAULT (GETDATE()),
		[ModifiedDate] [datetime] NOT NULL CONSTRAINT [Default_Currencies_ModifiedDate]  DEFAULT (GETDATE()),
		CONSTRAINT [PK_currencies] PRIMARY KEY CLUSTERED ([CurrencyId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[currencies]  WITH CHECK ADD  CONSTRAINT [CH_currencyUuid_length] CHECK  ((LEN([Uuid])=(36)))

	CREATE UNIQUE NONCLUSTERED INDEX [ixName_markets] ON [dbo].[currencies]
	([Name] ASC) WHERE ([IsActive]=(1)) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	INSERT INTO [dbo].[currencies] ([CurrencyId], [Name], [Symbol])
	SELECT 1, 'US Dollar', 'USD' UNION ALL
	SELECT 2, 'Janapese Yen', 'JPY'


END


--CURRENCY PAIRS
BEGIN

	CREATE TABLE [dbo].[assets](
		[AssetId] [int] NOT NULL,
		[Uuid] [nvarchar](36) NOT NULL CONSTRAINT [Default_Assets_Uuid] DEFAULT (NEWID()),
		[BaseCurrencyId] [int] NOT NULL,
		[CounterCurrencyId] [int] NOT NULL,
		[IsActive] [bit] NOT NULL CONSTRAINT [Default_Assets_IsActive]  DEFAULT ((1)),
		[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Assets_CreatedDate]  DEFAULT (GETDATE()),
		[ModifiedDate] [datetime] NOT NULL CONSTRAINT [Default_Assets_ModifiedDate]  DEFAULT (GETDATE()),
		CONSTRAINT [PK_assets] PRIMARY KEY CLUSTERED ([AssetId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[assets]  WITH CHECK ADD  CONSTRAINT [CH_assetUuid_length] CHECK  ((LEN([Uuid])=(36)))
	
	ALTER TABLE [dbo].[assets]  WITH CHECK ADD  CONSTRAINT [FK_Assets_BaseCurrency] FOREIGN KEY([BaseCurrencyId])
	REFERENCES [dbo].[currencies] ([CurrencyId])

	ALTER TABLE [dbo].[assets]  WITH CHECK ADD  CONSTRAINT [FK_Assets_CounterCurrency] FOREIGN KEY([CounterCurrencyId])
	REFERENCES [dbo].[currencies] ([CurrencyId])

	CREATE UNIQUE NONCLUSTERED INDEX [ixCurrencies_assets] ON [dbo].[assets]
	([BaseCurrencyId] ASC, [CounterCurrencyId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixBaseCurrency_assets] ON [dbo].[assets] ([BaseCurrencyId] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixCounterCurrency_assets] ON [dbo].[assets]([CounterCurrencyId] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	--INSERT INTO [assets2].[dbo].[assets] SELECT * FROM [assets].[dbo].[assets]
	INSERT INTO [dbo].[assets] ([AssetId], [BaseCurrencyId], [CounterCurrencyId])
	SELECT 1, 1, 2

END


--TIMEFRAMES
BEGIN

	CREATE TABLE [dbo].[timeframes](
		[TimeframeId] [int] NOT NULL,
		[Name] [nvarchar](4) NOT NULL,
		CONSTRAINT [PK_timeframes] PRIMARY KEY CLUSTERED ([TimeframeId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	);

	INSERT INTO [dbo].[timeframes]
	SELECT 1, '5M' UNION ALL
	SELECT 2, '15M' UNION ALL
	SELECT 3, '30M' UNION ALL
	SELECT 4, '1H' UNION ALL
	SELECT 5, '4H' UNION ALL
	SELECT 6, '1D' UNION ALL
	SELECT 7, '1W';

END


--DATES
BEGIN

	CREATE TABLE [dbo].[dates](
		[DateIndex] [int] NOT NULL CONSTRAINT [Default_Dates_DateIndex] DEFAULT ((1)),
		[TimeframeId] [int] NOT NULL CONSTRAINT [Default_Dates_Tiemframe] DEFAULT ((6)),
		[Date] [datetime] NOT NULL,		
		[ParentLevelDateIndex] [int] NULL,
		CONSTRAINT [PK_dates] PRIMARY KEY CLUSTERED ([DateIndex], [TimeframeId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[dates]  WITH CHECK ADD CONSTRAINT [CH_date_notWeekend] CHECK  ((datepart(weekday,[Date])>=(2) AND datepart(weekday,[Date])<=(6)))

	ALTER TABLE [dbo].[dates]  WITH CHECK ADD  CONSTRAINT [FK_dates_timeframe] FOREIGN KEY([TimeframeId])
	REFERENCES [dbo].[timeframes] ([TimeframeId])

	CREATE UNIQUE NONCLUSTERED INDEX [ixDate_dates] ON [dbo].[dates]  ([Date] ASC, [TimeframeId] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE UNIQUE NONCLUSTERED INDEX [ixDateIndexTimeframe_dates] ON [dbo].[dates] ([DateIndex] ASC, [TimeframeId] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixDateIndex_dates] ON [dbo].[dates] ([DateIndex] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
		
	CREATE NONCLUSTERED INDEX [ixTimeframe_dates] ON [dbo].[dates] ([TimeframeId] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixParentDateIndex_dates] ON [dbo].[dates] ([ParentLevelDateIndex] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

END

GO

IF OBJECT_ID('populateDateTable','P') IS NOT NULL DROP PROC [dbo].[populateDateTable];

GO

CREATE PROC [dbo].[populateDateTable] AS
BEGIN
		
	DECLARE @startDate AS DATETIME, @endDate AS DATETIME;
	SET @startDate = '2005-01-01 00:00:00';
	SET @endDate = '2018-12-31 23:55:00';


	-- Create initial 400000 periods with 1H range
	SELECT
		DATEADD(MINUTE, [number] * 5, @startDate) AS [date]
	INTO
		#All5M
	FROM
		[dbo].[predefinedNumbers]
	WHERE
		[number] BETWEEN 1 AND 4000000

	--Select only those in the given range
	SELECT 
		*
	INTO
		#5MInRange
	FROM 
		#All5M a
	WHERE
		a.[date] BETWEEN @startDate AND @endDate;
	DROP TABLE #All5M;

	--Append weekday.
	SELECT 
		*,
		DATEPART(DW, [Date]) AS [Weekday]
	INTO
		#Filtered5M
	FROM 
		#5MInRange;
	DROP TABLE #5MInRange;
			
	DELETE
	FROM #Filtered5M
	WHERE [Weekday] = 1 OR [Weekday] = 7;

	--Remove New Year.
	DELETE
	FROM #Filtered5M
	WHERE 
		(MONTH([Date]) = 1 AND DAY([Date]) = 1) OR
		(MONTH([Date]) = 12 AND DAY([Date]) = 31 AND CAST([Date] AS TIME) > '20:55:00');

	--Remove Christmas
	DELETE
	FROM #Filtered5M
	WHERE 
		(MONTH([Date]) = 12 AND DAY([Date]) = 25) OR
		(MONTH([Date]) = 12 AND DAY([Date]) = 24 AND CAST([Date] AS TIME) > '20:55:00');


	--Create table with 5M periods with minutes and hours
	SELECT
		d.*,
		DATEPART(MINUTE, d.[Date]) AS [OriginalMinute],
		DATEPART(HOUR, d.[Date]) AS [OriginalHour],
		CONVERT(DATE, d.[Date]) AS [OriginalDate]
	INTO #5MWithDateParts
	FROM
		#Filtered5M d;
	DROP TABLE #Filtered5M;

	--Create table with all date periods.
	SELECT
		a.[Date] AS [M5],
		DATEADD(MINUTE, -a.[OriginalMinute] % 15, a.[Date]) AS [M15],
		DATEADD(MINUTE, -a.[OriginalMinute] % 30, a.[Date]) AS [M30],
		DATEADD(MINUTE, -a.[OriginalMinute], a.[Date]) AS [H1],
		DATEADD(HOUR, -a.[OriginalHour] % 4, DATEADD(MINUTE, -a.[OriginalMinute], a.[Date])) AS [H4],
		a.[OriginalDate] AS [D1],
		DATEADD(D, -(DATEPART(dw, a.[OriginalDate]) + 5) % 7, a.[OriginalDate]) AS [W1]
	INTO 
		#AllDates
	FROM
		#5MWithDateParts a;
	DROP TABLE #5MWithDateParts;



	--Insert weeks
	INSERT INTO [dbo].[dates]
	SELECT
		[DateIndex] = ROW_NUMBER() OVER (ORDER BY a.[W1]),
		7 AS [TimeframeId],
		a.[W1] AS [Date],
		NULL AS [ParentLevelDateIndex]
	FROM
		(SELECT DISTINCT [W1] FROM #AllDates) a

	--Insert days
	INSERT INTO [dbo].[dates]
	SELECT
		[DateIndex] = ROW_NUMBER() OVER (ORDER BY x.[D1]),
		6 AS [TimeframeId],
		x.[D1] AS [Date],
		x.[DateIndex] AS [ParentLevelDateIndex]
	FROM
		(SELECT
			*
		FROM
			(SELECT DISTINCT [D1], [W1] FROM #AllDates) a
			LEFT JOIN (SELECT * FROM [dbo].[dates] WHERE [TimeframeId] = 7) w
		ON a.[W1] = w.[Date]) x

	--Insert H4
	INSERT INTO [dbo].[dates]
	SELECT
		[DateIndex] = ROW_NUMBER() OVER (ORDER BY x.[H4]),
		5 AS [TimeframeId],
		x.[H4] AS [Date],
		x.[DateIndex] AS [ParentLevelDateIndex]
	FROM
		(SELECT
			*
		FROM
			(SELECT DISTINCT [H4], [D1] FROM #AllDates) a
			LEFT JOIN (SELECT * FROM [dbo].[dates] WHERE [TimeframeId] = 6) w
		ON a.[D1] = w.[Date]) x

	--Insert H1
	INSERT INTO [dbo].[dates]
	SELECT
		[DateIndex] = ROW_NUMBER() OVER (ORDER BY x.[H1]),
		4 AS [TimeframeId],
		x.[H1] AS [Date],
		x.[DateIndex] AS [ParentLevelDateIndex]
	FROM
		(SELECT
			*
		FROM
			(SELECT DISTINCT [H1], [H4] FROM #AllDates) a
			LEFT JOIN (SELECT * FROM [dbo].[dates] WHERE [TimeframeId] = 5) w
		ON a.[H4] = w.[Date]) x

	--BEGIN
			
		----Insert M30
		--INSERT INTO [dbo].[dates]
		--SELECT
		--	[DateIndex] = ROW_NUMBER() OVER (ORDER BY x.[M30]),
		--	3 AS [TimeframeId],
		--	x.[M30] AS [Date],
		--	x.[DateIndex] AS [ParentLevelDateIndex]
		--FROM
		--	(SELECT
		--		*
		--	FROM
		--		(SELECT DISTINCT [M30], [H1] FROM #AllDates) a
		--		LEFT JOIN (SELECT * FROM [dbo].[dates] WHERE [TimeframeId] = 4) w
		--	ON a.[H1] = w.[Date]) x
		
		----Insert M15
		--INSERT INTO [dbo].[dates]
		--SELECT
		--	[DateIndex] = ROW_NUMBER() OVER (ORDER BY x.[M15]),
		--	2 AS [TimeframeId],
		--	x.[M15] AS [Date],
		--	x.[DateIndex] AS [ParentLevelDateIndex]
		--FROM
		--	(SELECT
		--		*
		--	FROM
		--		(SELECT DISTINCT [M15], [M30] FROM #AllDates) a
		--		LEFT JOIN (SELECT * FROM [dbo].[dates] WHERE [TimeframeId] = 3) w
		--	ON a.[M30] = w.[Date]) x

		----Insert M5
		--INSERT INTO [dbo].[dates]
		--SELECT
		--	[DateIndex] = ROW_NUMBER() OVER (ORDER BY x.[M5]),
		--	1 AS [TimeframeId],
		--	x.[M5] AS [Date],
		--	x.[DateIndex] AS [ParentLevelDateIndex]
		--FROM
		--	(SELECT
		--		*
		--	FROM
		--		(SELECT DISTINCT [M5], [M15] FROM #AllDates) a
		--		LEFT JOIN (SELECT * FROM [dbo].[dates] WHERE [TimeframeId] = 2) w
		--	ON a.[M15] = w.[Date]) x

	--END

	--Clean up
	BEGIN
		DROP TABLE #AllDates;
	END

END
GO

EXEC [dbo].[populateDateTable];

GO


--TIMESTAMPS
BEGIN
	
	CREATE TABLE [dbo].[timestamps](
		[AssetId] [int] NOT NULL,
		[TimeframeId] [int] NOT NULL,
		[ExtremaLastAnalyzedIndex] [int] NULL,
		[TrendlinesAnalysisLastQuotationIndex] [int] NULL,
		[TrendlinesAnalysisLastExtremumGroupId] [int] NULL,
		[AdxLastAnalyzedIndex] [int] NULL,
		[MacdLastAnalyzedIndex] [int] NULL
	) ON [PRIMARY];

	ALTER TABLE [dbo].[timestamps]  WITH CHECK ADD CONSTRAINT [FK_Timestamps_AssetId] FOREIGN KEY([AssetId])
	REFERENCES [dbo].[assets] ([AssetId])

	ALTER TABLE [dbo].[timestamps]  WITH CHECK ADD CONSTRAINT [FK_Timestamps_Timeframe] FOREIGN KEY([TimeframeId])
	REFERENCES [dbo].[timeframes] ([TimeframeId])

	CREATE UNIQUE NONCLUSTERED INDEX [ixAssetTimeframe_Timestamps] ON [dbo].[timestamps] 
	([AssetId], [TimeframeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixAssetId_Timestamps] ON [dbo].[timestamps] 
	([AssetId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixTimeframe_Timestamps] ON [dbo].[timestamps] 
	([TimeframeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
END


--QUOTES
BEGIN
	
	CREATE TABLE [dbo].[quotes](
		[AssetId] [int] NOT NULL,
		[TimeframeId] [int] NOT NULL,
		[DateIndex] [int] NOT NULL,
		[Open] [float] NOT NULL,
		[Low] [float] NOT NULL,
		[High] [float] NOT NULL,
		[Close] [float] NOT NULL,
		[Volume] [bigint] NOT NULL,
		[IsComplete] [bit] NOT NULL CONSTRAINT [Default_Quotes_IsComplete] DEFAULT(0),
		[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Quotes_CreatedDate] DEFAULT (GETDATE())
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[quotes]  WITH CHECK ADD CONSTRAINT [FK_Quotes_AssetId] FOREIGN KEY([AssetId])
	REFERENCES [dbo].[assets] ([AssetId])

	ALTER TABLE [dbo].[quotes]  WITH CHECK ADD CONSTRAINT [FK_Quotes_DateIndex] FOREIGN KEY([DateIndex], [TimeframeId])
	REFERENCES [dbo].[dates] ([DateIndex], [TimeframeId])
	
	CREATE UNIQUE NONCLUSTERED INDEX [ixAssetTimeframeDateIndex_Quotes] ON [dbo].[quotes] 
	([AssetId] ASC, [TimeframeId] ASC, [DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixAssetId_Quotes] ON [dbo].[quotes] 
	([AssetId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixTimeframe_Quotes] ON [dbo].[quotes] 
	([TimeframeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixDateIndex_Quotes] ON [dbo].[quotes] 
	([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	
END


--QUOTES OUT-OF-DATE
BEGIN
	
	CREATE TABLE [dbo].[quotesOutOfDate](
		[AssetId] [int] NOT NULL,
		[TimeframeId] [int] NOT NULL,
		[Date] [datetime] NOT NULL,
		[Open] [float] NOT NULL,
		[Low] [float] NOT NULL,
		[High] [float] NOT NULL,
		[Close] [float] NOT NULL,
		[Volume] [bigint] NOT NULL,
		[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_QuotesOutOfDate_CreatedDate]  DEFAULT (GETDATE())
	) ON [PRIMARY]
	
	CREATE NONCLUSTERED INDEX [ixAssetId_QuotesOutOfDate] ON [dbo].[quotesOutOfDate] 
	([AssetId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixTimeframe_QuotesOutOfDate] ON [dbo].[quotesOutOfDate] 
	([TimeframeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixDateIndex_QuotesOutOfDate] ON [dbo].[quotesOutOfDate] 
	([Date] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
END


--PRICES [TODO]
BEGIN

	PRINT 'TODO later'
	--CREATE TABLE [dbo].[prices](
	--	[AssetId] [int] NOT NULL,
	--	[TimeframeId] [int] NOT NULL,
	--	[DateIndex] [int] NOT NULL,
	--	[DeltaClosePrice] [float] NOT NULL,
	--	[PriceDirection2D] [int] NULL,
	--	[PriceDirection3D] [int] NULL,
	--	[PriceGap] [float] NULL,
	--	[CloseRatio] [float] NULL,
	--	[ExtremumRatio] [float] NULL,
	--	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_prices_CreatedDate]  DEFAULT (getdate())
	--) ON [PRIMARY]

	--ALTER TABLE [dbo].[prices]  WITH CHECK ADD  CONSTRAINT [FK_Prices_AssetId] FOREIGN KEY([AssetId])
	--REFERENCES [dbo].[assets] ([Id])

	--ALTER TABLE [dbo].[prices]  WITH CHECK ADD  CONSTRAINT [FK_Prices_DateIndex] FOREIGN KEY([DateIndex], [TimeframeId])
	--REFERENCES [dbo].[dates] ([DateIndex], [TimeframeId])
	
	--CREATE UNIQUE NONCLUSTERED INDEX [ixAssetTimeframeDateIndex_prices] ON [dbo].[prices] 
	--([AssetId] ASC, [TimeframeId] ASC, [DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	--CREATE NONCLUSTERED INDEX [ixAssetId_prices] ON [dbo].[prices] 
	--([AssetId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	--CREATE NONCLUSTERED INDEX [ixTimeframe_prices] ON [dbo].[prices] 
	--([TimeframeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	--CREATE NONCLUSTERED INDEX [ixDateIndex_prices] ON [dbo].[prices] 
	--([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
		
END


--EXTREMA TABLES
BEGIN

	--EXTREMUM TYPES
	BEGIN

		CREATE TABLE [dbo].[extremumTypes](
			[ExtremumTypeId] [int] NOT NULL,
			[ExtremumTypeName] [nvarchar](255) NOT NULL,
			[IsPeak] [bit] NOT NULL,
			CONSTRAINT [PK_extremumTypes] PRIMARY KEY CLUSTERED ([ExtremumTypeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		);
		INSERT INTO [dbo].[extremumTypes]
		SELECT 1, 'Peak by close', 1 UNION ALL
		SELECT 2, 'Peak by high', 1 UNION ALL
		SELECT 3, 'Trough by close', 0 UNION ALL
		SELECT 4, 'Trough by low', 0

	END
	
	--EXTREMA
	BEGIN

		CREATE TABLE [dbo].[extrema](
			--Metadata.
			[ExtremumId] [int] IDENTITY(1,1) NOT NULL,
			[AssetId] [int] NOT NULL,
			[TimeframeId] [int] NOT NULL,
			[DateIndex] [int] NOT NULL,
			[ExtremumTypeId] [int] NOT NULL,
			--Status
			[IsEvaluationOpen] [bit] NOT NULL,
			--Evaluation properties.
			[EarlierCounter] [int] NULL,
			[LaterCounter] [int] NULL,
			[EarlierAmplitude] [float] NULL,
			[EarlierTotalArea] [float] NULL,
			[EarlierAverageArea] [float] NULL,
			[LaterAmplitude] [float] NULL,
			[LaterTotalArea] [float] NULL,
			[LaterAverageArea] [float] NULL,
			[EarlierChange1] [float] NULL,
			[EarlierChange2] [float] NULL,
			[EarlierChange3] [float] NULL,
			[EarlierChange5] [float] NULL,
			[EarlierChange10] [float] NULL,
			[LaterChange1] [float] NULL,
			[LaterChange2] [float] NULL,
			[LaterChange3] [float] NULL,
			[LaterChange5] [float] NULL,
			[LaterChange10] [float] NULL,
			[Value] [float] NULL,
			CONSTRAINT [PK_extrema] PRIMARY KEY CLUSTERED ([ExtremumId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	
		ALTER TABLE [dbo].[extrema]  WITH CHECK ADD CONSTRAINT [FK_Extrema_ExtremumTypeId] FOREIGN KEY([ExtremumTypeId])
		REFERENCES [dbo].[extremumTypes] ([ExtremumTypeId]);

		ALTER TABLE [dbo].[extrema]  WITH CHECK ADD CONSTRAINT [FK_Extrema_AssetId] FOREIGN KEY([AssetId])
		REFERENCES [dbo].[assets] ([AssetId]);

		ALTER TABLE [dbo].[extrema]  WITH CHECK ADD CONSTRAINT [FK_Extrema_DateIndex] FOREIGN KEY([DateIndex], [TimeframeId])
		REFERENCES [dbo].[dates] ([DateIndex], [TimeframeId]);
		
		ALTER TABLE [dbo].[extrema] ADD  CONSTRAINT [Default_Extrema_IsEvaluationOpen]  DEFAULT ((1)) FOR [IsEvaluationOpen];

		CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_extrema] ON [dbo].[extrema]
		([AssetId], [TimeframeId], [DateIndex], [ExtremumTypeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixDateIndex_extrema] ON [dbo].[extrema]
		([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
		CREATE NONCLUSTERED INDEX [ixAsset_extrema] ON [dbo].[extrema]
		([AssetId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
		CREATE NONCLUSTERED INDEX [ixTimeframe_extrema] ON [dbo].[extrema]
		([TimeframeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixExtremumType_extrema] ON [dbo].[extrema]
		([ExtremumTypeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	END

	--EXTREMA GROUPS
	BEGIN

		CREATE TABLE [dbo].[extremumGroups](
			[ExtremumGroupId] [int] IDENTITY(1,1) NOT NULL,
			[AssetId] [int] NOT NULL,
			[TimeframeId] [int] NOT NULL,
			[IsPeak] [int] NOT NULL,
			[MasterExtremumId] [int] NOT NULL,
			[SlaveExtremumId] [int] NOT NULL,
			[MasterDateIndex] [int] NOT NULL,
			[SlaveDateIndex] [int] NOT NULL,
			[StartDateIndex] [int] NOT NULL,
			[EndDateIndex] [int] NOT NULL,
			[OCPriceLevel] [float] NOT NULL,
			[ExtremumPriceLevel] [float] NOT NULL,
			[MiddlePriceLevel] [float] NOT NULL,
			CONSTRAINT [PK_extremaGroups] PRIMARY KEY CLUSTERED ([ExtremumGroupId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
		
		ALTER TABLE [dbo].[extremumGroups]  WITH CHECK ADD CONSTRAINT [FK_ExtremumGroups_AssetId] FOREIGN KEY([AssetId])
		REFERENCES [dbo].[assets] ([AssetId]);

		ALTER TABLE [dbo].[extremumGroups]  WITH CHECK ADD CONSTRAINT [FK_ExtremumGroups_TimeframeId] FOREIGN KEY([TimeframeId])
		REFERENCES [dbo].[timeframes] ([TimeframeId]);
			
		ALTER TABLE [dbo].[extremumGroups]  WITH CHECK ADD  CONSTRAINT [FK_ExtremumGroups_MasterExtremum] FOREIGN KEY([MasterExtremumId])
		REFERENCES [dbo].[extrema] ([ExtremumId])
	
		ALTER TABLE [dbo].[extremumGroups]  WITH CHECK ADD  CONSTRAINT [FK_ExtremumGroups_SlaveExtremum] FOREIGN KEY([SlaveExtremumId])
		REFERENCES [dbo].[extrema] ([ExtremumId])

		CREATE NONCLUSTERED INDEX [ixAsset_extrema] ON [dbo].[extremumGroups]
		([AssetId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
		CREATE NONCLUSTERED INDEX [ixTimeframe_extrema] ON [dbo].[extremumGroups]
		([TimeframeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
		
		CREATE NONCLUSTERED INDEX [ixIsPeak_extremumGroups] ON [dbo].[extremumGroups]
		([IsPeak] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixMasterExtremumId_extremumGroups] ON [dbo].[extremumGroups]
		([MasterExtremumId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
		CREATE NONCLUSTERED INDEX [ixSlaveExtremumId_extremumGroups] ON [dbo].[extremumGroups]
		([SlaveExtremumId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixMasterDateIndex_extremumGroups] ON [dbo].[extremumGroups]
		([MasterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixSlaveDateIndex_extremumGroups] ON [dbo].[extremumGroups]
		([SlaveDateIndex]  ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixStartDateIndex_extremumGroups] ON [dbo].[extremumGroups]
		([StartDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixEndDateIndex_extremumGroups] ON [dbo].[extremumGroups]
		([EndDateIndex]  ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	END

END



--TRENDLINE ANALYSIS
BEGIN


	--TRENDLINES
	BEGIN

		CREATE TABLE [dbo].[trendlines](
			[TrendlineId] [int] IDENTITY(1,1) NOT NULL,	
			[AssetId] [int] NOT NULL,
			[TimeframeId] [int] NOT NULL,
			[BaseExtremumGroupId] [int] NOT NULL,
			[BaseDateIndex] [int] NOT NULL,
			[BaseLevel] [float] NOT NULL,
			[CounterExtremumGroupId] [int] NOT NULL,
			[CounterDateIndex] [int] NOT NULL,
			[CounterLevel] [float] NOT NULL,
			[Angle] [float] NOT NULL,
			[StartDateIndex] [int] NULL,
			[EndDateIndex] [int] NULL,
			[IsOpenFromLeft] [bit] NOT NULL CONSTRAINT [Default_Trendlines_IsOpenFromLeft] DEFAULT(1),
			[IsOpenFromRight] [bit] NOT NULL CONSTRAINT [Default_Trendlines_IsOpenFromRight] DEFAULT(1),
			[CandlesDistance] [int] NOT NULL,
			[ShowOnChart] [bit] NOT NULL CONSTRAINT [Default_Trendlines_ShowOnChart] DEFAULT(0),
			[Value] [float] NOT NULL CONSTRAINT [Default_Trendlines_Value] DEFAULT(0),
			CONSTRAINT [PK_trendlines] PRIMARY KEY CLUSTERED ([TrendlineId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	
		ALTER TABLE [dbo].[trendlines]  WITH CHECK ADD  CONSTRAINT [FK_Trendlines_AssetId] FOREIGN KEY([AssetId])
		REFERENCES [dbo].[assets] ([AssetId])

		ALTER TABLE [dbo].[trendlines]  WITH CHECK ADD  CONSTRAINT [FK_Trendlines_Timeframe] FOREIGN KEY([TimeframeId])
		REFERENCES [dbo].[timeframes] ([TimeframeId])
	
		ALTER TABLE [dbo].[trendlines]  WITH CHECK ADD  CONSTRAINT [FK_Trendlines_BaseExtremumGroup] FOREIGN KEY([BaseExtremumGroupId])
		REFERENCES [dbo].[extremumGroups] ([ExtremumGroupId])
	
		ALTER TABLE [dbo].[trendlines]  WITH CHECK ADD  CONSTRAINT [FK_Trendlines_CounterExtremumGroup] FOREIGN KEY([CounterExtremumGroupId])
		REFERENCES [dbo].[extremumGroups] ([ExtremumGroupId])

		CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_trendlines] ON [dbo].[trendlines]
		([AssetId], [TimeframeId], [BaseExtremumGroupId], [BaseLevel], [CounterExtremumGroupId], [CounterLevel]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
		CREATE NONCLUSTERED INDEX [ixAsset_trendlines] ON [dbo].[trendlines]
		([AssetId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
		CREATE NONCLUSTERED INDEX [ixTimeframe_trendlines] ON [dbo].[trendlines]
		([TimeframeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
		CREATE NONCLUSTERED INDEX [ixBaseDateIndex_trendlines] ON [dbo].[trendlines]
		([BaseDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
		CREATE NONCLUSTERED INDEX [ixCounterDateIndex_trendlines] ON [dbo].[trendlines]
		([CounterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixIsOpenFromLeft_trendlines] ON [dbo].[trendlines]
		([IsOpenFromLeft] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixIsOpenFromRight_trendlines] ON [dbo].[trendlines]
		([IsOpenFromRight] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	END


	--TREND HITS
	BEGIN

		CREATE TABLE [dbo].[trendHits](
			[TrendHitId] [int] IDENTITY(1, 1) NOT NULL,
			[TrendlineId] [int] NOT NULL,
			[ExtremumGroupId] [int] NOT NULL,
			[Value] [float] NULL,
			CONSTRAINT [PK_trendHits] PRIMARY KEY CLUSTERED ([TrendHitId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY];
	
		ALTER TABLE [dbo].[trendHits]  WITH CHECK ADD  CONSTRAINT [FK_TrendHits_TrendlineId] FOREIGN KEY([TrendlineId])
		REFERENCES [dbo].[trendlines] ([TrendlineId])

		ALTER TABLE [dbo].[trendHits]  WITH CHECK ADD  CONSTRAINT [FK_TrendHits_ExtremumGroupId] FOREIGN KEY([ExtremumGroupId])
		REFERENCES [dbo].[extremumGroups] ([ExtremumGroupId])
		
		CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_trendHits] ON [dbo].[trendHits]
		([TrendlineId] ASC, [ExtremumGroupId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixTrendlineId_trendHits] ON [dbo].[trendHits]
		([TrendlineId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixExtremumGroup_trendHits] ON [dbo].[trendHits]
		([ExtremumGroupId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	END


	--TREND BREAKS
	BEGIN

		CREATE TABLE [dbo].[trendBreaks](
			[TrendBreakId] [int] IDENTITY(1, 1) NOT NULL,
			[TrendlineId] [int] NOT NULL,
			[DateIndex] [int] NOT NULL,
			[BreakFromAbove] [int] NOT NULL,
			[Value] [float] NULL,
			CONSTRAINT [PK_trendBreaks] PRIMARY KEY CLUSTERED ([TrendBreakId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY];

		ALTER TABLE [dbo].[trendBreaks]  WITH CHECK ADD  CONSTRAINT [FK_TrendBreaks_TrendlineId] FOREIGN KEY([TrendlineId])
		REFERENCES [dbo].[trendlines] ([TrendlineId])

		CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_trendBreaks] ON [dbo].[trendBreaks]
		([TrendlineId] ASC, [DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixTrendlineId_trendlinesBreaks] ON [dbo].[trendBreaks]
		([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixDateIndex_trendlinesBreaks] ON [dbo].[trendBreaks]
		([DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	END


	--TREND RANGES
	BEGIN

		CREATE TABLE [dbo].[trendRanges](
			[TrendRangeId] [int] IDENTITY(1, 1) NOT NULL,
			[TrendlineId] [int] NOT NULL,
			[BaseId] [int] NOT NULL,
			[BaseIsHit] [int] NOT NULL,
			[BaseDateIndex] [int] NOT NULL,
			[CounterId] [int] NOT NULL,
			[CounterIsHit] [int] NOT NULL,
			[CounterDateIndex] [int] NOT NULL,
			[IsPeak] [int] NOT NULL,
			[ExtremumPriceCrossPenaltyPoints] [float] NULL,
			[ExtremumPriceCrossCounter] [int] NULL,
			[OCPriceCrossPenaltyPoints] [float] NULL,
			[OCPriceCrossCounter] [int] NULL,
			[TotalCandles] [int] NULL,
			[AverageVariation] [float] NULL,
			[ExtremumVariation] [float] NULL,
			[OpenCloseVariation] [float] NULL,
			[BaseHitValue] [float] NULL,
			[CounterHitValue] [float] NULL,
			[Value] [float] NULL,
			CONSTRAINT [PK_trendRanges] PRIMARY KEY CLUSTERED ([TrendRangeId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY];
		
		ALTER TABLE [dbo].[trendRanges]  WITH CHECK ADD  CONSTRAINT [FK_TrendRanges_TrendlineId] FOREIGN KEY([TrendlineId])
		REFERENCES [dbo].[trendlines] ([TrendlineId])

		CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_trendRanges] ON [dbo].[trendRanges]
		([TrendlineId] ASC, [BaseId] ASC, [CounterId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixTrendlineId_trendRanges] ON [dbo].[trendRanges]
		([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixBaseId_trendRanges] ON [dbo].[trendRanges]
		([BaseId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixBaseDateIndex_trendRanges] ON [dbo].[trendRanges]
		([BaseDateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixCounterId_trendRanges] ON [dbo].[trendRanges]
		([CounterId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixCounterDateIndex_trendRanges] ON [dbo].[trendRanges]
		([CounterDateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	END


END

GO

CREATE TRIGGER [dbo].[Trigger_Currencies_Delete] ON [dbo].[currencies] INSTEAD OF DELETE
AS
BEGIN
	DELETE FROM [dbo].[assets] WHERE [BaseCurrencyId] IN (SELECT [CurrencyId] FROM deleted)
	DELETE FROM [dbo].[assets] WHERE [CounterCurrencyId] IN (SELECT [CurrencyId] FROM deleted)
	DELETE FROM [dbo].[currencies] WHERE [CurrencyId] IN (SELECT [CurrencyId] FROM deleted)
END
GO

CREATE TRIGGER [Trigger_Currencies_Update] ON [dbo].[currencies] AFTER UPDATE
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE c
	SET 
		c.[ModifiedDate] = GETDATE()
	FROM 
		[dbo].[currencies] c
		INNER JOIN inserted i ON c.[CurrencyId] = i.[CurrencyId] 
END


GO

CREATE TRIGGER [dbo].[Trigger_Assets_Delete] ON [dbo].[assets] INSTEAD OF DELETE
AS

	SET NOCOUNT ON
	DELETE FROM [dbo].[timestamps] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
	DELETE FROM [dbo].[quotes] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
	DELETE FROM [dbo].[quotesOutOfDate] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
	DELETE FROM [dbo].[extrema] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
	DELETE FROM [dbo].[extremumGroups] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
	DELETE FROM [dbo].[trendlines] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
	DELETE FROM [dbo].[assets] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);

GO

CREATE TRIGGER [dbo].[Trigger_Assets_Update] ON [dbo].[assets] AFTER UPDATE
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE a
	SET 
		a.[ModifiedDate] = GETDATE()
	FROM 
		[dbo].[assets] a
		INNER JOIN inserted i ON a.[AssetId] = i.[AssetId] 
END


GO

CREATE TRIGGER [dbo].[Trigger_Timeframes_Delete] ON [dbo].[timeframes] INSTEAD OF DELETE
AS

	SET NOCOUNT ON
	DELETE FROM [dbo].[timestamps] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	DELETE FROM [dbo].[dates] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	DELETE FROM [dbo].[quotes] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	DELETE FROM [dbo].[quotesOutOfDate] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	DELETE FROM [dbo].[extrema] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	DELETE FROM [dbo].[extremumGroups] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	DELETE FROM [dbo].[trendlines] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	DELETE FROM [dbo].[timeframes] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);

GO

CREATE TRIGGER [dbo].[Trigger_Dates_Delete] ON [dbo].[dates] INSTEAD OF DELETE
AS

	SET NOCOUNT ON
	DELETE FROM [dbo].[quotes] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);
	DELETE FROM [dbo].[extrema] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);
	DELETE FROM [dbo].[trendlines] WHERE [BaseDateIndex] IN (SELECT [DateIndex] FROM deleted);
	DELETE FROM [dbo].[trendlines] WHERE [CounterDateIndex] IN (SELECT [DateIndex] FROM deleted);
	DELETE FROM [dbo].[dates] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);

GO

CREATE TRIGGER [dbo].[Trigger_ExtremumTypes_Delete] ON [dbo].[extremumTypes]
INSTEAD OF DELETE
AS
	SET NOCOUNT ON
	DELETE FROM [dbo].[extrema] WHERE [ExtremumTypeId] IN (SELECT [ExtremumTypeId] FROM deleted);
	DELETE FROM [dbo].[extremumTypes] WHERE [ExtremumTypeId] IN (SELECT [ExtremumTypeId] FROM deleted);

GO

CREATE TRIGGER [dbo].[Trigger_Extrema_Delete] ON [dbo].[extrema]
INSTEAD OF DELETE
AS
	SET NOCOUNT ON
	DELETE FROM [dbo].[extremumGroups] WHERE [MasterExtremumId] IN (SELECT [ExtremumId] FROM deleted);
	DELETE FROM [dbo].[extremumGroups] WHERE [SlaveExtremumId] IN (SELECT [ExtremumId] FROM deleted);
	DELETE FROM [dbo].[extrema] WHERE [ExtremumId] IN (SELECT [ExtremumId] FROM deleted);
GO

CREATE TRIGGER [dbo].[Trigger_ExtremumGroups_Delete] ON [dbo].[extremumGroups]
INSTEAD OF DELETE
AS
	SET NOCOUNT ON
	DELETE FROM [dbo].[trendHits] WHERE [ExtremumGroupId] IN (SELECT [ExtremumGroupId] FROM deleted);
	DELETE FROM [dbo].[trendlines] WHERE [BaseExtremumGroupId] IN (SELECT [ExtremumGroupId] FROM deleted);
	DELETE FROM [dbo].[trendlines] WHERE [CounterExtremumGroupId] IN (SELECT [ExtremumGroupId] FROM deleted);
	DELETE FROM [dbo].[extremumGroups] WHERE [ExtremumGroupId] IN (SELECT [ExtremumGroupId] FROM deleted);

	DELETE FROM [dbo].[trendlines]
	WHERE [BaseExtremumGroupId] IN (SELECT [ExtremumGroupId] FROM deleted) OR [CounterExtremumGroupId] IN (SELECT [ExtremumGroupId] FROM deleted)
	
	DELETE FROM [dbo].[extremumGroups] WHERE [ExtremumGroupId] IN (SELECT [ExtremumGroupId] FROM deleted);
GO

CREATE TRIGGER [dbo].[Trigger_Trendlines_Delete] ON [dbo].[trendlines]
INSTEAD OF DELETE
AS
	SET NOCOUNT ON
	DELETE FROM [dbo].[trendHits] WHERE [TrendlineId] IN (SELECT [TrendlineId] FROM deleted);
	DELETE FROM [dbo].[trendBreaks] WHERE [TrendlineId] IN (SELECT [TrendlineId] FROM deleted);
	DELETE FROM [dbo].[trendRanges] WHERE [TrendlineId] IN (SELECT [TrendlineId] FROM deleted);
	DELETE FROM [dbo].[trendlines] WHERE [TrendlineId] IN (SELECT [TrendlineId] FROM deleted);

GO

--FUNCTIONS
--Drop existing version of functions
BEGIN
	IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MinValue]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[MinValue]
	IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MaxValue]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[MaxValue]
END

GO

--MIN.VALUE
CREATE FUNCTION [dbo].[MinValue](@a AS FLOAT, @b AS FLOAT)
RETURNS FLOAT
AS
BEGIN
	RETURN IIF (@a < @b, @a, @b);
END

GO

--MAX.VALUE
CREATE FUNCTION [dbo].[MaxValue](@a AS FLOAT, @b AS FLOAT)
RETURNS FLOAT
AS
BEGIN
	RETURN IIF (@a > @b, @a, @b);
END

GO



--VIEWS
IF OBJECT_ID('ViewDataInfo','v') IS NOT NULL DROP VIEW [dbo].[ViewDataInfo];

GO

CREATE VIEW [dbo].[ViewDataInfo] AS
SELECT
	q.[AssetId],
	q.[TimeframeId],
	MIN(d.[Date]) AS [StartDate],
	MAX(d.[Date]) AS [EndDate],
	MIN(q.[DateIndex]) AS [StartIndex],
	MAX(q.[DateIndex]) AS [EndIndex],
	CAST(MIN(q.[Low]) AS NUMERIC(36,2)) AS [MinLevel],
	CAST(MAX(q.[High]) AS NUMERIC(36,2)) AS [MaxLevel],
	COUNT(*) AS [Counter]
FROM
	[dbo].[quotes] q
	LEFT JOIN [dbo].[dates] d
	ON q.[TimeframeId] = d.[TimeframeId] AND q.[DateIndex] = d.[DateIndex]
GROUP BY 
	q.[AssetId], q.[TimeframeId];

GO


IF OBJECT_ID('ViewQuotes','v') IS NOT NULL DROP VIEW [dbo].[ViewQuotes];

GO

CREATE VIEW [dbo].[ViewQuotes] AS
SELECT
	q.*,
	d.[Date] AS [Date]
FROM
	[dbo].[quotes] q
	LEFT JOIN [dbo].[dates] d
	ON q.[TimeframeId] = d.[TimeframeId] AND q.[DateIndex] = d.[DateIndex]

GO



--PROCEDURES
IF OBJECT_ID('removeAllTables','P') IS NOT NULL DROP PROC [dbo].[removeAllTables];
GO

CREATE PROC [dbo].[removeAllTables] AS
BEGIN
	IF OBJECT_ID('dbo.trendRanges', 'U') IS NOT NULL DROP TABLE [dbo].[trendRanges];
	IF OBJECT_ID('dbo.trendBreaks', 'U') IS NOT NULL DROP TABLE [dbo].[trendBreaks];
	IF OBJECT_ID('dbo.trendHits', 'U') IS NOT NULL DROP TABLE [dbo].[trendHits];
	IF OBJECT_ID('dbo.trendlines', 'U') IS NOT NULL  DROP TABLE [dbo].[trendlines];
	IF OBJECT_ID('dbo.timestamps', 'U') IS NOT NULL  DROP TABLE [dbo].[timestamps];
	IF OBJECT_ID('dbo.extremumGroups', 'U') IS NOT NULL  DROP TABLE [dbo].[extremumGroups];
	IF OBJECT_ID('dbo.extrema', 'U') IS NOT NULL  DROP TABLE [dbo].[extrema];
	IF OBJECT_ID('dbo.extremumTypes', 'U') IS NOT NULL  DROP TABLE [dbo].[extremumTypes];
	IF OBJECT_ID('dbo.settingsNumeric', 'U') IS NOT NULL  DROP TABLE [dbo].[settingsNumeric];
	IF OBJECT_ID('dbo.settingsText', 'U') IS NOT NULL  DROP TABLE [dbo].[settingsText];
	IF OBJECT_ID('dbo.quotes', 'U') IS NOT NULL  DROP TABLE [dbo].[quotes];
	IF OBJECT_ID('dbo.quotesOutOfDate', 'U') IS NOT NULL  DROP TABLE [dbo].[quotesOutOfDate];
	IF OBJECT_ID('dbo.predefinedNumbers', 'U') IS NOT NULL  DROP TABLE [dbo].[predefinedNumbers];
	IF OBJECT_ID('dbo.errorLogs', 'U') IS NOT NULL  DROP TABLE [dbo].[errorLogs];
	IF OBJECT_ID('dbo.dates', 'U') IS NOT NULL  DROP TABLE [dbo].[dates];
	IF OBJECT_ID('dbo.timeframes', 'U') IS NOT NULL  DROP TABLE [dbo].[timeframes];
	IF OBJECT_ID('dbo.assets', 'U') IS NOT NULL  DROP TABLE [dbo].[assets];
	IF OBJECT_ID('dbo.currencies', 'U') IS NOT NULL  DROP TABLE [dbo].[currencies];
END

GO


--ROLLBACK TRANSACTION;
--COMMIT TRANSACTION;