USE [shares2];

BEGIN TRANSACTION;


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

	SELECT (ones.n + 10*tens.n + 100*hundreds.n + 1000*thousands.n + 10000*tenThousands.n + 100000*hundredThousands.n) AS [number]
	INTO [dbo].[predefinedNumbers]
	FROM (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) ones(n),
			(VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) tens(n),
			(VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) hundreds(n),
			(VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) thousands(n),
			(VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) tenThousands(n),
			(VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) hundredThousands(n)
	ORDER BY [number];

	CREATE NONCLUSTERED INDEX [ixNumber_predefinedNumbers] ON [dbo].[predefinedNumbers]
	([number] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

END


--MARKETS
BEGIN

	CREATE TABLE [dbo].[markets](
		[Id] [int] NOT NULL,
		[Uuid] [nvarchar](36) NOT NULL DEFAULT (NEWID()),
		[Name] [nvarchar](255) NOT NULL,
		[IsActive] [bit] NOT NULL CONSTRAINT [Default_Markets_IsActive]  DEFAULT ((1)),
		[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Markets_CreatedDate]  DEFAULT (GETDATE()),
		[ModifiedDate] [datetime] NOT NULL CONSTRAINT [Default_Markets_ModifiedDate]  DEFAULT (GETDATE()),
		CONSTRAINT [PK_markets] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[markets]  WITH CHECK ADD  CONSTRAINT [CH_marketUuid_length] CHECK  ((LEN([Uuid])=(36)))

	CREATE UNIQUE NONCLUSTERED INDEX [ixName_markets] ON [dbo].[markets]
	([Name] ASC) WHERE ([IsActive]=(1)) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	INSERT INTO [shares2].[dbo].[markets] SELECT * FROM [shares].[dbo].[markets];

END


--STOCKS
BEGIN

	CREATE TABLE [dbo].[shares](
		[Id] [int] NOT NULL,
		[Uuid] [nvarchar](36) NOT NULL DEFAULT (NEWID()),
		[YahooSymbol] [nvarchar](255) NULL,
		[Plus500Symbol] [nvarchar](255) NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[MarketId] [int] NOT NULL,
		[IsActive] [bit] NOT NULL CONSTRAINT [Default_Campaigns_IsActive]  DEFAULT ((1)),
		[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Campaigns_CreatedDate]  DEFAULT (GETDATE()),
		[ModifiedDate] [datetime] NOT NULL CONSTRAINT [Default_Campaigns_ModifiedDate]  DEFAULT (GETDATE()),
		CONSTRAINT [PK_shares] PRIMARY KEY CLUSTERED ([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]

	ALTER TABLE [dbo].[shares]  WITH CHECK ADD  CONSTRAINT [CH_shareUuid_length] CHECK  ((LEN([Uuid])=(36)))
	
	ALTER TABLE [dbo].[shares]  WITH CHECK ADD  CONSTRAINT [FK_Shares_MarketId] FOREIGN KEY([MarketId])
	REFERENCES [dbo].[markets] ([Id]) ON DELETE CASCADE

	CREATE UNIQUE NONCLUSTERED INDEX [ixName_shares] ON [dbo].[shares]
	([Name] ASC, [MarketId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE UNIQUE NONCLUSTERED INDEX [ixYahooSymbol_shares] ON [dbo].[shares] ([YahooSymbol] ASC) WHERE ([YahooSymbol] IS NOT NULL)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE UNIQUE NONCLUSTERED INDEX [ixPlus500Symbol_shares] ON [dbo].[shares]([Plus500Symbol] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	INSERT INTO [shares2].[dbo].[shares] SELECT * FROM [shares].[dbo].[shares]


END


--TIMEFRAMES
BEGIN

	CREATE TABLE [dbo].[timeframes](
		[Id] [int] NOT NULL,
		[Name] [nvarchar](4) NOT NULL,
		CONSTRAINT [PK_timeframes] PRIMARY KEY CLUSTERED ([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	);

	INSERT INTO [dbo].[timeframes]
	SELECT 1, '5M' UNION ALL
	SELECT 2, '15M' UNION ALL
	SELECT 3, '30M' UNION ALL
	SELECT 4, '1H' UNION ALL
	SELECT 5, '4H' UNION ALL
	SELECT 6, '1D' UNION ALL
	SELECT 7, '1W' UNION ALL
	SELECT 8, '1M';

END


--DATES
BEGIN

	CREATE TABLE [dbo].[dates](
		[DateIndex] [int] NOT NULL DEFAULT ((1)),
		[Timeframe] [int] NOT NULL DEFAULT ((6)),
		[Date] [datetime] NOT NULL,		
		[ParentLevelDateIndex] [int] NULL,
		CONSTRAINT [PK_dates] PRIMARY KEY CLUSTERED ([DateIndex], [Timeframe] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[dates]  WITH CHECK ADD CONSTRAINT [CH_date_notWeekend] CHECK  ((datepart(weekday,[Date])>=(2) AND datepart(weekday,[Date])<=(6)))

	ALTER TABLE [dbo].[dates]  WITH CHECK ADD  CONSTRAINT [FK_dates_timeframe] FOREIGN KEY([Timeframe])
	REFERENCES [dbo].[timeframes] ([Id]) ON DELETE CASCADE

	CREATE UNIQUE NONCLUSTERED INDEX [ixDate_dates] ON [dbo].[dates]  ([Date] ASC, [Timeframe] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE UNIQUE NONCLUSTERED INDEX [ixDateIndexTimeframe_dates] ON [dbo].[dates] ([DateIndex] ASC, [Timeframe] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixDateIndex_dates] ON [dbo].[dates] ([DateIndex] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
		
	CREATE NONCLUSTERED INDEX [ixTimeframe_dates] ON [dbo].[dates] ([Timeframe] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	--Populate table
	BEGIN
		
		DECLARE @startDate AS DATETIME, @endDate AS DATETIME;
		SET @startDate = '2005-01-01';
		SET @endDate = '2022-12-31';

		--Insert days.
		INSERT INTO [dbo].[dates]
		SELECT
			[DateIndex] = ROW_NUMBER() OVER (ORDER BY b.[Date]),
			6 AS [Timeframe],
			b.[Date],
			NULL AS [ParentLevelDateIndex]
		FROM
			(SELECT 
				a.[date] AS [Date],
				DATEPART(dw, a.[date]) AS [Weekday]
			FROM
				(SELECT
					DATEADD(d, [number], @startDate) AS [date]
				FROM
					[dbo].[predefinedNumbers]
				WHERE
					[number] BETWEEN 1 AND DATEDIFF(d, @startDate, @endDate)) a) b
		WHERE
			(b.[Weekday] BETWEEN 2 AND 6) AND
			NOT (MONTH(b.[Date]) = 1 AND DAY(b.[Date]) = 1) AND
			NOT (MONTH(b.[Date]) = 12 AND DAY(b.[Date]) = 25)

		--Insert weeks
		INSERT INTO [dbo].[dates]([Date], [DateIndex], [Timeframe])
		SELECT
			w.[WeekBegin] AS [Date],
			[DateIndex] = ROW_NUMBER() OVER (ORDER BY w.[WeekBegin]),
			7 AS [Timeframe]
		FROM
			(SELECT DISTINCT
				DATEADD(d, -(DATEPART(dw, d.[Date]) + 5) % 7, d.[Date]) AS [WeekBegin]
			FROM
				[dates] d) w

		--Update days parental week.
		UPDATE d
		SET 
			d.[ParentLevelDateIndex] = d2.[DateIndex]
		FROM	
			[dbo].[dates] d
			LEFT JOIN (SELECT * FROM [dbo].[dates] d2 WHERE [Timeframe] = 7) d2
			ON d2.[Date] = DATEADD(d, -(DATEPART(dw, d.[Date]) + 5) % 7, d.[Date])
		WHERE
			d.[Timeframe] = 6;

	END

END


--HISTORICAL UPDATES LOGS
BEGIN

	CREATE TABLE [dbo].[historicalUpdatesLogs](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ShareId] [int] NOT NULL,
		[Timeframe] [int] NOT NULL,
		[QuotesUpdateTimestamp] [datetime] NULL,
		[DividendsUpdateTimestamp] [datetime] NULL,
		[SplitsUpdateTimestamp] [datetime] NULL
		CONSTRAINT [PK_historicalUpdatesLogs] PRIMARY KEY CLUSTERED ([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[historicalUpdatesLogs]  WITH CHECK ADD  CONSTRAINT [FK_HistoricalUpdatesLogs_ShareId] FOREIGN KEY([ShareId])
	REFERENCES [dbo].[shares] ([Id]) ON DELETE CASCADE

END


--DIVIDENDS
BEGIN

	CREATE TABLE [dbo].[dividends](
		[ShareId] [int] NOT NULL,
		[DateIndex] [int] NOT NULL,
		[Amount] [float] NOT NULL,
		[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Dividends_CreatedDate]  DEFAULT (GETDATE())
	) ON [PRIMARY]

	ALTER TABLE [dbo].[dividends]  WITH CHECK ADD  CONSTRAINT [FK_Dividends_ShareId] FOREIGN KEY([ShareId])
	REFERENCES [dbo].[shares] ([Id]) ON DELETE CASCADE

	CREATE UNIQUE NONCLUSTERED INDEX [ixShareDateIndex_dividends] ON [dbo].[dividends]
	([ShareId] ASC, [DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	--Copy data from previous database.
	INSERT INTO [shares2].[dbo].[dividends]
	SELECT
		d.[ShareId],
		d2.[DateIndex],
		d.[Amount],
		d.[CreatedDate]
	FROM
		[shares].[dbo].[dividends] d
		LEFT JOIN [shares2].[dbo].[dates] d2
		ON d.[Date] = d2.[Date]

END


--SPLITS
BEGIN

	CREATE TABLE [dbo].[splits](
		[ShareId] [int] NOT NULL,
		[DateIndex] [int] NOT NULL,
		[BaseValue] [int] NOT NULL,
		[CounterValue] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Splits_CreatedDate]  DEFAULT (GETDATE())
	) ON [PRIMARY]

	ALTER TABLE [dbo].[splits]  WITH CHECK ADD  CONSTRAINT [FK_Splits_ShareId] FOREIGN KEY([ShareId])
	REFERENCES [dbo].[shares] ([Id]) ON DELETE CASCADE
	
	CREATE UNIQUE NONCLUSTERED INDEX [ixShareDateIndex_splits] ON [dbo].[splits]
	([ShareId] ASC, [DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	--Copy data from previous database.
	INSERT INTO [shares2].[dbo].[splits]
	SELECT
		s.[ShareId],
		d2.[DateIndex],
		s.[BaseValue],
		s.[CounterValue],
		s.[CreatedDate]
	FROM
		[shares].[dbo].[splits] s
		LEFT JOIN [shares2].[dbo].[dates] d2
		ON s.[Date] = d2.[Date]

END


--QUOTES
BEGIN

	CREATE TABLE [dbo].[quotes](
		[ShareId] [int] NOT NULL,
		[Timeframe] [int] NOT NULL,
		[DateIndex] [int] NOT NULL,
		[Open] [float] NULL,
		[Low] [float] NULL,
		[High] [float] NULL,
		[Close] [float] NULL,
		[AdjClose] [float] NULL,
		[Volume] [bigint] NULL,
		[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Quotes_CreatedDate]  DEFAULT (GETDATE())
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[quotes]  WITH CHECK ADD CONSTRAINT [FK_Quotes_ShareId] FOREIGN KEY([ShareId])
	REFERENCES [dbo].[shares] ([Id]) ON DELETE CASCADE

	ALTER TABLE [dbo].[quotes]  WITH CHECK ADD CONSTRAINT [FK_Quotes_DateIndex] FOREIGN KEY([DateIndex], [Timeframe])
	REFERENCES [dbo].[dates] ([DateIndex], [Timeframe]) ON DELETE CASCADE
	
	CREATE UNIQUE NONCLUSTERED INDEX [ixShareTimeframeDateIndex_Quotes] ON [dbo].[quotes] 
	([ShareId] ASC, [Timeframe] ASC, [DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixShareId_Quotes] ON [dbo].[quotes] 
	([ShareId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixTimeframe_Quotes] ON [dbo].[quotes] 
	([Timeframe] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixDateIndex_Quotes] ON [dbo].[quotes] 
	([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	
	--Copy data from previous database.
	INSERT INTO [shares2].[dbo].[quotes]
	SELECT
		q.[ShareId],
		6 AS [Timeframe],
		d.[DateIndex],
		q.[Open],
		q.[Low],
		q.[High],
		q.[Close],
		q.[AdjClose],
		q.[Volume],
		q.[CreatedDate]
	FROM
		[shares].[dbo].[quotes] q
		LEFT JOIN [shares2].[dbo].[dates] d
		ON q.[Timeframe] = d.[Timeframe] AND q.[Date] = d.[Date]

END


--Create [GenerateWeeklyPrices] procedure.
GO
IF OBJECT_ID('generateWeeklyPrices','P') IS NOT NULL DROP PROC [dbo].[generateWeeklyPrices];
GO

CREATE PROCEDURE [dbo].[generateWeeklyPrices]
AS

BEGIN
	
	--Remove previous weekly quotes.
	DELETE FROM [dbo].[quotes] WHERE [Timeframe] = 7;

	--Create temporary table with all daily quotes.
	SELECT * INTO #QuotesTemp FROM [dbo].[quotes] WHERE [Timeframe] = 6;

	--Create temporary table with basic data about weeks.
	SELECT
		d.[ParentLevelDateIndex] AS [WeekIndex],
		q.ShareId AS [ShareId],
		MIN(q.[DateIndex]) AS [FirstQuote],
		MAX(q.[DateIndex]) AS [LastQuote],
		MIN([Low]) AS [Low],
		MAX([High]) AS [High],
		SUM([Volume]) AS [Volume]
	INTO
		#WeekTemp
	FROM
		#QuotesTemp q
		LEFT JOIN [dbo].[dates] d
		ON 
			q.[Timeframe] = d.[Timeframe] AND 
			q.[DateIndex] = d.[DateIndex]
	GROUP BY
		d.[ParentLevelDateIndex], q.ShareId;

	--Create week records.
	SELECT
		wt.[ShareId],
		7 As [Timeframe],
		wt.[WeekIndex] AS [DateIndex],
		q.[Open],
		wt.[Low],
		wt.[High],
		q2.[Close],
		q2.[Close] AS [AdjClose],
		wt.[Volume]
	INTO
		#WeeklyQuotes
	FROM
		#WeekTemp wt
		LEFT JOIN #QuotesTemp q ON wt.[ShareId] = q.[ShareId] AND wt.[FirstQuote] = q.[DateIndex]
		LEFT JOIN #QuotesTemp q2 ON wt.[ShareId] = q.[ShareId] AND wt.[LastQuote] = q2.[DateIndex]

	--Insert week records into quotes table.
	INSERT INTO [dbo].[quotes]([ShareId], [Timeframe], [DateIndex], [Open], [Low], [High], [Close], [AdjClose], [Volume])
	SELECT * FROM #WeeklyQuotes;

	--Clean up temporary tables.
	BEGIN

		DROP TABLE #WeekTemp;
		DROP TABLE #QuotesTemp;
		DROP TABLE #WeeklyQuotes;

	END

END

GO
EXEC [dbo].[generateWeeklyPrices];
GO



--PRICES
BEGIN

	CREATE TABLE [dbo].[prices](
		[ShareId] [int] NOT NULL,
		[Timeframe] [int] NOT NULL,
		[DateIndex] [int] NOT NULL,
		[DeltaClosePrice] [float] NOT NULL,
		[PriceDirection2D] [int] NULL,
		[PriceDirection3D] [int] NULL,
		[PriceGap] [float] NULL,
		[CloseRatio] [float] NULL,
		[ExtremumRatio] [float] NULL,
		[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_prices_CreatedDate]  DEFAULT (getdate())
	) ON [PRIMARY]

	ALTER TABLE [dbo].[prices]  WITH CHECK ADD  CONSTRAINT [FK_Prices_ShareId] FOREIGN KEY([ShareId])
	REFERENCES [dbo].[shares] ([Id]) ON DELETE CASCADE

	ALTER TABLE [dbo].[prices]  WITH CHECK ADD  CONSTRAINT [FK_Prices_DateIndex] FOREIGN KEY([DateIndex], [Timeframe])
	REFERENCES [dbo].[dates] ([DateIndex], [Timeframe]) ON DELETE CASCADE
	
	CREATE UNIQUE NONCLUSTERED INDEX [ixShareTimeframeDateIndex_prices] ON [dbo].[prices] 
	([ShareId] ASC, [Timeframe] ASC, [DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixShareId_prices] ON [dbo].[prices] 
	([ShareId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixTimeframe_prices] ON [dbo].[prices] 
	([Timeframe] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixDateIndex_prices] ON [dbo].[prices] 
	([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
		
END



--ANALYSIS - TIMESTAMPS
BEGIN

	CREATE TABLE [dbo].[timestamps_lastAnalysis](
		[ShareId] [int] NOT NULL,
		[Timeframe] [int] NOT NULL,
		[DateIndex] [int] NULL
	) ON [PRIMARY]

	CREATE UNIQUE NONCLUSTERED INDEX [ixShareTimeframe_AnalysisTimestamps] ON [dbo].[timestamps_lastAnalysis] 
	([ShareId] ASC, [Timeframe] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

END



--EXTREMA
BEGIN

	CREATE TABLE [dbo].[extrema](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ShareId] [int] NOT NULL,
		[Timeframe] [int] NOT NULL,
		[DateIndex] [int] NOT NULL,
		[ExtremumType] [int] NOT NULL,
		[IsConfirmed] [bit] NOT NULL DEFAULT(0),
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
		CONSTRAINT [PK_extrema] PRIMARY KEY CLUSTERED ([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[extrema]  WITH CHECK ADD CONSTRAINT [CH_extrema_ExtremumType] CHECK  ([ExtremumType] BETWEEN 1 AND 4)
	
	ALTER TABLE [dbo].[extrema]  WITH CHECK ADD CONSTRAINT [FK_Extrema_ShareId] FOREIGN KEY([ShareId])
	REFERENCES [dbo].[shares] ([Id]) ON DELETE CASCADE;

	ALTER TABLE [dbo].[extrema]  WITH CHECK ADD CONSTRAINT [FK_Extrema_Date] FOREIGN KEY([DateIndex], [Timeframe])
	REFERENCES [dbo].[dates] ([DateIndex], [Timeframe]) ON DELETE CASCADE;
	
	CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_extrema] ON [dbo].[extrema]
	([ShareId], [Timeframe], [DateIndex], [ExtremumType] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixDateIndex_extrema] ON [dbo].[extrema]
	([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixShare_extrema] ON [dbo].[extrema]
	([ShareId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixTimeframe_extrema] ON [dbo].[extrema]
	([Timeframe] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

END


--EXTREMUM GROUPS
BEGIN

	CREATE TABLE [dbo].[extremumGroups](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ShareId] [int] NOT NULL,
		[Timeframe] [int] NOT NULL,
		[IsPeak] [bit] NOT NULL,
		[MasterIndex] [int] NULL,
		[SlaveIndex] [int] NULL,
		[StartIndex] [int] NOT NULL,
		[EndIndex] [int] NOT NULL,
		[Close] [float] NOT NULL,
		[High] [float] NULL,
		[MasterHigh] [float] NULL,
		[Low] [float] NULL,
		[MasterLow] [float] NULL,
		[Value] [float]  NOT NULL,
		CONSTRAINT [PK_extremumGroups] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[extremumGroups]  WITH CHECK ADD  CONSTRAINT [FK_ExtremumGroups_ShareId] FOREIGN KEY([ShareId])
	REFERENCES [dbo].[shares] ([Id]) ON DELETE CASCADE
	
	ALTER TABLE [dbo].[extremumGroups]  WITH CHECK ADD  CONSTRAINT [FK_ExtremumGroups_Timeframe] FOREIGN KEY([Timeframe])
	REFERENCES [dbo].[timeframes] ([Id]) ON DELETE CASCADE
	
	CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_extremumGroups] ON [dbo].[extremumGroups]
	([ShareId], [Timeframe], [StartIndex], [IsPeak] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixStartIndex_extremumGroups] ON [dbo].[extremumGroups]
	([StartIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixEndIndex_extremumGroups] ON [dbo].[extremumGroups]
	([EndIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixMasterIndex_extremumGroups] ON [dbo].[extremumGroups]
	([MasterIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixSlaveIndex_extremumGroups] ON [dbo].[extremumGroups]
	([SlaveIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixTimeframe_extremumGroups] ON [dbo].[extremumGroups]
	([Timeframe] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

END


--TRENDLINES
BEGIN

	CREATE TABLE [dbo].[trendlines](
		[Id] [int] IDENTITY(1,1) NOT NULL,	
		[ShareId] [int] NOT NULL,
		[Timeframe] [int] NOT NULL,
		[BaseStartIndex] [int] NOT NULL,
		[BaseIsPeak] [bit] NOT NULL,
		[BaseLevel] [float] NOT NULL,
		[CounterStartIndex] [int] NOT NULL,
		[CounterIsPeak] [bit] NOT NULL,
		[CounterLevel] [float] NOT NULL,
		[Slope] [float] NOT NULL,
		[StartDateIndex] [int] NULL,
		[EndDateIndex] [int] NULL,
		[IsOpenFromLeft] [bit] NOT NULL,
		[IsOpenFromRight] [bit] NOT NULL,
		[CandlesDistance] [int] NOT NULL,
		[ShowOnChart] [bit] NOT NULL DEFAULT(0),
		[Value] [float] NOT NULL DEFAULT(0),
		CONSTRAINT [PK_trendlines] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[trendlines]  WITH CHECK ADD  CONSTRAINT [FK_Trendlines_ShareId] FOREIGN KEY([ShareId])
	REFERENCES [dbo].[shares] ([Id]) ON DELETE CASCADE

	ALTER TABLE [dbo].[trendlines]  WITH CHECK ADD  CONSTRAINT [FK_Trendlines_Timeframe] FOREIGN KEY([Timeframe])
	REFERENCES [dbo].[timeframes] ([Id]) ON DELETE CASCADE
	
	ALTER TABLE [dbo].[trendlines]  WITH CHECK ADD  CONSTRAINT [FK_Trendlines_BaseExtremumGroup] FOREIGN KEY([ShareId], [Timeframe], [BaseStartIndex], [BaseIsPeak])
	REFERENCES [dbo].[extremumGroups] ([ShareId], [Timeframe], [StartIndex], [IsPeak])
	
	ALTER TABLE [dbo].[trendlines]  WITH CHECK ADD  CONSTRAINT [FK_Trendlines_CounterExtremumGroup] FOREIGN KEY([ShareId], [Timeframe], [CounterStartIndex], [CounterIsPeak])
	REFERENCES [dbo].[extremumGroups] ([ShareId], [Timeframe], [StartIndex], [IsPeak])

	CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_trendlines] ON [dbo].[trendlines]
	([ShareId], [Timeframe], [BaseStartIndex], [BaseIsPeak], [CounterStartIndex], [CounterIsPeak] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixId_trendlines] ON [dbo].[trendlines]
	([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixShare_trendlines] ON [dbo].[trendlines]
	([ShareId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixTimeframe_trendlines] ON [dbo].[trendlines]
	([Timeframe] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixBaseStartIndex_trendlines] ON [dbo].[trendlines]
	([BaseStartIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixCounterStartIndex_trendlines] ON [dbo].[trendlines]
	([CounterStartIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
END


--TREND BREAKS
BEGIN

	CREATE TABLE [dbo].[trendBreaks](
		[TrendlineId] [int] NOT NULL,
		[DateIndex] [int] NOT NULL,
		[BreakFromAbove] [int] NOT NULL
	) ON [PRIMARY];

	ALTER TABLE [dbo].[trendBreaks]  WITH CHECK ADD  CONSTRAINT [FK_TrendBreaks_TrendlineId] FOREIGN KEY([TrendlineId])
	REFERENCES [dbo].[trendlines] ([Id]) ON DELETE CASCADE

	CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_trendBreaks] ON [dbo].[trendBreaks]
	([TrendlineId], [DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixTrendlineId_trendlinesBreaks] ON [dbo].[trendBreaks]
	([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixDateIndex_trendlinesBreaks] ON [dbo].[trendBreaks]
	([DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

END


--TREND HITS
BEGIN

	CREATE TABLE [dbo].[trendHits](
		[TrendlineId] [int] NOT NULL,
		[ExtremumGroupId] [int] NOT NULL,
		[DateIndex] [int] NOT NULL
	) ON [PRIMARY];
	
	ALTER TABLE [dbo].[trendHits]  WITH CHECK ADD  CONSTRAINT [FK_TrendHits_TrendlineId] FOREIGN KEY([TrendlineId])
	REFERENCES [dbo].[trendlines] ([Id]) ON DELETE CASCADE

	CREATE NONCLUSTERED INDEX [ixUniqueSet_trendHits] ON [dbo].[trendHits]
	([TrendlineId], [ExtremumGroupId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixTrendlineId_trendHits] ON [dbo].[trendHits]
	([TrendlineId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixExtremumGroup_trendHits] ON [dbo].[trendHits]
	([ExtremumGroupId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixDateIndex_trendHits] ON [dbo].[trendHits]
	([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

END


--TREND RANGES
BEGIN

	CREATE TABLE [dbo].[trendRanges](
		[TrendlineId] [int] NOT NULL,
		[BaseDateIndex] [int] NOT NULL,
		[BaseIsHit] [int] NOT NULL,
		[CounterDateIndex] [int] NOT NULL,
		[CounterIsHit] [int] NOT NULL,
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
		[Value] [float] NULL
	) ON [PRIMARY];

	CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_trendRanges] ON [dbo].[trendRanges]
	([TrendlineId] ASC, [BaseDateIndex] ASC, [CounterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixTrendlineId_trendRanges] ON [dbo].[trendRanges]
	([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixBaseDateIndex_trendRanges] ON [dbo].[trendRanges]
	([BaseDateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixCounterDateIndex_trendRanges] ON [dbo].[trendRanges]
	([CounterDateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

END




--ARCHIVE EXTREMA
BEGIN

	CREATE TABLE [dbo].[archive_extrema](
		[Id] [int] NOT NULL,
		[ShareId] [int] NOT NULL,
		[Timeframe] [int] NOT NULL,
		[DateIndex] [int] NOT NULL,
		[ExtremumType] [int] NOT NULL,
		[IsConfirmed] [bit] NOT NULL DEFAULT(0),
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
		CONSTRAINT [PK_archive_extrema] PRIMARY KEY CLUSTERED ([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[archive_extrema]  WITH CHECK ADD CONSTRAINT [CH_archive_extrema_ExtremumType] CHECK  ([ExtremumType] BETWEEN 1 AND 4)
	
	ALTER TABLE [dbo].[archive_extrema]  WITH CHECK ADD CONSTRAINT [FK_archive_Extrema_ShareId] FOREIGN KEY([ShareId])
	REFERENCES [dbo].[shares] ([Id]) ON DELETE CASCADE;

	ALTER TABLE [dbo].[archive_extrema]  WITH CHECK ADD CONSTRAINT [FK_archive_Extrema_Date] FOREIGN KEY([DateIndex], [Timeframe])
	REFERENCES [dbo].[dates] ([DateIndex], [Timeframe]) ON DELETE CASCADE;
	
	CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_archive_extrema] ON [dbo].[extrema]
	([ShareId], [Timeframe], [DateIndex], [ExtremumType] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixDateIndex_archive_extrema] ON [dbo].[archive_extrema]
	([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixShare_archive_extrema] ON [dbo].[archive_extrema]
	([ShareId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixTimeframe_archive_extrema] ON [dbo].[archive_extrema]
	([Timeframe] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

END


--ARCHIVE EXTREMUM GROUPS
BEGIN

	CREATE TABLE [dbo].[archive_extremumGroups](
		[Id] [int] NOT NULL,
		[ShareId] [int] NOT NULL,
		[Timeframe] [int] NOT NULL,
		[IsPeak] [bit] NOT NULL,
		[MasterIndex] [int] NULL,
		[SlaveIndex] [int] NULL,
		[StartIndex] [int] NOT NULL,
		[EndIndex] [int] NOT NULL,
		[Close] [float] NOT NULL,
		[High] [float] NULL,
		[MasterHigh] [float] NULL,
		[Low] [float] NULL,
		[MasterLow] [float] NULL,
		[Value] [float]  NOT NULL,
		CONSTRAINT [PK_archive_extremumGroups] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[archive_extremumGroups]  WITH CHECK ADD  CONSTRAINT [FK_archive_ExtremumGroups_ShareId] FOREIGN KEY([ShareId])
	REFERENCES [dbo].[shares] ([Id]) ON DELETE CASCADE
	
	ALTER TABLE [dbo].[archive_extremumGroups]  WITH CHECK ADD  CONSTRAINT [FK_archive_ExtremumGroups_Timeframe] FOREIGN KEY([Timeframe])
	REFERENCES [dbo].[timeframes] ([Id]) ON DELETE CASCADE
	
	CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_archive_extremumGroups] ON [dbo].[archive_extremumGroups]
	([ShareId], [Timeframe], [StartIndex], [IsPeak] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixStartIndex_archive_extremumGroups] ON [dbo].[archive_extremumGroups]
	([StartIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixEndIndex_archive_extremumGroups] ON [dbo].[archive_extremumGroups]
	([EndIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixMasterIndex_archive_extremumGroups] ON [dbo].[archive_extremumGroups]
	([MasterIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixSlaveIndex_archive_extremumGroups] ON [dbo].[archive_extremumGroups]
	([SlaveIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixTimeframe_archive_extremumGroups] ON [dbo].[archive_extremumGroups]
	([Timeframe] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

END


--ARCHIVE TRENDLINES
BEGIN

	CREATE TABLE [dbo].[archive_trendlines](
		[Id] [int] NOT NULL,	
		[ShareId] [int] NOT NULL,
		[Timeframe] [int] NOT NULL,
		[BaseStartIndex] [int] NOT NULL,
		[BaseIsPeak] [bit] NOT NULL,
		[BaseLevel] [float] NOT NULL,
		[CounterStartIndex] [int] NOT NULL,
		[CounterIsPeak] [bit] NOT NULL,
		[CounterLevel] [float] NOT NULL,
		[Slope] [float] NOT NULL,
		[StartDateIndex] [int] NULL,
		[EndDateIndex] [int] NULL,
		[IsOpenFromLeft] [bit] NOT NULL,
		[IsOpenFromRight] [bit] NOT NULL,
		[CandlesDistance] [int] NOT NULL,
		[ShowOnChart] [bit] NOT NULL DEFAULT(0),
		[Value] [float] NOT NULL DEFAULT(0),
		CONSTRAINT [PK_archive_trendlines] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[archive_trendlines]  WITH CHECK ADD  CONSTRAINT [FK_archive_Trendlines_ShareId] FOREIGN KEY([ShareId])
	REFERENCES [dbo].[shares] ([Id]) ON DELETE CASCADE

	ALTER TABLE [dbo].[archive_trendlines]  WITH CHECK ADD  CONSTRAINT [FK_archive_Trendlines_Timeframe] FOREIGN KEY([Timeframe])
	REFERENCES [dbo].[timeframes] ([Id]) ON DELETE CASCADE
	
	ALTER TABLE [dbo].[archive_trendlines]  WITH CHECK ADD  CONSTRAINT [FK_archive_Trendlines_BaseExtremumGroup] FOREIGN KEY([ShareId], [Timeframe], [BaseStartIndex], [BaseIsPeak])
	REFERENCES [dbo].[archive_extremumGroups] ([ShareId], [Timeframe], [StartIndex], [IsPeak])
	
	ALTER TABLE [dbo].[archive_trendlines]  WITH CHECK ADD  CONSTRAINT [FK_archive_Trendlines_CounterExtremumGroup] FOREIGN KEY([ShareId], [Timeframe], [CounterStartIndex], [CounterIsPeak])
	REFERENCES [dbo].[archive_extremumGroups] ([ShareId], [Timeframe], [StartIndex], [IsPeak])

	CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_archive_trendlines] ON [dbo].[archive_trendlines]
	([ShareId], [Timeframe], [BaseStartIndex], [BaseIsPeak], [CounterStartIndex], [CounterIsPeak] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixId_archive_trendlines] ON [dbo].[archive_trendlines]
	([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixShare_archive_trendlines] ON [dbo].[archive_trendlines]
	([ShareId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixTimeframe_archive_trendlines] ON [dbo].[archive_trendlines]
	([Timeframe] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixBaseStartIndex_archive_trendlines] ON [dbo].[archive_trendlines]
	([BaseStartIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
	CREATE NONCLUSTERED INDEX [ixCounterStartIndex_archive_trendlines] ON [dbo].[archive_trendlines]
	([CounterStartIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
END


--ARCHIVE TREND BREAKS
BEGIN

	CREATE TABLE [dbo].[archive_trendBreaks](
		[TrendlineId] [int] NOT NULL,
		[DateIndex] [int] NOT NULL,
		[BreakFromAbove] [int] NOT NULL
	) ON [PRIMARY];

	ALTER TABLE [dbo].[archive_trendBreaks]  WITH CHECK ADD  CONSTRAINT [FK_archive_TrendBreaks_TrendlineId] FOREIGN KEY([TrendlineId])
	REFERENCES [dbo].[archive_trendlines] ([Id]) ON DELETE CASCADE

	CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_archive_trendBreaks] ON [dbo].[archive_trendBreaks]
	([TrendlineId], [DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixTrendlineId_archive_trendlinesBreaks] ON [dbo].[archive_trendBreaks]
	([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixDateIndex_archive_trendlinesBreaks] ON [dbo].[archive_trendBreaks]
	([DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

END


--ARCHIVE TREND HITS
BEGIN

	CREATE TABLE [dbo].[archive_trendHits](
		[TrendlineId] [int] NOT NULL,
		[ExtremumGroupId] [int] NOT NULL,
		[DateIndex] [int] NOT NULL
	) ON [PRIMARY];
		
	ALTER TABLE [dbo].[archive_trendHits]  WITH CHECK ADD  CONSTRAINT [FK_archive_trendHits_TrendlineId] FOREIGN KEY([TrendlineId])
	REFERENCES [dbo].[archive_trendlines] ([Id]) ON DELETE CASCADE

	CREATE NONCLUSTERED INDEX [ixUniqueSet_archive_trendHits] ON [dbo].[archive_TrendHits]
	([TrendlineId], [ExtremumGroupId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixTrendlineId_archive_trendHits] ON [dbo].[archive_TrendHits]
	([TrendlineId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixExtremumGroup_archive_trendHits] ON [dbo].[archive_TrendHits]
	([ExtremumGroupId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixDateIndex_archive_trendHits] ON [dbo].[archive_TrendHits]
	([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

END


--ARCHIVE TREND RANGES
BEGIN

	CREATE TABLE [dbo].[archive_trendRanges](
		[TrendlineId] [int] NOT NULL,
		[BaseDateIndex] [int] NOT NULL,
		[BaseIsHit] [int] NOT NULL,
		[CounterDateIndex] [int] NOT NULL,
		[CounterIsHit] [int] NOT NULL,
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
		[Value] [float] NULL
	) ON [PRIMARY];
	
	ALTER TABLE [dbo].[archive_trendRanges]  WITH CHECK ADD  CONSTRAINT [FK_archive_trendRanges_TrendlineId] FOREIGN KEY([TrendlineId])
	REFERENCES [dbo].[archive_trendlines] ([Id]) ON DELETE CASCADE

	CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_archive_trendRanges] ON [dbo].[archive_trendRanges]
	([TrendlineId] ASC, [BaseDateIndex] ASC, [CounterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixTrendlineId_archive_trendRanges] ON [dbo].[archive_trendRanges]
	([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixBaseDateIndex_archive_trendRanges] ON [dbo].[archive_trendRanges]
	([BaseDateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixCounterDateIndex_archive_trendRanges] ON [dbo].[archive_trendRanges]
	([CounterDateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

END



ROLLBACK TRANSACTION;