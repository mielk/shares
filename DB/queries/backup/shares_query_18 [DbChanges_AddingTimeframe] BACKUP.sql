USE [shares];

BEGIN TRANSACTION;

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

ALTER TABLE [dbo].[quotes]
ADD [Timeframe] [int] NOT NULL DEFAULT(6);

CREATE NONCLUSTERED INDEX [ixTimeframe_quotes] ON [dbo].[quotes]
(
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

CREATE NONCLUSTERED INDEX [ixShareDateIndexTimeframe_quotes] ON [dbo].[quotes]
(
	[ShareId] ASC,
	[DateIndex] ASC,
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


ALTER TABLE [dbo].[prices]
ADD [Timeframe] [int] NOT NULL DEFAULT(6);

CREATE NONCLUSTERED INDEX [ixTimeframe_prices] ON [dbo].[quotes]
(
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

CREATE NONCLUSTERED INDEX [ixShareDateIndexTimeframe_prices] ON [dbo].[quotes]
(
	[ShareId] ASC,
	[DateIndex] ASC,
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)




ALTER TABLE [dbo].[extrema]
ADD [Timeframe] [int] NOT NULL DEFAULT(6);

CREATE NONCLUSTERED INDEX [ixTimeframe_extrema] ON [dbo].[quotes]
(
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


CREATE NONCLUSTERED INDEX [ixShareDateIndexTimeframe_extrema] ON [dbo].[quotes]
(
	[ShareId] ASC,
	[DateIndex] ASC,
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)



ALTER TABLE [dbo].[extremumGroups]
ADD [Timeframe] [int] NOT NULL DEFAULT(6);

CREATE NONCLUSTERED INDEX [ixTimeframe_extremumGroups] ON [dbo].[quotes]
(
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


CREATE NONCLUSTERED INDEX [ixShareDateIndexTimeframe_extremumGroups] ON [dbo].[quotes]
(
	[ShareId] ASC,
	[DateIndex] ASC,
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)




ALTER TABLE [dbo].[trendlines]
ADD [Timeframe] [int] NOT NULL DEFAULT(6);

CREATE NONCLUSTERED INDEX [ixTimeframe_trendlines] ON [dbo].[quotes]
(
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


CREATE NONCLUSTERED INDEX [ixShareDateIndexTimeframe_trendlines] ON [dbo].[quotes]
(
	[ShareId] ASC,
	[DateIndex] ASC,
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)




CREATE TABLE [dbo].[timestamps_lastAnalysis](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShareId] [int] NOT NULL,
	[Timeframe] [int] NOT NULL,
	[DateIndex] [int] NULL,
	CONSTRAINT [PK_timestamps_lastAnalysis] PRIMARY KEY CLUSTERED ([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
);


CREATE UNIQUE INDEX [ixShareTimeframe_timestampsLastAnalysis] ON [dbo].[timestamps_lastAnalysis]
([ShareId] ASC, [Timeframe] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)






--Create archive tables
BEGIN

	CREATE TABLE [dbo].[archive_trendlinesBreaks](
		[TrendlineId] [int] NOT NULL,
		[DateIndex] [int] NOT NULL,
		[BreakFromAbove] [int] NOT NULL,
		CONSTRAINT [PK_archive_trendlinesBreaks] PRIMARY KEY CLUSTERED ([TrendlineId], [DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY];

	CREATE NONCLUSTERED INDEX [ixTrendlineId_archive_trendlinesBreaks] ON [dbo].[archive_trendlinesBreaks]
	([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixDateIndex_archive_trendlinesBreaks] ON [dbo].[archive_trendlinesBreaks]
	([DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


	CREATE TABLE [dbo].[archive_trendlinesHits](
		[TrendlineId] [int] NOT NULL,
		[ExtremumGroupId] [int] NOT NULL,
		[DateIndex] [int] NOT NULL,
		CONSTRAINT [PK_archive_trendlinesHits] PRIMARY KEY CLUSTERED ([TrendlineId], [ExtremumGroupId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY];

	CREATE NONCLUSTERED INDEX [ixTrendlineId_archive_trendlinesHits] ON [dbo].[archive_trendlinesHits]
	([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixDateIndex_archive_trendlinesHits] ON [dbo].[archive_trendlinesHits]
	([DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixExtremumGroupId_archive_trendlinesHits] ON [dbo].[archive_trendlinesHits]
	([ExtremumGroupId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


	CREATE TABLE [dbo].[archive_trendRanges](
		[TrendlineId] [int] NOT NULL,
		[BaseDateIndex] [int] NOT NULL,
		[BaseIsHit] [int] NOT NULL,
		[CounterDateIndex] [int] NOT NULL,
		[CounterIsHit] [int] NOT NULL,
		[IsPeak] [int] NOT NULL,
		[ExtremumPriceCrossPenaltyPoints] [float] NULL,
		[OCPriceCrossPenaltyPoints] [float] NULL,
		[TotalCandles] [int] NULL,
		[AverageVariation] [float] NULL,
		[ExtremumVariation] [float] NULL,
		[OpenCloseVariation] [float] NULL,
		[BaseHitValue] [float] NULL,
		[CounterHitValue] [float] NULL,
		[Value] [float] NULL,
		[ExtremumPriceCrossCounter] [int] NULL,
		[OCPriceCrossCounter] [int] NULL,
		CONSTRAINT [PK_archive_trendRanges] PRIMARY KEY CLUSTERED ([TrendlineId], [BaseDateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY];

	CREATE NONCLUSTERED INDEX [ixTrendlineId_archive_trendRanges] ON [dbo].[archive_trendRanges]
	([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixBaseDateIndex_archive_trendRanges] ON [dbo].[archive_trendRanges]
	([BaseDateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixCounterDateIndex_archive_trendRanges] ON [dbo].[archive_trendRanges]
	([CounterDateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


	CREATE TABLE [dbo].[archive_trendlines](
		[Id] [int],
		[ShareId] [int] NOT NULL,
		[Timeframe] [int] NOT NULL,
		[BaseId] [int] NOT NULL,
		[BaseStartIndex] [int] NOT NULL,
		[BaseIsPeak] [bit] NOT NULL,
		[BaseLevel] [float] NOT NULL,
		[CounterId] [int] NOT NULL,
		[CounterStartIndex] [int] NOT NULL,
		[CounterIsPeak] [bit] NOT NULL,
		[CounterLevel] [float] NOT NULL,
		[Slope] [float] NOT NULL,
		[StartDateIndex] [int] NULL,
		[EndDateIndex] [int] NULL,
		[~IsOpenFromLeft] [bit] NOT NULL,
		[~IsOpenFromRight] [bit] NOT NULL,
		[~CandlesDistance] [int] NOT NULL,
		[ShowOnChart] [bit] NOT NULL DEFAULT ((0)),
		[Value] [float] NOT NULL DEFAULT ((0)),
	 CONSTRAINT [PK_archive_trendlines] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]



	CREATE NONCLUSTERED INDEX [ixId_archive_trendlines] ON [dbo].[archive_trendlines]
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixBaseStartIndex_archive_trendlines] ON [dbo].[archive_trendlines]
	(
		[BaseStartIndex] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixBaseId_archive_trendlines] ON [dbo].[archive_trendlines]
	(
		[BaseId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixCounterStartIndex_archive_trendlines] ON [dbo].[archive_trendlines]
	(
		[CounterStartIndex] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixCounterId_archive_trendlines] ON [dbo].[archive_trendlines]
	(
		[CounterId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


	ALTER TABLE [dbo].[archive_trendlinesBreaks]  WITH CHECK ADD  CONSTRAINT [FK_archive_TrendlinesBreaks_TrendlineId] FOREIGN KEY([TrendlineId])
	REFERENCES [dbo].[archive_trendlines] ([Id]) ON DELETE CASCADE;

	GO


	ALTER TABLE [dbo].[archive_trendlinesHits]  WITH CHECK ADD  CONSTRAINT [FK_archive_TrendlinesHits_TrendlineId] FOREIGN KEY([TrendlineId])
	REFERENCES [dbo].[archive_trendlines] ([Id]) ON DELETE CASCADE;

	GO

	ALTER TABLE [dbo].[archive_trendRanges]  WITH CHECK ADD  CONSTRAINT [FK_archive_TrendRanges_TrendlineId] FOREIGN KEY([TrendlineId])
	REFERENCES [dbo].[archive_trendlines] ([Id]) ON DELETE CASCADE;

END
COMMIT TRANSACTION;