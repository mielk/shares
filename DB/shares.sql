USE [master]
GO
/****** Object:  Database [shares]    Script Date: 2018-01-03 01:00:56 ******/
CREATE DATABASE [shares]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'shares', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\shares.mdf' , SIZE = 2497536KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'shares_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\shares_log.ldf' , SIZE = 28333632KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [shares] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [shares].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [shares] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [shares] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [shares] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [shares] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [shares] SET ARITHABORT OFF 
GO
ALTER DATABASE [shares] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [shares] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [shares] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [shares] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [shares] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [shares] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [shares] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [shares] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [shares] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [shares] SET  DISABLE_BROKER 
GO
ALTER DATABASE [shares] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [shares] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [shares] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [shares] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [shares] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [shares] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [shares] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [shares] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [shares] SET  MULTI_USER 
GO
ALTER DATABASE [shares] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [shares] SET DB_CHAINING OFF 
GO
ALTER DATABASE [shares] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [shares] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [shares] SET DELAYED_DURABILITY = DISABLED 
GO
USE [shares]
GO
/****** Object:  Table [dbo].[archive_trendlines]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[archive_trendlines](
	[Id] [int] NOT NULL,
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
	[ShowOnChart] [bit] NOT NULL,
	[Value] [float] NOT NULL,
 CONSTRAINT [PK_archive_trendlines] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[archive_trendlinesBreaks]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[archive_trendlinesBreaks](
	[TrendlineId] [int] NOT NULL,
	[DateIndex] [int] NOT NULL,
	[BreakFromAbove] [int] NOT NULL,
 CONSTRAINT [PK_archive_trendlinesBreaks] PRIMARY KEY CLUSTERED 
(
	[TrendlineId] ASC,
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[archive_trendlinesHits]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[archive_trendlinesHits](
	[TrendlineId] [int] NOT NULL,
	[ExtremumGroupId] [int] NOT NULL,
	[DateIndex] [int] NOT NULL,
 CONSTRAINT [PK_archive_trendlinesHits] PRIMARY KEY CLUSTERED 
(
	[TrendlineId] ASC,
	[ExtremumGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[archive_trendRanges]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
 CONSTRAINT [PK_archive_trendRanges] PRIMARY KEY CLUSTERED 
(
	[TrendlineId] ASC,
	[BaseDateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[dates]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[DateIndex] [int] NOT NULL DEFAULT ((1)),
	[Timeframe] [int] NOT NULL DEFAULT ((6)),
	[ParentLevelDateIndex] [int] NULL,
 CONSTRAINT [PK_dates] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[dividends]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dividends](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShareId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Amount] [float] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dividends] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[errorLogs]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[errorLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[Class] [nvarchar](255) NOT NULL,
	[Method] [nvarchar](255) NOT NULL,
	[InputParams] [nvarchar](max) NULL,
	[ErrNumber] [int] NOT NULL,
	[ErrDescription] [nvarchar](max) NOT NULL,
	[SqlString] [nvarchar](max) NULL,
 CONSTRAINT [PK_errorLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[extrema]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	[Timeframe] [int] NOT NULL DEFAULT ((6)),
 CONSTRAINT [PK_extrema] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[extremumGroups]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	[Value] [float] NOT NULL,
	[Timeframe] [int] NOT NULL DEFAULT ((6)),
 CONSTRAINT [PK_extremumGroups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[historicalUpdatesLogs]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[historicalUpdatesLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShareId] [int] NOT NULL,
	[QuotesUpdateTimestamp] [datetime] NULL,
	[DividendsUpdateTimestamp] [datetime] NULL,
	[SplitsUpdateTimestamp] [datetime] NULL,
 CONSTRAINT [PK_historicalUpdatesLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[markets]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[markets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Uuid] [nvarchar](36) NOT NULL DEFAULT (newid()),
	[Name] [nvarchar](255) NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [Default_Markets_IsActive]  DEFAULT ((1)),
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Markets_CreatedDate]  DEFAULT (getdate()),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [Default_Markets_ModifiedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_markets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[predefinedNumbers]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[predefinedNumbers](
	[number] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[prices]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[prices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShareId] [int] NOT NULL,
	[Timeframe] [int] NOT NULL,
	[DateIndex] [int] NOT NULL,
	[DeltaClosePrice] [float] NOT NULL,
	[PriceDirection2D] [int] NULL,
	[PriceDirection3D] [int] NULL,
	[PriceGap] [float] NULL,
	[CloseRatio] [float] NULL,
	[ExtremumRatio] [float] NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_prices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[quotes]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_quotes_CreatedDate]  DEFAULT (getdate()),
	[DateIndex] [int] NULL,
	[Timeframe] [int] NOT NULL DEFAULT ((6)),
 CONSTRAINT [PK_quotes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[shares]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[shares](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Uuid] [nvarchar](36) NOT NULL DEFAULT (newid()),
	[YahooSymbol] [nvarchar](255) NULL,
	[Plus500Symbol] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[MarketId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [Default_Campaigns_IsActive]  DEFAULT ((1)),
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Campaigns_CreatedDate]  DEFAULT (getdate()),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [Default_Campaigns_ModifiedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_shares] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[splits]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[splits](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShareId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[BaseValue] [int] NOT NULL,
	[CounterValue] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_splits] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[timeframes]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[timeframes](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](4) NOT NULL,
 CONSTRAINT [PK_timeframes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[timestamps_lastAnalysis]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[timestamps_lastAnalysis](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShareId] [int] NOT NULL,
	[Timeframe] [int] NOT NULL,
	[DateIndex] [int] NULL,
 CONSTRAINT [PK_timestamps_lastAnalysis] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[trendlines]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	[ShowOnChart] [bit] NOT NULL DEFAULT ((0)),
	[Value] [float] NOT NULL DEFAULT ((0)),
	[Timeframe] [int] NOT NULL DEFAULT ((6)),
 CONSTRAINT [PK_trendlines] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[trendlinesBreaks]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trendlinesBreaks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrendlineId] [int] NOT NULL,
	[DateIndex] [int] NOT NULL,
	[BreakFromAbove] [int] NOT NULL,
 CONSTRAINT [PK_trendlinesBreaks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[trendlinesByHits]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trendlinesByHits](
	[TrendlineId] [int] NOT NULL,
	[hits] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[trendlinesHits]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trendlinesHits](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrendlineId] [int] NOT NULL,
	[ExtremumGroupId] [int] NOT NULL,
	[DateIndex] [int] NOT NULL,
 CONSTRAINT [PK_trendlinesHits] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[trendRanges]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trendRanges](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrendlineId] [int] NOT NULL,
	[BaseId] [int] NOT NULL,
	[BaseIsHit] [int] NOT NULL,
	[BaseDateIndex] [int] NOT NULL,
	[CounterId] [int] NOT NULL,
	[CounterIsHit] [int] NOT NULL,
	[CounterDateIndex] [int] NOT NULL,
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
 CONSTRAINT [PK_trendRanges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  UserDefinedFunction [dbo].[Minimum]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Minimum]
(@Param1 INTEGER, @Param2 INTEGER)
RETURNS TABLE AS
RETURN(SELECT CASE WHEN @Param1 < @Param2 
                   THEN @Param1 ELSE @Param2 END MinValue)

GO
/****** Object:  View [dbo].[firstQuotes]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[firstQuotes] AS
SELECT
	sh.[Id],
	sh.[Name],
	sh.[YahooSymbol],
	firstQuotes.[fq]

FROM
	[dbo].[shares] sh
	LEFT JOIN (SELECT [ShareId], MIN([Date]) AS fq
				FROM  [dbo].[quotes]
				GROUP BY [ShareId]) firstQuotes
	ON sh.[Id] = firstQuotes.[ShareId]


GO
/****** Object:  View [dbo].[missingData]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[missingData] AS
SELECT
	sh.[Name] AS [Share],
	sh.[YahooSymbol],
	sh.[Plus500Symbol],
	m.[Name] AS [Market],
	hul.[dataType]
FROM 
	(SELECT [ShareId], 'quotes' AS [dataType] FROM  [dbo].[historicalUpdatesLogs] WHERE  [QuotesUpdateTimestamp] IS NULL
	UNION ALL
	SELECT [ShareId], 'dividends' AS [dataType] FROM  [dbo].[historicalUpdatesLogs] WHERE  [DividendsUpdateTimestamp] IS NULL
	UNION ALL
	SELECT [ShareId], 'splits' AS [dataType] FROM  [dbo].[historicalUpdatesLogs] WHERE  [SplitsUpdateTimestamp] IS NULL) hul
	INNER JOIN [dbo].[shares] sh ON hul.[ShareId] = sh.[Id]
	INNER JOIN [dbo].[markets] m ON sh.[MarketId] = m.[Id]



GO
/****** Object:  View [dbo].[sharesWithMissingData]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[sharesWithMissingData] AS
(SELECT
	*
FROM 
	[dbo].[shares] sh
WHERE
	[Id] IN  (select [ShareId]
				from  [dbo].[historicalUpdatesLogs]
				where
					[QuotesUpdateTimestamp] IS NULL OR
					[DividendsUpdateTimestamp] IS NULL OR
					[SplitsUpdateTimestamp] IS NULL))



GO
/****** Object:  View [dbo].[ViewDataInfo]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewDataInfo] AS
SELECT
	[ShareId],
	MIN([Date]) AS [StartDate],
	MAX([Date]) AS [EndDate],
	MIN([DateIndex]) AS [StartIndex],
	MAX([DateIndex]) AS [EndIndex],
	CAST(MIN([Low]) AS NUMERIC(36,2)) AS [MinLevel],
	CAST(MAX([High]) AS NUMERIC(36,2)) AS [MaxLevel],
	COUNT(*) AS [Counter]
FROM
	[dbo].[quotes]
GROUP BY 
	[ShareId];



GO
/****** Object:  Index [ixBaseId_archive_trendlines]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixBaseId_archive_trendlines] ON [dbo].[archive_trendlines]
(
	[BaseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixBaseStartIndex_archive_trendlines]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixBaseStartIndex_archive_trendlines] ON [dbo].[archive_trendlines]
(
	[BaseStartIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixCounterId_archive_trendlines]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixCounterId_archive_trendlines] ON [dbo].[archive_trendlines]
(
	[CounterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixCounterStartIndex_archive_trendlines]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixCounterStartIndex_archive_trendlines] ON [dbo].[archive_trendlines]
(
	[CounterStartIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixId_archive_trendlines]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixId_archive_trendlines] ON [dbo].[archive_trendlines]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDateIndex_archive_trendlinesBreaks]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixDateIndex_archive_trendlinesBreaks] ON [dbo].[archive_trendlinesBreaks]
(
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTrendlineId_archive_trendlinesBreaks]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixTrendlineId_archive_trendlinesBreaks] ON [dbo].[archive_trendlinesBreaks]
(
	[TrendlineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDateIndex_archive_trendlinesHits]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixDateIndex_archive_trendlinesHits] ON [dbo].[archive_trendlinesHits]
(
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixExtremumGroupId_archive_trendlinesHits]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixExtremumGroupId_archive_trendlinesHits] ON [dbo].[archive_trendlinesHits]
(
	[ExtremumGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTrendlineId_archive_trendlinesHits]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixTrendlineId_archive_trendlinesHits] ON [dbo].[archive_trendlinesHits]
(
	[TrendlineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixBaseDateIndex_archive_trendRanges]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixBaseDateIndex_archive_trendRanges] ON [dbo].[archive_trendRanges]
(
	[BaseDateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixCounterDateIndex_archive_trendRanges]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixCounterDateIndex_archive_trendRanges] ON [dbo].[archive_trendRanges]
(
	[CounterDateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTrendlineId_archive_trendRanges]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixTrendlineId_archive_trendRanges] ON [dbo].[archive_trendRanges]
(
	[TrendlineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDateIndexTimeframe_dates]    Script Date: 2018-01-03 01:00:56 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixDateIndexTimeframe_dates] ON [dbo].[dates]
(
	[DateIndex] ASC,
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDateIndex_extrema]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixDateIndex_extrema] ON [dbo].[extrema]
(
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixShare_extrema]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixShare_extrema] ON [dbo].[extrema]
(
	[ShareId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixEndIndex_extremumGroups]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixEndIndex_extremumGroups] ON [dbo].[extremumGroups]
(
	[EndIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixStartIndex_extremumGroups]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixStartIndex_extremumGroups] ON [dbo].[extremumGroups]
(
	[StartIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [ixName_markets]    Script Date: 2018-01-03 01:00:56 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixName_markets] ON [dbo].[markets]
(
	[Name] ASC
)
WHERE ([IsActive]=(1))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixValue_predefinedNumbers]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixValue_predefinedNumbers] ON [dbo].[predefinedNumbers]
(
	[number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDateIndex_prices]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixDateIndex_prices] ON [dbo].[prices]
(
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixShareId_prices]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixShareId_prices] ON [dbo].[prices]
(
	[ShareId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTimeframe_prices]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixTimeframe_prices] ON [dbo].[prices]
(
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDate_quotes]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixDate_quotes] ON [dbo].[quotes]
(
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDateIndex_quotes]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixDateIndex_quotes] ON [dbo].[quotes]
(
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixShareDateIndexTimeframe_extrema]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixShareDateIndexTimeframe_extrema] ON [dbo].[quotes]
(
	[ShareId] ASC,
	[DateIndex] ASC,
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixShareDateIndexTimeframe_extremumGroups]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixShareDateIndexTimeframe_extremumGroups] ON [dbo].[quotes]
(
	[ShareId] ASC,
	[DateIndex] ASC,
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixShareDateIndexTimeframe_prices]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixShareDateIndexTimeframe_prices] ON [dbo].[quotes]
(
	[ShareId] ASC,
	[DateIndex] ASC,
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixShareDateIndexTimeframe_quotes]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixShareDateIndexTimeframe_quotes] ON [dbo].[quotes]
(
	[ShareId] ASC,
	[DateIndex] ASC,
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixShareDateIndexTimeframe_trendlines]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixShareDateIndexTimeframe_trendlines] ON [dbo].[quotes]
(
	[ShareId] ASC,
	[DateIndex] ASC,
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixShareId_quotes]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixShareId_quotes] ON [dbo].[quotes]
(
	[ShareId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTimeframe_extrema]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixTimeframe_extrema] ON [dbo].[quotes]
(
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTimeframe_extremumGroups]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixTimeframe_extremumGroups] ON [dbo].[quotes]
(
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTimeframe_prices]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixTimeframe_prices] ON [dbo].[quotes]
(
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTimeframe_quotes]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixTimeframe_quotes] ON [dbo].[quotes]
(
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTimeframe_trendlines]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixTimeframe_trendlines] ON [dbo].[quotes]
(
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [ixName_shares]    Script Date: 2018-01-03 01:00:56 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixName_shares] ON [dbo].[shares]
(
	[Name] ASC,
	[MarketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [ixPlus500Symbol_shares]    Script Date: 2018-01-03 01:00:56 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixPlus500Symbol_shares] ON [dbo].[shares]
(
	[Plus500Symbol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [ixYahooSymbol_shares]    Script Date: 2018-01-03 01:00:56 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixYahooSymbol_shares] ON [dbo].[shares]
(
	[YahooSymbol] ASC
)
WHERE ([YahooSymbol] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixShareTimeframe_timestampsLastAnalysis]    Script Date: 2018-01-03 01:00:56 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixShareTimeframe_timestampsLastAnalysis] ON [dbo].[timestamps_lastAnalysis]
(
	[ShareId] ASC,
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixBaseId_trendlines]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixBaseId_trendlines] ON [dbo].[trendlines]
(
	[BaseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixBaseStartIndex_trendlines]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixBaseStartIndex_trendlines] ON [dbo].[trendlines]
(
	[BaseStartIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixCounterId_trendlines]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixCounterId_trendlines] ON [dbo].[trendlines]
(
	[CounterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixCounterStartIndex_trendlines]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixCounterStartIndex_trendlines] ON [dbo].[trendlines]
(
	[CounterStartIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixId_trendlines]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixId_trendlines] ON [dbo].[trendlines]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDateIndex_trendlinesBreaks]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixDateIndex_trendlinesBreaks] ON [dbo].[trendlinesBreaks]
(
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixId_trendlinesBreaks]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixId_trendlinesBreaks] ON [dbo].[trendlinesBreaks]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTrendlineId_trendlinesBreaks]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixTrendlineId_trendlinesBreaks] ON [dbo].[trendlinesBreaks]
(
	[TrendlineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDateIndex_trendlinesHits]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixDateIndex_trendlinesHits] ON [dbo].[trendlinesHits]
(
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixId_trendlinesHits]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixId_trendlinesHits] ON [dbo].[trendlinesHits]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTrendlineId_trendlinesHits]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixTrendlineId_trendlinesHits] ON [dbo].[trendlinesHits]
(
	[TrendlineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixBaseDateIndex_trendRanges]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixBaseDateIndex_trendRanges] ON [dbo].[trendRanges]
(
	[BaseDateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixBaseId_trendRanges]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixBaseId_trendRanges] ON [dbo].[trendRanges]
(
	[BaseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixCounterDateIndex_trendRanges]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixCounterDateIndex_trendRanges] ON [dbo].[trendRanges]
(
	[CounterDateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixCounterId_trendRanges]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixCounterId_trendRanges] ON [dbo].[trendRanges]
(
	[CounterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixId_trendRanges]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixId_trendRanges] ON [dbo].[trendRanges]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTrendlineId_trendRanges]    Script Date: 2018-01-03 01:00:56 ******/
