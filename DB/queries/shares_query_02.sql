USE [shares];

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

BEGIN TRANSACTION;

/****** Object:  Table [dbo].[errorLogs]    Script Date: 2017-12-11 05:04:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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

GO

ALTER TABLE [dbo].[errorLogs]  WITH CHECK ADD  CONSTRAINT [CH_notEmpty_Class] CHECK  ((LEN(RTRIM(LTRIM([Class])))>(0)))
GO

ALTER TABLE [dbo].[errorLogs] CHECK CONSTRAINT [CH_notEmpty_Class]
GO

ALTER TABLE [dbo].[errorLogs]  WITH CHECK ADD  CONSTRAINT [CH_notEmpty_ErrNumber] CHECK  (([ErrNumber]<>(0)))
GO

ALTER TABLE [dbo].[errorLogs] CHECK CONSTRAINT [CH_notEmpty_ErrNumber]
GO

ALTER TABLE [dbo].[errorLogs]  WITH CHECK ADD  CONSTRAINT [CH_notEmpty_ErrorDescription] CHECK  ((LEN(RTRIM(LTRIM([ErrDescription])))>(0)))
GO

ALTER TABLE [dbo].[errorLogs] CHECK CONSTRAINT [CH_notEmpty_ErrorDescription]
GO

ALTER TABLE [dbo].[errorLogs]  WITH CHECK ADD  CONSTRAINT [CH_notEmpty_Method] CHECK  ((LEN(RTRIM(LTRIM([Method])))>(0)))
GO

ALTER TABLE [dbo].[errorLogs] CHECK CONSTRAINT [CH_notEmpty_Method]
GO



CREATE TABLE [dbo].[historicalUpdatesLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShareId] [int] NOT NULL,
	[QuotesUpdateTimestamp] [datetime] NULL,
	[DividendsUpdateTimestamp] [datetime] NULL,
	[SplitsUpdateTimestamp] [datetime] NULL
 CONSTRAINT [PK_historicalUpdatesLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[historicalUpdatesLogs]  WITH CHECK ADD  CONSTRAINT [FK_HistoricalUpdatesLogs_ShareId] FOREIGN KEY([ShareId])
REFERENCES [dbo].[shares] ([Id])



GO



CREATE TABLE [dbo].[quotes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShareId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Open] [float] NULL,
	[Low] [float] NULL,
	[High] [float] NULL,
	[Close] [float] NULL,
	[AdjClose] [float] NULL,
	[Volume] [bigint] NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Quotes_CreatedDate]  DEFAULT (GETDATE())
 CONSTRAINT [PK_quotes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[quotes]  WITH CHECK ADD  CONSTRAINT [FK_Quotes_ShareId] FOREIGN KEY([ShareId])
REFERENCES [dbo].[shares] ([Id])


GO

CREATE NONCLUSTERED INDEX [ixShareId_quotes] ON [dbo].[quotes]
(
	[ShareId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [ixDate_quotes] ON [dbo].[quotes]
(
	[Date] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [ixDateIndex_quotes] ON [dbo].[quotes]
(
	[DateIndex] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO



CREATE TABLE [dbo].[dividends](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShareId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Amount] [float] NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Dividends_CreatedDate]  DEFAULT (GETDATE())
 CONSTRAINT [PK_dividends] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[dividends]  WITH CHECK ADD  CONSTRAINT [FK_Dividends_ShareId] FOREIGN KEY([ShareId])
REFERENCES [dbo].[shares] ([Id])


GO


CREATE TABLE [dbo].[splits](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShareId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[BaseValue] [int] NOT NULL,
	[CounterValue] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Splits_CreatedDate]  DEFAULT (GETDATE())
 CONSTRAINT [PK_splits] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[splits]  WITH CHECK ADD  CONSTRAINT [FK_Splits_ShareId] FOREIGN KEY([ShareId])
REFERENCES [dbo].[shares] ([Id])


COMMIT TRANSACTION;
--ROLLBACK TRANSACTION;