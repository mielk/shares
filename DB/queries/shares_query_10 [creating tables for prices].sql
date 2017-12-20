USE	[shares];

GO

BEGIN TRANSACTION;

CREATE TABLE [dbo].[prices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShareId] [int] NOT NULL,
	[DateIndex] [int] NOT NULL,
	[DeltaClosePrice] [float] NOT NULL,
	[PriceDirection2D] [int] NULL,
	[PriceDirection3D] [int] NULL,
	[PriceGap] [float] NULL,
	[CloseRatio] [float] NULL,
	[ExtremumRatio] [float] NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_prices_CreatedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_prices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[prices]  WITH CHECK ADD  CONSTRAINT [FK_Prices_ShareId] FOREIGN KEY([ShareId])
REFERENCES [dbo].[shares] ([Id])
GO

CREATE NONCLUSTERED INDEX [ixDateIndex_prices] ON [dbo].[prices] 
(
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [ixShareId_prices] ON [dbo].[prices] 
(
	[ShareId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO




CREATE TABLE [dbo].[extrema](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateIndex] [int] NOT NULL,
	[ShareId] [int] NOT NULL,
	[ExtremumType] [int] NOT NULL,
	[earlierCounter] [int] NULL,
	[laterCounter] [int] NULL,
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
CONSTRAINT [PK_extrema] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[extrema]  WITH CHECK ADD  CONSTRAINT [CH_extrema_ExtremumType] CHECK  ([ExtremumType] BETWEEN 1 AND 4)
GO

ALTER TABLE [dbo].[extrema]  WITH CHECK ADD  CONSTRAINT [FK_Extrema_ShareId] FOREIGN KEY([ShareId])
REFERENCES [dbo].[shares] ([Id])
GO

CREATE NONCLUSTERED INDEX [ixDateIndex_extrema] ON [dbo].[extrema]
(
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [ixShare_extrema] ON [dbo].[extrema]
(
	[ShareId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO



CREATE TABLE [dbo].[trendlines](
	[Id] [int] IDENTITY(1,1) NOT NULL,	
	[ShareId] [int] NOT NULL,
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
	[ShowOnChart] [bit] NOT NULL default(0),
	[Value] [float] NOT NULL default(0),
CONSTRAINT [PK_trendlines] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ixId_trendlines] ON [dbo].[trendlines]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

CREATE NONCLUSTERED INDEX [ixBaseStartIndex_trendlines] ON [dbo].[trendlines]
(
	[BaseStartIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

CREATE NONCLUSTERED INDEX [ixBaseId_trendlines] ON [dbo].[trendlines]
(
	[BaseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

CREATE NONCLUSTERED INDEX [ixCounterStartIndex_trendlines] ON [dbo].[trendlines]
(
	[CounterStartIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

CREATE NONCLUSTERED INDEX [ixCounterId_trendlines] ON [dbo].[trendlines]
(
	[CounterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)



GO


CREATE TABLE [dbo].[extremumGroups](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShareId] [int] NOT NULL,
	[IsPeak] [bit] NOT NULL,
	[MasterId] [int] NULL,
	[MasterIndex] [int] NULL,
	[SlaveId] [int] NULL,
	[SlaveIndex] [int] NULL,
	[StartIndex] [int] NOT NULL,
	[EndIndex] [int] NOT NULL,
	[Close] [float] NOT NULL,
	[High] [float] NULL,
	[MasterHigh] [float] NULL,
	[Low] [float] NULL,
	[MasterLow] [float] NULL,
	[Value] [float]  NOT NULL
CONSTRAINT [PK_extremumGroups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ixStartIndex_extremumGroups] ON [dbo].[extremumGroups]
(
	[StartIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

CREATE NONCLUSTERED INDEX [ixEndIndex_extremumGroups] ON [dbo].[extremumGroups]
(
	[EndIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)



COMMIT TRANSACTION;