CREATE NONCLUSTERED INDEX [ixTrendlineId_trendRanges] ON [dbo].[trendRanges]
(
	[TrendlineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[archive_trendlines] ADD  DEFAULT ((0)) FOR [ShowOnChart]
GO
ALTER TABLE [dbo].[archive_trendlines] ADD  DEFAULT ((0)) FOR [Value]
GO
ALTER TABLE [dbo].[dividends] ADD  CONSTRAINT [Default_Dividends_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[errorLogs] ADD  CONSTRAINT [Default_ErrorLogs_Timestamp]  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[prices] ADD  CONSTRAINT [DF_prices_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[splits] ADD  CONSTRAINT [Default_Splits_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[trendRanges] ADD  DEFAULT ((0)) FOR [IsPeak]
GO
ALTER TABLE [dbo].[archive_trendlinesBreaks]  WITH CHECK ADD  CONSTRAINT [FK_archive_TrendlinesBreaks_TrendlineId] FOREIGN KEY([TrendlineId])
REFERENCES [dbo].[archive_trendlines] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[archive_trendlinesBreaks] CHECK CONSTRAINT [FK_archive_TrendlinesBreaks_TrendlineId]
GO
ALTER TABLE [dbo].[archive_trendlinesHits]  WITH CHECK ADD  CONSTRAINT [FK_archive_TrendlinesHits_TrendlineId] FOREIGN KEY([TrendlineId])
REFERENCES [dbo].[archive_trendlines] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[archive_trendlinesHits] CHECK CONSTRAINT [FK_archive_TrendlinesHits_TrendlineId]
GO
ALTER TABLE [dbo].[archive_trendRanges]  WITH CHECK ADD  CONSTRAINT [FK_archive_TrendRanges_TrendlineId] FOREIGN KEY([TrendlineId])
REFERENCES [dbo].[archive_trendlines] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[archive_trendRanges] CHECK CONSTRAINT [FK_archive_TrendRanges_TrendlineId]
GO
ALTER TABLE [dbo].[dividends]  WITH CHECK ADD  CONSTRAINT [FK_Dividends_ShareId] FOREIGN KEY([ShareId])
REFERENCES [dbo].[shares] ([Id])
GO
ALTER TABLE [dbo].[dividends] CHECK CONSTRAINT [FK_Dividends_ShareId]
GO
ALTER TABLE [dbo].[extrema]  WITH CHECK ADD  CONSTRAINT [FK_Extrema_ShareId] FOREIGN KEY([ShareId])
REFERENCES [dbo].[shares] ([Id])
GO
ALTER TABLE [dbo].[extrema] CHECK CONSTRAINT [FK_Extrema_ShareId]
GO
ALTER TABLE [dbo].[historicalUpdatesLogs]  WITH CHECK ADD  CONSTRAINT [FK_HistoricalUpdatesLogs_ShareId] FOREIGN KEY([ShareId])
REFERENCES [dbo].[shares] ([Id])
GO
ALTER TABLE [dbo].[historicalUpdatesLogs] CHECK CONSTRAINT [FK_HistoricalUpdatesLogs_ShareId]
GO
ALTER TABLE [dbo].[prices]  WITH CHECK ADD  CONSTRAINT [FK_Prices_ShareId] FOREIGN KEY([ShareId])
REFERENCES [dbo].[shares] ([Id])
GO
ALTER TABLE [dbo].[prices] CHECK CONSTRAINT [FK_Prices_ShareId]
GO
ALTER TABLE [dbo].[prices]  WITH CHECK ADD  CONSTRAINT [FK_Prices_Timeframe] FOREIGN KEY([Timeframe])
REFERENCES [dbo].[timeframes] ([Id])
GO
ALTER TABLE [dbo].[prices] CHECK CONSTRAINT [FK_Prices_Timeframe]
GO
ALTER TABLE [dbo].[quotes]  WITH CHECK ADD  CONSTRAINT [FK_Quotes_ShareId] FOREIGN KEY([ShareId])
REFERENCES [dbo].[shares] ([Id])
GO
ALTER TABLE [dbo].[quotes] CHECK CONSTRAINT [FK_Quotes_ShareId]
GO
ALTER TABLE [dbo].[shares]  WITH CHECK ADD  CONSTRAINT [FK_Shares_MarketId] FOREIGN KEY([MarketId])
REFERENCES [dbo].[markets] ([Id])
GO
ALTER TABLE [dbo].[shares] CHECK CONSTRAINT [FK_Shares_MarketId]
GO
ALTER TABLE [dbo].[splits]  WITH CHECK ADD  CONSTRAINT [FK_Splits_ShareId] FOREIGN KEY([ShareId])
REFERENCES [dbo].[shares] ([Id])
GO
ALTER TABLE [dbo].[splits] CHECK CONSTRAINT [FK_Splits_ShareId]
GO
ALTER TABLE [dbo].[dates]  WITH CHECK ADD  CONSTRAINT [CH_date_notWeekend] CHECK  ((datepart(weekday,[Date])>=(2) AND datepart(weekday,[Date])<=(6)))
GO
ALTER TABLE [dbo].[dates] CHECK CONSTRAINT [CH_date_notWeekend]
GO
ALTER TABLE [dbo].[errorLogs]  WITH CHECK ADD  CONSTRAINT [CH_notEmpty_Class] CHECK  ((len(rtrim(ltrim([Class])))>(0)))
GO
ALTER TABLE [dbo].[errorLogs] CHECK CONSTRAINT [CH_notEmpty_Class]
GO
ALTER TABLE [dbo].[errorLogs]  WITH CHECK ADD  CONSTRAINT [CH_notEmpty_ErrNumber] CHECK  (([ErrNumber]<>(0)))
GO
ALTER TABLE [dbo].[errorLogs] CHECK CONSTRAINT [CH_notEmpty_ErrNumber]
GO
ALTER TABLE [dbo].[errorLogs]  WITH CHECK ADD  CONSTRAINT [CH_notEmpty_ErrorDescription] CHECK  ((len(rtrim(ltrim([ErrDescription])))>(0)))
GO
ALTER TABLE [dbo].[errorLogs] CHECK CONSTRAINT [CH_notEmpty_ErrorDescription]
GO
ALTER TABLE [dbo].[errorLogs]  WITH CHECK ADD  CONSTRAINT [CH_notEmpty_Method] CHECK  ((len(rtrim(ltrim([Method])))>(0)))
GO
ALTER TABLE [dbo].[errorLogs] CHECK CONSTRAINT [CH_notEmpty_Method]
GO
ALTER TABLE [dbo].[extrema]  WITH CHECK ADD  CONSTRAINT [CH_extrema_ExtremumType] CHECK  (([ExtremumType]>=(1) AND [ExtremumType]<=(4)))
GO
ALTER TABLE [dbo].[extrema] CHECK CONSTRAINT [CH_extrema_ExtremumType]
GO
ALTER TABLE [dbo].[markets]  WITH CHECK ADD  CONSTRAINT [CH_marketUuid_length] CHECK  ((len([Uuid])=(36)))
GO
ALTER TABLE [dbo].[markets] CHECK CONSTRAINT [CH_marketUuid_length]
GO
ALTER TABLE [dbo].[shares]  WITH CHECK ADD  CONSTRAINT [CH_shareUuid_length] CHECK  ((len([Uuid])=(36)))
GO
ALTER TABLE [dbo].[shares] CHECK CONSTRAINT [CH_shareUuid_length]
GO
/****** Object:  StoredProcedure [dbo].[DisplayTrendlineAllData]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DisplayTrendlineAllData] @trendlineId AS INT
AS

SELECT
	* 
FROM 
	[dbo].[trendlines]
WHERE
	[Id] = @trendlineId;

SELECT
	*
FROM
	[dbo].[trendlinesHits]
WHERE
	[TrendlineId] = @trendlineId
ORDER BY 
	[DateIndex] ASC;

SELECT
	*
FROM
	[dbo].[trendRanges]
WHERE
	[TrendlineId] = @trendlineId
ORDER BY 
	[BaseDateIndex] ASC

SELECT
	*
FROM
	[dbo].[trendlinesBreaks]
WHERE
	[TrendlineId] = @trendlineId
ORDER BY
	[DateIndex] ASC;
GO
/****** Object:  StoredProcedure [dbo].[evaluateTrendlines]    Script Date: 2018-01-03 01:00:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[evaluateTrendlines] @shareId INT, @baseExtremumId INT, @counterExtremumId INT

AS

BEGIN TRANSACTION

SELECT @shareId, @baseExtremumId, @counterExtremumId;

--Clear previous results.
BEGIN

	SELECT
		[Id]
	INTO
		#CurrentPairTrendlineIds
	FROM
		[dbo].[Trendlines]
	WHERE
		[ShareId] = @shareId AND 
		[BaseId] = @baseExtremumId AND 
		[CounterId] = @counterExtremumId;

	UPDATE
		[dbo].[Trendlines]
	SET
		[StartDateIndex] = NULL,
		[EndDateIndex] = NULL,
		[~IsOpenFromLeft] = 1,
		[~IsOpenFromRight] = 1,
		[ShowOnChart] = 0
	WHERE 
		[Id] IN (SELECT * FROM #CurrentPairTrendlineIds);

	DELETE FROM [dbo].[trendlinesBreaks]
	WHERE [TrendlineId] IN (SELECT * FROM #CurrentPairTrendlineIds);

	DELETE FROM [dbo].[TrendlinesHits]
	WHERE [TrendlineId] IN (SELECT * FROM #CurrentPairTrendlineIds);

	DELETE FROM [dbo].[TrendRanges]
	WHERE [TrendlineId] IN (SELECT * FROM #CurrentPairTrendlineIds);

	DROP TABLE #CurrentPairTrendlineIds;

END



DECLARE @distanceIterations INT, @maxIterationCounter INT;
DECLARE @trendlineStartOffset INT, @maxDeviationFromTrendline FLOAT;
DECLARE @displayLeftSidePreviewTables BIT, @displayRightSidePreviewTables BIT;
DECLARE @i INT, @j INT;
DECLARE @minQuoteIndex INT, @maxQuoteIndex INT;
DECLARE @minDistanceFromExtremumToBreak INT, @quotesAnalyzedForBreakEvaluation INT;
----------------------------------------------------------------------------------------------------------------
SET @maxIterationCounter = 10;
SET @minDistanceFromExtremumToBreak = 5;
SET @quotesAnalyzedForBreakEvaluation = 5;
SET @distanceIterations = 10;
SET @trendlineStartOffset = 0;
SET @maxDeviationFromTrendline = 0.008;
----------------------------------------------------------------------------------------------------------------
SET @displayLeftSidePreviewTables = 0;
SET @displayRightSidePreviewTables = 0;
----------------------------------------------------------------------------------------------------------------


--Temporary tables.
--SELECT * INTO #Trendlines FROM [dbo].[trendlines] WHERE [ShareId] = @shareId AND [BaseId] = @baseExtremumId AND [CounterId] = @counterExtremumId AND [~IsOpenFromRight] = 1;
SELECT * INTO #Trendlines FROM [dbo].[trendlines] WHERE [Id] = 1350568;
SELECT * INTO #ExtremumGroups FROM [dbo].[extremumGroups] WHERE [ShareId] = @shareId;
SELECT [DateIndex], [Date], [Open], [Low], [High], [Close], [Volume] INTO #Quotes FROM [dbo].[quotes] WHERE [ShareId] = @shareId;



--PREPARE TABLES
BEGIN

	--Create required tables.
	CREATE TABLE #TrendlinesBreaks(
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[TrendlineId] [int] NOT NULL,
		[DateIndex] [int] NOT NULL,
		[BreakFromAbove] [int] NOT NULL,
		CONSTRAINT [PK_temp_trendlinesBreaks] PRIMARY KEY CLUSTERED ([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY];

	CREATE NONCLUSTERED INDEX [ixId_temp_trendlinesBreaks] ON #TrendlinesBreaks
	([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixTrendlineId_temp_trendlinesBreaks] ON #TrendlinesBreaks
	([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixDateIndex_temp_trendlinesBreaks] ON #TrendlinesBreaks
	([DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


	CREATE TABLE #TrendlinesHits(
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[TrendlineId] [int] NOT NULL,
		[ExtremumGroupId] [int] NOT NULL,
		[DateIndex] [int] NOT NULL,
		CONSTRAINT [PK_temp_trendlinesHits] PRIMARY KEY CLUSTERED ([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY];

	CREATE NONCLUSTERED INDEX [ixId_temp_trendlinesHits] ON #TrendlinesHits
	([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixTrendlineId_temp_trendlinesHits] ON #TrendlinesHits
	([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixDateIndex_temp_trendlinesHits] ON #TrendlinesHits
	([DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


	CREATE TABLE #TrendRanges(
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[TrendlineId] [int] NOT NULL,
		[BaseId] [int] NOT NULL,
		[BaseIsHit] [int] NOT NULL,
		[BaseDateIndex] [int] NOT NULL,
		[CounterId] [int] NOT NULL,
		[CounterIsHit] [int] NOT NULL,
		[CounterDateIndex] [int] NOT NULL,
		[IsPeak] [int] NOT NULL DEFAULT(0),
		CONSTRAINT [PK_temp_trendRanges] PRIMARY KEY CLUSTERED ([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY];

	CREATE NONCLUSTERED INDEX [ixId_temp_trendRanges] ON #TrendRanges
	([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixTrendlineId_temp_trendRanges] ON #TrendRanges
	([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixBaseId_temp_trendRanges] ON #TrendRanges
	([BaseId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixBaseDateIndex_temp_trendRanges] ON #TrendRanges
	([BaseDateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixCounterId_temp_trendRanges] ON #TrendRanges
	([CounterId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [ixCounterDateIndex_temp_trendRanges] ON #TrendRanges
	([CounterDateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)



	SELECT 
		t.*, 
		NULL AS [BreakIndex],
		NULL AS [PrevBreakIndex],
		NULL AS [HitIndex],
		NULL AS [PrevHitIndex],
		IIF(t.[BaseIsPeak] = 1, 1, -1) AS [LookForPeaks],
		t.[BaseStartIndex] AS [AnalysisStartPoint]
	INTO 
		#TrendlinesTemp 
	FROM 
		#Trendlines t;





END


-- LEFT-SIDE ANALYSIS
BEGIN

	SET @j = 1;
	WHILE @j <= @maxIterationCounter
	BEGIN

		--FIND FIRST BREAK TO THE LEFT OF THE CURRENT POINT [1]
		BEGIN

			--Create temporary tables.
			BEGIN

				SELECT * INTO #TrendlinesBreaks_LeftSide FROM #TrendlinesBreaks WHERE 0 = 1;

				SELECT 
					* 
				INTO
					#OpenTrendlines
				FROM
					#TrendlinesTemp
				WHERE
					[~IsOpenFromLeft] = 1;

			END

			--Get proper set of quotes required for analysis.																									
			BEGIN

				SELECT
					MIN(a.[startQuote]) AS [Min],
					MAX(a.[endQuote]) AS [Max]
				INTO #BorderPoints
				FROM
					(SELECT 
						tt.[AnalysisStartPoint] - (@distanceIterations * tt.[~CandlesDistance]) AS [startQuote],
						tt.[AnalysisStartPoint] - 1 AS [endQuote]
					FROM 
						#OpenTrendlines tt) a;

				SET @minQuoteIndex = (SELECT [Min] FROM #BorderPoints);
				SET @maxQuoteIndex = (SELECT [Max] FROM #BorderPoints);
	
				SELECT
					*
				INTO 
					#Quotes_Temp
				FROM
					#Quotes q
				WHERE
					q.[DateIndex] BETWEEN @minQuoteIndex AND @maxQuoteIndex;

				DROP TABLE #BorderPoints;

			END

			--Look for break by searching batches (each batch contains as many elements as given trendline [~CandlesDistance]).
			BEGIN

				SET @i = 0;
				WHILE @i < @distanceIterations

				BEGIN
				
					--Create temporary table containing only trendlines without break already found.
					SELECT
						tt.* 
					INTO
						#TrendlinesWithoutBreak
					FROM 
						#OpenTrendlines tt
						LEFT JOIN #TrendlinesBreaks_LeftSide tb
						ON tt.[Id] = tb.[TrendlineId]
					WHERE 
						tb.[TrendlineId] IS NULL;

					--Leave loop if there is no trendlines left without break.
					IF (SELECT COUNT(*) FROM #TrendlinesWithoutBreak) = 0 
					BEGIN
						DROP TABLE #TrendlinesWithoutBreak;
						BREAK;
					END

					--Append current batch dates range to the table containing all trendlines without break.
					SELECT
						t.[Id] AS [TrendlineId], 
						t.[BaseLevel],
						t.[BaseStartIndex],
						t.[LookForPeaks],
						t.[Slope],
						t.[AnalysisStartPoint] - t.[~CandlesDistance] * (@i + 1) AS [BatchStartIndex],
						t.[AnalysisStartPoint] - t.[~CandlesDistance] * @i AS [BatchEndIndex]
					INTO
						#TrendlinesWithoutBreakWithDatesRange
					FROM
						#TrendlinesWithoutBreak t;

					--Create table with pairs.
					SELECT
						t.[TrendlineId], 
						t.[Slope],
						q.[Close] * t.[LookForPeaks] AS [ModifiedClose],
						q.[Open] * t.[LookForPeaks] AS [ModifiedOpen],
						q.[DateIndex],
						q.[Date],
						(t.[baseLevel] + (q.[DateIndex] - t.[BaseStartIndex]) * t.[Slope]) * t.[LookForPeaks] AS [ModifiedTrendlineLevel],
						t.[LookForPeaks]
					INTO
						#TrendlineQuotePairs
					FROM
						#TrendlinesWithoutBreakWithDatesRange t
						INNER JOIN #Quotes_Temp q
						ON q.[DateIndex] BETWEEN t.[BatchStartIndex] AND t.[BatchEndIndex]

--SELECT '#TrendlineQuotePairs', * from #TrendlineQuotePairs;

					--Filter only those pairs where Close and Open prices are above Resistance line or below Support line.
					SELECT
						t.[TrendlineId], 
						t.[DateIndex], 
						t.[LookForPeaks]
					INTO
						#FilteredTrendlineQuotePairs
					FROM
						#TrendlineQuotePairs t
					WHERE
						t.[ModifiedTrendlineLevel] < t.[ModifiedClose] AND t.[ModifiedTrendlineLevel] < t.[ModifiedOpen]

					--Select the latest break found before.
					SELECT
						ft.[TrendlineId], 
						MAX(ft.[DateIndex]) AS [DateIndex], 
						ft.[LookForPeaks] AS [BreakFromAbove]
					INTO 
						#TrendlineFirstBreak
					FROM
						#FilteredTrendlineQuotePairs ft
					GROUP BY
						ft.[TrendlineId], ft.[LookForPeaks];

					--Insert result into temporary breaks table.
					BEGIN 

						INSERT INTO #TrendlinesBreaks_LeftSide
						SELECT * FROM #TrendlineFirstBreak;

					END

					--Clean up
					BEGIN

						DROP TABLE #TrendlinesWithoutBreak;
						DROP TABLE #TrendlinesWithoutBreakWithDatesRange;
						DROP TABLE #TrendlineQuotePairs;
						DROP TABLE #FilteredTrendlineQuotePairs;
						DROP TABLE #TrendlineFirstBreak;

					END

					SET @i = @i + 1;

				END;

			END

			--Append breaks into #TrendlinesBreaks table.
			INSERT INTO #TrendlinesBreaks([TrendlineId], [DateIndex], [BreakFromAbove])
			SELECT [TrendlineId], [DateIndex], [BreakFromAbove] FROM #TrendlinesBreaks_LeftSide;

			--Clean up.
			BEGIN

				DROP TABLE #OpenTrendlines;
				DROP TABLE #Quotes_Temp;
				DROP TABLE #TrendlinesBreaks_LeftSide;

			END

		END

		IF (@displayLeftSidePreviewTables = 1) SELECT '#TrendlinesBreaks', @j AS [iteration], * FROM #TrendlinesBreaks;

		--MATCH BREAKS INFO TO TRENDLINES TABLE
		BEGIN

			--Update TrendBreaks pointers.
			UPDATE 
				tt
			SET 
				[BreakIndex] = tb.[DateIndex]
			FROM
				#TrendlinesTemp tt
				LEFT JOIN #TrendlinesBreaks tb
				ON tt.[Id] = tb.[TrendlineId]
			WHERE
				tt.[~IsOpenFromLeft] = 1 AND
				tb.[DateIndex] <= tt.[AnalysisStartPoint];

		END

		IF (@displayLeftSidePreviewTables = 1) SELECT '#TrendlinesTemp.AfterBreaks', @j AS [iteration], * FROM #TrendlinesTemp;

		--FIND HITS BETWEEN LEFT-SIDE ELEMENT AND LAST ANALYZED.
		BEGIN

			--Create temporary tables.
			BEGIN

				SELECT 
					* 
				INTO
					#OpenTrendlinesForHits
				FROM
					#TrendlinesTemp
				WHERE
					[~IsOpenFromLeft] = 1;

			END

			--Predefine [ExtremumGroups] table for better performance.
			BEGIN
		
				DECLARE @minExtremumIndex INT, @maxExtremumIndex INT;

				SELECT
					MIN(a.[startIndex]) AS [Min],
					MAX(a.[endIndex]) AS [Max]
				INTO #BorderExtremaPoints
				FROM
					(SELECT 
						MIN([AnalysisStartPoint] - tt.[~CandlesDistance] * @distanceIterations) + 1 AS [startIndex],
						MAX([AnalysisStartPoint]) - 1 AS [endIndex]
					FROM 
						#OpenTrendlinesForHits tt) a;

				SET @minExtremumIndex = (SELECT [Min] FROM #BorderExtremaPoints);
				SET @maxExtremumIndex = (SELECT [Max] FROM #BorderExtremaPoints);
		
				SELECT
					*
				INTO 
					#ExtremumGroups_Temp
				FROM
					#ExtremumGroups eg
				WHERE
					eg.[StartIndex] BETWEEN @minExtremumIndex AND @maxExtremumIndex;

				DROP TABLE #BorderExtremaPoints;

			END

			--Create temporary table with all possible combinations of hits (only distance is taken into account at this time).
			BEGIN
			
				--Append calculating fields to #OpenTrendlinesForHits
				SELECT
					*,
					[AnalysisStartPoint] - (@distanceIterations * [~CandlesDistance]) AS [CheckingStartPointIfNoBreakIndex],
					IIF([LookForPeaks] = 1, 1, 0) AS [LookForPeaksAsBit]
				INTO
					#TrendlinesWithCalculatingFields
				FROM
					#OpenTrendlinesForHits

				--Append match borders
				SELECT
					tt.*,
					COALESCE(tt.[BreakIndex] + @minDistanceFromExtremumToBreak, tt.[CheckingStartPointIfNoBreakIndex]) + 1 AS [MatchingLeftBorder],
					tt.[AnalysisStartPoint] AS [MatchingRightBorder]
				INTO
					#TrendlinesForHitsMatching
				FROM
					#TrendlinesWithCalculatingFields tt

				--Get table with possible matches Trendline-ExtremumGroup.
				SELECT
					-- Trendline properties
					tfhm.[Id] AS [TrendlineId],
					tfhm.[LookForPeaks] AS [SupResFactor],
					tfhm.[BaseLevel],
					tfhm.[BaseStartIndex],
					tfhm.[Slope],
					-- Extremum properties
					eg.[Id] AS [ExtremumGroupId],
					eg.[StartIndex] AS [ExtremumStartIndex],
					COALESCE(eg.[SlaveIndex], eg.[MasterIndex]) AS [ExtremumIndex],
					IIF(tfhm.[LookForPeaks] = 1, eg.[High], eg.[Low]) AS [ExtremumPrice]
				INTO
					#TrendlineExtremumPossibleMatches
				FROM
					#TrendlinesForHitsMatching tfhm
					LEFT JOIN  
					#ExtremumGroups_Temp eg
					ON  
							eg.[IsPeak] = tfhm.[LookForPeaksAsBit]
						AND eg.[StartIndex] BETWEEN tfhm.[MatchingLeftBorder] AND tfhm.[MatchingRightBorder]
		
				--Append trendline level.
				SELECT
					[TrendlineId],
					[SupResFactor],
					[ExtremumGroupId],
					[ExtremumStartIndex],
					[ExtremumPrice],
					([baseLevel] + ([ExtremumIndex] - [BaseStartIndex]) * [Slope]) AS [TrendlineLevel]
				INTO
					#TrendlineMatchesWithPriceLevels
				FROM
					#TrendlineExtremumPossibleMatches

				--Append prices modified by SupResFactor.
				SELECT
					[TrendlineId],
					[ExtremumGroupId],
					[ExtremumStartIndex],
					[SupResFactor],
					[ExtremumPrice] * [SupResFactor] AS [ModifiedPrice],
					[TrendlineLevel] * [SupResFactor] AS [ModifiedTrendlineLevel],
					IIF([TrendlineLevel] > 0, 1, -1) AS [TrendlineAboveZero],
					([TrendlineLevel] - [ExtremumPrice]) / [TrendlineLevel] AS [PriceTrendlineDistance]
				INTO
					#TrendlineMatchesWithModifiedPrices
				FROM
					#TrendlineMatchesWithPriceLevels

			END

			--Filter out all matches with price too far away from trendline.
		--Insert the rest of them into temporary #TrendlinesHits table.
			BEGIN

				INSERT INTO #TrendlinesHits([TrendlineId], [ExtremumGroupId], [DateIndex])
				SELECT
					[TrendlineId],
					[ExtremumGroupId],
					[ExtremumStartIndex]
				FROM
					#TrendlineMatchesWithModifiedPrices
				WHERE
					[SupResFactor] * [PriceTrendlineDistance] * [TrendlineAboveZero] < @maxDeviationFromTrendline;

			END


			--Remove duplicates from #TrendlinesHits table.
			BEGIN
				
				WITH CTE AS(
				   SELECT [TrendlineId], [ExtremumGroupId], [DateIndex], RN = ROW_NUMBER()
				   OVER(PARTITION BY [TrendlineId], [ExtremumGroupId], [DateIndex] ORDER BY [TrendlineId], [ExtremumGroupId], [DateIndex])
				   FROM #TrendlinesHits
				)
				DELETE FROM CTE WHERE RN > 1

			END



			--Clean up after looking for trend hits.
			BEGIN

				DROP TABLE #TrendlinesWithCalculatingFields;
				DROP TABLE #OpenTrendlinesForHits;
				DROP TABLE #ExtremumGroups_Temp;
				DROP TABLE #TrendlinesForHitsMatching;
				DROP TABLE #TrendlineExtremumPossibleMatches;
				DROP TABLE #TrendlineMatchesWithPriceLevels;
				DROP TABLE #TrendlineMatchesWithModifiedPrices;

			END

		END

		IF (@displayLeftSidePreviewTables = 1) SELECT '#TrendlinesHits', @j AS [iteration], * FROM #TrendlinesHits;

		--MATCH HITS INFO TO TRENDLINES TABLE
		BEGIN

			--Create temporary table with the earliest trend hit for each trendline.
			SELECT
				th.[TrendlineId],
				MIN(th.[DateIndex]) AS [FirstHit]
			INTO 
				#EarliestTrendHits
			FROM
				#TrendlinesHits th
			GROUP BY
				th.[TrendlineId]

			--Update TrendHit pointers.
			UPDATE 
				tt
			SET 
				[HitIndex] = h.[FirstHit]
			FROM
				#TrendlinesTemp tt
				LEFT JOIN #EarliestTrendHits h
				ON tt.[Id] = h.[TrendlineId]
			WHERE
				tt.[~IsOpenFromLeft] = 1 AND
				h.[FirstHit] <= tt.[AnalysisStartPoint];

			--Clean up
			DROP TABLE #EarliestTrendHits;

		END
	
		IF (@displayLeftSidePreviewTables = 1) SELECT '#TrendlinesTemp.AfterHits', @j AS [iteration], * FROM #TrendlinesTemp;

		--PREPARE TRENDLINES TABLE FOR NEXT ANALYSIS ITERATION
		BEGIN

			--Inactivate trendlines with no Break and no Hit found
			UPDATE
				#TrendlinesTemp
			SET
				[~IsOpenFromLeft] = 0,
				[StartDateIndex] = COALESCE([PrevHitIndex], [BaseStartIndex]) - @trendlineStartOffset
			WHERE
				[~IsOpenFromLeft] = 1 AND
				[BreakIndex] IS NULL AND [HitIndex] IS NULL;

			--Inactivate trendlines with two breaks with no hit between
			UPDATE
				#TrendlinesTemp
			SET
				[~IsOpenFromLeft] = 0,
				[StartDateIndex] = COALESCE([PrevHitIndex], [BaseStartIndex]) - @trendlineStartOffset
			WHERE
				[~IsOpenFromLeft] = 1 AND 
				[BreakIndex] IS NOT NULL AND [PrevBreakIndex] IS NOT NULL AND [HitIndex] IS NULL;

			--Inactivate if distance between hits is too large


			--Update indices of remaining trendlines for next analysis iteration.
			UPDATE
				#TrendlinesTemp
			SET 
				[LookForPeaks] = [LookForPeaks] * IIF([BreakIndex] IS NULL, 1, -1),
				[AnalysisStartPoint] = COALESCE(IIF([BreakIndex] IS NOT NULL, [BreakIndex] - 1, [HitIndex] - 1), 0),
				[PrevBreakIndex] = IIF([BreakIndex] IS NULL, [PrevBreakIndex], [BreakIndex]),
				[BreakIndex] = NULL,
				[PrevHitIndex] = IIF([HitIndex] IS NULL, [PrevHitIndex], [HitIndex]),
				[HitIndex] = NULL
			WHERE
				[~IsOpenFromLeft] = 1;

		END
	
		IF (@displayLeftSidePreviewTables = 1) SELECT '#TrendlinesTemp.BeforeNextIteration', @j AS [iteration], * FROM #TrendlinesTemp;
	
		--CHECK IF THERE ARE STILL SOME OPEN TRENDLINES
		IF (SELECT COUNT(*) FROM #TrendlinesTemp WHERE [~IsOpenFromLeft] = 1) = 0 BREAK;

		SET @j = @j + 1;

	END;

END



-- RIGHT-SIDE ANALYSIS
BEGIN
	
	--PREPARE TRENDLINES TABLE
	BEGIN

		UPDATE 
			#TrendlinesTemp
		SET 
			[AnalysisStartPoint] = [BaseStartIndex],
			[LookForPeaks] = IIF([BaseIsPeak] = 1, 1, -1)
	
		IF (@displayRightSidePreviewTables = 1) SELECT '#TrendlinesBreaks.BeforeStart', * FROM #TrendlinesTemp; --WHERE [TrendlineId] = 24019795 ORDER BY DateIndex ASC;

	END

	SET @j = 1;
	WHILE @j <= 10
	BEGIN

		--FIND FIRST BREAK TO THE RIGHT OF THE CURRENT POINT [1]
		BEGIN

			--Create temporary tables.
			BEGIN

				SELECT * INTO #TrendlinesBreaks_RightSide FROM #TrendlinesBreaks WHERE 0 = 1;

				SELECT 
					* 
				INTO
					#OpenTrendlinesRs
				FROM
					#TrendlinesTemp
				WHERE
					[~IsOpenFromRight] = 1;

			END

			--Get proper set of quotes required for analysis.			
			BEGIN

				SELECT
					MIN(a.[startQuote]) AS [Min],
					MAX(a.[endQuote]) AS [Max]
				INTO #BorderPointsRs
				FROM
					(SELECT 
						tt.[AnalysisStartPoint] + 1 AS [startQuote],
						tt.[AnalysisStartPoint] + (@distanceIterations * tt.[~CandlesDistance]) AS [endQuote]
					FROM 
						#OpenTrendlinesRs tt) a;

				SET @minQuoteIndex = (SELECT [Min] FROM #BorderPointsRs);
				SET @maxQuoteIndex = (SELECT [Max] FROM #BorderPointsRs);
	
				SELECT *
				INTO #Quotes_RightSide
				FROM #Quotes q
				WHERE q.[DateIndex] BETWEEN @minQuoteIndex AND @maxQuoteIndex;

				DROP TABLE #BorderPointsRs;

			END

			--Look for break by searching batches (each batch contains as many elements as given trendline [~CandlesDistance]).
			BEGIN

				SET @i = 0;
				WHILE @i < @distanceIterations
				BEGIN
				
					--Create temporary table containing only trendlines without break already found.
					SELECT
						t.* 
					INTO
						#RsTrendlinesWithoutBreak
					FROM 
						#OpenTrendlinesRs t
						LEFT JOIN #TrendlinesBreaks_RightSide tb
						ON t.[Id] = tb.[TrendlineId]
					WHERE 
						tb.[TrendlineId] IS NULL;

					--Leave loop if there is no trendlines left without break.
					IF (SELECT COUNT(*) FROM #RsTrendlinesWithoutBreak) = 0 
					BEGIN
						DROP TABLE #RsTrendlinesWithoutBreak;
						BREAK;
					END

					--Append current batch dates range to the table containing all trendlines without break.
					SELECT
						t.[Id] AS [TrendlineId], 
						t.[BaseLevel],
						t.[BaseStartIndex],
						t.[LookForPeaks],
						t.[Slope],
						t.[AnalysisStartPoint] + t.[~CandlesDistance] * @i AS [BatchStartIndex],
						t.[AnalysisStartPoint] + t.[~CandlesDistance] * (@i + 1) AS [BatchEndIndex]
					INTO
						#RsTrendlinesWithoutBreakWithDatesRange
					FROM
						#RsTrendlinesWithoutBreak t;

					--Create table with pairs.
					SELECT
						t.[TrendlineId], 
						t.[Slope],
						q.[Close] * t.[LookForPeaks] AS [ModifiedClose],
						q.[Open] * t.[LookForPeaks] AS [ModifiedOpen],
						q.[DateIndex],
						q.[Date],
						(t.[baseLevel] + (q.[DateIndex] - t.[BaseStartIndex]) * t.[Slope]) * t.[LookForPeaks] AS [ModifiedTrendlineLevel],
						t.[LookForPeaks]
					INTO
						#RsTrendlineQuotePairs
					FROM
						#RsTrendlinesWithoutBreakWithDatesRange t
						INNER JOIN #Quotes_RightSide q
						ON q.[DateIndex] BETWEEN t.[BatchStartIndex] AND t.[BatchEndIndex]

					--Filter only those pairs where Close and Open prices are above Resistance line or below Support line.
					SELECT
						t.[TrendlineId], 
						t.[DateIndex], 
						t.[LookForPeaks]
					INTO
						#RsFilteredTrendlineQuotePairs
					FROM
						#RsTrendlineQuotePairs t
					WHERE
						t.[ModifiedTrendlineLevel] < t.[ModifiedClose] AND t.[ModifiedTrendlineLevel] < t.[ModifiedOpen]

					--Select the latest break found before.
					SELECT
						ft.[TrendlineId], 
						MIN(ft.[DateIndex]) AS [DateIndex], 
						ft.[LookForPeaks] * (-1) AS [BreakFromAbove]
					INTO 
						#RsTrendlineFirstBreak
					FROM
						#RsFilteredTrendlineQuotePairs ft
					GROUP BY
						ft.[TrendlineId], ft.[LookForPeaks];

					--Insert result into temporary breaks table.
					BEGIN 

						INSERT INTO #TrendlinesBreaks_RightSide
						SELECT * FROM #RsTrendlineFirstBreak;

					END

					--Clean up
					BEGIN

						DROP TABLE #RsTrendlinesWithoutBreak;
						DROP TABLE #RsTrendlinesWithoutBreakWithDatesRange;
						DROP TABLE #RsTrendlineQuotePairs;
						DROP TABLE #RsFilteredTrendlineQuotePairs;
						DROP TABLE #RsTrendlineFirstBreak;

					END

					SET @i = @i + 1;

				END;

			END
			
			--Append breaks into #TrendlinesBreaks table.
			INSERT INTO #TrendlinesBreaks([TrendlineId], [DateIndex], [BreakFromAbove])
			SELECT [TrendlineId], [DateIndex], [BreakFromAbove] FROM #TrendlinesBreaks_RightSide;

			--Clean up.
			BEGIN

				DROP TABLE #OpenTrendlinesRs;
				DROP TABLE #Quotes_RightSide;
				DROP TABLE #TrendlinesBreaks_RightSide;

			END

		END
		
		IF (@displayRightSidePreviewTables = 1) SELECT '#TrendlinesBreaks', @j AS [iteration], * FROM #TrendlinesBreaks ORDER BY [DateIndex] DESC;


		--MATCH BREAKS INFO TO TRENDLINES TABLE
		BEGIN

			--Update TrendBreaks pointers.
			UPDATE 
				tt
			SET 
				[BreakIndex] = tb.[DateIndex]
			FROM
				#TrendlinesTemp tt
				LEFT JOIN #TrendlinesBreaks tb
				ON tt.[Id] = tb.[TrendlineId]
			WHERE
				tt.[~IsOpenFromRight] = 1 AND
				tb.[DateIndex] >= tt.[AnalysisStartPoint];

		END

		IF (@displayRightSidePreviewTables = 1) SELECT '#TrendlinesTemp.AfterBreaks', @j AS [iteration], * FROM #TrendlinesTemp;


		--FIND HITS BETWEEN LEFT-SIDE ELEMENT AND LAST ANALYZED.
		BEGIN

			--Create temporary tables.
			BEGIN

				SELECT 
					* 
				INTO
					#RsOpenTrendlinesForHits
				FROM
					#TrendlinesTemp
				WHERE
					[~IsOpenFromRight] = 1;

			END

			--Predefine [ExtremumGroups] table for better performance.
			BEGIN
		
				DECLARE @minRsExtremumIndex INT, @maxRsExtremumIndex INT;

				SELECT
					MIN(a.[startIndex]) AS [Min],
					MAX(a.[endIndex]) AS [Max]
				INTO #RsBorderExtremaPoints
				FROM
					(SELECT 
						MIN([AnalysisStartPoint]) AS [startIndex],
						MAX([AnalysisStartPoint] + (tt.[~CandlesDistance] * @distanceIterations) - 1) AS [endIndex]
					FROM
						#RsOpenTrendlinesForHits tt) a;

				SET @minRsExtremumIndex  = (SELECT [Min] FROM #RsBorderExtremaPoints);
				SET @maxRsExtremumIndex  = (SELECT [Max] FROM #RsBorderExtremaPoints);
		
				SELECT
					*
				INTO 
					#RsExtremumGroups_Temp
				FROM
					#ExtremumGroups eg
				WHERE
					eg.[EndIndex] >= @minRsExtremumIndex AND eg.[StartIndex] <= @maxRsExtremumIndex;

				DROP TABLE #RsBorderExtremaPoints;

			END

			--Create temporary table with all possible combinations of hits (only distance is taken into account at this time).
			BEGIN
			
				--Append calculating fields to #OpenTrendlinesForHits
				SELECT
					*,
					[AnalysisStartPoint] + (@distanceIterations * [~CandlesDistance]) - 1 AS [CheckingEndPointIfNoBreakIndex],
					IIF([LookForPeaks] = 1, 1, 0) AS [LookForPeaksAsBit]
				INTO
					#RsTrendlinesWithCalculatingFields
				FROM
					#RsOpenTrendlinesForHits

				--Append match borders
				SELECT
					tt.*,
					tt.[AnalysisStartPoint] AS [MatchingLeftBorder],
					COALESCE(tt.[BreakIndex] - + @minDistanceFromExtremumToBreak, tt.[CheckingEndPointIfNoBreakIndex]) - 1 AS [MatchingRightBorder]
				INTO
					#RsTrendlinesForHitsMatching
				FROM
					#RsTrendlinesWithCalculatingFields tt


				--Get table with possible matches Trendline-ExtremumGroup.
				SELECT
					-- Trendline properties
					tfhm.[Id] AS [TrendlineId],
					tfhm.[LookForPeaks] AS [SupResFactor],
					tfhm.[BaseLevel],
					tfhm.[BaseStartIndex],
					tfhm.[Slope],
					-- Extremum properties
					eg.[Id] AS [ExtremumGroupId],
					eg.[StartIndex] AS [ExtremumStartIndex],
					COALESCE(eg.[SlaveIndex], eg.[MasterIndex]) AS [ExtremumIndex],
					IIF(tfhm.[LookForPeaks] = 1, eg.[High], eg.[Low]) AS [ExtremumPrice]
				INTO
					#RsTrendlineExtremumPossibleMatches
				FROM
					#RsTrendlinesForHitsMatching tfhm
					LEFT JOIN
					#RsExtremumGroups_Temp eg
					ON
							eg.[IsPeak] = tfhm.[LookForPeaksAsBit]
						AND eg.[EndIndex] >= tfhm.[MatchingLeftBorder] AND eg.[StartIndex] <= tfhm.[MatchingRightBorder]
		

				--Append trendline level.
				SELECT
					[TrendlineId],
					[SupResFactor],
					[ExtremumGroupId],
					[ExtremumStartIndex],
					[ExtremumPrice],
					([baseLevel] + ([ExtremumIndex] - [BaseStartIndex]) * [Slope]) AS [TrendlineLevel]
				INTO
					#RsTrendlineMatchesWithPriceLevels
				FROM
					#RsTrendlineExtremumPossibleMatches

				--Append prices modified by SupResFactor.
				SELECT
					[TrendlineId],
					[ExtremumGroupId],
					[ExtremumStartIndex],
					[SupResFactor],
					[ExtremumPrice] * [SupResFactor] AS [ModifiedPrice],
					[TrendlineLevel] * [SupResFactor] AS [ModifiedTrendlineLevel],
					IIF([TrendlineLevel] > 0, 1, -1) AS [TrendlineAboveZero],
					([TrendlineLevel] - [ExtremumPrice]) / [TrendlineLevel] AS [PriceTrendlineDistance]
				INTO
					#RsTrendlineMatchesWithModifiedPrices
				FROM
					#RsTrendlineMatchesWithPriceLevels

			END

			--Filter out all matches with price too far away from trendline.
			--Insert the rest of them into temporary #TrendlinesHits table.
			BEGIN

				INSERT INTO #TrendlinesHits([TrendlineId], [ExtremumGroupId], [DateIndex])
				SELECT DISTINCT
					[TrendlineId],
					[ExtremumGroupId],
					[ExtremumStartIndex]
				FROM
					#RsTrendlineMatchesWithModifiedPrices
				WHERE
					[SupResFactor] * [PriceTrendlineDistance] * [TrendlineAboveZero] < @maxDeviationFromTrendline;

			END

			--Remove duplicates from #TrendlinesHits table.
			BEGIN
				
				WITH CTE AS(
				   SELECT [TrendlineId], [ExtremumGroupId], [DateIndex], RN = ROW_NUMBER()
				   OVER(PARTITION BY [TrendlineId], [ExtremumGroupId], [DateIndex] ORDER BY [TrendlineId], [ExtremumGroupId], [DateIndex])
				   FROM #TrendlinesHits
				)
				DELETE FROM CTE WHERE RN > 1

			END


			--Clean up after looking for trend hits.
			BEGIN

				DROP TABLE #RsOpenTrendlinesForHits;
				DROP TABLE #RsExtremumGroups_Temp;
				DROP TABLE #RsTrendlinesWithCalculatingFields;
				DROP TABLE #RsTrendlinesForHitsMatching;
				DROP TABLE #RsTrendlineExtremumPossibleMatches;
				DROP TABLE #RsTrendlineMatchesWithPriceLevels;
				DROP TABLE #RsTrendlineMatchesWithModifiedPrices;

			END

		END

		IF (@displayRightSidePreviewTables = 1) SELECT '#TrendlinesHits', @j AS [iteration], * FROM #TrendlinesHits ORDER BY DateIndex DESC;


		--MATCH HITS INFO TO TRENDLINES TABLE
		BEGIN

			--Create temporary table with the latest trend hit for each trendline.
			SELECT
				th.[TrendlineId],
				MAX(th.[DateIndex]) AS [LastHit]
			INTO 
				#LatestTrendHits
			FROM
				#TrendlinesHits th
			GROUP BY
				th.[TrendlineId]

			--Update TrendHit pointers.
			UPDATE 
				tt
			SET 
				[HitIndex] = h.[LastHit]
			FROM
				#TrendlinesTemp tt
				LEFT JOIN #LatestTrendHits h
				ON tt.[Id] = h.[TrendlineId]
			WHERE
				tt.[~IsOpenFromRight] = 1 AND
				h.[LastHit] >= tt.[AnalysisStartPoint];

			--Clean up
			DROP TABLE #LatestTrendHits;

		END
	
		IF (@displayRightSidePreviewTables = 1) SELECT '#TrendlinesTemp.AfterHits', @j AS [iteration], * FROM #TrendlinesTemp --WHERE [Id] = 24019795;

			
		--PREPARE TRENDLINES TABLE FOR NEXT ANALYSIS ITERATION
		BEGIN

			--Inactivate trendlines with no Break and no Hit found
			UPDATE
				#TrendlinesTemp
			SET
				[~IsOpenFromRight] = 0,
				[EndDateIndex] = COALESCE([PrevHitIndex] + @trendlineStartOffset, [BaseStartIndex])
			WHERE
				[~IsOpenFromRight] = 1 AND
				[BreakIndex] IS NULL AND [HitIndex] IS NULL;

			--Inactivate trendlines with two breaks with no hit between
			UPDATE
				#TrendlinesTemp
			SET
				[~IsOpenFromRight] = 0,
				[EndDateIndex] = COALESCE([PrevHitIndex] + @trendlineStartOffset, [BaseStartIndex])
			WHERE
				[~IsOpenFromRight] = 1 AND
				[BreakIndex] IS NOT NULL AND [PrevBreakIndex] IS NOT NULL AND [HitIndex] IS NULL;

			--Inactivate if distance between hits is too large


			--Update indices of remaining trendlines for next analysis iteration.
			UPDATE
				#TrendlinesTemp
			SET 
				[LookForPeaks] = [LookForPeaks] * IIF([BreakIndex] IS NULL, 1, -1),
				[AnalysisStartPoint] = COALESCE(IIF([BreakIndex] IS NOT NULL, [BreakIndex] + 1, [HitIndex] + 1), 0),
				[PrevBreakIndex] = IIF([BreakIndex] IS NULL, [PrevBreakIndex], [BreakIndex]),
				[BreakIndex] = NULL,
				[PrevHitIndex] = IIF([HitIndex] IS NULL, [PrevHitIndex], [HitIndex]),
				[HitIndex] = NULL
			WHERE
				[~IsOpenFromRight] = 1;

		END
	
		IF (@displayRightSidePreviewTables = 1) SELECT '#TrendlinesTemp.BeforeNextIteration', @j AS [iteration], * FROM #TrendlinesTemp --WHERE [Id] = 24019795;
	


		--TODO: Check if not end of data.

		--CHECK IF THERE ARE STILL SOME OPEN TRENDLINES
		IF (SELECT COUNT(*) FROM #TrendlinesTemp WHERE [~IsOpenFromRight] = 1) = 0 BREAK;

		--INCREMENT ITERATION COUNTER
		SET @j = @j + 1;

	END
	

END



-- CLEAR RESULTS
BEGIN

	--Remove all breaks out of trendlines range.
	DELETE tb
	FROM
		#TrendlinesBreaks tb
		INNER JOIN #TrendlinesTemp tt
		ON tb.[TrendlineId] = tt.[Id]
	WHERE
		tb.[DateIndex] < tt.[StartDateIndex] OR
		tb.[DateIndex] > tt.[EndDateIndex];

	--Remove all hits out of trendlines range.
	DELETE th
	FROM
		#TrendlinesHits th
		INNER JOIN #TrendlinesTemp tt
		ON th.[TrendlineId] = tt.[Id]
	WHERE
		th.[DateIndex] < tt.[StartDateIndex] OR
		th.[DateIndex] > tt.[EndDateIndex];

END



-- EVALUTE TRENDLINES
BEGIN

	--Create table with ranges (Hit-Hit, Hit-Break or Break-Hit)
	BEGIN 
		
		--Create table containing all breaks and hits assigned to the proper trendline.
		SELECT
			*
		INTO
			#CombinedBreaksAndHits
		FROM
			(SELECT [Id], [TrendlineId], [DateIndex], 0 AS [IsHit]
			FROM #TrendlinesBreaks
			UNION ALL
			SELECT [Id], [TrendlineId], [DateIndex], 1 AS [IsHit]
			FROM #TrendlinesHits) a;

		--Create table like above, but without right border points
		SELECT
			cbh.*
		INTO
			#CombinedBreaksAndHitsExcludingEndPoint
		FROM
			#CombinedBreaksAndHits cbh
			LEFT JOIN #TrendlinesTemp tt
			ON cbh.[TrendlineId] = tt.[Id]
		WHERE
			cbh.[DateIndex] < tt.[EndDateIndex]

		--Create table containing all pairs hit/break-break/break.
		SELECT
			c1.[TrendlineId],
			c1.[Id] AS [BaseId], 
			c1.[IsHit] AS [BaseIsHit],
			c1.[DateIndex] AS [BaseDateIndex],
			c2.[DateIndex] AS [CounterDateIndex]
		INTO 
			#PossibleBreakHitPairs
		FROM
			#CombinedBreaksAndHitsExcludingEndPoint c1
			LEFT JOIN #CombinedBreaksAndHits c2
			ON 
				c1.[TrendlineId] = c2.[TrendlineId]
				AND c1.[DateIndex] < c2.[DateIndex];

		--For each point (Hit/Break) select only adjacent point.
		SELECT 
			bhp.[TrendlineId],
			bhp.[BaseId],
			bhp.[BaseIsHit],
			bhp.[BaseDateIndex],
			MIN(bhp.[CounterDateIndex]) AS [CounterDateIndex]
		INTO
			#TrendRanges_SimpleView
		FROM 
			#PossibleBreakHitPairs bhp
		GROUP BY
			bhp.[BaseId],
			bhp.[BaseIsHit],
			bhp.[BaseDateIndex],
			bhp.[TrendlineId];

		--Join it back with all data required for further analysis.
		INSERT INTO #TrendRanges([TrendlineId], [BaseId], [BaseIsHit], [BaseDateIndex], [CounterId], [CounterIsHit], [CounterDateIndex])
		SELECT 
			tr.[TrendlineId],
			tr.[BaseId],
			tr.[BaseIsHit],
			tr.[BaseDateIndex],
			cbh.[Id] AS [CounterId],
			cbh.[IsHit] AS [CounterIsHit],
			cbh.[DateIndex] AS [CounterDateIndex]
		FROM 
			#TrendRanges_SimpleView tr
			LEFT JOIN #CombinedBreaksAndHits cbh
			ON 
				tr.[TrendlineId] = cbh.[TrendlineId] 
				AND tr.[CounterDateIndex] = cbh.[DateIndex];

		--Clean up
		BEGIN
			
			DROP TABLE #TrendRanges_SimpleView;
			DROP TABLE #PossibleBreakHitPairs;
			DROP TABLE #CombinedBreaksAndHits;
			DROP TABLE #CombinedBreaksAndHitsExcludingEndPoint;

		END

	END
	
	--Append trendline type.	
	BEGIN
		
		--Append HitId
		SELECT
			tr.*, 
			IIF(tr.[BaseIsHit] = 1, tr.[BaseId], tr.[CounterId]) AS [HitId]
		INTO
			#TrendRangesWithHitId
		FROM
			#TrendRanges tr
			LEFT JOIN #TrendlinesHits th
			ON tr.[BaseId] = th.[Id];

		--Append ExtremumId
		SELECT
			tr.*, th.[ExtremumGroupId]
		INTO
			#TrendRangesWithExtremumGroupId
		FROM
			#TrendRangesWithHitId tr
			LEFT JOIN #TrendlinesHits th
			ON tr.[HitId] = th.[Id];

		--Append IsPeak info.
		SELECT
			trw.*, 
			IIF(eg.[IsPeak] = 1, 1, -1) AS [ExtremumIsPeak]
		INTO 
			#TrendRangesWithIsPeak
		FROM
			#TrendRangesWithExtremumGroupId trw
			LEFT JOIN #ExtremumGroups eg
			ON trw.[ExtremumGroupId] = eg.[Id];

		--Update base table.
		UPDATE tr
		SET [IsPeak] = trwip.[ExtremumIsPeak]
		FROM
			#TrendRanges tr
			LEFT JOIN #TrendRangesWithIsPeak trwip
			ON tr.[Id] = trwip.[Id];

		--Clean up
		BEGIN

			DROP TABLE #TrendRangesWithHitId;
			DROP TABLE #TrendRangesWithExtremumGroupId;
			DROP TABLE #TrendRangesWithIsPeak
		END

	END
	
	--Get proper set of quotes and trendline levels required for analysis.			
	BEGIN

		SELECT
			MIN(tr.[BaseDateIndex]) AS [Min],
			MAX(tr.[CounterDateIndex]) AS [Max]
		INTO #TrendRangesEvaluation_QuotesBorderPointsRs
		FROM
			#TrendRanges tr;

		SET @minQuoteIndex = (SELECT [Min] FROM #TrendRangesEvaluation_QuotesBorderPointsRs);
		SET @maxQuoteIndex = (SELECT [Max] FROM #TrendRangesEvaluation_QuotesBorderPointsRs);
		
		--Create table of quotes.
		SELECT 
			q.*, 
			IIF(q.[Close] > q.[Open], 1, 0) AS [IsBullish],
			IIF(q.[Close] < q.[Open], 1, 0) AS [IsBearish]
		INTO 
			#Quotes_TrendRangesEvaluation
		FROM #Quotes q
		WHERE q.[DateIndex] BETWEEN @minQuoteIndex AND @maxQuoteIndex;

		--Create table of trendline levels.
		SELECT
			t.[Id] AS [TrendlineId],
			q.[DateIndex],
			(t.[BaseLevel] + (q.[DateIndex] - t.[BaseStartIndex]) * t.[Slope]) AS [TrendlineLevel]
		INTO
			#TrendlinesLevels_TrendRangesEvaluation
		FROM
			#TrendlinesTemp t,
			#Quotes_TrendRangesEvaluation q
		WHERE
				q.[DateIndex] >= t.[StartDateIndex]
			AND q.[DateIndex] <= t.[EndDateIndex];

		DROP TABLE #TrendRangesEvaluation_QuotesBorderPointsRs;

	END

	--Append partial evaluations.
	BEGIN
		
		--Create table with all quotes matched to ranges.
		SELECT
			tr.*,
			q.[DateIndex],
			IIF(tr.[IsPeak] = 1, q.[High], q.[Low]) AS [ExtremumPrice],
			IIF(tr.[IsPeak] = q.[IsBullish], q.[Close], q.[Open]) AS [OpenClosePrice]
		INTO
			#TrendRangesQuotesMatched
		FROM
			#TrendRanges tr
			INNER JOIN #Quotes_TrendRangesEvaluation q
			ON q.[DateIndex] BETWEEN tr.[BaseDateIndex] + 1 AND tr.[CounterDateIndex] - IIF([CounterIsHit] = 1, 1, 2)
		ORDER BY [TrendlineId];

		--Create table above with trendline level appended.
		SELECT
			tr.*,
			tl.[TrendlineLevel],
			tr.[ExtremumPrice] * tr.[IsPeak] AS [ModifiedExtremumPrice],
			tr.[OpenClosePrice] * tr.[IsPeak] AS [ModifiedOpenClosePrice],
			tl.[TrendlineLevel] * tr.[IsPeak] AS [ModifiedTrendline]
		INTO 
			#TrendRangesQuotesTrendlineLevels
		FROM
			#TrendRangesQuotesMatched tr
			LEFT JOIN #TrendlinesLevels_TrendRangesEvaluation tl
			ON 
					tr.[TrendlineId] = tl.[TrendlineId] 
				AND tr.[DateIndex] = tl.[DateIndex];
			
		--[AVERAGE VARIATION]
		BEGIN

			--Calculate variation area for each range.
			SELECT
				tr.[Id],
				ABS(tr.[TrendlineLevel] - tr.[ExtremumPrice]) AS [Variation]
			INTO
				#TrendRangeVariations
			FROM
				#TrendRangesQuotesTrendlineLevels tr

			--Calculate variation area for each range.
			SELECT
				tv.[Id],
				COUNT(tv.[Variation]) AS [TotalCandles],
				SUM(tv.[Variation]) AS [TotalVariation]
			INTO
				#TrendRangesTotalVariations
			FROM
				#TrendRangeVariations tv
			GROUP BY
				tv.[Id];
			
			--Append average variation.
			SELECT
				trtv.[Id],
				trtv.[TotalCandles],
				trtv.[TotalVariation],
				trtv.[TotalVariation] / trtv.[TotalCandles] AS [AverageVariation]
			INTO
				#TrendRangesWithAverageVariation
			FROM
				#TrendRangesTotalVariations trtv

			--Calculate maximum variation by extremum price.
			SELECT
				tr.[Id],
				MAX(tr.[ModifiedTrendline] - tr.[ModifiedExtremumPrice]) AS [ExtremumVariation],
				MAX(tr.[ModifiedTrendline] - tr.[ModifiedOpenClosePrice]) AS [OpenCloseVariation]
			INTO
				#TrendRangesMaxVariations
			FROM
				#TrendRangesQuotesTrendlineLevels tr
			GROUP BY
				tr.[Id];

			--Clean up
			BEGIN

				DROP TABLE #TrendRangeVariations;
				DROP TABLE #TrendRangesTotalVariations

			END

		END

		--[TRENDLINE CROSSING]
		BEGIN
			
			SELECT
				tr.[Id],
				tr.[DateIndex],
				tr.[ExtremumPrice] * tr.[IsPeak] AS [ModifiedExtremumPrice],
				tr.[OpenClosePrice] * tr.[IsPeak] AS [ModifiedOpenClosePrice],
				IIF(tl.[TrendlineLevel] > 0, 1, -1) AS [TrendlineAboveZero],
				tl.[TrendlineLevel] * tr.[IsPeak] AS [ModifiedTrendlineLevel]
			INTO
				#TrendlinePriceComparison
			FROM
				#TrendRangesQuotesMatched tr
				LEFT JOIN #TrendlinesLevels_TrendRangesEvaluation tl
				ON tr.[TrendlineId] = tl.[TrendlineId] 
					AND tr.[DateIndex] = tl.[DateIndex]
			
			--Extremum crossing penalty points.
			BEGIN

				--Select quotes where trendline is broken by extremum price.
				SELECT
					tpc.[Id],
					tpc.[DateIndex],
					ABS(tpc.[ModifiedExtremumPrice] - tpc.[ModifiedTrendlineLevel]) AS [CrossRange]
				INTO
					#ExtremumPriceCrossingTrendline
				FROM
					#TrendlinePriceComparison tpc
				WHERE
					tpc.[ModifiedExtremumPrice] > tpc.[ModifiedTrendlineLevel]

				--Get statistics for extremum price crossing trendline.
				SELECT
					e.[Id],
					SUM([CrossRange]) AS CrossRangeSum,
					AVG([CrossRange]) AS CrossRangeAverage,
					STDEVP([CrossRange]) AS CrossRangeStandardDeviation,
					COUNT([CrossRange]) AS CrossRangeCounter
				INTO
					#ExtremumPriceCrossingTrendline_Stats
				FROM
					#ExtremumPriceCrossingTrendline e
				GROUP BY
					e.[Id];

				--Calculate penalty points for extremum price crossing trendline.
				SELECT
					e.[Id],
					e.[CrossRangeSum] + e.[CrossRangeAverage] + e.[CrossRangeStandardDeviation] AS [ExtremumPriceCrossPenaltyPoints],
					e.[CrossRangeCounter] AS [ExtremumPriceCrossCounter]
				INTO
					#ExtremumPriceCrossingPenaltyPoints
				FROM
					#ExtremumPriceCrossingTrendline_Stats e;


				--Clean up
				BEGIN
				
					DROP TABLE #ExtremumPriceCrossingTrendline;
					DROP TABLE #ExtremumPriceCrossingTrendline_Stats;

				END


			END

			--Close/Open crossing penalty points.
			BEGIN

				--Select quotes where trendline is broken by open/close price.
				SELECT
					tpc.[Id],
					tpc.[DateIndex],
					ABS(tpc.[ModifiedOpenClosePrice] - tpc.[ModifiedTrendlineLevel]) AS [CrossRange]
				INTO
					#OCPriceCrossingTrendline
				FROM
					#TrendlinePriceComparison tpc
				WHERE
					tpc.[ModifiedOpenClosePrice] > tpc.[ModifiedTrendlineLevel]

--select '#OCPriceCrossingTrendline', * from #OCPriceCrossingTrendline;

				--Get statistics for extremum price crossing trendline.
				SELECT
					e.[Id],
					SUM([CrossRange]) AS CrossRangeSum,
					AVG([CrossRange]) AS CrossRangeAverage,
					STDEVP([CrossRange]) AS CrossRangeStandardDeviation,
					COUNT([CrossRange]) AS CrossRangeCounter
				INTO
					#OCPriceCrossingTrendline_Stats
				FROM
					#OCPriceCrossingTrendline e
				GROUP BY
					e.[Id];

				--Calculate penalty points for extremum price crossing trendline.
				SELECT
					e.[Id],
					e.[CrossRangeSum] + e.[CrossRangeAverage] + e.[CrossRangeStandardDeviation] AS [OCPriceCrossPenaltyPoints],
					e.[CrossRangeCounter] AS [OCPriceCrossCounter]
				INTO
					#OCPriceCrossingPenaltyPoints
				FROM
					#OCPriceCrossingTrendline_Stats e;

				--Clean up
				BEGIN
				
					DROP TABLE #OCPriceCrossingTrendline;
					DROP TABLE #OCPriceCrossingTrendline_Stats;

				END
				

			END

		END

	END



	--[BREAK VALUES]
	BEGIN

		--Get proper set of quotes required for breaks evaluation.
		BEGIN

			SELECT
				MIN(tb.[DateIndex] - @quotesAnalyzedForBreakEvaluation) AS [Min],
				MAX(tb.[DateIndex]) AS [Max]
			INTO #BorderPointsForBreaksEvaluation
			FROM
				#TrendlinesBreaks tb;

			SET @minQuoteIndex = (SELECT [Min] FROM #BorderPointsForBreaksEvaluation);
			SET @maxQuoteIndex = (SELECT [Max] FROM #BorderPointsForBreaksEvaluation);
	
			SELECT
				*
			INTO 
				#Quotes_BreaksEvaluation
			FROM
				#Quotes q
			WHERE
				q.[DateIndex] BETWEEN @minQuoteIndex AND @maxQuoteIndex;

			DROP TABLE #BorderPointsForBreaksEvaluation;

		END
	
		--Get all required quotes matched with break.
		BEGIN

			SELECT
				tb.[Id] AS [BreakId],
				tb.[BreakFromAbove],
				tb.[DateIndex] AS [BreakIndex],
				q.[DateIndex] AS [PriceIndex],
				q.[Open],
				q.[Low],
				q.[High],
				q.[Close],
				q.[Volume],
				tl.[TrendlineLevel]
			INTO 
				#BreakEvaluationQuotesMatch
			FROM
				#TrendlinesBreaks tb
				LEFT JOIN #Quotes_BreaksEvaluation q
				ON tb.[DateIndex] - q.[DateIndex] BETWEEN 0 AND @quotesAnalyzedForBreakEvaluation
				LEFT JOIN #TrendlinesLevels_TrendRangesEvaluation tl
				ON tb.[TrendlineId] = tl.[TrendlineId] AND q.[DateIndex] = tl.[DateIndex];

		END

		
		--Check if there is price hole.
		BEGIN

			SELECT
				b.[BreakId],
				b.[Open] * b.[BreakFromAbove] AS [ModifiedOpen],
				b.[Low] * b.[BreakFromAbove] AS [ModifiedLow],
				b.[High] * b.[BreakFromAbove] AS [ModifiedHigh],
				b.[Close] * b.[BreakFromAbove] AS [ModifiedClose],
				IIF(b.[BreakFromAbove] = 1, b.[Low] * b.[BreakFromAbove], b.[High] * b.[BreakFromAbove]) AS [ModifiedProperExtremum],
				b.[TrendlineLevel] * b.[BreakFromAbove] AS [ModifiedTrendline]
			INTO
				#BreakEveModifiedPrices
			FROM
				#BreakEvaluationQuotesMatch b
			WHERE
				b.[BreakIndex] - b.[PriceIndex] = 1;

			SELECT
				b.*,
				IIF(b.[ModifiedProperExtremum] > b.[ModifiedTrendline], 1, 0) AS [FullHole],
				IIF(b.[ModifiedClose] > b.[ModifiedTrendline] AND b.[ModifiedOpen] > b.[ModifiedTrendline], 1, 0) AS [CandleHole],
				IIF(b.[ModifiedClose] > b.[ModifiedTrendline], 1, 0) AS [PartialHole]
			INTO
				#BreakPriceHolesInfo
			FROM
				#BreakEveModifiedPrices b;

			--Evaluates price holes.
			--SELECT
			--	b.*,
			--	b.[ModifiedProperExtremum] - b.[ModifiedTrendline] AS [ExtremumTrendlineDistance],
			--	b.[ModifiedOpen] - b.[ModifiedTrendline] AS [OpenTrendlineDistance],
			--	b.[ModifiedClose] - b.[ModifiedTrendline] AS [CloseTrendlineDistance]
			--FROM
			--	#BreakEveModifiedPrices b;

			--Clean up
			DROP TABLE #BreakPriceHolesInfo;
			DROP TABLE #BreakEveModifiedPrices;

		END


		--Calculate points for volume.
		BEGIN

			--Table with average volumes.
			SELECT
				b.[BreakId],
				AVG(b.[Volume]) AS [PreviousAverageVolume]
			INTO
				#QuotesBeforeBreakAverageVolume
			FROM
				#BreakEvaluationQuotesMatch b
			WHERE b.[BreakIndex] - b.[PriceIndex] BETWEEN 2 AND 5
			GROUP BY
				b.[BreakId];

			--Table with break moment volumes.
			SELECT
				b.[BreakId],
				b.[Volume] AS [BreakCandleVolume]
			INTO
				#VolumesAtBreakTime
			FROM
				#BreakEvaluationQuotesMatch b
			WHERE b.[BreakIndex] - b.[PriceIndex] = 1;
			
			--Table with average volumes.
			SELECT
				v.[BreakId],
				v.[BreakCandleVolume],
				a.[PreviousAverageVolume]
			INTO
				#BreakVolumesComparing
			FROM
				#VolumesAtBreakTime v
				LEFT JOIN #QuotesBeforeBreakAverageVolume a
				ON v.[BreakId] = a.[BreakId];

			--Clean up
			DROP TABLE #BreakEvaluationQuotesMatch;
			DROP TABLE #VolumesAtBreakTime;


		END


		SELECT
			tb.[Id],
			0.5 AS [Value]
		INTO
			#TrendlineBreaksEvaluation
		FROM
			#TrendlinesBreaks tb


		--Clean up
		BEGIN

			DROP TABLE #QuotesBeforeBreakAverageVolume;
			DROP TABLE #Quotes_BreaksEvaluation;
			DROP TABLE #BreakVolumesComparing;

		END

	END

	
	--[HITS VALUES]
	SELECT
		th.*,
		eg.[Value] AS [Value]
	INTO
		#TrendlineHitsValues
	FROM
		#TrendlinesHits th
		LEFT JOIN #ExtremumGroups eg
		ON th.[ExtremumGroupId] = eg.[Id];

	--Combine all evaluation data for trend ranges.
	SELECT
		tr.*,
		epcpp.[ExtremumPriceCrossPenaltyPoints],
		epcpp.[ExtremumPriceCrossCounter],
		tbe.[Value],
		ocpp.[OCPriceCrossPenaltyPoints],
		ocpp.[OCPriceCrossCounter],
		trwav.[TotalCandles],
		trwav.[AverageVariation],
		trmv.[ExtremumVariation],
		trmv.[OpenCloseVariation],
		COALESCE(thv1.[Value], -0.5) AS [BaseHitValue],
		COALESCE(thv2.[Value], -0.5) AS [CounterHitValue]
	INTO 
		#TrendRangesPartValues
	FROM
		#TrendRanges tr
		LEFT JOIN #ExtremumPriceCrossingPenaltyPoints epcpp ON tr.[Id] = epcpp.[Id]
		LEFT JOIN #TrendlineBreaksEvaluation tbe ON tr.[Id] = tbe.[Id]
		LEFT JOIN #OCPriceCrossingPenaltyPoints ocpp ON tr.[Id] = ocpp.[Id]
		LEFT JOIN #TrendRangesWithAverageVariation trwav ON tr.[Id] = trwav.[Id]
		LEFT JOIN #TrendRangesMaxVariations trmv ON tr.[Id] = trmv.[Id]
		LEFT JOIN #TrendlineHitsValues thv1 ON (tr.[BaseIsHit] = 1 AND tr.[BaseId] = thv1.[Id])
		LEFT JOIN #TrendlineHitsValues thv2 ON (tr.[CounterIsHit] = 1 AND tr.[CounterId] = thv2.[Id])


	--SELECT 'TrendlinesTemp', * FROM #TrendlinesTemp;
	--SELECT 'TrendlinesHits', * FROM #TrendlinesHits ORDER BY [TrendlineId] ASC;
	--SELECT * FROM #TrendRangesPartValues ORDER BY [BaseDateIndex] ASC;


	--Clean up
	BEGIN
		
		DROP TABLE #Quotes_TrendRangesEvaluation;
		DROP TABLE #TrendlinesLevels_TrendRangesEvaluation;
		DROP TABLE #TrendRangesQuotesMatched;
		DROP TABLE #TrendlinePriceComparison;
		DROP TABLE #TrendRangesQuotesTrendlineLevels
		--Trendline evaluation tables.
		DROP TABLE #ExtremumPriceCrossingPenaltyPoints;
		DROP TABLE #TrendlineBreaksEvaluation;
		DROP TABLE #OCPriceCrossingPenaltyPoints;
		DROP TABLE #TrendRangesWithAverageVariation;
		DROP TABLE #TrendRangesMaxVariations;
		--DROP TABLE #TrendRangesPartValues;
		DROP TABLE #TrendlineHitsValues;

	END

END




-----------------


-- POST-ANALYSIS QUERIES
BEGIN

	--Create table with counter of hits for each trendline.
	SELECT
		tt.[Id],
		COUNT(th.[TrendlineId]) AS [Counter]
	INTO
		#HitCounters
	FROM	
		#TrendlinesTemp tt
		LEFT JOIN #TrendlinesHits th
		ON tt.[Id] = th.[TrendlineId]
	GROUP BY
		tt.[Id];

	--Create table with IDs of trendlines to be removed (less than 3 hits, closed from right side)
	SELECT
		t.[Id]
	INTO
		#TrendlinesToBeRemoved
	FROM
		#TrendlinesTemp t
		LEFT JOIN #HitCounters hc
		ON t.[Id] = hc.[Id]
	WHERE
		hc.[Counter] <= 2 AND
		t.[~IsOpenFromRight] = 0;

	--Delete trendlines mentioned above.
	DELETE
	FROM
		[dbo].[trendlines]
	WHERE
		[Id] IN (SELECT [Id] FROM #TrendlinesToBeRemoved);

	DELETE
	FROM
		#TrendlinesTemp
	WHERE
		[Id] IN (SELECT [Id] FROM #TrendlinesToBeRemoved);
		
	-----------------

	UPDATE t 
	SET
		[StartDateIndex] = tt.[StartDateIndex],
		[EndDateIndex] = tt.[EndDateIndex],
		[~IsOpenFromLeft] = tt.[~IsOpenFromLeft],
		[~IsOpenFromRight] = tt.[~IsOpenFromRight],
		[ShowOnChart] = 1
	FROM	
		[dbo].[trendlines] t
		INNER JOIN #TrendlinesTemp tt
		ON t.[Id] = tt.[Id]

	-----------------

	--Save info about hits/breaks/ranges.

	INSERT INTO [dbo].[trendlinesBreaks]([TrendlineId], [DateIndex], [BreakFromAbove])
	SELECT
		[TrendlineId], [DateIndex], [BreakFromAbove]
	FROM
		#TrendlinesBreaks
	WHERE
		[TrendlineId] IN (SELECT [Id] FROM #TrendlinesTemp);

	INSERT INTO [dbo].[TrendlinesHits]([TrendlineId], [ExtremumGroupId], [DateIndex])
	SELECT
		[TrendlineId], [ExtremumGroupId], [DateIndex]
	FROM
		#TrendlinesHits
	WHERE
		[TrendlineId] IN (SELECT [Id] FROM #TrendlinesTemp);

	INSERT INTO [dbo].[TrendRanges](	
		[TrendlineId], 
		[BaseId],
		[BaseIsHit],
		[BaseDateIndex],
		[CounterId],
		[CounterIsHit],
		[CounterDateIndex],
		[IsPeak],
		[ExtremumPriceCrossPenaltyPoints],
		[ExtremumPriceCrossCounter],
		[OCPriceCrossPenaltyPoints], 
		[OCPriceCrossCounter], 
		[TotalCandles], 
		[AverageVariation], 
		[ExtremumVariation], 
		[OpenCloseVariation], 
		[BaseHitValue], 
		[CounterHitValue], 
		[Value])
	SELECT
		[TrendlineId], 
		[BaseId],
		[BaseIsHit],
		[BaseDateIndex],
		[CounterId],
		[CounterIsHit],
		[CounterDateIndex],
		[IsPeak],
		[ExtremumPriceCrossPenaltyPoints],
		[ExtremumPriceCrossCounter],
		[OCPriceCrossPenaltyPoints], 
		[OCPriceCrossCounter], 
		[TotalCandles], 
		[AverageVariation], 
		[ExtremumVariation], 
		[OpenCloseVariation], 
		[BaseHitValue], 
		[CounterHitValue], 
		[Value]
	FROM
		#TrendRangesPartValues
	WHERE
		[TrendlineId] IN (SELECT [Id] FROM #TrendlinesTemp);

	--SELECT 
	--	* 
	--FROM 
	--	[dbo].[trendlines]
	--WHERE
	--	[ShareId] = @shareId AND
	--	[BaseId] = @baseExtremumId AND
	--	[CounterId] = @counterExtremumId

END




-- CLEAN-UP
BEGIN

	DROP TABLE #TrendlinesBreaks;
	DROP TABLE #TrendlinesHits;
	DROP TABLE #Trendlines;
	DROP TABLE #TrendRanges;
	DROP TABLE #ExtremumGroups;
	DROP TABLE #Quotes;
	DROP TABLE #TrendlinesTemp;
	DROP TABLE #HitCounters;
	DROP TABLE #TrendlinesToBeRemoved;
	DROP TABLE #TrendRangesPartValues;

END


COMMIT TRANSACTION



-- [1] Break - each quote that have Close price and Open price above resistance line or below support line.
GO
USE [master]
GO
ALTER DATABASE [shares] SET  READ_WRITE 
GO
