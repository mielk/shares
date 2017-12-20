USE [shares];

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

BEGIN TRANSACTION;

CREATE TABLE [dbo].[macd](
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



--COMMIT TRANSACTION;
ROLLBACK TRANSACTION;