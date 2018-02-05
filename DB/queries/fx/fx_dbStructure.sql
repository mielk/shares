USE [fx];

BEGIN TRANSACTION;


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
		[Uuid] [nvarchar](36) NOT NULL DEFAULT (NEWID()),
		[Name] [nvarchar](255) NOT NULL,
		[Symbol] [nvarchar] (3) NOT NULL,
		[IsActive] [bit] NOT NULL CONSTRAINT [Default_Markets_IsActive]  DEFAULT ((1)),
		[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Markets_CreatedDate]  DEFAULT (GETDATE()),
		[ModifiedDate] [datetime] NOT NULL CONSTRAINT [Default_Markets_ModifiedDate]  DEFAULT (GETDATE()),
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
		[Uuid] [nvarchar](36) NOT NULL DEFAULT (NEWID()),
		[BaseCurrencyId] [int] NOT NULL,
		[CounterCurrencyId] [int] NOT NULL,
		[IsActive] [bit] NOT NULL CONSTRAINT [Default_Campaigns_IsActive]  DEFAULT ((1)),
		[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Campaigns_CreatedDate]  DEFAULT (GETDATE()),
		[ModifiedDate] [datetime] NOT NULL CONSTRAINT [Default_Campaigns_ModifiedDate]  DEFAULT (GETDATE()),
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
		[DateIndex] [int] NOT NULL DEFAULT ((1)),
		[TimeframeId] [int] NOT NULL DEFAULT ((6)),
		[Date] [datetime] NOT NULL,		
		[ParentLevelDateIndex] [int] NULL,
		CONSTRAINT [PK_dates] PRIMARY KEY CLUSTERED ([DateIndex], [TimeframeId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[dates]  WITH CHECK ADD CONSTRAINT [CH_date_notWeekend] CHECK  ((datepart(weekday,[Date])>=(2) AND datepart(weekday,[Date])<=(6)))

	ALTER TABLE [dbo].[dates]  WITH CHECK ADD  CONSTRAINT [FK_dates_timeframe] FOREIGN KEY([TimeframeId])
	REFERENCES [dbo].[timeframes] ([TimeframeId]) ON DELETE CASCADE

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

	--Populate table
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

END


--TIMESTAMPS
BEGIN

	CREATE TABLE [dbo].[timestamps](
		[AssetId] [int] NOT NULL,
		[TimeframeId] [int] NOT NULL,
		[ExtremaLastAnalyzedIndex] [int] NULL,
		[TrendlinesLastAnalyzedIndex] [int] NULL
	) ON [PRIMARY];

	ALTER TABLE [dbo].[timestamps]  WITH CHECK ADD CONSTRAINT [FK_Timestamps_AssetId] FOREIGN KEY([AssetId])
	REFERENCES [dbo].[assets] ([AssetId]) ON DELETE CASCADE

	ALTER TABLE [dbo].[timestamps]  WITH CHECK ADD CONSTRAINT [FK_Timestamps_Timeframe] FOREIGN KEY([TimeframeId])
	REFERENCES [dbo].[timeframes] ([TimeframeId]) ON DELETE CASCADE

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
		[IsComplete] [bit] NOT NULL DEFAULT(0),
		[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Quotes_CreatedDate]  DEFAULT (GETDATE())
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[quotes]  WITH CHECK ADD CONSTRAINT [FK_Quotes_AssetId] FOREIGN KEY([AssetId])
	REFERENCES [dbo].[assets] ([AssetId]) ON DELETE CASCADE

	ALTER TABLE [dbo].[quotes]  WITH CHECK ADD CONSTRAINT [FK_Quotes_DateIndex] FOREIGN KEY([DateIndex], [TimeframeId])
	REFERENCES [dbo].[dates] ([DateIndex], [TimeframeId]) ON DELETE CASCADE
	
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


--PRICES
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
	--REFERENCES [dbo].[assets] ([Id]) ON DELETE CASCADE

	--ALTER TABLE [dbo].[prices]  WITH CHECK ADD  CONSTRAINT [FK_Prices_DateIndex] FOREIGN KEY([DateIndex], [TimeframeId])
	--REFERENCES [dbo].[dates] ([DateIndex], [TimeframeId]) ON DELETE CASCADE
	
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
			[MasterExtremumDateIndex] [int] NOT NULL,
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
		REFERENCES [dbo].[extremumTypes] ([ExtremumTypeId]) ON DELETE CASCADE;

		ALTER TABLE [dbo].[extrema]  WITH CHECK ADD CONSTRAINT [FK_Extrema_AssetId] FOREIGN KEY([AssetId])
		REFERENCES [dbo].[assets] ([AssetId]) ON DELETE CASCADE;

		ALTER TABLE [dbo].[extrema]  WITH CHECK ADD CONSTRAINT [FK_Extrema_DateIndex] FOREIGN KEY([DateIndex], [TimeframeId])
		REFERENCES [dbo].[dates] ([DateIndex], [TimeframeId]) ON DELETE CASCADE;

		ALTER TABLE [dbo].[extrema]  WITH CHECK ADD CONSTRAINT [FK_Extrema_MasterExtremumDateIndex] FOREIGN KEY([MasterExtremumDateIndex], [TimeframeId])
		REFERENCES [dbo].[dates] ([DateIndex], [TimeframeId]);
		
		ALTER TABLE [dbo].[extrema] ADD  CONSTRAINT [Default_Extrema_IsEvaluationOpen]  DEFAULT ((1)) FOR [IsEvaluationOpen];

		CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_extrema] ON [dbo].[extrema]
		([AssetId], [TimeframeId], [DateIndex], [ExtremumTypeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixDateIndex_extrema] ON [dbo].[extrema]
		([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixMasterExtremumDateIndex_extrema] ON [dbo].[extrema]
		([MasterExtremumDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
		CREATE NONCLUSTERED INDEX [ixAsset_extrema] ON [dbo].[extrema]
		([AssetId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
		CREATE NONCLUSTERED INDEX [ixTimeframe_extrema] ON [dbo].[extrema]
		([TimeframeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixExtremumType_extrema] ON [dbo].[extrema]
		([ExtremumTypeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	END

	--ARCHIVE EXTREMA
	BEGIN

		CREATE TABLE [dbo].[archive_extrema](
			--Metadata.
			[ExtremumId] [int] IDENTITY(1,1) NOT NULL,
			[AssetId] [int] NOT NULL,
			[TimeframeId] [int] NOT NULL,
			[DateIndex] [int] NOT NULL,
			[ExtremumTypeId] [int] NOT NULL,
			[MasterExtremumDateIndex] [int] NOT NULL,
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
			CONSTRAINT [PK_archiveExtrema] PRIMARY KEY CLUSTERED ([ExtremumId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	
		ALTER TABLE [dbo].[archive_extrema]  WITH CHECK ADD CONSTRAINT [FK_ArchiveExtrema_ExtremumTypeId] FOREIGN KEY([ExtremumTypeId])
		REFERENCES [dbo].[extremumTypes] ([ExtremumTypeId]) ON DELETE CASCADE;

		ALTER TABLE [dbo].[archive_extrema]  WITH CHECK ADD CONSTRAINT [FK_ArchiveExtrema_AssetId] FOREIGN KEY([AssetId])
		REFERENCES [dbo].[assets] ([AssetId]) ON DELETE CASCADE;

		ALTER TABLE [dbo].[archive_extrema]  WITH CHECK ADD CONSTRAINT [FK_ArchiveExtrema_DateIndex] FOREIGN KEY([DateIndex], [TimeframeId])
		REFERENCES [dbo].[dates] ([DateIndex], [TimeframeId]) ON DELETE CASCADE;
		
		ALTER TABLE [dbo].[extrema]  WITH CHECK ADD CONSTRAINT [FK_ArchiveExtrema_MasterExtremumDateIndex] FOREIGN KEY([MasterExtremumDateIndex], [TimeframeId])
		REFERENCES [dbo].[dates] ([DateIndex], [TimeframeId]);
		
		ALTER TABLE [dbo].[archive_extrema] ADD  CONSTRAINT [Default_ArchiveExtrema_IsEvaluationOpen]  DEFAULT ((0)) FOR [IsEvaluationOpen];
		
		CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_archiveExtrema] ON [dbo].[archive_extrema]
		([AssetId], [TimeframeId], [DateIndex], [ExtremumTypeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixDateIndex_archiveExtrema] ON [dbo].[archive_extrema]
		([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
		CREATE NONCLUSTERED INDEX [ixMasterExtremumDateIndex_archiveExtrema] ON [dbo].[extrema]
		([MasterExtremumDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
		CREATE NONCLUSTERED INDEX [ixAsset_archiveExtrema] ON [dbo].[archive_extrema]
		([AssetId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
		CREATE NONCLUSTERED INDEX [ixTimeframe_archiveExtrema] ON [dbo].[archive_extrema]
		([TimeframeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixExtremumType_archiveExtrema] ON [dbo].[archive_extrema]
		([ExtremumTypeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	END


END



--TRENDLINE TABLES
BEGIN

	

END






----TRENDLINES
--BEGIN

--	CREATE TABLE [dbo].[trendlines](
--		[Id] [int] IDENTITY(1,1) NOT NULL,	
--		[AssetId] [int] NOT NULL,
--		[TimeframeId] [int] NOT NULL,
--		[BaseStartIndex] [int] NOT NULL,
--		[BaseIsPeak] [bit] NOT NULL,
--		[BaseLevel] [float] NOT NULL,
--		[CounterStartIndex] [int] NOT NULL,
--		[CounterIsPeak] [bit] NOT NULL,
--		[CounterLevel] [float] NOT NULL,
--		[Slope] [float] NOT NULL,
--		[StartDateIndex] [int] NULL,
--		[EndDateIndex] [int] NULL,
--		[IsOpenFromLeft] [bit] NOT NULL,
--		[IsOpenFromRight] [bit] NOT NULL,
--		[CandlesDistance] [int] NOT NULL,
--		[ShowOnChart] [bit] NOT NULL DEFAULT(0),
--		[Value] [float] NOT NULL DEFAULT(0),
--		CONSTRAINT [PK_trendlines] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--	) ON [PRIMARY]
	
--	ALTER TABLE [dbo].[trendlines]  WITH CHECK ADD  CONSTRAINT [FK_Trendlines_AssetId] FOREIGN KEY([AssetId])
--	REFERENCES [dbo].[assets] ([Id]) ON DELETE CASCADE

--	ALTER TABLE [dbo].[trendlines]  WITH CHECK ADD  CONSTRAINT [FK_Trendlines_Timeframe] FOREIGN KEY([TimeframeId])
--	REFERENCES [dbo].[timeframes] ([Id]) ON DELETE CASCADE
	
--	ALTER TABLE [dbo].[trendlines]  WITH CHECK ADD  CONSTRAINT [FK_Trendlines_BaseExtremumGroup] FOREIGN KEY([AssetId], [TimeframeId], [BaseStartIndex], [BaseIsPeak])
--	REFERENCES [dbo].[extremumGroups] ([AssetId], [TimeframeId], [StartIndex], [IsPeak])
	
--	ALTER TABLE [dbo].[trendlines]  WITH CHECK ADD  CONSTRAINT [FK_Trendlines_CounterExtremumGroup] FOREIGN KEY([AssetId], [TimeframeId], [CounterStartIndex], [CounterIsPeak])
--	REFERENCES [dbo].[extremumGroups] ([AssetId], [TimeframeId], [StartIndex], [IsPeak])

--	CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_trendlines] ON [dbo].[trendlines]
--	([AssetId], [TimeframeId], [BaseStartIndex], [BaseIsPeak], [CounterStartIndex], [CounterIsPeak] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
--	CREATE NONCLUSTERED INDEX [ixId_trendlines] ON [dbo].[trendlines]
--	([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
--	CREATE NONCLUSTERED INDEX [ixAsset_trendlines] ON [dbo].[trendlines]
--	([AssetId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
--	CREATE NONCLUSTERED INDEX [ixTimeframe_trendlines] ON [dbo].[trendlines]
--	([TimeframeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
--	CREATE NONCLUSTERED INDEX [ixBaseStartIndex_trendlines] ON [dbo].[trendlines]
--	([BaseStartIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
--	CREATE NONCLUSTERED INDEX [ixCounterStartIndex_trendlines] ON [dbo].[trendlines]
--	([CounterStartIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
--END


----TREND BREAKS
--BEGIN

--	CREATE TABLE [dbo].[trendBreaks](
--		[TrendlineId] [int] NOT NULL,
--		[DateIndex] [int] NOT NULL,
--		[BreakFromAbove] [int] NOT NULL
--	) ON [PRIMARY];

--	ALTER TABLE [dbo].[trendBreaks]  WITH CHECK ADD  CONSTRAINT [FK_TrendBreaks_TrendlineId] FOREIGN KEY([TrendlineId])
--	REFERENCES [dbo].[trendlines] ([Id]) ON DELETE CASCADE

--	CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_trendBreaks] ON [dbo].[trendBreaks]
--	([TrendlineId], [DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--	CREATE NONCLUSTERED INDEX [ixTrendlineId_trendlinesBreaks] ON [dbo].[trendBreaks]
--	([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--	CREATE NONCLUSTERED INDEX [ixDateIndex_trendlinesBreaks] ON [dbo].[trendBreaks]
--	([DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--END


----TREND HITS
--BEGIN

--	CREATE TABLE [dbo].[trendHits](
--		[TrendlineId] [int] NOT NULL,
--		[ExtremumGroupId] [int] NOT NULL,
--		[DateIndex] [int] NOT NULL
--	) ON [PRIMARY];
	
--	ALTER TABLE [dbo].[trendHits]  WITH CHECK ADD  CONSTRAINT [FK_TrendHits_TrendlineId] FOREIGN KEY([TrendlineId])
--	REFERENCES [dbo].[trendlines] ([Id]) ON DELETE CASCADE

--	CREATE NONCLUSTERED INDEX [ixUniqueSet_trendHits] ON [dbo].[trendHits]
--	([TrendlineId], [ExtremumGroupId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--	CREATE NONCLUSTERED INDEX [ixTrendlineId_trendHits] ON [dbo].[trendHits]
--	([TrendlineId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--	CREATE NONCLUSTERED INDEX [ixExtremumGroup_trendHits] ON [dbo].[trendHits]
--	([ExtremumGroupId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--	CREATE NONCLUSTERED INDEX [ixDateIndex_trendHits] ON [dbo].[trendHits]
--	([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--END


----TREND RANGES
--BEGIN

--	CREATE TABLE [dbo].[trendRanges](
--		[TrendlineId] [int] NOT NULL,
--		[BaseDateIndex] [int] NOT NULL,
--		[BaseIsHit] [int] NOT NULL,
--		[CounterDateIndex] [int] NOT NULL,
--		[CounterIsHit] [int] NOT NULL,
--		[IsPeak] [int] NOT NULL,
--		[ExtremumPriceCrossPenaltyPoints] [float] NULL,
--		[ExtremumPriceCrossCounter] [int] NULL,
--		[OCPriceCrossPenaltyPoints] [float] NULL,
--		[OCPriceCrossCounter] [int] NULL,
--		[TotalCandles] [int] NULL,
--		[AverageVariation] [float] NULL,
--		[ExtremumVariation] [float] NULL,
--		[OpenCloseVariation] [float] NULL,
--		[BaseHitValue] [float] NULL,
--		[CounterHitValue] [float] NULL,
--		[Value] [float] NULL
--	) ON [PRIMARY];

--	CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_trendRanges] ON [dbo].[trendRanges]
--	([TrendlineId] ASC, [BaseDateIndex] ASC, [CounterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--	CREATE NONCLUSTERED INDEX [ixTrendlineId_trendRanges] ON [dbo].[trendRanges]
--	([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--	CREATE NONCLUSTERED INDEX [ixBaseDateIndex_trendRanges] ON [dbo].[trendRanges]
--	([BaseDateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--	CREATE NONCLUSTERED INDEX [ixCounterDateIndex_trendRanges] ON [dbo].[trendRanges]
--	([CounterDateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--END




----ARCHIVE TRENDLINES
--BEGIN

--	CREATE TABLE [dbo].[archive_trendlines](
--		[Id] [int] NOT NULL,	
--		[AssetId] [int] NOT NULL,
--		[TimeframeId] [int] NOT NULL,
--		[BaseStartIndex] [int] NOT NULL,
--		[BaseIsPeak] [bit] NOT NULL,
--		[BaseLevel] [float] NOT NULL,
--		[CounterStartIndex] [int] NOT NULL,
--		[CounterIsPeak] [bit] NOT NULL,
--		[CounterLevel] [float] NOT NULL,
--		[Slope] [float] NOT NULL,
--		[StartDateIndex] [int] NULL,
--		[EndDateIndex] [int] NULL,
--		[IsOpenFromLeft] [bit] NOT NULL,
--		[IsOpenFromRight] [bit] NOT NULL,
--		[CandlesDistance] [int] NOT NULL,
--		[ShowOnChart] [bit] NOT NULL DEFAULT(0),
--		[Value] [float] NOT NULL DEFAULT(0),
--		CONSTRAINT [PK_archive_trendlines] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--	) ON [PRIMARY]
	
--	ALTER TABLE [dbo].[archive_trendlines]  WITH CHECK ADD  CONSTRAINT [FK_archive_Trendlines_AssetId] FOREIGN KEY([AssetId])
--	REFERENCES [dbo].[assets] ([Id]) ON DELETE CASCADE

--	ALTER TABLE [dbo].[archive_trendlines]  WITH CHECK ADD  CONSTRAINT [FK_archive_Trendlines_Timeframe] FOREIGN KEY([TimeframeId])
--	REFERENCES [dbo].[timeframes] ([Id]) ON DELETE CASCADE
	
--	ALTER TABLE [dbo].[archive_trendlines]  WITH CHECK ADD  CONSTRAINT [FK_archive_Trendlines_BaseExtremumGroup] FOREIGN KEY([AssetId], [TimeframeId], [BaseStartIndex], [BaseIsPeak])
--	REFERENCES [dbo].[archive_extremumGroups] ([AssetId], [TimeframeId], [StartIndex], [IsPeak])
	
--	ALTER TABLE [dbo].[archive_trendlines]  WITH CHECK ADD  CONSTRAINT [FK_archive_Trendlines_CounterExtremumGroup] FOREIGN KEY([AssetId], [TimeframeId], [CounterStartIndex], [CounterIsPeak])
--	REFERENCES [dbo].[archive_extremumGroups] ([AssetId], [TimeframeId], [StartIndex], [IsPeak])

--	CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_archive_trendlines] ON [dbo].[archive_trendlines]
--	([AssetId], [TimeframeId], [BaseStartIndex], [BaseIsPeak], [CounterStartIndex], [CounterIsPeak] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
--	CREATE NONCLUSTERED INDEX [ixId_archive_trendlines] ON [dbo].[archive_trendlines]
--	([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
--	CREATE NONCLUSTERED INDEX [ixAsset_archive_trendlines] ON [dbo].[archive_trendlines]
--	([AssetId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
--	CREATE NONCLUSTERED INDEX [ixTimeframe_archive_trendlines] ON [dbo].[archive_trendlines]
--	([TimeframeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
--	CREATE NONCLUSTERED INDEX [ixBaseStartIndex_archive_trendlines] ON [dbo].[archive_trendlines]
--	([BaseStartIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
--	CREATE NONCLUSTERED INDEX [ixCounterStartIndex_archive_trendlines] ON [dbo].[archive_trendlines]
--	([CounterStartIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
--END


----ARCHIVE TREND BREAKS
--BEGIN

--	CREATE TABLE [dbo].[archive_trendBreaks](
--		[TrendlineId] [int] NOT NULL,
--		[DateIndex] [int] NOT NULL,
--		[BreakFromAbove] [int] NOT NULL
--	) ON [PRIMARY];

--	ALTER TABLE [dbo].[archive_trendBreaks]  WITH CHECK ADD  CONSTRAINT [FK_archive_TrendBreaks_TrendlineId] FOREIGN KEY([TrendlineId])
--	REFERENCES [dbo].[archive_trendlines] ([Id]) ON DELETE CASCADE

--	CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_archive_trendBreaks] ON [dbo].[archive_trendBreaks]
--	([TrendlineId], [DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--	CREATE NONCLUSTERED INDEX [ixTrendlineId_archive_trendlinesBreaks] ON [dbo].[archive_trendBreaks]
--	([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--	CREATE NONCLUSTERED INDEX [ixDateIndex_archive_trendlinesBreaks] ON [dbo].[archive_trendBreaks]
--	([DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--END


----ARCHIVE TREND HITS
--BEGIN

--	CREATE TABLE [dbo].[archive_trendHits](
--		[TrendlineId] [int] NOT NULL,
--		[ExtremumGroupId] [int] NOT NULL,
--		[DateIndex] [int] NOT NULL
--	) ON [PRIMARY];
		
--	ALTER TABLE [dbo].[archive_trendHits]  WITH CHECK ADD  CONSTRAINT [FK_archive_trendHits_TrendlineId] FOREIGN KEY([TrendlineId])
--	REFERENCES [dbo].[archive_trendlines] ([Id]) ON DELETE CASCADE

--	CREATE NONCLUSTERED INDEX [ixUniqueSet_archive_trendHits] ON [dbo].[archive_TrendHits]
--	([TrendlineId], [ExtremumGroupId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--	CREATE NONCLUSTERED INDEX [ixTrendlineId_archive_trendHits] ON [dbo].[archive_TrendHits]
--	([TrendlineId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--	CREATE NONCLUSTERED INDEX [ixExtremumGroup_archive_trendHits] ON [dbo].[archive_TrendHits]
--	([ExtremumGroupId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--	CREATE NONCLUSTERED INDEX [ixDateIndex_archive_trendHits] ON [dbo].[archive_TrendHits]
--	([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--END


----ARCHIVE TREND RANGES
--BEGIN

--	CREATE TABLE [dbo].[archive_trendRanges](
--		[TrendlineId] [int] NOT NULL,
--		[BaseDateIndex] [int] NOT NULL,
--		[BaseIsHit] [int] NOT NULL,
--		[CounterDateIndex] [int] NOT NULL,
--		[CounterIsHit] [int] NOT NULL,
--		[IsPeak] [int] NOT NULL,
--		[ExtremumPriceCrossPenaltyPoints] [float] NULL,
--		[ExtremumPriceCrossCounter] [int] NULL,
--		[OCPriceCrossPenaltyPoints] [float] NULL,
--		[OCPriceCrossCounter] [int] NULL,
--		[TotalCandles] [int] NULL,
--		[AverageVariation] [float] NULL,
--		[ExtremumVariation] [float] NULL,
--		[OpenCloseVariation] [float] NULL,
--		[BaseHitValue] [float] NULL,
--		[CounterHitValue] [float] NULL,
--		[Value] [float] NULL
--	) ON [PRIMARY];
	
--	ALTER TABLE [dbo].[archive_trendRanges]  WITH CHECK ADD  CONSTRAINT [FK_archive_trendRanges_TrendlineId] FOREIGN KEY([TrendlineId])
--	REFERENCES [dbo].[archive_trendlines] ([Id]) ON DELETE CASCADE

--	CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_archive_trendRanges] ON [dbo].[archive_trendRanges]
--	([TrendlineId] ASC, [BaseDateIndex] ASC, [CounterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--	CREATE NONCLUSTERED INDEX [ixTrendlineId_archive_trendRanges] ON [dbo].[archive_trendRanges]
--	([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--	CREATE NONCLUSTERED INDEX [ixBaseDateIndex_archive_trendRanges] ON [dbo].[archive_trendRanges]
--	([BaseDateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--	CREATE NONCLUSTERED INDEX [ixCounterDateIndex_archive_trendRanges] ON [dbo].[archive_trendRanges]
--	([CounterDateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

--END






--ROLLBACK TRANSACTION;
COMMIT TRANSACTION;