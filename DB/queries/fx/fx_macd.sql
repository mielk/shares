USE [fx];
begin transaction

CREATE TABLE [dbo].[macd] (
	[AssetId] [int] NOT NULL,
	[TimeframeId] [int] NOT NULL,
	[DateIndex] [int] NOT NULL,
	[MA13] [float] NULL,
	[EMA13] [float] NULL,
	[MA26] [float] NULL,
	[EMA26] [float] NULL,
	[MACDLine] [float] NULL,
	[SignalLine] [float] NULL,
	[Histogram] [float] NULL,
	[HistogramAvg] [float] NULL,
	[HistogramExtremum] [float] NULL,
	[DeltaHistogram] [float] NULL,
	[DeltaHistogramPositive] [int] NULL,
	[DeltaHistogramNegative] [int] NULL,
	[DeltaHistogramZero] [int] NULL,
	[HistogramDirection2D] [int] NULL,
	[HistogramDirection3D] [int] ,
	[HistogramDirectionChanged] [int] NULL,
	[HistogramToOX] [int] NULL,
	[HistogramRow] [int] ,
	[OxCrossing] [float] NULL,
	[MacdPeak] [int] NULL,
	[LastMACDPeak] [float] NULL,
	[MACDPeakSlope] [float] NULL,
	[MACDTrough] [int] NULL,
	[LastMACDTrough] [float] NULL,
	[MACDTroughSlope] [float] NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Macd_CreatedDate]  DEFAULT (GETDATE())
);


ALTER TABLE [dbo].[macd]  WITH CHECK ADD  CONSTRAINT [FK_Macd_AssetId] FOREIGN KEY([AssetId])
REFERENCES [dbo].[assets] ([AssetId])
ON DELETE CASCADE


ALTER TABLE [dbo].[macd]  WITH CHECK ADD  CONSTRAINT [FK_Macd_DateIndex] FOREIGN KEY([DateIndex], [TimeframeId])
REFERENCES [dbo].[dates] ([DateIndex], [TimeframeId])
ON DELETE CASCADE

/****** Object:  Index [ixAssetId_Macd]    Script Date: 2018-05-28 02:49:55 ******/
CREATE NONCLUSTERED INDEX [ixAssetId_Macd] ON [dbo].[macd]
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

CREATE UNIQUE NONCLUSTERED INDEX [ixAssetTimeframeDateIndex_Macd] ON [dbo].[macd]
(
	[AssetId] ASC,
	[TimeframeId] ASC,
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

CREATE NONCLUSTERED INDEX [ixDateIndex_Macd] ON [dbo].[macd]
(
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

CREATE NONCLUSTERED INDEX [ixTimeframe_Macd] ON [dbo].[macd]
(
	[TimeframeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


rollback transaction