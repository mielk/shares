USE [master]
GO
/****** Object:  Database [fx]    Script Date: 2018-08-25 14:47:14 ******/
CREATE DATABASE [fx]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'fx', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\fx.mdf' , SIZE = 365568KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'fx_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\fx_log.ldf' , SIZE = 517184KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 COLLATE SQL_Latin1_General_CP1_CI_AS
GO
ALTER DATABASE [fx] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [fx].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [fx] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [fx] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [fx] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [fx] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [fx] SET ARITHABORT OFF 
GO
ALTER DATABASE [fx] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [fx] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [fx] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [fx] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [fx] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [fx] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [fx] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [fx] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [fx] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [fx] SET  DISABLE_BROKER 
GO
ALTER DATABASE [fx] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [fx] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [fx] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [fx] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [fx] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [fx] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [fx] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [fx] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [fx] SET  MULTI_USER 
GO
ALTER DATABASE [fx] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [fx] SET DB_CHAINING OFF 
GO
ALTER DATABASE [fx] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [fx] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [fx] SET DELAYED_DURABILITY = DISABLED 
GO
USE [fx]
GO
/****** Object:  UserDefinedTableType [dbo].[DateIndexPrice]    Script Date: 2018-08-25 14:47:14 ******/
CREATE TYPE [dbo].[DateIndexPrice] AS TABLE(
	[DateIndex] [int] NOT NULL,
	[Price] [float] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[DateIndex] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[DatetimesTransferTable]    Script Date: 2018-08-25 14:47:14 ******/
CREATE TYPE [dbo].[DatetimesTransferTable] AS TABLE(
	[Date] [datetime] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[IdAndDateIndex]    Script Date: 2018-08-25 14:47:14 ******/
CREATE TYPE [dbo].[IdAndDateIndex] AS TABLE(
	[Id] [int] NOT NULL,
	[DateIndex] [int] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[ParentChildTimeframesTransferTable]    Script Date: 2018-08-25 14:47:14 ******/
CREATE TYPE [dbo].[ParentChildTimeframesTransferTable] AS TABLE(
	[ParentTimeframe] [int] NULL,
	[DateIndex] [int] NULL,
	[ChildDateIndex] [int] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[QuotesTransferTable]    Script Date: 2018-08-25 14:47:14 ******/
CREATE TYPE [dbo].[QuotesTransferTable] AS TABLE(
	[Date] [datetime] NULL,
	[Open] [float] NULL,
	[High] [float] NULL,
	[Low] [float] NULL,
	[Close] [float] NULL,
	[Volume] [int] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[SourceForEmaCalculation]    Script Date: 2018-08-25 14:47:14 ******/
CREATE TYPE [dbo].[SourceForEmaCalculation] AS TABLE(
	[DateIndex] [int] NOT NULL,
	[Close] [float] NULL,
	[Ema] [float] NULL,
	[Number] [int] NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[SourceForTrAvg]    Script Date: 2018-08-25 14:47:14 ******/
CREATE TYPE [dbo].[SourceForTrAvg] AS TABLE(
	[DateIndex] [int] NOT NULL,
	[Tr] [float] NULL,
	[Avg] [float] NULL,
	[Number] [int] NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[TimeframeDateIndexTransferTable]    Script Date: 2018-08-25 14:47:14 ******/
CREATE TYPE [dbo].[TimeframeDateIndexTransferTable] AS TABLE(
	[TimeframeId] [int] NULL,
	[DateIndex] [int] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[TrendRangeBasicData]    Script Date: 2018-08-25 14:47:14 ******/
CREATE TYPE [dbo].[TrendRangeBasicData] AS TABLE(
	[TrendRangeId] [int] NOT NULL,
	[TrendlineStartDateIndex] [int] NOT NULL,
	[TrendlineStartLevel] [float] NOT NULL,
	[TrendlineAngle] [float] NOT NULL,
	[StartIndex] [int] NOT NULL,
	[EndIndex] [int] NOT NULL,
	[IsPeak] [int] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[TrendRangeId] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedFunction [dbo].[GetAssetCalculatingTrendlineStepFactor]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetAssetCalculatingTrendlineStepFactor](@assetId AS INT)
RETURNS INT
AS
BEGIN
	DECLARE @stepPrecision AS FLOAT = 2;
	RETURN POWER(10, @stepPrecision);
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetDateForDateIndex]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetDateForDateIndex](@timeframeId AS INT, @dateIndex AS INT)
RETURNS DATETIME
AS
BEGIN
	DECLARE @dt AS DATETIME;
	SELECT @dt = [Date] FROM [dbo].[dates] WHERE [TimeframeId] = @timeframeId AND [DateIndex] = @dateIndex;
	RETURN @dt;
END

GO
/****** Object:  UserDefinedFunction [dbo].[GetExtremumCheckDistance]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetExtremumCheckDistance]()
RETURNS INT
AS
BEGIN
	DECLARE @value AS INT;
	SELECT @value = [SettingValue] FROM [dbo].[settingsNumeric] WHERE [SettingName] = 'ExtremumAnalysisCheckDistance';
	RETURN IIF(@value IS NULL, 0, @value);
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetExtremumMinDistance]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetExtremumMinDistance]()
RETURNS INT
AS
BEGIN
	DECLARE @value AS INT;
	SELECT @value = [SettingValue] FROM [dbo].[settingsNumeric] WHERE [SettingName] = 'ExtremumAnalysisMinDistance';
	RETURN IIF(@value IS NULL, 0, @value);
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetLastAdxAnalysisDate]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE FUNCTION [dbo].[GetLastAdxAnalysisDate](@assetId AS INT, @timeframeId AS INT)
	RETURNS INT
	AS
	BEGIN
		DECLARE @index AS INT;
		SELECT @index = [AdxLastAnalyzedIndex] FROM [dbo].[timestamps] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;
		RETURN IIF(@index IS NULL, 0, @index);
	END


GO
/****** Object:  UserDefinedFunction [dbo].[GetLastExtremumAnalysisDate]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--Creating types.
--CREATE TYPE [dbo].[DateIndexPrice] AS TABLE(
--	[DateIndex] [int] NOT NULL PRIMARY KEY CLUSTERED,
--	[Price] [float] NOT NULL
--);

--GO

--CREATE TYPE [dbo].[IdAndDateIndex] AS TABLE(
--	[Id] [int] NOT NULL PRIMARY KEY CLUSTERED,
--	[DateIndex] [int] NOT NULL
--);

--GO



--Creating functions.
CREATE FUNCTION [dbo].[GetLastExtremumAnalysisDate](@assetId AS INT, @timeframeId AS INT)
RETURNS INT
AS
BEGIN
	DECLARE @index AS INT;
	SELECT @index = [ExtremaLastAnalyzedIndex] FROM [dbo].[timestamps] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;
	RETURN IIF(@index IS NULL, 0, @index);
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetLastQuote]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE FUNCTION [dbo].[GetLastQuote](@assetId AS INT, @timeframeId AS INT)
	RETURNS INT
	AS
	BEGIN
		DECLARE @index AS INT;
		SELECT @index = MAX([DateIndex]) FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId AND [IsComplete] = 1;
		RETURN IIF(@index IS NULL, 0, @index);
	END


GO
/****** Object:  UserDefinedFunction [dbo].[GetTrendlineCheckDistance]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetTrendlineCheckDistance]()
RETURNS INT
AS
BEGIN
	DECLARE @value AS INT;
	SELECT @value = [SettingValue] FROM [dbo].[settingsNumeric] WHERE [SettingName] = 'TrendlineAnalysisCheckDistance';
	RETURN IIF(@value IS NULL, 0, @value);
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetTrendlinesAnalysisLastExtremumGroupId]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetTrendlinesAnalysisLastExtremumGroupId](@assetId AS INT, @timeframeId AS INT)
RETURNS INT
AS
BEGIN
	DECLARE @index AS INT;
	SELECT @index = [TrendlinesAnalysisLastExtremumGroupId] FROM [dbo].[timestamps] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;
	RETURN IIF(@index IS NULL, 0, @index);
END


GO
/****** Object:  UserDefinedFunction [dbo].[GetTrendlinesAnalysisLastQuotationIndex]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetTrendlinesAnalysisLastQuotationIndex](@assetId AS INT, @timeframeId AS INT)
RETURNS INT
AS
BEGIN
	DECLARE @index AS INT;
	SELECT @index = [TrendlinesAnalysisLastQuotationIndex] FROM [dbo].[timestamps] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;
	RETURN IIF(@index IS NULL, 0, @index);
END


GO
/****** Object:  UserDefinedFunction [dbo].[MaxValue]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--MAX.VALUE
CREATE FUNCTION [dbo].[MaxValue](@a AS FLOAT, @b AS FLOAT)
RETURNS FLOAT
AS
BEGIN
	RETURN IIF (@a > @b, @a, @b);
END


GO
/****** Object:  UserDefinedFunction [dbo].[MinValue]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--MIN.VALUE
CREATE FUNCTION [dbo].[MinValue](@a AS FLOAT, @b AS FLOAT)
RETURNS FLOAT
AS
BEGIN
	RETURN IIF (@a < @b, @a, @b);
END


GO
/****** Object:  Table [dbo].[assets]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[assets](
	[AssetId] [int] NOT NULL,
	[Uuid] [nvarchar](36) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [Default_Assets_Uuid]  DEFAULT (newid()),
	[BaseCurrencyId] [int] NOT NULL,
	[CounterCurrencyId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [Default_Assets_IsActive]  DEFAULT ((1)),
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Assets_CreatedDate]  DEFAULT (getdate()),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [Default_Assets_ModifiedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_assets] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[currencies]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[currencies](
	[CurrencyId] [int] NOT NULL,
	[Uuid] [nvarchar](36) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [Default_Currencies_Uuid]  DEFAULT (newid()),
	[Name] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Symbol] [nvarchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [Default_Currencies_IsActive]  DEFAULT ((1)),
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Currencies_CreatedDate]  DEFAULT (getdate()),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [Default_Currencies_ModifiedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_currencies] PRIMARY KEY CLUSTERED 
(
	[CurrencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[dates]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dates](
	[DateIndex] [int] NOT NULL CONSTRAINT [Default_Dates_DateIndex]  DEFAULT ((1)),
	[TimeframeId] [int] NOT NULL CONSTRAINT [Default_Dates_Tiemframe]  DEFAULT ((6)),
	[Date] [datetime] NOT NULL,
	[ParentLevelDateIndex] [int] NULL,
 CONSTRAINT [PK_dates] PRIMARY KEY CLUSTERED 
(
	[DateIndex] ASC,
	[TimeframeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[errorLogs]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[errorLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[Class] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Method] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[InputParams] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ErrNumber] [int] NOT NULL,
	[ErrDescription] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SqlString] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_errorLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[extrema]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[extrema](
	[ExtremumId] [int] IDENTITY(1,1) NOT NULL,
	[AssetId] [int] NOT NULL,
	[TimeframeId] [int] NOT NULL,
	[DateIndex] [int] NOT NULL,
	[ExtremumTypeId] [int] NOT NULL,
	[IsEvaluationOpen] [bit] NOT NULL CONSTRAINT [Default_Extrema_IsEvaluationOpen]  DEFAULT ((1)),
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
 CONSTRAINT [PK_extrema] PRIMARY KEY CLUSTERED 
(
	[ExtremumId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[extremumGroups]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
 CONSTRAINT [PK_extremaGroups] PRIMARY KEY CLUSTERED 
(
	[ExtremumGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[extremumTypes]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[extremumTypes](
	[ExtremumTypeId] [int] NOT NULL,
	[ExtremumTypeName] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IsPeak] [bit] NOT NULL,
 CONSTRAINT [PK_extremumTypes] PRIMARY KEY CLUSTERED 
(
	[ExtremumTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[predefinedNumbers]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[predefinedNumbers](
	[number] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[quotes]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[quotes](
	[AssetId] [int] NOT NULL,
	[TimeframeId] [int] NOT NULL,
	[DateIndex] [int] NOT NULL,
	[Open] [float] NOT NULL,
	[Low] [float] NOT NULL,
	[High] [float] NOT NULL,
	[Close] [float] NOT NULL,
	[Volume] [bigint] NOT NULL,
	[IsComplete] [bit] NOT NULL CONSTRAINT [Default_Quotes_IsComplete]  DEFAULT ((0)),
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Quotes_CreatedDate]  DEFAULT (getdate())
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[quotesOutOfDate]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[quotesOutOfDate](
	[AssetId] [int] NOT NULL,
	[TimeframeId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Open] [float] NOT NULL,
	[Low] [float] NOT NULL,
	[High] [float] NOT NULL,
	[Close] [float] NOT NULL,
	[Volume] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RawH1Data]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RawH1Data](
	[Date] [datetime] NOT NULL,
	[Open] [float] NOT NULL,
	[Low] [float] NOT NULL,
	[High] [float] NOT NULL,
	[Close] [float] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[settingsNumeric]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[settingsNumeric](
	[SettingName] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SettingValue] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[settingsText]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[settingsText](
	[SettingName] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SettingValue] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[timeframes]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[timeframes](
	[TimeframeId] [int] NOT NULL,
	[Name] [nvarchar](4) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_timeframes] PRIMARY KEY CLUSTERED 
(
	[TimeframeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[timestamps]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[timestamps](
	[AssetId] [int] NOT NULL,
	[TimeframeId] [int] NOT NULL,
	[ExtremaLastAnalyzedIndex] [int] NULL,
	[TrendlinesAnalysisLastQuotationIndex] [int] NULL,
	[TrendlinesAnalysisLastExtremumGroupId] [int] NULL,
	[AdxLastAnalyzedIndex] [int] NULL,
	[MacdLastAnalyzedIndex] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[trendBreaks]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trendBreaks](
	[TrendBreakId] [int] IDENTITY(1,1) NOT NULL,
	[TrendlineId] [int] NOT NULL,
	[DateIndex] [int] NOT NULL,
	[BreakFromAbove] [int] NOT NULL,
	[Value] [float] NULL,
 CONSTRAINT [PK_trendBreaks] PRIMARY KEY CLUSTERED 
(
	[TrendBreakId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[trendHits]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trendHits](
	[TrendHitId] [int] IDENTITY(1,1) NOT NULL,
	[TrendlineId] [int] NOT NULL,
	[ExtremumGroupId] [int] NOT NULL,
	[Value] [float] NULL,
 CONSTRAINT [PK_trendHits] PRIMARY KEY CLUSTERED 
(
	[TrendHitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[trendlines]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
	[IsOpenFromLeft] [bit] NOT NULL CONSTRAINT [Default_Trendlines_IsOpenFromLeft]  DEFAULT ((1)),
	[IsOpenFromRight] [bit] NOT NULL CONSTRAINT [Default_Trendlines_IsOpenFromRight]  DEFAULT ((1)),
	[CandlesDistance] [int] NOT NULL,
	[ShowOnChart] [bit] NOT NULL CONSTRAINT [Default_Trendlines_ShowOnChart]  DEFAULT ((0)),
	[Value] [float] NOT NULL CONSTRAINT [Default_Trendlines_Value]  DEFAULT ((0)),
 CONSTRAINT [PK_trendlines] PRIMARY KEY CLUSTERED 
(
	[TrendlineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[trendRanges]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trendRanges](
	[TrendRangeId] [int] IDENTITY(1,1) NOT NULL,
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
 CONSTRAINT [PK_trendRanges] PRIMARY KEY CLUSTERED 
(
	[TrendRangeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  UserDefinedFunction [dbo].[CalculateExtremaRightSideProperties]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[CalculateExtremaRightSideProperties](
	@isPeak AS INT, 
	@maxDistance AS INT, 
	@extrema AS [dbo].[IdAndDateIndex] READONLY,
	@basePrices AS [dbo].[DateIndexPrice] READONLY, 
	@oppositePrices AS [dbo].[DateIndexPrice] READONLY
)
RETURNS TABLE
AS
RETURN 
(

	SELECT
		withAmplitude.*,
		@isPeak * (withAmplitude.[Price] - delta1.[Price]) AS [Delta1],
		@isPeak * (withAmplitude.[Price] - delta2.[Price]) AS [Delta2],
		@isPeak * (withAmplitude.[Price] - delta3.[Price]) AS [Delta3],
		@isPeak * (withAmplitude.[Price] - delta5.[Price]) AS [Delta5],
		@isPeak * (withAmplitude.[Price] - delta10.[Price]) AS [Delta10]
	FROM
		(SELECT
				withCounter.*,
				@isPeak * (withCounter.[Price] - IIF(@isPeak = 1, MIN(op.[Price]), MAX(op.[Price]))) AS [LaterAmplitude],
				@isPeak * SUM(withCounter.[Price] - op.[Price]) AS [TotalArea]
			FROM
				(SELECT
						ex2.[Id] AS [ExtremumId], ex2.[DateIndex], ex2.[Price],
						COALESCE(MIN(bp3.[DateIndex]) - ex2.[DateIndex], ex2.[TotalLater]) AS [LaterCounter],
						IIF(MIN(bp3.[DateIndex]) IS NOT NULL OR ex2.[TotalLater] >= @maxDistance, 0, 1) AS [IsEvaluationOpen]
					FROM
						(SELECT
								ex.[Id], ex.[DateIndex], ex.[Price], 
								COUNT(bp2.[DateIndex]) AS [TotalLater]
							FROM
								(SELECT 
									e.[Id], bp.*
								FROM
									@extrema e
									LEFT JOIN @basePrices bp
									ON e.[DateIndex] = bp.[DateIndex]) ex
								LEFT JOIN @basePrices bp2
								ON (bp2.[DateIndex] - ex.[DateIndex]) BETWEEN 1 AND @maxDistance 
							GROUP BY ex.[Id], ex.[DateIndex], ex.[Price]) ex2
						LEFT JOIN @basePrices bp3
						ON (bp3.[DateIndex] - ex2.[DateIndex]) BETWEEN 1 AND @maxDistance 
							AND (@isPeak * bp3.[Price]) > (@isPeak * ex2.[Price])
					GROUP BY ex2.[Id], ex2.[DateIndex], ex2.[Price], ex2.[TotalLater]) withCounter
				LEFT JOIN @oppositePrices op
				ON (op.[DateIndex] - withCounter.[DateIndex]) BETWEEN 1 AND withCounter.[LaterCounter]
			GROUP BY
				withCounter.[ExtremumId], withCounter.[DateIndex], withCounter.[Price], withCounter.[LaterCounter], withCounter.[IsEvaluationOpen]) withAmplitude

		LEFT JOIN @basePrices delta1 ON (withAmplitude.[DateIndex] = delta1.[DateIndex] - 1)
		LEFT JOIN @basePrices delta2 ON (withAmplitude.[DateIndex] = delta2.[DateIndex] - 2)
		LEFT JOIN @basePrices delta3 ON (withAmplitude.[DateIndex] = delta3.[DateIndex] - 3)
		LEFT JOIN @basePrices delta5 ON (withAmplitude.[DateIndex] = delta5.[DateIndex] - 5)
		LEFT JOIN @basePrices delta10 ON (withAmplitude.[DateIndex] = delta10.[DateIndex] - 10)

);

GO
/****** Object:  UserDefinedFunction [dbo].[FindExtremaForSingleExtremumType]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FindExtremaForSingleExtremumType](
	@startIndex AS INT,
	@endIndex AS INT,
	@isPeak AS INT, 
	@minDistance AS INT, 
	@maxDistance AS INT, 
	@basePrices AS [dbo].[DateIndexPrice] READONLY, 
	@oppositePrices AS [dbo].[DateIndexPrice] READONLY
)
RETURNS TABLE
AS
RETURN 
(
	
	SELECT
		withAmplitude.*,
		@isPeak * (withAmplitude.[Price] - delta1.[Price]) AS [Delta1],
		@isPeak * (withAmplitude.[Price] - delta2.[Price]) AS [Delta2],
		@isPeak * (withAmplitude.[Price] - delta3.[Price]) AS [Delta3],
		@isPeak * (withAmplitude.[Price] - delta5.[Price]) AS [Delta5],
		@isPeak * (withAmplitude.[Price] - delta10.[Price]) AS [Delta10]
	FROM
		(SELECT
			withCounter.*,
			@isPeak * (withCounter.[Price] - IIF(@isPeak = 1, MIN(op.[Price]), MAX(op.[Price]))) AS [EarlierAmplitude],
			@isPeak * (SUM(withCounter.[Price] - op.[Price])) AS [TotalArea]
		FROM
			(SELECT
					extrema.[DateIndex],
					extrema.[Price],
					(extrema.[DateIndex] - MAX(c.[DateIndex]) - 1) AS [EarlierCounter]
				FROM

					(SELECT 
						leftFiltered.*
					FROM
						(SELECT
								bp.*
							FROM
								(SELECT * FROM @basePrices WHERE [DateIndex] BETWEEN @startIndex AND @endIndex) bp
								LEFT JOIN @basePrices cp
								ON (bp.[DateIndex] - cp.[DateIndex] BETWEEN 1 AND @minDistance) AND ((bp.[Price] * @isPeak) <= (cp.[Price] * @isPeak))
							WHERE cp.[DateIndex] IS NULL) leftFiltered
						LEFT JOIN @basePrices cp2
						ON (cp2.[DateIndex] - leftFiltered.[DateIndex] BETWEEN 1 AND @minDistance) AND ((leftFiltered.[Price] * @isPeak) < (cp2.[Price] * @isPeak))
						WHERE cp2.[DateIndex] IS NULL) extrema

					LEFT JOIN @basePrices c
					ON (extrema.[DateIndex] - c.[DateIndex]) BETWEEN 1 AND @maxDistance AND (@isPeak * extrema.[Price] < @isPeak * c.[Price])
				GROUP BY 
					extrema.[DateIndex], extrema.[Price]) withCounter
			LEFT JOIN @oppositePrices op
			ON  (withCounter.[DateIndex] - op.[DateIndex]) BETWEEN 1 AND COALESCE(withCounter.[EarlierCounter], withCounter.[DateIndex])

		GROUP BY
			withCounter.[DateIndex], withCounter.[EarlierCounter], withCounter.[Price]) withAmplitude

		LEFT JOIN @basePrices delta1 ON (withAmplitude.[DateIndex] = delta1.[DateIndex] + 1)
		LEFT JOIN @basePrices delta2 ON (withAmplitude.[DateIndex] = delta2.[DateIndex] + 2)
		LEFT JOIN @basePrices delta3 ON (withAmplitude.[DateIndex] = delta3.[DateIndex] + 3)
		LEFT JOIN @basePrices delta5 ON (withAmplitude.[DateIndex] = delta5.[DateIndex] + 5)
		LEFT JOIN @basePrices delta10 ON (withAmplitude.[DateIndex] = delta10.[DateIndex] + 10)

		
);

GO
/****** Object:  UserDefinedFunction [dbo].[GetExtremaWithEvaluationOpened]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetExtremaWithEvaluationOpened](@assetId AS INT, @timeframeId AS INT)
RETURNS TABLE
AS
RETURN
(
	SELECT 
		*
	FROM
		[dbo].[extrema]
	WHERE
		[AssetId] = @assetId AND
		[TimeframeId] = @timeframeId AND
		[IsEvaluationOpen] = 1
);


GO
/****** Object:  UserDefinedFunction [dbo].[GetLowerLevelIndices]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetLowerLevelIndices](@baseTimeframe AS INT, @timeframeIdIndices AS [dbo].[TimeframeDateIndexTransferTable] READONLY)
RETURNS TABLE
AS
RETURN 
(SELECT
	c.[TimeframeId],
	c.[DateIndex],
	d3.[DateIndex] AS [BaseTimeframeIndex]
FROM
	(SELECT DISTINCT
		b.*
	FROM
		@timeframeIdIndices a
		LEFT JOIN
			(SELECT
				d1.[TimeframeId], d1.[DateIndex], d1.[Date] AS [StartDate], d2.[Date] AS [EndDate]
			FROM
				[dbo].[dates] d1
				LEFT JOIN [dbo].[dates] d2
				ON d1.[TimeframeId] = d2.[TimeframeId] AND d1.[DateIndex] = d2.[DateIndex] - 1) b
		ON a.[TimeframeId] = b.[TimeframeId] AND a.[DateIndex] = b.[DateIndex]) c
	LEFT JOIN (SELECT * FROM [dbo].[dates] WHERE [TimeframeId] = @baseTimeframe) d3
	ON d3.[Date] >= c.[StartDate] AND d3.[Date] < c.[EndDate]);


GO
/****** Object:  UserDefinedFunction [dbo].[GetTimeframesIndices]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetTimeframesIndices](@dates AS [dbo].[DatetimesTransferTable] READONLY)
RETURNS TABLE
AS
RETURN 
(SELECT DISTINCT
	b.[TimeframeId], b.[DateIndex]
FROM
	@dates a
	LEFT JOIN
		(SELECT
			d1.[TimeframeId], d1.[DateIndex], d1.[Date] AS [StartDate], d2.[Date] AS [EndDate]
		FROM
			[dbo].[dates] d1
			LEFT JOIN [dbo].[dates] d2
			ON d1.[TimeframeId] = d2.[TimeframeId] AND d1.[DateIndex] = d2.[DateIndex] - 1) b
	ON a.[Date] >= [StartDate] AND a.[Date] < [EndDate]);


GO
/****** Object:  UserDefinedFunction [dbo].[GetTrendlineExtremaPairingPriceLevels]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetTrendlineExtremaPairingPriceLevels](@minPrice AS FLOAT, @maxPrice AS FLOAT, @stepFactor AS FLOAT)
RETURNS TABLE
AS
RETURN
(
	SELECT
		(pn.[number] / @stepFactor) AS [level]
	FROM
		[dbo].[predefinedNumbers] pn,
		(SELECT
			CEILING(@minPrice * @stepFactor) / @stepFactor AS [Min],
			FLOOR(@maxPrice * @stepFactor) / @stepFactor AS [Max]) pr
	WHERE
		pn.[number] BETWEEN (pr.[Min] * @stepFactor) AND (pr.[Max] * @stepFactor)
);


GO
/****** Object:  UserDefinedFunction [dbo].[GetTrendRangesCrossDetails]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[GetTrendRangesCrossDetails](
		@assetId AS INT,
		@timeframeId AS INT, 
		@basicData AS [dbo].[TrendRangeBasicData] READONLY
	)
RETURNS TABLE
AS
RETURN
(

	SELECT
		d.[TrendRangeId],
		d.[ExCrossRangeSum] + d.[ExCrossRangeAverage] + [ExCrossRangeStDeviation] AS [ExtremumPriceCrossPenaltyPoints],
		d.[ExCrossRangeCounter] AS [ExtremumPriceCrossCounter],
		d.[OcCrossRangeSum] + d.[OcCrossRangeAverage] + [OcCrossRangeStDeviation] AS [OCPriceCrossPenaltyPoints],
		d.[OcCrossRangeCounter] AS [OCPriceCrossCounter]
	FROM
		(SELECT
			c.[TrendRangeId],
			SUM([ExCrossRange]) AS ExCrossRangeSum,
			AVG([ExCrossRange]) AS ExCrossRangeAverage,
			STDEVP([ExCrossRange]) AS ExCrossRangeStDeviation,
			COUNT([ExCrossRange]) AS ExCrossRangeCounter,
			SUM([OcCrossRange]) AS OcCrossRangeSum,
			AVG([OcCrossRange]) AS OcCrossRangeAverage,
			STDEVP([OcCrossRange]) AS OcCrossRangeStDeviation,
			COUNT([OcCrossRange]) AS OcCrossRangeCounter
		FROM
			(SELECT
				b.[TrendRangeId],
				b.[DateIndex],
				IIF(b.[ModifiedExtremumPrice] > b.[ModifiedTrendlineLevel], b.[ModifiedExtremumPrice] - b.[ModifiedTrendlineLevel], NULL) AS [ExCrossRange],
				IIF(b.[ModifiedOpenClosePrice] > b.[ModifiedTrendlineLevel], b.[ModifiedOpenClosePrice] - b.[ModifiedTrendlineLevel], NULL) AS [OcCrossRange]
			FROM
				(SELECT
						a.*,
						a.[TrendlineLevel] * CAST(a.[IsPeak] AS FLOAT) AS [ModifiedTrendlineLevel],
						a.[ExtremumPrice] * CAST(a.[IsPeak] AS FLOAT) AS [ModifiedExtremumPrice],
						a.[OpenClosePrice] * CAST(a.[IsPeak] AS FLOAT) AS [ModifiedOpenClosePrice]
					FROM
						(SELECT
							bd.[TrendRangeId],
							tq.[DateIndex],
							CAST((tq.[DateIndex] - bd.[TrendlineStartDateIndex]) AS FLOAT) * bd.[TrendlineAngle] + bd.[TrendlineStartLevel] AS [TrendlineLevel],
							IIF(bd.[IsPeak] = 1, tq.[High], tq.[Low]) AS [ExtremumPrice],
							IIF(bd.[IsPeak] = tq.[IsBullish], tq.[Close], tq.[Open]) AS [OpenClosePrice],
							bd.[IsPeak]
						FROM
							@basicData bd
							LEFT JOIN (	SELECT *, IIF(q.[Close] > q.[Open], 1, -1) AS [IsBullish]
										FROM
											(SELECT * FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) q
											LEFT JOIN (SELECT MIN([StartIndex]) AS [Min], MAX([EndIndex]) AS [Max] FROM @basicData) qr
											ON q.[DateIndex] BETWEEN qr.[Min] AND qr.[Max]
										WHERE	
											[AssetId] = @assetId AND [TimeframeId] = @timeframeId) tq
							ON tq.[DateIndex] BETWEEN bd.[StartIndex] AND bd.[EndIndex]) a) b) c
			GROUP BY c.[TrendRangeId]) d
);


GO
/****** Object:  UserDefinedFunction [dbo].[GetTrendRangesVariations]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[GetTrendRangesVariations](
		@assetId AS INT,
		@timeframeId AS INT, 
		@basicData AS [dbo].[TrendRangeBasicData] READONLY
	)
RETURNS TABLE
AS
RETURN
(

		SELECT
			b.[TrendRangeId],
			COUNT(b.[HLVariation]) AS [TotalCandles],
			SUM(b.[HLVariation]) AS [TotalVariation],
			MAX(b.[HLVariation]) AS [ExtremumVariation],
			MAX(b.[OCVariation]) AS [OCVariation]
		FROM	
			(SELECT
				ptc.*,
				ABS(ptc.[ModifiedTrendlineLevel] - ptc.[ModifiedExtremumPrice]) AS [HLVariation],
				ABS(ptc.[ModifiedTrendlineLevel] - ptc.[ModifiedOpenClosePrice]) AS [OCVariation]
			FROM
				(SELECT
					a.*,
					a.[TrendlineLevel] * CAST(a.[IsPeak] AS FLOAT) AS [ModifiedTrendlineLevel],
					a.[ExtremumPrice] * CAST(a.[IsPeak] AS FLOAT) AS [ModifiedExtremumPrice],
					a.[OpenClosePrice] * CAST(a.[IsPeak] AS FLOAT) AS [ModifiedOpenClosePrice]
				FROM
					(SELECT
						bd.[TrendRangeId],
						CAST((tq.[DateIndex] - bd.[TrendlineStartDateIndex]) AS FLOAT) * bd.[TrendlineAngle] + bd.[TrendlineStartLevel] AS [TrendlineLevel],
						IIF(bd.[IsPeak] = 1, tq.[High], tq.[Low]) AS [ExtremumPrice],
						IIF(bd.[IsPeak] = tq.[IsBullish], tq.[Close], tq.[Open]) AS [OpenClosePrice],
						bd.[IsPeak]
					FROM
						@basicData bd
						LEFT JOIN (	SELECT *, IIF(q.[Close] > q.[Open], 1, -1) AS [IsBullish]
									FROM
										(SELECT * FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) q
										LEFT JOIN (SELECT MIN([StartIndex]) AS [Min], MAX([EndIndex]) AS [Max] FROM @basicData) qr
										ON q.[DateIndex] BETWEEN qr.[Min] AND qr.[Max]
									WHERE	
										[AssetId] = @assetId AND [TimeframeId] = @timeframeId) tq
						ON tq.[DateIndex] BETWEEN bd.[StartIndex] AND bd.[EndIndex]) a) ptc) b
		GROUP BY 
			b.[TrendRangeId]

);



GO
/****** Object:  View [dbo].[ViewDataInfo]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  View [dbo].[ViewQuotes]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Index [ixBaseCurrency_assets]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixBaseCurrency_assets] ON [dbo].[assets]
(
	[BaseCurrencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixCounterCurrency_assets]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixCounterCurrency_assets] ON [dbo].[assets]
(
	[CounterCurrencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixCurrencies_assets]    Script Date: 2018-08-25 14:47:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixCurrencies_assets] ON [dbo].[assets]
(
	[BaseCurrencyId] ASC,
	[CounterCurrencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [ixName_markets]    Script Date: 2018-08-25 14:47:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixName_markets] ON [dbo].[currencies]
(
	[Name] ASC
)
WHERE ([IsActive]=(1))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDate_dates]    Script Date: 2018-08-25 14:47:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixDate_dates] ON [dbo].[dates]
(
	[Date] ASC,
	[TimeframeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDateIndex_dates]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixDateIndex_dates] ON [dbo].[dates]
(
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDateIndexTimeframe_dates]    Script Date: 2018-08-25 14:47:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixDateIndexTimeframe_dates] ON [dbo].[dates]
(
	[DateIndex] ASC,
	[TimeframeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixParentDateIndex_dates]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixParentDateIndex_dates] ON [dbo].[dates]
(
	[ParentLevelDateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTimeframe_dates]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixTimeframe_dates] ON [dbo].[dates]
(
	[TimeframeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixAsset_extrema]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixAsset_extrema] ON [dbo].[extrema]
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDateIndex_extrema]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixDateIndex_extrema] ON [dbo].[extrema]
(
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixExtremumType_extrema]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixExtremumType_extrema] ON [dbo].[extrema]
(
	[ExtremumTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTimeframe_extrema]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixTimeframe_extrema] ON [dbo].[extrema]
(
	[TimeframeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixUniqueSet_extrema]    Script Date: 2018-08-25 14:47:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_extrema] ON [dbo].[extrema]
(
	[AssetId] ASC,
	[TimeframeId] ASC,
	[DateIndex] ASC,
	[ExtremumTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixAsset_extrema]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixAsset_extrema] ON [dbo].[extremumGroups]
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixEndDateIndex_extremumGroups]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixEndDateIndex_extremumGroups] ON [dbo].[extremumGroups]
(
	[EndDateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixIsPeak_extremumGroups]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixIsPeak_extremumGroups] ON [dbo].[extremumGroups]
(
	[IsPeak] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixMasterDateIndex_extremumGroups]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixMasterDateIndex_extremumGroups] ON [dbo].[extremumGroups]
(
	[MasterDateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixMasterExtremumId_extremumGroups]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixMasterExtremumId_extremumGroups] ON [dbo].[extremumGroups]
(
	[MasterExtremumId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixSlaveDateIndex_extremumGroups]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixSlaveDateIndex_extremumGroups] ON [dbo].[extremumGroups]
(
	[SlaveDateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixSlaveExtremumId_extremumGroups]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixSlaveExtremumId_extremumGroups] ON [dbo].[extremumGroups]
(
	[SlaveExtremumId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixStartDateIndex_extremumGroups]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixStartDateIndex_extremumGroups] ON [dbo].[extremumGroups]
(
	[StartDateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTimeframe_extrema]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixTimeframe_extrema] ON [dbo].[extremumGroups]
(
	[TimeframeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixNumber_predefinedNumbers]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixNumber_predefinedNumbers] ON [dbo].[predefinedNumbers]
(
	[number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixAssetId_Quotes]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixAssetId_Quotes] ON [dbo].[quotes]
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixAssetTimeframeDateIndex_Quotes]    Script Date: 2018-08-25 14:47:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixAssetTimeframeDateIndex_Quotes] ON [dbo].[quotes]
(
	[AssetId] ASC,
	[TimeframeId] ASC,
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDateIndex_Quotes]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixDateIndex_Quotes] ON [dbo].[quotes]
(
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTimeframe_Quotes]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixTimeframe_Quotes] ON [dbo].[quotes]
(
	[TimeframeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixAssetId_QuotesOutOfDate]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixAssetId_QuotesOutOfDate] ON [dbo].[quotesOutOfDate]
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDateIndex_QuotesOutOfDate]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixDateIndex_QuotesOutOfDate] ON [dbo].[quotesOutOfDate]
(
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTimeframe_QuotesOutOfDate]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixTimeframe_QuotesOutOfDate] ON [dbo].[quotesOutOfDate]
(
	[TimeframeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDate_Raw1HData]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixDate_Raw1HData] ON [dbo].[RawH1Data]
(
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [ixName_SettingsNumeric]    Script Date: 2018-08-25 14:47:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixName_SettingsNumeric] ON [dbo].[settingsNumeric]
(
	[SettingName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [ixName_SettingText]    Script Date: 2018-08-25 14:47:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixName_SettingText] ON [dbo].[settingsText]
(
	[SettingName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixAssetId_Timestamps]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixAssetId_Timestamps] ON [dbo].[timestamps]
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixAssetTimeframe_Timestamps]    Script Date: 2018-08-25 14:47:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixAssetTimeframe_Timestamps] ON [dbo].[timestamps]
(
	[AssetId] ASC,
	[TimeframeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTimeframe_Timestamps]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixTimeframe_Timestamps] ON [dbo].[timestamps]
(
	[TimeframeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDateIndex_trendlinesBreaks]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixDateIndex_trendlinesBreaks] ON [dbo].[trendBreaks]
(
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTrendlineId_trendlinesBreaks]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixTrendlineId_trendlinesBreaks] ON [dbo].[trendBreaks]
(
	[TrendlineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixUniqueSet_trendBreaks]    Script Date: 2018-08-25 14:47:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_trendBreaks] ON [dbo].[trendBreaks]
(
	[TrendlineId] ASC,
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixExtremumGroup_trendHits]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixExtremumGroup_trendHits] ON [dbo].[trendHits]
(
	[ExtremumGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTrendlineId_trendHits]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixTrendlineId_trendHits] ON [dbo].[trendHits]
(
	[TrendlineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixUniqueSet_trendHits]    Script Date: 2018-08-25 14:47:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_trendHits] ON [dbo].[trendHits]
(
	[TrendlineId] ASC,
	[ExtremumGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixAsset_trendlines]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixAsset_trendlines] ON [dbo].[trendlines]
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixBaseDateIndex_trendlines]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixBaseDateIndex_trendlines] ON [dbo].[trendlines]
(
	[BaseDateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixCounterDateIndex_trendlines]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixCounterDateIndex_trendlines] ON [dbo].[trendlines]
(
	[CounterDateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixIsOpenFromLeft_trendlines]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixIsOpenFromLeft_trendlines] ON [dbo].[trendlines]
(
	[IsOpenFromLeft] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixIsOpenFromRight_trendlines]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixIsOpenFromRight_trendlines] ON [dbo].[trendlines]
(
	[IsOpenFromRight] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTimeframe_trendlines]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixTimeframe_trendlines] ON [dbo].[trendlines]
(
	[TimeframeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixUniqueSet_trendlines]    Script Date: 2018-08-25 14:47:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_trendlines] ON [dbo].[trendlines]
(
	[AssetId] ASC,
	[TimeframeId] ASC,
	[BaseExtremumGroupId] ASC,
	[BaseLevel] ASC,
	[CounterExtremumGroupId] ASC,
	[CounterLevel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixBaseDateIndex_trendRanges]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixBaseDateIndex_trendRanges] ON [dbo].[trendRanges]
(
	[BaseDateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixBaseId_trendRanges]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixBaseId_trendRanges] ON [dbo].[trendRanges]
(
	[BaseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixCounterDateIndex_trendRanges]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixCounterDateIndex_trendRanges] ON [dbo].[trendRanges]
(
	[CounterDateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixCounterId_trendRanges]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixCounterId_trendRanges] ON [dbo].[trendRanges]
(
	[CounterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTrendlineId_trendRanges]    Script Date: 2018-08-25 14:47:14 ******/
CREATE NONCLUSTERED INDEX [ixTrendlineId_trendRanges] ON [dbo].[trendRanges]
(
	[TrendlineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixUniqueSet_trendRanges]    Script Date: 2018-08-25 14:47:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_trendRanges] ON [dbo].[trendRanges]
(
	[TrendlineId] ASC,
	[BaseId] ASC,
	[CounterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[errorLogs] ADD  CONSTRAINT [Default_ErrorLogs_Timestamp]  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[quotesOutOfDate] ADD  CONSTRAINT [Default_QuotesOutOfDate_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[assets]  WITH CHECK ADD  CONSTRAINT [FK_Assets_BaseCurrency] FOREIGN KEY([BaseCurrencyId])
REFERENCES [dbo].[currencies] ([CurrencyId])
GO
ALTER TABLE [dbo].[assets] CHECK CONSTRAINT [FK_Assets_BaseCurrency]
GO
ALTER TABLE [dbo].[assets]  WITH CHECK ADD  CONSTRAINT [FK_Assets_CounterCurrency] FOREIGN KEY([CounterCurrencyId])
REFERENCES [dbo].[currencies] ([CurrencyId])
GO
ALTER TABLE [dbo].[assets] CHECK CONSTRAINT [FK_Assets_CounterCurrency]
GO
ALTER TABLE [dbo].[dates]  WITH CHECK ADD  CONSTRAINT [FK_dates_timeframe] FOREIGN KEY([TimeframeId])
REFERENCES [dbo].[timeframes] ([TimeframeId])
GO
ALTER TABLE [dbo].[dates] CHECK CONSTRAINT [FK_dates_timeframe]
GO
ALTER TABLE [dbo].[extrema]  WITH CHECK ADD  CONSTRAINT [FK_Extrema_AssetId] FOREIGN KEY([AssetId])
REFERENCES [dbo].[assets] ([AssetId])
GO
ALTER TABLE [dbo].[extrema] CHECK CONSTRAINT [FK_Extrema_AssetId]
GO
ALTER TABLE [dbo].[extrema]  WITH CHECK ADD  CONSTRAINT [FK_Extrema_DateIndex] FOREIGN KEY([DateIndex], [TimeframeId])
REFERENCES [dbo].[dates] ([DateIndex], [TimeframeId])
GO
ALTER TABLE [dbo].[extrema] CHECK CONSTRAINT [FK_Extrema_DateIndex]
GO
ALTER TABLE [dbo].[extrema]  WITH CHECK ADD  CONSTRAINT [FK_Extrema_ExtremumTypeId] FOREIGN KEY([ExtremumTypeId])
REFERENCES [dbo].[extremumTypes] ([ExtremumTypeId])
GO
ALTER TABLE [dbo].[extrema] CHECK CONSTRAINT [FK_Extrema_ExtremumTypeId]
GO
ALTER TABLE [dbo].[extremumGroups]  WITH CHECK ADD  CONSTRAINT [FK_ExtremumGroups_AssetId] FOREIGN KEY([AssetId])
REFERENCES [dbo].[assets] ([AssetId])
GO
ALTER TABLE [dbo].[extremumGroups] CHECK CONSTRAINT [FK_ExtremumGroups_AssetId]
GO
ALTER TABLE [dbo].[extremumGroups]  WITH CHECK ADD  CONSTRAINT [FK_ExtremumGroups_MasterExtremum] FOREIGN KEY([MasterExtremumId])
REFERENCES [dbo].[extrema] ([ExtremumId])
GO
ALTER TABLE [dbo].[extremumGroups] CHECK CONSTRAINT [FK_ExtremumGroups_MasterExtremum]
GO
ALTER TABLE [dbo].[extremumGroups]  WITH CHECK ADD  CONSTRAINT [FK_ExtremumGroups_SlaveExtremum] FOREIGN KEY([SlaveExtremumId])
REFERENCES [dbo].[extrema] ([ExtremumId])
GO
ALTER TABLE [dbo].[extremumGroups] CHECK CONSTRAINT [FK_ExtremumGroups_SlaveExtremum]
GO
ALTER TABLE [dbo].[extremumGroups]  WITH CHECK ADD  CONSTRAINT [FK_ExtremumGroups_TimeframeId] FOREIGN KEY([TimeframeId])
REFERENCES [dbo].[timeframes] ([TimeframeId])
GO
ALTER TABLE [dbo].[extremumGroups] CHECK CONSTRAINT [FK_ExtremumGroups_TimeframeId]
GO
ALTER TABLE [dbo].[quotes]  WITH CHECK ADD  CONSTRAINT [FK_Quotes_AssetId] FOREIGN KEY([AssetId])
REFERENCES [dbo].[assets] ([AssetId])
GO
ALTER TABLE [dbo].[quotes] CHECK CONSTRAINT [FK_Quotes_AssetId]
GO
ALTER TABLE [dbo].[quotes]  WITH CHECK ADD  CONSTRAINT [FK_Quotes_DateIndex] FOREIGN KEY([DateIndex], [TimeframeId])
REFERENCES [dbo].[dates] ([DateIndex], [TimeframeId])
GO
ALTER TABLE [dbo].[quotes] CHECK CONSTRAINT [FK_Quotes_DateIndex]
GO
ALTER TABLE [dbo].[timestamps]  WITH CHECK ADD  CONSTRAINT [FK_Timestamps_AssetId] FOREIGN KEY([AssetId])
REFERENCES [dbo].[assets] ([AssetId])
GO
ALTER TABLE [dbo].[timestamps] CHECK CONSTRAINT [FK_Timestamps_AssetId]
GO
ALTER TABLE [dbo].[timestamps]  WITH CHECK ADD  CONSTRAINT [FK_Timestamps_Timeframe] FOREIGN KEY([TimeframeId])
REFERENCES [dbo].[timeframes] ([TimeframeId])
GO
ALTER TABLE [dbo].[timestamps] CHECK CONSTRAINT [FK_Timestamps_Timeframe]
GO
ALTER TABLE [dbo].[trendBreaks]  WITH CHECK ADD  CONSTRAINT [FK_TrendBreaks_TrendlineId] FOREIGN KEY([TrendlineId])
REFERENCES [dbo].[trendlines] ([TrendlineId])
GO
ALTER TABLE [dbo].[trendBreaks] CHECK CONSTRAINT [FK_TrendBreaks_TrendlineId]
GO
ALTER TABLE [dbo].[trendHits]  WITH CHECK ADD  CONSTRAINT [FK_TrendHits_ExtremumGroupId] FOREIGN KEY([ExtremumGroupId])
REFERENCES [dbo].[extremumGroups] ([ExtremumGroupId])
GO
ALTER TABLE [dbo].[trendHits] CHECK CONSTRAINT [FK_TrendHits_ExtremumGroupId]
GO
ALTER TABLE [dbo].[trendHits]  WITH CHECK ADD  CONSTRAINT [FK_TrendHits_TrendlineId] FOREIGN KEY([TrendlineId])
REFERENCES [dbo].[trendlines] ([TrendlineId])
GO
ALTER TABLE [dbo].[trendHits] CHECK CONSTRAINT [FK_TrendHits_TrendlineId]
GO
ALTER TABLE [dbo].[trendlines]  WITH CHECK ADD  CONSTRAINT [FK_Trendlines_AssetId] FOREIGN KEY([AssetId])
REFERENCES [dbo].[assets] ([AssetId])
GO
ALTER TABLE [dbo].[trendlines] CHECK CONSTRAINT [FK_Trendlines_AssetId]
GO
ALTER TABLE [dbo].[trendlines]  WITH CHECK ADD  CONSTRAINT [FK_Trendlines_BaseExtremumGroup] FOREIGN KEY([BaseExtremumGroupId])
REFERENCES [dbo].[extremumGroups] ([ExtremumGroupId])
GO
ALTER TABLE [dbo].[trendlines] CHECK CONSTRAINT [FK_Trendlines_BaseExtremumGroup]
GO
ALTER TABLE [dbo].[trendlines]  WITH CHECK ADD  CONSTRAINT [FK_Trendlines_CounterExtremumGroup] FOREIGN KEY([CounterExtremumGroupId])
REFERENCES [dbo].[extremumGroups] ([ExtremumGroupId])
GO
ALTER TABLE [dbo].[trendlines] CHECK CONSTRAINT [FK_Trendlines_CounterExtremumGroup]
GO
ALTER TABLE [dbo].[trendlines]  WITH CHECK ADD  CONSTRAINT [FK_Trendlines_Timeframe] FOREIGN KEY([TimeframeId])
REFERENCES [dbo].[timeframes] ([TimeframeId])
GO
ALTER TABLE [dbo].[trendlines] CHECK CONSTRAINT [FK_Trendlines_Timeframe]
GO
ALTER TABLE [dbo].[trendRanges]  WITH CHECK ADD  CONSTRAINT [FK_TrendRanges_TrendlineId] FOREIGN KEY([TrendlineId])
REFERENCES [dbo].[trendlines] ([TrendlineId])
GO
ALTER TABLE [dbo].[trendRanges] CHECK CONSTRAINT [FK_TrendRanges_TrendlineId]
GO
ALTER TABLE [dbo].[assets]  WITH CHECK ADD  CONSTRAINT [CH_assetUuid_length] CHECK  ((len([Uuid])=(36)))
GO
ALTER TABLE [dbo].[assets] CHECK CONSTRAINT [CH_assetUuid_length]
GO
ALTER TABLE [dbo].[currencies]  WITH CHECK ADD  CONSTRAINT [CH_currencyUuid_length] CHECK  ((len([Uuid])=(36)))
GO
ALTER TABLE [dbo].[currencies] CHECK CONSTRAINT [CH_currencyUuid_length]
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
/****** Object:  StoredProcedure [dbo].[addNewQuote]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[addNewQuote] @assetId AS INT, @timeframeId AS INT, @quotes AS [dbo].[QuotesTransferTable] READONLY
AS
BEGIN
	
	--Insert data into [dbo].[quotes]
	BEGIN
		
		--Create temporary table with DateIndex instead of date.
		SELECT
			@assetId AS [AssetId],
			@timeframeId AS [TimeframeId],
			d.[DateIndex],
			q.[Date],
			q.[Open],
			q.[High],
			q.[Low],
			q.[Close],
			q.[Volume],
			1 AS [IsComplete]
		INTO
			#Quotes
		FROM
			@quotes q
			LEFT JOIN (SELECT * FROM [dbo].[dates]) d
			ON @timeframeId = d.[TimeframeId] AND q.[Date] = d.[Date];

		--Insert data from the table above into [dbo].[quotes]
		INSERT INTO [dbo].[quotes]([AssetId], [TimeframeId], [DateIndex], [Open], [High], [Low], [Close], [Volume], [IsComplete])
		SELECT [AssetId], [TimeframeId], [DateIndex], [Open], [High], [Low], [Close], [Volume], [IsComplete] FROM #Quotes WHERE [DateIndex] IS NOT NULL;
			
		--Insert data with missing DateIndex into [dbo].[quotesOutOfDate]
		INSERT INTO [dbo].[quotesOutOfDate]([AssetId], [TimeframeId], [Date], [Open], [High], [Low], [Close], [Volume])
		SELECT [AssetId], [TimeframeId], [Date], [Open], [High], [Low], [Close], [Volume] FROM #Quotes WHERE [DateIndex] IS NULL;

	END

	--Insert missing quotations.
	EXEC [dbo].[insertMissingQuotations] @assetId = @assetId, @timeframeId = @timeframeId;

	--Insert quotations for higher timeframe levels.
	BEGIN
		
		DECLARE @dates AS [dbo].[DatetimesTransferTable];
		DECLARE @timeframeIdIndices AS [dbo].[TimeframeDateIndexTransferTable];
		DECLARE @timeframeIdsMapping AS [dbo].[ParentChildTimeframesTransferTable];

		INSERT INTO @dates SELECT [Date] FROM @quotes;
		INSERT INTO @timeframeIdIndices SELECT * FROM [dbo].[GetTimeframesIndices](@dates) WHERE [TimeframeId] > @timeframeId;	
		INSERT INTO @timeframeIdsMapping SELECT * FROM [dbo].[GetLowerLevelIndices](@timeframeId, @timeframeIdIndices);

		--Join timeframe mapping fetched in previous step with quotes for base timeframe.
		SELECT
			tm.*, q.[Open], q.[High], q.[Low], q.[Close], q.[Volume]
		INTO 
			#JoinTimeframeMappingWithBaseTimeframeQuotes
		FROM
			@timeframeIdsMapping tm
			LEFT JOIN (SELECT * FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) q
			ON tm.[ChildDateIndex] = q.[DateIndex]
		WHERE
			q.[Close] IS NOT NULL;

		--Calculate candles for higher level timeframes.
		BEGIN
			SELECT 
				a.[ParentTimeframe], a.[DateIndex], b.[Open], a.[High], a.[Low], c.[Close], a.[Volume]
			INTO
				#HigherLevelQuotes
			FROM
				(	SELECT q.[ParentTimeframe], q.[DateIndex], MIN(q.[ChildDateIndex]) AS [MinIndex], MAX(q.[ChildDateIndex]) AS [MaxIndex], MIN(q.[Low]) AS [Low], MAX(q.[High]) AS [High], SUM(q.[Volume]) AS [Volume]
					FROM #JoinTimeframeMappingWithBaseTimeframeQuotes q
					GROUP BY q.[ParentTimeframe], q.[DateIndex]) a
				LEFT JOIN #JoinTimeframeMappingWithBaseTimeframeQuotes b ON a.[ParentTimeframe] = b.[ParentTimeframe] AND a.[DateIndex] = b.[DateIndex] AND a.[MinIndex] = b.[ChildDateIndex]
				LEFT JOIN #JoinTimeframeMappingWithBaseTimeframeQuotes c ON a.[ParentTimeframe] = c.[ParentTimeframe] AND a.[DateIndex] = c.[DateIndex] AND a.[MaxIndex] = c.[ChildDateIndex];

			DROP TABLE #JoinTimeframeMappingWithBaseTimeframeQuotes;

		END

		--Insert high-level candles fetched in the previous step to the [quotes] table (if they already exists, override them).
		BEGIN

			DELETE q 
			FROM [dbo].[quotes] q 
			WHERE EXISTS
			(
			   SELECT 1 FROM #HigherLevelQuotes hlq 
			   WHERE q.[AssetId] = @assetId AND q.[TimeframeId] = hlq.[ParentTimeframe] AND q.[DateIndex] = hlq.[DateIndex]
			);

			INSERT INTO [dbo].[quotes]([AssetId], [TimeframeId], [DateIndex], [Open], [High], [Low], [Close], [Volume])
			SELECT @assetId AS [AssetId], hlv.* FROM  #HigherLevelQuotes hlv;

		END

		--Updating [IsComplete] field.
		BEGIN

			-- Temporary table for better performance.
			SELECT * INTO #TempQuotes FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;

			-- Select the last base timeframe quotation.
			DECLARE @lastIndex AS INT = (SELECT MAX([DateIndex]) FROM #TempQuotes);
			DECLARE @dt AS DATETIME = [dbo].[GetDateForDateIndex](@timeframeId, @lastIndex);
			
			--Fetch the higher level date index for the highest quote from the base timeframe.
			DELETE FROM @dates;
			DELETE FROM @timeframeIdIndices;

			INSERT INTO @dates SELECT @dt;
			INSERT INTO @timeframeIdIndices SELECT * FROM [dbo].[GetTimeframesIndices](@dates) WHERE [TimeframeId] > @timeframeId;

			--Create temporary table with all higher level quotes match with the proper record from @timeframeIdIndices fetched above.
			SELECT
				q.*,
				IIF(ti.[TimeframeId] IS NULL, 0, 1) AS [IsCovered]
			INTO
				#HigherLevelTimeframesWithCoverageJoined
			FROM
				(SELECT * FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] > @timeframeId) q
				LEFT JOIN @timeframeIdIndices ti
				ON q.[AssetId] = @assetId AND q.[TimeframeId] = ti.[TimeframeId] AND q.[DateIndex] < ti.[DateIndex];

			--Update [IsComplete] field in [quotes] table.
			UPDATE q
			SET [IsComplete] = hlt.[IsCovered]
			FROM
				[dbo].[quotes] q
				LEFT JOIN #HigherLevelTimeframesWithCoverageJoined hlt
				ON q.[AssetId] = hlt.[AssetId] AND q.[TimeframeId] = hlt.[TimeframeId] AND q.[DateIndex] = hlt.[DateIndex]
			WHERE
				q.[AssetId] = @assetId AND q.[TimeframeId] > @timeframeId;

			DROP TABLE #HigherLevelTimeframesWithCoverageJoined;

		END

	END

	--Clean up temporary tables
	BEGIN
		DROP TABLE #Quotes;
		DROP TABLE #HigherLevelQuotes;
	END

END

	


GO
/****** Object:  StoredProcedure [dbo].[analyzeExtrema]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[analyzeExtrema] @assetId AS INT, @timeframeId AS INT
AS
BEGIN

	DECLARE @lastAnalyzedIndex AS INT = [dbo].[GetLastExtremumAnalysisDate](@assetId, @timeframeId);
	DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);
	DECLARE @maxDistance AS INT = [dbo].[GetExtremumCheckDistance]();
	DECLARE @minExtremumValue AS FLOAT = 50;

	IF (@lastQuote > @lastAnalyzedIndex)
	BEGIN
		
		--Create temporary table with all evaluation-opened extrema.
		SELECT 
			[ExtremumId], [DateIndex], [ExtremumTypeId]
		INTO 
			#EvaluationOpenedExtrema
		FROM 
			[dbo].[GetExtremaWithEvaluationOpened](@assetId, @timeframeId);

		--Get minimum amount of data required to look for extrema (later than minimum DateIndex in evaluation-opened extrema table).
		SELECT
			q.[DateIndex],
			IIF(q.[Close] > q.[Open], q.[Close], q.[Open]) AS [MaxOC],
			IIF(q.[Close] < q.[Open], q.[Close], q.[Open]) AS [MinOC],
			q.[High],
			q.[Low]
		INTO 
			#QuotesForComparing
		FROM
			[dbo].[quotes] q
		WHERE
			q.[AssetId] = @assetId AND
			q.[TimeframeId] = @timeframeId AND
			q.[DateIndex] >= (SELECT MIN([DateIndex]) FROM #EvaluationOpenedExtrema);
		CREATE NONCLUSTERED INDEX [ixDateIndex_QuotesForComparing] ON #QuotesForComparing ([DateIndex] ASC);


		--Insert new extrema to the database.
		BEGIN

			DECLARE @peaksByClose AS [dbo].[IdAndDateIndex];
			DECLARE @peaksByHigh AS [dbo].[IdAndDateIndex];
			DECLARE @troughsByClose AS [dbo].[IdAndDateIndex];
			DECLARE @troughsByLow AS [dbo].[IdAndDateIndex];
			INSERT INTO @peaksByClose SELECT [ExtremumId], [DateIndex] FROM #EvaluationOpenedExtrema WHERE [ExtremumTypeId] = 1;
			INSERT INTO @peaksByHigh SELECT [ExtremumId], [DateIndex] FROM #EvaluationOpenedExtrema WHERE [ExtremumTypeId] = 2;
			INSERT INTO @troughsByClose SELECT [ExtremumId], [DateIndex] FROM #EvaluationOpenedExtrema WHERE [ExtremumTypeId] = 3;
			INSERT INTO @troughsByLow SELECT [ExtremumId], [DateIndex] FROM #EvaluationOpenedExtrema WHERE [ExtremumTypeId] = 4;
			---------------------------------------------------
			DECLARE @ocMaxPrices AS [dbo].[DateIndexPrice];
			DECLARE @ocMinPrices AS [dbo].[DateIndexPrice];
			DECLARE @lowPrices AS [dbo].[DateIndexPrice];
			DECLARE @highPrices AS [dbo].[DateIndexPrice];
			INSERT INTO @ocMaxPrices SELECT [DateIndex], [MaxOC] FROM #QuotesForComparing;
			INSERT INTO @ocMinPrices SELECT [DateIndex], [MinOC] FROM #QuotesForComparing;
			INSERT INTO @lowPrices SELECT [DateIndex], [Low] FROM #QuotesForComparing;
			INSERT INTO @highPrices SELECT [DateIndex], [High] FROM #QuotesForComparing;
			---------------------------------------------------


			--Collect right-side analysis for all extremum types.
			SELECT 
				a.*
			INTO
				#UpdatedExtrema
			FROM
				(SELECT * FROM [dbo].[CalculateExtremaRightSideProperties](1, @maxDistance, @peaksByClose, @ocMaxPrices, @lowPrices)
				UNION ALL
				SELECT * FROM [dbo].[CalculateExtremaRightSideProperties](1, @maxDistance, @peaksByHigh, @highPrices, @lowPrices)
				UNION ALL
				SELECT * FROM [dbo].[CalculateExtremaRightSideProperties](-1, @maxDistance, @troughsByClose, @ocMinPrices, @highPrices)
				UNION ALL
				SELECT * FROM [dbo].[CalculateExtremaRightSideProperties](-1, @maxDistance, @troughsByLow, @lowPrices, @highPrices)) a;

			--Update right-side properties.
			UPDATE e
			SET 
				[IsEvaluationOpen] = a.[IsEvaluationOpen],
				[LaterCounter] = a.[LaterCounter],	
				[LaterAmplitude] = a.[LaterAmplitude],
				[LaterTotalArea] = a.[TotalArea],
				[LaterAverageArea] = (a.[TotalArea] / a.[LaterCounter]),
				[LaterChange1] = a.[Delta1],
				[LaterChange2] = a.[Delta2],
				[LaterChange3] = a.[Delta3],
				[LaterChange5] = a.[Delta5],
				[LaterChange10] = a.[Delta10]
			FROM
				[dbo].[extrema] e
				INNER JOIN #UpdatedExtrema a
				ON e.[ExtremumId] = a.[ExtremumId]

			--Update [Value] of updated extrema.
			UPDATE e
			SET
				[Value] = p.[EarlierAmplitudePoints] + p.[LaterAmplitudePoints] + p.[EarlierCounterPoints] + p.[LaterCounterPoints] + p.[EarlierVolatilityPoints] + p.[LaterVolatilityPoints]
			FROM
				[dbo].[extrema] e
				INNER JOIN
				(SELECT
							a.*,
							a.[EarlierAmplitude] / IIF(a.[ExtremumTypeId] > 2, a.[EarlierAmplitude] + a.[price], a.[price]) * 5 AS [EarlierAmplitudePoints],
							a.[LaterAmplitude] / IIF(a.[ExtremumTypeId] > 2, a.[LaterAmplitude] + a.[price], a.[price]) * 5 AS [LaterAmplitudePoints],
							LOG(LOG(IIF(a.[EarlierCounter] < 5.0, 5.0, a.[EarlierCounter]* 1.0), 260) * 10.0, 10) * 35 AS [EarlierCounterPoints],
							LOG(LOG(IIF(a.[LaterCounter] < 5.0, 5.0, a.[LaterCounter]* 1.0), 260) * 10.0, 10) * 35 AS [LaterCounterPoints],
							IIF(a.[EarlierAverageArea] > 100, 10, [EarlierAverageArea] / 10) AS [EarlierVolatilityPoints],
							IIF(a.[LaterAverageArea] > 100, 10, [LaterAverageArea] / 10) AS [LaterVolatilityPoints]
						FROM
							(SELECT
								CASE e.[ExtremumTypeId]
									WHEN 1 THEN q.[MaxOC]
									WHEN 2 THEN q.[High]
									WHEN 3 THEN q.[MinOC]
									WHEN 4 THEN q.[Low]
								END AS [price],
								e.*
							FROM
								(SELECT * FROM [dbo].[extrema] WHERE [ExtremumId] IN (SELECT [ExtremumId] FROM #UpdatedExtrema))  e
								LEFT JOIN #QuotesForComparing q
								ON e.[DateIndex] = q.[DateIndex]) a) p
				ON e.[ExtremumId] = p.[ExtremumId];

		END


		--Filter out extrema with too low value.
		BEGIN

			DELETE 
			FROM 
				[dbo].[extrema]
			WHERE
				[IsEvaluationOpen] = 0 AND
				[Value] < @minExtremumValue OR [Value] IS NUll;

		END


		--Clean up
		BEGIN
			DROP TABLE #EvaluationOpenedExtrema;
			DROP TABLE #QuotesForComparing;
			DROP TABLE #UpdatedExtrema;
		END

			
	END

END

GO
/****** Object:  StoredProcedure [dbo].[analyzeTrendlinesLeftSide]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[analyzeTrendlinesLeftSide] @assetId AS INT, @timeframeId AS INT
AS
BEGIN
	
	-- Prepare temp tables.
	BEGIN
		
		-- Trend breaks
		BEGIN

			CREATE TABLE #TrendBreaks(
				[TrendBreakId] [int] IDENTITY(1,1) NOT NULL,
				[TrendlineId] [int] NOT NULL,
				[DateIndex] [int] NOT NULL,
				[BreakFromAbove] [int] NOT NULL,
				[ProductionId] [int] NULL,
				CONSTRAINT [PK_temp_trendBreaks] PRIMARY KEY CLUSTERED ([TrendBreakId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY];

			CREATE NONCLUSTERED INDEX [ixTrendlineId_temp_trendlinesBreaks] ON #TrendBreaks
			([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixDateIndex_temp_trendlinesBreaks] ON #TrendBreaks
			([DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		END

		-- Trend hits
		BEGIN

			CREATE TABLE #TrendHits(
				[TrendHitId] [int] IDENTITY(1,1) NOT NULL,
				[TrendlineId] [int] NOT NULL,
				[ExtremumGroupId] [int] NOT NULL,
				[DateIndex] [int] NOT NULL,
				[ProductionId] [int] NULL,
				[Value] [float] NULL,
				CONSTRAINT [PK_temp_trendlinesHits] PRIMARY KEY CLUSTERED ([TrendHitId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY];

			CREATE NONCLUSTERED INDEX [ixTrendlineId_temp_trendlinesHits] ON #TrendHits
			([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixExtremumGroup_temp_trendlinesHits] ON #TrendHits
			([ExtremumGroupId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixDateIndex_temp_trendlinesHits] ON #TrendHits
			([DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		END

		-- Trend ranges
		BEGIN
		
			CREATE TABLE #TrendRanges(
				[TrendRangeId] [int] IDENTITY(1,1) NOT NULL,
				[TrendlineId] [int] NOT NULL,
				[BaseId] [int] NOT NULL,
				[BaseIsHit] [int] NOT NULL,
				[BaseDateIndex] [int] NOT NULL,
				[CounterId] [int] NOT NULL,
				[CounterIsHit] [int] NOT NULL,
				[CounterDateIndex] [int] NOT NULL,
				[ProductionId] [int] NULL,
				[IsPeak] [int] NOT NULL DEFAULT(0),
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
				CONSTRAINT [PK_temp_trendRanges] PRIMARY KEY CLUSTERED ([TrendRangeId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY];

			CREATE NONCLUSTERED INDEX [ixTrendlineId_temp_trendRanges] ON #TrendRanges
			([TrendlineId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixBaseId_temp_trendRanges] ON #TrendRanges
			([BaseId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixBaseDateIndex_temp_trendRanges] ON #TrendRanges
			([BaseDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixCounterId_temp_trendRanges] ON #TrendRanges
			([CounterId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
			
			CREATE NONCLUSTERED INDEX [ixCounterDateIndex_temp_trendRanges] ON #TrendRanges
			([CounterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
			
			CREATE NONCLUSTERED INDEX [ixIsPeak_temp_trendRanges] ON #TrendRanges
			([IsPeak] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		END

		-- Trendlines
		BEGIN

			-- All trendlines
			BEGIN

				CREATE TABLE #Trendlines(
					[TrendlineId] [int] NOT NULL,	
					[BaseExtremumGroupId] [int] NOT NULL,
					[BaseDateIndex] [int] NOT NULL,
					[BaseLevel] [float] NOT NULL,
					[CounterExtremumGroupId] [int] NOT NULL,
					[CounterDateIndex] [int] NOT NULL,
					[CounterLevel] [float] NOT NULL,
					[Angle] [float] NOT NULL,
					[StartDateIndex] [int] NULL,
					[EndDateIndex] [int] NULL,
					[IsOpenFromLeft] [bit] NOT NULL DEFAULT(1),
					[IsOpenFromRight] [bit] NOT NULL DEFAULT(1),
					[CandlesDistance] [int] NOT NULL,
					[BreakIndex] [int] NULL,
					[PrevBreakIndex] [int] NULL,
					[HitIndex] [int] NULL,
					[PrevHitIndex] [int] NULL,
					[LookForPeaks] [int] NOT NULL,
					[AnalysisStartPoint] [int] NOT NULL
					CONSTRAINT [PK_temp_trendlines] PRIMARY KEY CLUSTERED ([TrendlineId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
				) ON [PRIMARY]
		
				CREATE NONCLUSTERED INDEX [ixBaseDateIndex_temp_trendlines] ON #Trendlines
				([BaseDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
				CREATE NONCLUSTERED INDEX [ixCounterDateIndex_temp_trendlines] ON #Trendlines
				([CounterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixIsOpenFromLeft_temp_trendlines] ON #Trendlines
				([IsOpenFromLeft] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixBreakIndex_temp_trendlines] ON #Trendlines
				(BreakIndex ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixPrevBreakIndex_temp_trendlines] ON #Trendlines
				([PrevBreakIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixHitIndex_temp_trendlines] ON #Trendlines
				([HitIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixPrevHitIndex_temp_trendlines] ON #Trendlines
				([PrevHitIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixLookForPeaks_temp_trendlines] ON #Trendlines
				([LookForPeaks] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixAnalysisStartPoint_temp_trendlines] ON #Trendlines
				([AnalysisStartPoint] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			END

			-- Closed trendlines
			BEGIN

				CREATE TABLE #ClosedTrendlines(
					[TrendlineId] [int] NOT NULL,	
					[BaseExtremumGroupId] [int] NOT NULL,
					[BaseDateIndex] [int] NOT NULL,
					[BaseLevel] [float] NOT NULL,
					[CounterExtremumGroupId] [int] NOT NULL,
					[CounterDateIndex] [int] NOT NULL,
					[CounterLevel] [float] NOT NULL,
					[Angle] [float] NOT NULL,
					[StartDateIndex] [int] NULL,
					[EndDateIndex] [int] NULL,
					[IsOpenFromLeft] [bit] NOT NULL DEFAULT(1),
					[IsOpenFromRight] [bit] NOT NULL DEFAULT(1),
					[CandlesDistance] [int] NOT NULL,
					[BreakIndex] [int] NULL,
					[PrevBreakIndex] [int] NULL,
					[HitIndex] [int] NULL,
					[PrevHitIndex] [int] NULL,
					[LookForPeaks] [int] NOT NULL,
					[AnalysisStartPoint] [int] NOT NULL
					CONSTRAINT [PK_temp_openTrendlines] PRIMARY KEY CLUSTERED ([TrendlineId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
				) ON [PRIMARY]
		
				CREATE NONCLUSTERED INDEX [ixBaseDateIndex_temp_closedTrendlines] ON #ClosedTrendlines
				([BaseDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
				CREATE NONCLUSTERED INDEX [ixCounterDateIndex_temp_closedTrendlines] ON #ClosedTrendlines
				([CounterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixIsOpenFromLeft_temp_closedTrendlines] ON #ClosedTrendlines
				([IsOpenFromLeft] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixBreakIndex_temp_closedTrendlines] ON #ClosedTrendlines
				(BreakIndex ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixPrevBreakIndex_temp_closedTrendlines] ON #ClosedTrendlines
				([PrevBreakIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixHitIndex_temp_closedTrendlines] ON #ClosedTrendlines
				([HitIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixPrevHitIndex_temp_closedTrendlines] ON #ClosedTrendlines
				([PrevHitIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixLookForPeaks_temp_closedTrendlines] ON #ClosedTrendlines
				([LookForPeaks] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixAnalysisStartPoint_temp_closedTrendlines] ON #ClosedTrendlines
				([AnalysisStartPoint] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			END

		END

		-- Quotes
		BEGIN

			CREATE TABLE #Quotes_AssetTimeframe(
				[DateIndex] [int] NOT NULL,
				[Open] [float] NOT NULL,
				[Low] [float] NOT NULL,
				[High] [float] NOT NULL,
				[Close] [float] NOT NULL,
				CONSTRAINT [PK_temp_quotesAssetTimeframe] PRIMARY KEY CLUSTERED ([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]

			CREATE NONCLUSTERED INDEX [ixDateIndex_temp_QuotesAssetTimeframe] ON #Quotes_AssetTimeframe
			([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE TABLE #Quotes_Iteration(
				[DateIndex] [int] NOT NULL,
				[Open] [float] NOT NULL,
				[Low] [float] NOT NULL,
				[High] [float] NOT NULL,
				[Close] [float] NOT NULL,
				CONSTRAINT [PK_temp_quotesIteration] PRIMARY KEY CLUSTERED ([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]

			CREATE NONCLUSTERED INDEX [ixDateIndex_temp_QuotesIteration] ON #Quotes_Iteration
			([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		END

		-- Extremum groups
		BEGIN
			
			CREATE TABLE #ExtremumGroups(
				[ExtremumGroupId] [int] NOT NULL,
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
			);

		
			CREATE NONCLUSTERED INDEX [ixIsPeak_temp_extremumGroups] ON #ExtremumGroups
			([IsPeak] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixMasterExtremumId_temp_extremumGroups] ON #ExtremumGroups
			([MasterExtremumId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
			CREATE NONCLUSTERED INDEX [ixSlaveExtremumId_temp_extremumGroups] ON #ExtremumGroups
			([SlaveExtremumId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixMasterDateIndex_temp_extremumGroups] ON #ExtremumGroups
			([MasterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixSlaveDateIndex_temp_extremumGroups] ON #ExtremumGroups
			([SlaveDateIndex]  ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixStartDateIndex_temp_extremumGroups] ON #ExtremumGroups
			([StartDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixEndDateIndex_temp_extremumGroups] ON #ExtremumGroups
			([EndDateIndex]  ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


		END

	END

	-- Select initial data:
	-- * trendlines for analysis (with [IsLeftOpen] = 1).
	-- * quotes for the given AssetId and TimeframeId
	BEGIN

		INSERT INTO #Trendlines
		SELECT 
			t.[TrendlineId], t.[BaseExtremumGroupId], t.[BaseDateIndex], t.[BaseLevel], t.[CounterExtremumGroupId], t.[CounterDateIndex], 
			t.[CounterLevel], t.[Angle], t.[StartDateIndex], t.[EndDateIndex], t.[IsOpenFromLeft], t.[IsOpenFromRight], t.[CandlesDistance],
			NULL AS [BreakIndex],
			NULL AS [PrevBreakIndex],
			NULL AS [HitIndex],
			NULL AS [PrevHitIndex],
			eg.[IsPeak] AS [LookForPeaks],
			t.[CounterDateIndex] AS [AnalysisStartPoint]
		FROM 
			(SELECT * FROM [dbo].[trendlines] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) t
			LEFT JOIN (SELECT * FROM [dbo].[extremumGroups] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) eg
			ON t.[CounterExtremumGroupId] = eg.[ExtremumGroupId]
		WHERE
			t.[IsOpenFromLeft] = 1;
		
		INSERT INTO #Quotes_AssetTimeframe
		SELECT 
			[DateIndex], [Open], [Low], [High], [Close]
		FROM
			[dbo].[quotes]
		WHERE
			[AssetId] = @assetId AND 
			[TimeframeId] = @timeframeId;

	END

	-- Proper analysis process.
	BEGIN
		
		DECLARE @trendlineStartOffset AS INT = 0;
		DECLARE @maxDeviationFromTrendline AS FLOAT = 0.001;
		DECLARE @minDistanceFromExtremumToBreak AS INT = 5;
		DECLARE @maxCheckRange AS INT = 10; -- as multiplier of distance between extrema.
		DECLARE @remainingTrendlines AS INT = (SELECT COUNT(*) FROM #Trendlines);

		WHILE @remainingTrendlines > 0
		BEGIN
			
			-- [1] Find first breaks to the left of the current point.
			BEGIN

				-- [1.1] Get proper set of quotes required for analysis and insert them into Quotes_Iteration table.
				BEGIN

					-- [1.1.1] Calculate minimal and maximal required quotation.
					SELECT
						MIN(a.[startQuote]) AS [Min],
						MAX(a.[endQuote]) AS [Max]
					INTO
						#BorderPoints
					FROM
						(SELECT 
							t.[AnalysisStartPoint] - (@maxCheckRange * t.[CandlesDistance]) AS [startQuote],
							t.[AnalysisStartPoint] - 1 AS [endQuote]
						FROM 
							#Trendlines t) a;
					
					DECLARE @minQuoteIndex AS INT = (SELECT [Min] FROM #BorderPoints);
					DECLARE @maxQuoteIndex AS INT = (SELECT [Max] FROM #BorderPoints);


					-- [1.1.2] Load proper set of quotes based on [min] and [max] value obtained above.
					DELETE FROM #Quotes_Iteration;
					INSERT INTO #Quotes_Iteration
					SELECT * 
					FROM #Quotes_AssetTimeframe
					WHERE [DateIndex] BETWEEN @minQuoteIndex AND @maxQuoteIndex;


					-- [1.1.3] Drop temporary tables.
					DROP TABLE #BorderPoints;
									
				END

				-- [1.2] Create matching table between trendlines and quotations.
				BEGIN

					SELECT
						t.[TrendlineId],
						q.[DateIndex],
						q.[Close] * t.[LookForPeaks] AS [ModifiedClose],
						q.[Open] * t.[LookForPeaks] AS [ModifiedOpen],
						(t.[baseLevel] + (q.[DateIndex] - t.[BaseDateIndex]) * t.[Angle]) * t.[LookForPeaks] AS [ModifiedTrendlineLevel],
						t.[LookForPeaks]
					INTO
						#TrendlineQuotePairs
					FROM
						#Trendlines t
						LEFT JOIN #Quotes_Iteration q
						ON q.[DateIndex] BETWEEN (t.[AnalysisStartPoint] - (@maxCheckRange * t.[CandlesDistance])) AND (t.[AnalysisStartPoint] - 1);
					
				END

				-- [1.3] Filter only data with Close and Open prices above Resistance line or below Support Line.
				BEGIN

					SELECT
						t.[TrendlineId], 
						t.[DateIndex], 
						t.[LookForPeaks]
					INTO
						#FilteredTrendlineQuotePairs
					FROM
						#TrendlineQuotePairs t
					WHERE
						NOT(t.[ModifiedTrendlineLevel] > t.[ModifiedClose] AND t.[ModifiedTrendlineLevel] > t.[ModifiedOpen])
					
				END

				-- [1.4] Select the latest break for each analyzed trendline.
				BEGIN

					SELECT
						ft.[TrendlineId], 
						ft.[LookForPeaks],
						MAX(ft.[DateIndex]) + 1 AS [DateIndex]
					INTO 
						#TrendlinesFirstBreaks
					FROM
						#FilteredTrendlineQuotePairs ft
					GROUP BY
						ft.[TrendlineId], ft.[LookForPeaks];

				END


				-- [1.5] Insert information obtained above to the proper tables for the next iteration of analysis.
				BEGIN
					
					-- [Trend breaks]
					INSERT INTO #TrendBreaks([TrendlineId], [DateIndex], [BreakFromAbove])
					SELECT tfb.[TrendlineId], tfb.[DateIndex], tfb.[LookForPeaks]
					FROM #TrendlinesFirstBreaks tfb;

					-- [Trendlines]
					UPDATE t
					SET [BreakIndex] = tfb.[DateIndex]
					FROM 
						#Trendlines t
						LEFT JOIN #TrendlinesFirstBreaks tfb
						ON t.[TrendlineId] = tfb.[TrendlineId];

				END

				-- [1.6] Clean up.
				BEGIN
					DROP TABLE #TrendlineQuotePairs;
					DROP TABLE #FilteredTrendlineQuotePairs;
					DROP TABLE #TrendlinesFirstBreaks;
				END

			END

			-- [2] Find all trend hits between 
			BEGIN

				-- [2.1] Select extremum groups required for this analysis.
				BEGIN

					-- [2.1.1] Calculate minimal and maximal required quotation.
					SELECT
						MIN(a.[startDateIndex]) AS [Min],
						MAX(a.[endDateIndex]) AS [Max]
					INTO
						#ExtremumGroupsBorderPoints
					FROM
						(SELECT 
							t.[AnalysisStartPoint] - (@maxCheckRange * t.[CandlesDistance]) AS [startDateIndex],
							t.[AnalysisStartPoint] AS [endDateIndex]
						FROM 
							#Trendlines t) a;
					
					DECLARE @minDateIndex AS INT = (SELECT [Min] FROM #ExtremumGroupsBorderPoints);
					DECLARE @maxDateIndex AS INT = (SELECT [Max] FROM #ExtremumGroupsBorderPoints);


					-- [2.1.2] Load proper set of extremum group based on [min] and [max] value obtained above.
					DELETE FROM #ExtremumGroups;
					INSERT INTO #ExtremumGroups
					SELECT * 
					FROM [dbo].[extremumGroups]
					WHERE 
						[AssetId] = @assetId AND
						[TimeframeId] = @timeframeId AND
						[StartDateIndex] >= @minQuoteIndex AND 
						[EndDateIndex] <= @maxQuoteIndex;


					-- [2.1.3] Drop temporary tables.
					DROP TABLE #ExtremumGroupsBorderPoints;
					
				END
				
				-- [2.2] Find borders for matching for each separate trendline (depending on breaks found before).
				BEGIN

					SELECT
						t.*,
						COALESCE(t.[BreakIndex] + @minDistanceFromExtremumToBreak, t.[AnalysisStartPoint] - (@maxCheckRange * t.[CandlesDistance])) + 1 AS [MatchingLeftBorder],
						t.[AnalysisStartPoint] AS [MatchingRightBorder]
					INTO 
						#TrendlinesHitsSearchBorders
					FROM
						#Trendlines t;

				END

				-- [2.3] Create table with all possible matches Trendline-ExtremumGroup.
				BEGIN
					
					SELECT
						t.[TrendlineId],
						t.[LookForPeaks],
						(t.[BaseLevel] + (eg.[SlaveDateIndex] - t.[BaseDateIndex]) * t.[Angle]) AS [TrendlineLevel],
						eg.[ExtremumGroupId],
						eg.[StartDateIndex] AS [ExtremumStartIndex],
						eg.[ExtremumPriceLevel] AS [ExtremumPrice]
					INTO
						#TrendlineExtremumPossibleMatches
					FROM
						#TrendlinesHitsSearchBorders t
						LEFT JOIN #ExtremumGroups eg
						ON  eg.[IsPeak] = t.[LookForPeaks] AND 
							eg.[StartDateIndex] BETWEEN t.[MatchingLeftBorder] AND t.[MatchingRightBorder];

					SELECT 
						t.[TrendlineId],
						t.[ExtremumGroupId],
						t.[ExtremumStartIndex] AS [ExtremumStartIndex],
						t.[LookForPeaks],
						t.[ExtremumPrice] * t.[LookForPeaks] AS [ModifiedPrice],
						t.[TrendlineLevel] * t.[LookForPeaks] AS [ModifiedTrendlineLevel],
						IIF(t.[TrendlineLevel] > 0, 1, -1) AS [TrendlineAboveZero],
						(t.[TrendlineLevel] - t.[ExtremumPrice]) / t.[TrendlineLevel] AS [PriceTrendlineDistance]
					INTO
						#TrendlineMatchesWithModifiedPrices
					FROM
						#TrendlineExtremumPossibleMatches t;

				END

				-- [2.4] Filter out prices that are not close enough to matched trendline and insert rest of records into TrendHits temporary table.
				BEGIN

					INSERT INTO #TrendHits([TrendlineId], [ExtremumGroupId], [DateIndex])
					SELECT
						t.[TrendlineId],
						t.[ExtremumGroupId],
						t.[ExtremumStartIndex]
					FROM
						#TrendlineMatchesWithModifiedPrices t
					WHERE
						t.[LookForPeaks] * t.[PriceTrendlineDistance] * t.[TrendlineAboveZero]  < @maxDeviationFromTrendline;

				END

				-- [2.5] Remove duplicates from temporary #TrendHits table.
				BEGIN

					WITH CTE AS(
					   SELECT [TrendlineId], [ExtremumGroupId], RN = ROW_NUMBER()
					   OVER(PARTITION BY [TrendlineId], [ExtremumGroupId] ORDER BY [TrendlineId], [ExtremumGroupId])
					   FROM #TrendHits
					)
					DELETE FROM CTE WHERE RN > 1					

				END

				-- [2.6] Append info about trend hits found to temporary #Trendlines table.
				BEGIN

					-- [2.6.1] Create temporary table with the earliest trend hit for each trendline.
					SELECT
						th.[TrendlineId],
						MIN(th.[DateIndex]) AS [FirstHit]
					INTO 
						#EarliestTrendHits
					FROM
						#TrendHits th
					GROUP BY
						th.[TrendlineId];

					-- [2.6.2] Update TrendHit pointers.
					UPDATE t
					SET 
						[HitIndex] = h.[FirstHit]
					FROM
						#Trendlines t
						LEFT JOIN #EarliestTrendHits h
						ON t.[TrendlineId] = h.[TrendlineId]
					WHERE
						t.[IsOpenFromLeft] = 1 AND
						h.[FirstHit] <= t.[AnalysisStartPoint];

					-- [2.6.3] Clean up
					DROP TABLE #EarliestTrendHits;					

				END

				-- [2.7] Remove temporary tables.
				BEGIN
					DROP TABLE #TrendlinesHitsSearchBorders
					DROP TABLE #TrendlineExtremumPossibleMatches;
					DROP TABLE #TrendlineMatchesWithModifiedPrices;
				END

			END

			-- [3] Prepare data for next iteration based on breaks and hits found.
			BEGIN

				-- [3.1] Move all trendlines without break nor hit to ClosedTrendlines table.
				BEGIN

					-- [3.1.1] Select all trendlines without break nor hit.
					SELECT *
					INTO #TrendlinesWithoutEvent
					FROM #Trendlines t
					WHERE 
						(t.[BreakIndex] IS NULL AND t.[HitIndex] IS NULL) OR
						(t.[BreakIndex] IS NOT NULL AND t.[PrevBreakIndex] IS NOT NULL AND t.[HitIndex] IS NULL) OR
						(t.[BreakIndex] IS NOT NULL AND t.[HitIndex] IS NULL AND t.[PrevHitIndex] IS NULL);

					-- [3.1.2] Update their [StartDateIndex] property.
					UPDATE #TrendlinesWithoutEvent
					SET 
						[IsOpenFromLeft] = 0,
						[StartDateIndex] = COALESCE([PrevHitIndex], [AnalysisStartPoint]) - @trendlineStartOffset;

					-- [3.1.3] Insert those records into [ClosedTrendlines] table.
					INSERT INTO #ClosedTrendlines
					SELECT * FROM #TrendlinesWithoutEvent;

					-- [3.1.4] Remove them from table with open trendlines.
					DELETE 
					FROM #Trendlines
					WHERE 
						([BreakIndex] IS NULL AND [HitIndex] IS NULL) OR
						([BreakIndex] IS NOT NULL AND [PrevBreakIndex] IS NOT NULL AND [HitIndex] IS NULL) OR
						([BreakIndex] IS NOT NULL AND [HitIndex] IS NULL AND [PrevHitIndex] IS NULL);

					-- [3.1.5] Drop temporary table.
					DROP TABLE #TrendlinesWithoutEvent;

				END

				-- [3.2] Update status of all remaining records in #Trendlines table.
				BEGIN

					UPDATE
						#Trendlines
					SET 
						[LookForPeaks] = [LookForPeaks] * IIF([BreakIndex] IS NULL, 1, -1),
						[AnalysisStartPoint] = COALESCE(IIF([BreakIndex] IS NOT NULL, [BreakIndex] - 1, [HitIndex] - 1), 0),
						[PrevBreakIndex] = IIF([BreakIndex] IS NULL, [PrevBreakIndex], [BreakIndex]),
						[BreakIndex] = NULL,
						[PrevHitIndex] = IIF([HitIndex] IS NULL, [PrevHitIndex], [HitIndex]),
						[HitIndex] = NULL;

				END

			END


			SET @remainingTrendlines = (SELECT COUNT(*) FROM #Trendlines);

		END

	END


	-- Feed production DB tables with data obtained above.
	BEGIN
		
		-- [1] Update data about validated trendlines into production tables.
		BEGIN
			
			-- [1.0] Create temporary table with trendlines qualified for further analysis.
			BEGIN

				-- [1.0.1] Create table with data about validated trendlines.
				SELECT 
					*
				INTO
					#ValidatedTrendlines
				FROM
					#ClosedTrendlines ct
				WHERE
					ct.[StartDateIndex] <= ct.[BaseDateIndex];

				-- [1.0.2] Create table containing IDs of all trendlines without a single hit to the left side.
				SELECT 
					ct.[TrendlineId]
				INTO
					#InvalidatedTrendlines
				FROM
					#ClosedTrendlines ct
				WHERE
					ct.[StartDateIndex] > ct.[BaseDateIndex];

			END

			-- [1.1] Move data of trend hits into the production TrendHits table.
			BEGIN
				
				-- [1.1.1] Remove info about trend breaks for invalidated trendlines.
				DELETE
				FROM 
					#TrendHits
				WHERE 
					[TrendlineId] IN (SELECT * FROM #InvalidatedTrendlines);

				-- [1.1.2] Remove records with trend hits before trendline start.
				DELETE th
				FROM
					#TrendHits th
					LEFT JOIN #ClosedTrendlines ct
					ON th.[TrendlineId] = ct.[TrendlineId]
				WHERE
					th.[DateIndex] < ct.[StartDateIndex];

				-- [1.1.3] Insert records for hits at Counter Extremum Group date index.
				INSERT INTO #TrendHits([TrendlineId], [ExtremumGroupId], [DateIndex])
				SELECT 
					vt.[TrendlineId],
					vt.[CounterExtremumGroupId],
					vt.[CounterDateIndex]
				FROM 
					#ValidatedTrendlines vt;
					
				-- [1.1.4] Remove duplicates from #TrendHits table.
				WITH CTE AS(
					SELECT [TrendlineId], [ExtremumGroupId], RN = ROW_NUMBER()
					OVER(PARTITION BY [TrendlineId], [ExtremumGroupId] ORDER BY [TrendlineId], [ExtremumGroupId])
					FROM #TrendHits
				)
				DELETE FROM CTE WHERE RN > 1

				-- [1.1.5] #removed

				-- [1.1.6] Create temporary table to store IDs given by DB engine.
				CREATE TABLE #TempTrendHitsForIdentityMatching(
					[TrendHitId] [int] NOT NULL,
					[TrendlineId] [int] NOT NULL,
					[ExtremumGroupId] [int] NOT NULL
				);

				-- [1.1.7] Insert data into DB table.
				INSERT INTO [dbo].[trendHits]([TrendlineId], [ExtremumGroupId])
				OUTPUT Inserted.[TrendHitId], Inserted.[TrendlineId], Inserted.[ExtremumGroupId]
				INTO #TempTrendHitsForIdentityMatching
				SELECT [TrendlineId], [ExtremumGroupId]
				FROM #TrendHits;


				-- [1.1.8] Append IDs given by the DB engine to the records in the temporary table.
				UPDATE th
				SET 
					[ProductionId] = h.[TrendHitId]
				FROM
					#TrendHits th
					LEFT JOIN #TempTrendHitsForIdentityMatching h
					ON  th.[TrendlineId] = h.[TrendlineId] AND
						th.[ExtremumGroupId] = h.[ExtremumGroupId];

				-- [1.1.9] Drop temporary table.
				DROP TABLE #TempTrendHitsForIdentityMatching;

			END

			-- [1.2] Move data of trend breaks into the production TrendBreaks table.
			BEGIN
				
				-- [1.2.1] Remove info about trend breaks for invalidated trendlines.
				DELETE
				FROM 
					#TrendBreaks
				WHERE 
					[TrendlineId] IN (SELECT * FROM #InvalidatedTrendlines);

				-- [1.2.2] Remove records with trend breaks before trendline start.
				DELETE tb
				FROM
					#TrendBreaks tb
					LEFT JOIN #ClosedTrendlines ct
					ON tb.[TrendlineId] = ct.[TrendlineId]
				WHERE
					tb.[DateIndex] < ct.[StartDateIndex];

				-- [1.2.3] Create temporary table to store IDs given by DB engine.
				CREATE TABLE #TempTrendBreaksForIdentityMatching(
					[TrendBreakId] [int] NOT NULL,
					[TrendlineId] [int] NOT NULL,
					[DateIndex] [int] NOT NULL
				);

				-- [1.2.4] Insert remaining trendlines into TrendBreaks table.
				INSERT INTO [dbo].[TrendBreaks]([TrendlineId], [DateIndex], [BreakFromAbove])
				OUTPUT Inserted.[TrendBreakId], Inserted.[TrendlineId], Inserted.[DateIndex]
				INTO #TempTrendBreaksForIdentityMatching
				SELECT
					tb.[TrendlineId],
					tb.[DateIndex],
					tb.[BreakFromAbove]
				FROM
					#TrendBreaks tb;

				-- [1.2.5] Append IDs given by the DB engine to the records in the temporary table.
				UPDATE tb
				SET 
					[ProductionId] = b.[TrendBreakId]
				FROM
					#TrendBreaks tb
					LEFT JOIN #TempTrendBreaksForIdentityMatching b
					ON  tb.[TrendlineId] = b.[TrendlineId] AND
						tb.[DateIndex] = b.[DateIndex];

				-- [1.2.6] Drop temporary table.
				DROP TABLE #TempTrendBreaksForIdentityMatching;

			END

			-- [1.3] Create and insert records about trend ranges.
			BEGIN

				-- [1.3.1] Create temporary table containing all breaks and hits from temporary tables.
				SELECT
					*, 
					[number] = ROW_NUMBER() OVER (ORDER BY [TrendlineId], [DateIndex])
				INTO
					#CombinedBreaksAndHits
				FROM
					(SELECT [TrendlineId], [ProductionId], [DateIndex], 0 AS [IsHit]
					FROM #TrendBreaks
					UNION ALL
					SELECT [TrendlineId], [ProductionId], [DateIndex], 1 AS [IsHit]
					FROM #TrendHits) a;

				-- [1.3.2] Create trend range border pairs and insert them into #TrendRanges temporary table.
				INSERT INTO #TrendRanges([TrendlineId], [BaseId], [BaseIsHit], [BaseDateIndex], [CounterId], [CounterIsHit], [CounterDateIndex])
				SELECT 
					cb1.[TrendlineId], cb1.[ProductionId], cb1.[IsHit], cb1.[DateIndex], cb2.[ProductionId], cb2.[IsHit], cb2.[DateIndex]
				FROM 
					#CombinedBreaksAndHits cb1
					INNER JOIN #CombinedBreaksAndHits cb2
					ON  cb1.[TrendlineId] = cb2.[TrendlineId] AND
						cb1.[number] = cb2.[number] - 1;

				-- [1.3.3] Append info if the given trend range is top or bottom.
				UPDATE tr
				SET
					[IsPeak] = eg.[IsPeak]
				FROM
					#TrendRanges tr
					LEFT JOIN #TrendHits th ON (tr.[BaseIsHit] = 1 AND tr.[BaseId] = th.[ProductionId]) OR (tr.[CounterIsHit] = 1 AND tr.[CounterId] = th.[ProductionId])
					LEFT JOIN [dbo].[extremumGroups] eg ON th.[ExtremumGroupId] = eg.[ExtremumGroupId];

				-- [1.3.4] Call function evaluating trend ranges.
				BEGIN
					
					DECLARE @TrendRangeBasicData AS [dbo].[TrendRangeBasicData];
					INSERT INTO @TrendRangeBasicData
					SELECT
						tr.[TrendRangeId],
						--tr.[TrendlineId],
						t.[BaseDateIndex] AS [TrendlineStartDateIndex],
						t.[BaseLevel] AS [TrendlineStartLevel],
						t.[Angle] As [TrendlineAngle],
						IIF(tr.[BaseIsHit] = 1, eg.[EndDateIndex] + 1, tr.[BaseDateIndex]) AS [StartIndex],
						IIF(tr.[CounterIsHit] = 1, eg2.[StartDateIndex] - 1, tr.[CounterDateIndex]) AS [EndIndex],
						tr.[IsPeak]
					FROM
						#TrendRanges tr
						LEFT JOIN #TrendHits th ON tr.[BaseId] = th.[ProductionId]
						LEFT JOIN [dbo].[extremumGroups] eg ON th.[ExtremumGroupId] = eg.[ExtremumGroupId]
						LEFT JOIN #TrendHits th2 ON tr.[CounterId] = th2.[ProductionId]
						LEFT JOIN [dbo].[extremumGroups] eg2 ON th2.[ExtremumGroupId] = eg2.[ExtremumGroupId]
						LEFT JOIN [dbo].[trendlines] t ON tr.[TrendlineId] = t.[TrendlineId];


					-- [1.3.4.1] Update variation data.
					UPDATE tr
					SET
						[TotalCandles] = v.[TotalCandles],
						[AverageVariation] = v.[TotalVariation] / v.[TotalCandles],
						[ExtremumVariation] = v.[ExtremumVariation],
						[OpenCloseVariation] = v.[OCVariation]
					FROM
						#TrendRanges tr
						LEFT JOIN [dbo].[GetTrendRangesVariations](@assetId, @timeframeId, @TrendRangeBasicData) v ON tr.[TrendRangeId] = v.[TrendRangeId]

					-- [1.3.4.2] Update cross data.
					UPDATE tr
					SET
						[ExtremumPriceCrossPenaltyPoints] = v.[ExtremumPriceCrossPenaltyPoints],
						[ExtremumPriceCrossCounter] = v.[ExtremumPriceCrossCounter],
						[OCPriceCrossPenaltyPoints] = v.[OCPriceCrossPenaltyPoints],
						[OCPriceCrossCounter] = v.[OCPriceCrossCounter]
					FROM
						#TrendRanges tr
						LEFT JOIN [dbo].[GetTrendRangesCrossDetails](@assetId, @timeframeId, @TrendRangeBasicData) v ON tr.[TrendRangeId] = v.[TrendRangeId]
							
				END


				-- [1.3.x] Move ranges to the production table.
				INSERT INTO [dbo].[trendRanges]([TrendlineId], [BaseId], [BaseIsHit], [BaseDateIndex], [CounterId], [CounterIsHit], [CounterDateIndex], [IsPeak],
												[ExtremumPriceCrossPenaltyPoints], [ExtremumPriceCrossCounter], [OCPriceCrossPenaltyPoints], [OCPriceCrossCounter],
												[TotalCandles], [AverageVariation], [ExtremumVariation], [OpenCloseVariation], [BaseHitValue], [CounterHitValue])
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
					[CounterHitValue]
				FROM
					#TrendRanges;

				-- [1.3.y] Clean up
				BEGIN
					DROP TABLE #CombinedBreaksAndHits;
				END

			END

			-- [1.4] Update trendlines with any hit found.
			UPDATE t
			SET
				[IsOpenFromLeft] = 0,
				[StartDateIndex] = ct.[StartDateIndex]
			FROM
				[dbo].[trendlines] t
				LEFT JOIN (SELECT * FROM #ClosedTrendlines WHERE [StartDateIndex] <= [BaseDateIndex]) ct
				ON t.[TrendlineId] = ct.[TrendlineId]
			WHERE
				ct.[TrendlineId] IS NOT NULL;
			
		END

		-- [2] Remove trendlines without a single hit.
		BEGIN

			-- [2.1] Remove trendlines with IDs listed above from production table.
			BEGIN
				
				DELETE t
				FROM
					[dbo].[trendlines] t
					LEFT JOIN #InvalidatedTrendlines it
					ON t.[TrendlineId] = it.[TrendlineId]
				WHERE
					it.[TrendlineId] IS NOT NULL;

			END

		END

		-- [3] Clean up
		BEGIN
			DROP TABLE #InvalidatedTrendlines;
			DROP TABLE #ValidatedTrendlines;
		END

	
	END


	-- Drop temporary tables.
	BEGIN

		DROP TABLE #TrendBreaks;
		DROP TABLE #TrendHits;
		DROP TABLE #TrendRanges;
		DROP TABLE #Trendlines;
		DROP TABLE #ClosedTrendlines;
		DROP TABLE #Quotes_AssetTimeframe;
		DROP TABLE #Quotes_Iteration;
		DROP TABLE #ExtremumGroups;

	END

END

GO
/****** Object:  StoredProcedure [dbo].[analyzeTrendlinesRightSide]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[analyzeTrendlinesRightSide] @assetId AS INT, @timeframeId AS INT
AS
BEGIN


	-- [1] Preparing temporary tables.
	BEGIN
		
		-- [1.1] Trend breaks
		BEGIN

			CREATE TABLE #TrendBreaks(
				[TrendBreakId] [int] IDENTITY(1,1) NOT NULL,
				[TrendlineId] [int] NOT NULL,
				[DateIndex] [int] NOT NULL,
				[BreakFromAbove] [int] NOT NULL,
				[ProductionId] [int] NULL,
				CONSTRAINT [PK_temp_trendBreaks] PRIMARY KEY CLUSTERED ([TrendBreakId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY];

			CREATE NONCLUSTERED INDEX [ixTrendlineId_temp_trendlinesBreaks] ON #TrendBreaks
			([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixDateIndex_temp_trendlinesBreaks] ON #TrendBreaks
			([DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			--CREATE UNIQUE NONCLUSTERED INDEX [ixTrendlineIdDateIndex_temp_trendlinesBreaks] ON #TrendBreaks
			--([TrendlineId] ASC, [DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


		END

		-- [1.2] Trend hits
		BEGIN

			CREATE TABLE #TrendHits(
				[TrendHitId] [int] IDENTITY(1,1) NOT NULL,
				[TrendlineId] [int] NOT NULL,
				[ExtremumGroupId] [int] NOT NULL,
				[DateIndex] [int] NOT NULL,
				[ProductionId] [int] NULL,
				[Value] [float] NULL,
				CONSTRAINT [PK_temp_trendlinesHits] PRIMARY KEY CLUSTERED ([TrendHitId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY];

			CREATE NONCLUSTERED INDEX [ixTrendlineId_temp_trendlinesHits] ON #TrendHits
			([TrendlineId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixExtremumGroup_temp_trendlinesHits] ON #TrendHits
			([ExtremumGroupId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixDateIndex_temp_trendlinesHits] ON #TrendHits
			([DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE UNIQUE NONCLUSTERED INDEX [ixTrendlineIdDateIndex_temp_trendlinesHits] ON #TrendHits
			([TrendlineId] ASC, [DateIndex] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


		END

		-- [1.3] Trend ranges
		BEGIN
		
			CREATE TABLE #TrendRanges(
				[TrendRangeId] [int] IDENTITY(1,1) NOT NULL,
				[TrendlineId] [int] NOT NULL,
				[BaseId] [int] NOT NULL,
				[BaseIsHit] [int] NOT NULL,
				[BaseDateIndex] [int] NOT NULL,
				[CounterId] [int] NOT NULL,
				[CounterIsHit] [int] NOT NULL,
				[CounterDateIndex] [int] NOT NULL,
				[ProductionId] [int] NULL,
				[IsPeak] [int] NOT NULL DEFAULT(0),
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
				CONSTRAINT [PK_temp_trendRanges] PRIMARY KEY CLUSTERED ([TrendRangeId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY];

			CREATE NONCLUSTERED INDEX [ixTrendlineId_temp_trendRanges] ON #TrendRanges
			([TrendlineId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixBaseId_temp_trendRanges] ON #TrendRanges
			([BaseId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixBaseDateIndex_temp_trendRanges] ON #TrendRanges
			([BaseDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixCounterId_temp_trendRanges] ON #TrendRanges
			([CounterId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
			
			CREATE NONCLUSTERED INDEX [ixCounterDateIndex_temp_trendRanges] ON #TrendRanges
			([CounterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
			
			CREATE NONCLUSTERED INDEX [ixIsPeak_temp_trendRanges] ON #TrendRanges
			([IsPeak] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE UNIQUE NONCLUSTERED INDEX [ixUniqueSet_temp_trendlinesHits] ON #TrendRanges
			([TrendlineId] ASC, [BaseDateIndex] ASC, [CounterDateIndex] ASC, [IsPeak] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


		END

		-- [1.4] Trendlines
		BEGIN

			-- All trendlines
			BEGIN

				CREATE TABLE #Trendlines(
					[TrendlineId] [int] NOT NULL,	
					[BaseExtremumGroupId] [int] NOT NULL,
					[BaseDateIndex] [int] NOT NULL,
					[BaseLevel] [float] NOT NULL,
					[CounterExtremumGroupId] [int] NOT NULL,
					[CounterDateIndex] [int] NOT NULL,
					[CounterLevel] [float] NOT NULL,
					[Angle] [float] NOT NULL,
					[StartDateIndex] [int] NULL,
					[EndDateIndex] [int] NULL,
					[IsOpenFromLeft] [bit] NOT NULL DEFAULT(1),
					[IsOpenFromRight] [bit] NOT NULL DEFAULT(1),
					[CandlesDistance] [int] NOT NULL,
					[BreakIndex] [int] NULL,
					[PrevBreakIndex] [int] NULL,
					[HitIndex] [int] NULL,
					[PrevHitIndex] [int] NULL,
					[LookForPeaks] [int] NOT NULL,
					[AnalysisStartPoint] [int] NOT NULL
					CONSTRAINT [PK_temp_trendlines] PRIMARY KEY CLUSTERED ([TrendlineId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
				) ON [PRIMARY]
		
				CREATE NONCLUSTERED INDEX [ixBaseDateIndex_temp_trendlines] ON #Trendlines
				([BaseDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
				CREATE NONCLUSTERED INDEX [ixCounterDateIndex_temp_trendlines] ON #Trendlines
				([CounterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixIsOpenFromLeft_temp_trendlines] ON #Trendlines
				([IsOpenFromLeft] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixBreakIndex_temp_trendlines] ON #Trendlines
				(BreakIndex ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixPrevBreakIndex_temp_trendlines] ON #Trendlines
				([PrevBreakIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixHitIndex_temp_trendlines] ON #Trendlines
				([HitIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixPrevHitIndex_temp_trendlines] ON #Trendlines
				([PrevHitIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixLookForPeaks_temp_trendlines] ON #Trendlines
				([LookForPeaks] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixAnalysisStartPoint_temp_trendlines] ON #Trendlines
				([AnalysisStartPoint] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			END

			-- Closed trendlines
			BEGIN

				CREATE TABLE #ClosedTrendlines(
					[TrendlineId] [int] NOT NULL,	
					[BaseExtremumGroupId] [int] NOT NULL,
					[BaseDateIndex] [int] NOT NULL,
					[BaseLevel] [float] NOT NULL,
					[CounterExtremumGroupId] [int] NOT NULL,
					[CounterDateIndex] [int] NOT NULL,
					[CounterLevel] [float] NOT NULL,
					[Angle] [float] NOT NULL,
					[StartDateIndex] [int] NULL,
					[EndDateIndex] [int] NULL,
					[IsOpenFromLeft] [bit] NOT NULL DEFAULT(1),
					[IsOpenFromRight] [bit] NOT NULL DEFAULT(1),
					[CandlesDistance] [int] NOT NULL,
					[BreakIndex] [int] NULL,
					[PrevBreakIndex] [int] NULL,
					[HitIndex] [int] NULL,
					[PrevHitIndex] [int] NULL,
					[LookForPeaks] [int] NOT NULL,
					[AnalysisStartPoint] [int] NOT NULL
					CONSTRAINT [PK_temp_openTrendlines] PRIMARY KEY CLUSTERED ([TrendlineId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
				) ON [PRIMARY]
		
				CREATE NONCLUSTERED INDEX [ixBaseDateIndex_temp_closedTrendlines] ON #ClosedTrendlines
				([BaseDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
				CREATE NONCLUSTERED INDEX [ixCounterDateIndex_temp_closedTrendlines] ON #ClosedTrendlines
				([CounterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixIsOpenFromLeft_temp_closedTrendlines] ON #ClosedTrendlines
				([IsOpenFromLeft] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixBreakIndex_temp_closedTrendlines] ON #ClosedTrendlines
				(BreakIndex ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixPrevBreakIndex_temp_closedTrendlines] ON #ClosedTrendlines
				([PrevBreakIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixHitIndex_temp_closedTrendlines] ON #ClosedTrendlines
				([HitIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixPrevHitIndex_temp_closedTrendlines] ON #ClosedTrendlines
				([PrevHitIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixLookForPeaks_temp_closedTrendlines] ON #ClosedTrendlines
				([LookForPeaks] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

				CREATE NONCLUSTERED INDEX [ixAnalysisStartPoint_temp_closedTrendlines] ON #ClosedTrendlines
				([AnalysisStartPoint] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			END

		END

		-- [1.5] Quotes
		BEGIN

			CREATE TABLE #Quotes_AssetTimeframe(
				[DateIndex] [int] NOT NULL,
				[Open] [float] NOT NULL,
				[Low] [float] NOT NULL,
				[High] [float] NOT NULL,
				[Close] [float] NOT NULL,
				CONSTRAINT [PK_temp_quotesAssetTimeframe] PRIMARY KEY CLUSTERED ([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]

			CREATE NONCLUSTERED INDEX [ixDateIndex_temp_QuotesAssetTimeframe] ON #Quotes_AssetTimeframe
			([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE TABLE #Quotes_Iteration(
				[DateIndex] [int] NOT NULL,
				[Open] [float] NOT NULL,
				[Low] [float] NOT NULL,
				[High] [float] NOT NULL,
				[Close] [float] NOT NULL,
				CONSTRAINT [PK_temp_quotesIteration] PRIMARY KEY CLUSTERED ([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]

			CREATE NONCLUSTERED INDEX [ixDateIndex_temp_QuotesIteration] ON #Quotes_Iteration
			([DateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		END

		-- [1.6] Extremum groups
		BEGIN
			
			CREATE TABLE #ExtremumGroups(
				[ExtremumGroupId] [int] NOT NULL,
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
			);

		
			CREATE NONCLUSTERED INDEX [ixIsPeak_temp_extremumGroups] ON #ExtremumGroups
			([IsPeak] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixMasterExtremumId_temp_extremumGroups] ON #ExtremumGroups
			([MasterExtremumId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	
			CREATE NONCLUSTERED INDEX [ixSlaveExtremumId_temp_extremumGroups] ON #ExtremumGroups
			([SlaveExtremumId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixMasterDateIndex_temp_extremumGroups] ON #ExtremumGroups
			([MasterDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixSlaveDateIndex_temp_extremumGroups] ON #ExtremumGroups
			([SlaveDateIndex]  ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixStartDateIndex_temp_extremumGroups] ON #ExtremumGroups
			([StartDateIndex] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

			CREATE NONCLUSTERED INDEX [ixEndDateIndex_temp_extremumGroups] ON #ExtremumGroups
			([EndDateIndex]  ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


		END

	END


	-- [2] Select initial data:
	BEGIN

		-- [2.1] Select trendlines open for right-side analysis.
		BEGIN

			-- [2.1.1] Filter out irrelevant trendlines.
			SELECT
				*
			INTO
				#FilteredTrendlines
			FROM 
				[dbo].[trendlines] 
			WHERE 
				[AssetId] = @assetId 
				AND [TimeframeId] = @timeframeId
				AND [IsOpenFromRight] = 1;

			-- [2.1.2] Filter out irrelevant extremum groups.
			SELECT
				*
			INTO
				#FilteredExtremumGroups
			FROM 
				[dbo].[extremumGroups] 
			WHERE 
				[AssetId] = @assetId AND [TimeframeId] = @timeframeId;

			-- [2.1.3] For each filtered trendline get its last trend-ranges.
			SELECT
				*
			INTO
				#FilteredTrendlinesLastRanges
			FROM
				(SELECT
					ft.[TrendlineId],
					tr.[BaseIsHit],
					tr.[BaseDateIndex],
					tr.[CounterIsHit],
					tr.[CounterDateIndex],
					tr.[IsPeak],
					number = ROW_NUMBER() OVER(PARTITION BY tr.[TrendlineId] ORDER BY tr.[TrendlineId] ASC, tr.[CounterDateIndex] DESC)
				FROM
					#FilteredTrendlines ft 
					LEFT JOIN [dbo].[trendRanges] tr ON ft.[TrendlineId] = tr.[TrendlineId]) a 
			WHERE
				a.[number] = 1;

			-- [2.1.4] Append pointers for analysis and insert data into table with Trendlines for analysis.
			INSERT INTO #Trendlines([TrendlineId], [BaseExtremumGroupId], [BaseDateIndex], [BaseLevel], [CounterExtremumGroupId], [CounterDateIndex], [CounterLevel], [Angle], [StartDateIndex], [EndDateIndex], 
									[IsOpenFromLeft], [IsOpenFromRight], [CandlesDistance], [BreakIndex], [PrevBreakIndex], [HitIndex], [PrevHitIndex], [LookForPeaks], [AnalysisStartPoint])
			SELECT
				ft.[TrendlineId],
				ft.[BaseExtremumGroupId],
				ft.[BaseDateIndex], 
				ft.[BaseLevel], 
				ft.[CounterExtremumGroupId], 
				ft.[CounterDateIndex], 
				ft.[CounterLevel], 
				ft.[Angle], 
				ft.[StartDateIndex], 
				ft.[EndDateIndex], 
				ft.[IsOpenFromLeft], 
				ft.[IsOpenFromRight], 
				ft.[CandlesDistance],
				NULL AS [BreakIndex],
				IIF(ftlr.[CounterIsHit] = 1, NULL, ftlr.[CounterDateIndex]) AS [PrevBreakIndex],
				NULL AS [HitIndex],
				IIF(ftlr.[CounterIsHit] = 1, ftlr.[CounterDateIndex], ftlr.[BaseDateIndex]) AS [PrevHitIndex],
				IIF(ftlr.[CounterIsHit] = 1, ftlr.[IsPeak], ftlr.[IsPeak] * (-1)) AS [LookForPeaks],
				ftlr.[CounterDateIndex] + 1 AS [AnalysisStartPoint]
			FROM
				#FilteredTrendlines ft
				LEFT JOIN #FilteredTrendlinesLastRanges ftlr ON ft.[TrendlineId] = ftlr.[TrendlineId];

			-- [2.1.5] Drop temporary tables.
			BEGIN
				DROP TABLE #FilteredTrendlines;
				DROP TABLE #FilteredExtremumGroups;
				DROP TABLE #FilteredTrendlinesLastRanges;
			END

		END
		
		-- [2.2] Select quotes.
		BEGIN

			INSERT INTO #Quotes_AssetTimeframe
			SELECT 
				[DateIndex], [Open], [Low], [High], [Close]
			FROM
				[dbo].[quotes]
			WHERE
				[AssetId] = @assetId AND 
				[TimeframeId] = @timeframeId AND 
				[DateIndex] >= (SELECT MIN([CounterDateIndex]) FROM #Trendlines);

		END


--SELECT 'DEBUG', '#Trendlines [2.2]' AS [2.2. #Trendlines], * FROM #Trendlines;


	END


	-- [3] Proper analysis.
	BEGIN


		DECLARE @trendlineStartOffset AS INT = 0;
		DECLARE @maxDeviationFromTrendline AS FLOAT = 0.001;
		DECLARE @minDistanceFromExtremumToBreak AS INT = 3;
		DECLARE @maxCheckRange AS INT = 10; -- as multiplier of distance between extrema.
		DECLARE @remainingTrendlines AS INT = (SELECT COUNT(*) FROM #Trendlines);
		DECLARE @maxQuoteIndex AS INT = (SELECT MAX([DateIndex]) FROM #Quotes_AssetTimeframe);
		
PRINT 'Max quote index:  ' + CAST(@maxQuoteIndex AS NVARCHAR(255));
--SELECT 'DEBUG', 'Pointer', @maxQuoteIndex;

		WHILE @remainingTrendlines > 0
		BEGIN
			
			-- [3.1] Find first breaks to the right of the current point.
			BEGIN
			
				
				-- [3.1.1] Get proper set of quotes required for analysis and insert them into Quotes_Iteration table.
				BEGIN

					-- [3.1.1.1] Calculate minimal and maximal required quotation.
					BEGIN

						SELECT
							MIN(a.[startQuote]) AS [Min],
							MAX(a.[endQuote]) AS [Max]
						INTO
							#BorderPoints
						FROM
							(SELECT 
								t.[AnalysisStartPoint] AS [startQuote],
								t.[AnalysisStartPoint] + (@maxCheckRange * t.[CandlesDistance]) AS [endQuote]
							FROM
								#Trendlines t) a;

					END -- [3.1.1.1]

					-- [3.1.1.2] Load proper set of quotes based on [min] and [max] value obtained above.
					BEGIN

						DELETE FROM #Quotes_Iteration;

						INSERT INTO #Quotes_Iteration
						SELECT 
							qat.* 
						FROM 
							#Quotes_AssetTimeframe qat
							LEFT JOIN #BorderPoints bp ON qat.[DateIndex] BETWEEN bp.[Min] AND bp.[Max]
						WHERE
							bp.[Min] IS NOT NULL;

					END -- [3.1.1.2]
					
					-- [3.1.1.X] Drop temporary tables.
					BEGIN
						DROP TABLE #BorderPoints;
					END

				END --[3.1.1]

----SELECT 'DEBUG', '[3.1.1]', '#Quotes_Iteration', * FROM #Quotes_Iteration;



				-- [3.1.2] Create matching table between trendlines and quotations.
				BEGIN

					SELECT
						t.[TrendlineId],
						q.[DateIndex],
						q.[Close] * IIF(t.[LookForPeaks] = 1, 1, -1) AS [ModifiedClose],
						q.[Open] * IIF(t.[LookForPeaks] = 1, 1, -1) AS [ModifiedOpen],
						(t.[baseLevel] + (q.[DateIndex] - t.[BaseDateIndex]) * t.[Angle]) * IIF(t.[LookForPeaks] = 1, 1, -1) AS [ModifiedTrendlineLevel],
						IIF(t.[LookForPeaks] = 1, 1, -1) AS [LookForPeaks]
					INTO
						#TrendlineQuotePairs
					FROM
						#Trendlines t
						LEFT JOIN #Quotes_Iteration q
						ON q.[DateIndex] BETWEEN t.[AnalysisStartPoint] AND (t.[AnalysisStartPoint] + (@maxCheckRange * t.[CandlesDistance]));
					
				END

--SELECT 'DEBUG', '[3.1.2]', '#TrendlineQuotePairs', * FROM #TrendlineQuotePairs;



				-- [3.1.3] Filter only data with Close and Open prices above Resistance line or below Support Line.
				BEGIN

					SELECT
						t.[TrendlineId], 
						t.[DateIndex], 
						t.[LookForPeaks]
					INTO
						#FilteredTrendlineQuotePairs
					FROM
						#TrendlineQuotePairs t
					WHERE
						t.[ModifiedTrendlineLevel] < t.[ModifiedClose] AND t.[ModifiedTrendlineLevel] < t.[ModifiedOpen];
					
				END

--SELECT 'DEBUG', '[3.1.3]', '#FilteredTrendlineQuotePairs', * FROM #FilteredTrendlineQuotePairs;



				-- [3.1.4] Select the first break for each analyzed trendline.
				BEGIN

					SELECT
						ft.[TrendlineId], 
						ft.[LookForPeaks] AS [LookForPeaks],
						MIN(ft.[DateIndex]) AS [DateIndex]
					INTO 
						#TrendlinesFirstBreaks
					FROM
						#FilteredTrendlineQuotePairs ft
					GROUP BY
						ft.[TrendlineId], ft.[LookForPeaks];

				END

--SELECT 'DEBUG', '[3.1.4]', '#TrendlinesFirstBreaks', * FROM #TrendlinesFirstBreaks;



				-- [3.1.5] Insert information obtained above to the proper tables for the next iteration of analysis.
				BEGIN
					
					-- [Trend breaks]
					INSERT INTO #TrendBreaks([TrendlineId], [DateIndex], [BreakFromAbove])
					SELECT tfb.[TrendlineId], tfb.[DateIndex], tfb.[LookForPeaks] * (-1)
					FROM #TrendlinesFirstBreaks tfb;

					-- [Trendlines]
					UPDATE t
					SET [BreakIndex] = tfb.[DateIndex]
					FROM 
						#Trendlines t
						LEFT JOIN #TrendlinesFirstBreaks tfb
						ON t.[TrendlineId] = tfb.[TrendlineId];

				END

--SELECT 'DEBUG', '[3.1.5]' AS [3.1.5 - TrendBreaks], '#TrendBreaks', * FROM #TrendBreaks;
--SELECT 'DEBUG', '[3.1.5]' AS [3.1.5 - Trendlines], '#Trendlines', * FROM #Trendlines;



				-- [3.1.X] Clean-up.
				BEGIN
					DROP TABLE #TrendlineQuotePairs
					DROP TABLE #FilteredTrendlineQuotePairs;
					DROP TABLE #TrendlinesFirstBreaks;
				END -- [3.1.X]


			END -- [3.1]



			-- [3.2] Find trend hits between analysis start and break or end of quotations.
			BEGIN


				-- [3.2.1] Select extremum groups required for this analysis.
				BEGIN

					-- [3.2.1.1] Calculate minimal and maximal required quotation.
					BEGIN

						SELECT
							MIN(a.[startDateIndex]) AS [Min],
							MAX(a.[endDateIndex]) AS [Max]
						INTO
							#ExtremumGroupsBorderPoints
						FROM
							(SELECT 
								t.[AnalysisStartPoint] AS [startDateIndex],
								t.[AnalysisStartPoint] + (@maxCheckRange * t.[CandlesDistance]) AS [endDateIndex]
							FROM 
								#Trendlines t) a;

					END -- [3.2.1.1]
					
					-- [3.2.1.2] Load proper set of extremum group based on [min] and [max] value obtained above.
					BEGIN

						DELETE FROM #ExtremumGroups;

						INSERT INTO #ExtremumGroups
						SELECT 
							eg.* 
						FROM 
							[dbo].[extremumGroups] eg
							LEFT JOIN #ExtremumGroupsBorderPoints egbp ON eg.[StartDateIndex] >= egbp.[Min] AND eg.[EndDateIndex] <= egbp.[Max]
						WHERE 
							[AssetId] = @assetId AND
							[TimeframeId] = @timeframeId AND
							egbp.[Min] IS NOT NULL;

					END -- [3.2.1.2] 

					-- [3.2.1.3] Drop temporary tables.
					BEGIN
						DROP TABLE #ExtremumGroupsBorderPoints;
					END
					
				END -- [3.2.1]	

				

				-- [3.2.2] Calculate borders for matching for each separate trendline (depending on breaks found before).
				BEGIN

					SELECT
						t.*,
						t.[AnalysisStartPoint] AS [MatchingLeftBorder],
						IIF(t.[BreakIndex] IS NULL, t.[AnalysisStartPoint] + (@maxCheckRange * t.[CandlesDistance]), t.[BreakIndex] - @minDistanceFromExtremumToBreak) AS [MatchingRightBorder]
					INTO 
						#TrendlinesHitsSearchBorders
					FROM
						#Trendlines t;

				END -- [3.2.2]



				-- [3.2.3] Create table with all possible matches Trendline-ExtremumGroup.
				BEGIN
				
					-- [3.2.3.1] Get all combination based on [Peaks/Bottom] + [Extremum is within searching bounds]
					BEGIN

						SELECT
							t.[TrendlineId],
							t.[LookForPeaks],
							(t.[BaseLevel] + (eg.[SlaveDateIndex] - t.[BaseDateIndex]) * t.[Angle]) AS [TrendlineLevel],
							eg.[ExtremumGroupId],
							eg.[StartDateIndex] AS [ExtremumStartIndex],
							eg.[ExtremumPriceLevel] AS [ExtremumPrice]
						INTO
							#TrendlineExtremumPossibleMatches
						FROM
							#TrendlinesHitsSearchBorders t
							LEFT JOIN #ExtremumGroups eg
							ON  eg.[IsPeak] = t.[LookForPeaks] AND 
								eg.[StartDateIndex] BETWEEN t.[MatchingLeftBorder] AND t.[MatchingRightBorder];

					END

					-- [3.2.3.2] Add modified price to the table created above (for comparison purposes).
					BEGIN

						SELECT 
							t.[TrendlineId],
							t.[ExtremumGroupId],
							t.[ExtremumStartIndex] AS [ExtremumStartIndex],
							t.[LookForPeaks],
							t.[ExtremumPrice] * t.[LookForPeaks] AS [ModifiedPrice],
							t.[TrendlineLevel] * t.[LookForPeaks] AS [ModifiedTrendlineLevel],
							IIF(t.[TrendlineLevel] > 0, 1, -1) AS [TrendlineAboveZero],
							(t.[TrendlineLevel] - t.[ExtremumPrice]) / t.[TrendlineLevel] AS [PriceTrendlineDistance]
						INTO
							#TrendlineMatchesWithModifiedPrices
						FROM
							#TrendlineExtremumPossibleMatches t;

					END


				END -- [3.2.3]

----SELECT 'DEBUG', '[3.2.3]', '#TrendlineMatchesWithModifiedPrices', * FROM TrendlineMatchesWithModifiedPrices;



				-- [3.2.4] Filter out prices that are not close enough to matched trendline and insert rest of records into TrendHits temporary table.
				BEGIN

					INSERT INTO #TrendHits([TrendlineId], [ExtremumGroupId], [DateIndex])
					SELECT
						t.[TrendlineId],
						t.[ExtremumGroupId],
						t.[ExtremumStartIndex]
					FROM
						#TrendlineMatchesWithModifiedPrices t
					WHERE
						t.[LookForPeaks] * t.[PriceTrendlineDistance] * t.[TrendlineAboveZero]  < @maxDeviationFromTrendline;

				END -- [3.2.4]

----SELECT 'DEBUG', '[3.2.4]', '#TrendHits', * FROM #TrendHits;



				-- [3.2.5] Remove duplicates from temporary #TrendHits table.
				BEGIN

					WITH CTE AS(
					   SELECT [TrendlineId], [ExtremumGroupId], RN = ROW_NUMBER()
					   OVER(PARTITION BY [TrendlineId], [ExtremumGroupId] ORDER BY [TrendlineId], [ExtremumGroupId])
					   FROM #TrendHits
					)
					DELETE FROM CTE WHERE RN > 1					

				END -- [3.2.5]

----SELECT 'DEBUG', '[3.2.5]', '#TrendHits: after removing duplicates', * FROM #TrendHits;



				-- [3.2.6] Append info about trend hits found to temporary #Trendlines table.
				BEGIN


					-- [3.2.6.1] Create temporary table with the latest trend hit for each trendline.
					BEGIN

						SELECT
							th.[TrendlineId],
							MAX(th.[DateIndex]) AS [LastHit]
						INTO 
							#LatestTrendHits
						FROM
							#TrendHits th
						GROUP BY
							th.[TrendlineId];

					END -- [3.2.6.1] 


					-- [3.2.6.2] Update TrendHit pointers.
					BEGIN

						UPDATE t
						SET 
							[HitIndex] = h.[LastHit]
						FROM
							#Trendlines t
							LEFT JOIN #LatestTrendHits h ON t.[TrendlineId] = h.[TrendlineId]
						WHERE
							t.[IsOpenFromRight] = 1 AND
							h.[LastHit] >= t.[AnalysisStartPoint];
					
					END -- [3.2.6.2] 


					-- [2.6.3] Clean up
					BEGIN
						DROP TABLE #LatestTrendHits;	
					END


				END -- [3.2.6]

----SELECT 'DEBUG', '[3.2.6]', '#Trendlines: after trendhit update', * FROM #Trendlines;



				-- [3.2.7] Remove temporary tables.
				BEGIN
					DROP TABLE #TrendlinesHitsSearchBorders
					DROP TABLE #TrendlineExtremumPossibleMatches;
					DROP TABLE #TrendlineMatchesWithModifiedPrices;
				END


			END -- [3.2]



			-- [3.3] Prepare data for next iteration based on breaks and hits found.
			BEGIN
				

				-- [3.3.1] Move all trendlines without break nor hit to ClosedTrendlines table (unless too less quotations have been checked).
				BEGIN


					-- [3.3.1.1] Select all trendlines without break nor hit.
					BEGIN

						SELECT 
							*
						INTO 
							#TrendlinesToBeSkippedInThisIteration
						FROM 
							#Trendlines t
						WHERE 
							t.[HitIndex] IS NULL AND (t.[BreakIndex] IS NULL OR (t.[BreakIndex] IS NOT NULL AND t.[PrevBreakIndex] IS NOT NULL));

					END -- [3.3.1.1]

--SELECT 'DEBUG', '[3.3.1.1]' AS [3.3.1.1], '#TrendlinesToBeSkippedInThisIteration', * FROM #TrendlinesToBeSkippedInThisIteration;


					-- [3.3.1.2] Find trendlines to be marked as Right-Side closed and update their [EndDateIndex] and [IsOpenFromRight] properties.
					BEGIN

						UPDATE 
							twe
						SET
							twe.[IsOpenFromRight] = 0,
							twe.[EndDateIndex] = COALESCE(twe.[PrevHitIndex], twe.[AnalysisStartPoint]) + @trendlineStartOffset
						FROM 
							#TrendlinesToBeSkippedInThisIteration twe
						WHERE
							(twe.[BreakIndex] IS NOT NULL AND twe.[PrevBreakIndex] IS NOT NULL) OR
							(@maxQuoteIndex - twe.[AnalysisStartPoint] > @maxCheckRange * twe.[CandlesDistance]);
							
					END -- [3.3.1.2] 

--SELECT 'DEBUG', '[3.3.1.2]' AS [3.3.1.2], '#TrendlinesToBeSkippedInThisIteration', * FROM #TrendlinesToBeSkippedInThisIteration;


					-- [3.3.1.3] Move all trendlines without break nor hit to ClosedTrendlines table.
					BEGIN
						
						INSERT INTO #ClosedTrendlines
						SELECT 
							* 
						FROM 
							#TrendlinesToBeSkippedInThisIteration;

						DELETE
						FROM
							#Trendlines
						WHERE
							[TrendlineId] IN (SELECT [TrendlineId] FROM #ClosedTrendlines);

					END -- [3.3.1.3]

----SELECT 'DEBUG', '[3.3.1.3]' AS [3.3.1.3], '#ClosedTrendlines', * FROM #ClosedTrendlines;



				END -- [3.3.1] 


				-- [3.3.2] Update status of all remaining trendlines.
				BEGIN
					
					UPDATE
						#Trendlines
					SET 
						[LookForPeaks] = [LookForPeaks] * IIF([BreakIndex] IS NULL, 1, -1),
						[AnalysisStartPoint] = COALESCE(IIF([BreakIndex] IS NOT NULL, [BreakIndex] + 1, [HitIndex] + 1), 0),
						[PrevBreakIndex] = [BreakIndex],
						[BreakIndex] = NULL,
						[PrevHitIndex] = IIF([HitIndex] IS NULL, [PrevHitIndex], [HitIndex]),
						[HitIndex] = NULL;

				END

--SELECT 'DEBUG', '[3.3.2]' AS [3.3.2 Trendlines before next iteration], '#Trendlines', * FROM #Trendlines;


				-- [3.3.X] Clean-up
				BEGIN
					DROP TABLE #TrendlinesToBeSkippedInThisIteration;
				END



			END -- [3.3] 


			SET @remainingTrendlines = (SELECT COUNT(*) FROM #Trendlines);


		END -- [WHILE]

	END -- [3]


--SELECT 'DEBUG', '[3.X]' AS [3.X ClosedTrendlines], '#ClosedTrendlines', * FROM #ClosedTrendlines;




	-- [4] Feed production tables with data calculated above
	BEGIN


		-- [4.1] Trend hits
		BEGIN

			
			-- [4.1.0] Remove trend hits 
			BEGIN
				
				DELETE th
				FROM 
					#TrendHits th
					LEFT JOIN #ClosedTrendlines ct ON th.[TrendlineId] = ct.[TrendlineId]
				WHERE
					ct.[EndDateIndex] IS NOT NULL AND th.[DateIndex] > ct.[EndDateIndex];

			END -- [4.1.0]


			-- [4.1.1] Remove duplicates from #TrendHits table.
			BEGIN

				WITH CTE AS(
					SELECT [TrendlineId], [ExtremumGroupId], RN = ROW_NUMBER()
					OVER(PARTITION BY [TrendlineId], [ExtremumGroupId] ORDER BY [TrendlineId], [ExtremumGroupId])
					FROM #TrendHits
				)
				DELETE FROM CTE WHERE RN > 1

			END -- [4.1.1]


			-- [4.1.2] Create temporary table to store IDs given by DB engine.
			BEGIN
				
				CREATE TABLE #TempTrendHitsForIdentityMatching(
					[TrendHitId] [int] NOT NULL,
					[TrendlineId] [int] NOT NULL,
					[ExtremumGroupId] [int] NOT NULL
				);

			END -- [4.1.2]
			

			-- [4.1.3] Insert data into DB table.
			BEGIN

				INSERT INTO [dbo].[trendHits]([TrendlineId], [ExtremumGroupId])
				OUTPUT Inserted.[TrendHitId], Inserted.[TrendlineId], Inserted.[ExtremumGroupId] INTO #TempTrendHitsForIdentityMatching
				SELECT 
					[TrendlineId], 
					[ExtremumGroupId]
				FROM 
					#TrendHits;

			END -- [4.1.3]


			-- [4.1.4] Append IDs given by the DB engine to the records in the temporary table.
			BEGIN

				UPDATE th
				SET 
					[ProductionId] = h.[TrendHitId]
				FROM
					#TrendHits th
					LEFT JOIN #TempTrendHitsForIdentityMatching h 
									ON  th.[TrendlineId] = h.[TrendlineId] AND
										th.[ExtremumGroupId] = h.[ExtremumGroupId];

			END -- [4.1.4]


			-- [4.1.5] Drop temporary table.
			BEGIN
				DROP TABLE #TempTrendHitsForIdentityMatching;
			END -- [4.1.5]

		END -- [4.1]


		-- [4.2] Trend breaks
		BEGIN


			-- [4.2.0] Remove trend hits 
			BEGIN

				DELETE tb
				FROM 
					#TrendBreaks tb
					LEFT JOIN #ClosedTrendlines ct ON tb.[TrendlineId] = ct.[TrendlineId]
				WHERE
					ct.[EndDateIndex] IS NOT NULL AND tb.[DateIndex] > ct.[EndDateIndex];

			END -- [4.2.0]


			-- [4.2.1] Create temporary table to store IDs given by DB engine.
			BEGIN
				CREATE TABLE #TempTrendBreaksForIdentityMatching(
					[TrendBreakId] [int] NOT NULL,
					[TrendlineId] [int] NOT NULL,
					[DateIndex] [int] NOT NULL
				);
			END -- [4.2.1]


			-- [4.2.2] Insert remaining trendlines into TrendBreaks table.
			BEGIN

--SELECT 'DEBUG', '[4.2.2]' AS [4.2.2 #CurrentTrendBreaks], '[dbo].[TrendBreaks]', * FROM [dbo].[TrendBreaks];
--SELECT 'DEBUG', '[4.2.2]' AS [4.2.2 #TrendBreaksToBeAdded], '#TrendBreaksToBeAdded', * FROM #TrendBreaks;

				INSERT INTO [dbo].[TrendBreaks]([TrendlineId], [DateIndex], [BreakFromAbove])
				OUTPUT Inserted.[TrendBreakId], Inserted.[TrendlineId], Inserted.[DateIndex]
				INTO #TempTrendBreaksForIdentityMatching
				SELECT
					tb.[TrendlineId],
					tb.[DateIndex],
					tb.[BreakFromAbove]
				FROM
					#TrendBreaks tb;

			END -- [4.2.2]

				
			-- [4.2.3] Append IDs given by the DB engine to the records in the temporary table.
			BEGIN

				UPDATE tb
				SET 
					[ProductionId] = b.[TrendBreakId]
				FROM
					#TrendBreaks tb
					LEFT JOIN #TempTrendBreaksForIdentityMatching b
					ON  tb.[TrendlineId] = b.[TrendlineId] AND
						tb.[DateIndex] = b.[DateIndex];

			END -- [4.2.3]


			-- [4.2.4] Drop temporary table.
			BEGIN
				DROP TABLE #TempTrendBreaksForIdentityMatching;
			END -- [4.2.4]


		END -- [4.2] 


		-- [4.3] Trend ranges
		BEGIN

			
			-- [4.3.1] Create temporary table containing all breaks and hits from temporary tables.
			BEGIN

				SELECT
					*
				INTO
					#CurrentAnalysisBreaksAndHits
				FROM
					(SELECT [TrendlineId], [ProductionId], [DateIndex], 0 AS [IsHit]
					FROM #TrendBreaks
					UNION ALL
					SELECT [TrendlineId], [ProductionId], [DateIndex], 1 AS [IsHit]
					FROM #TrendHits) a;

			END -- [4.3.1]

--SELECT 'DEBUG', '[4.3.2]' AS [4.3.2], '#CurrentAnalysisBreaksAndHits', * FROM #CurrentAnalysisBreaksAndHits;


			-- [4.3.2] Create table containing one record for each trendline - with the last break or hit from previous analysis for this trendline.
			BEGIN

				SELECT
					[TrendlineId], [ProductionId], [DateIndex], [IsHit]
				INTO 
					#PrevAnalysisLastBreakOrHit
				FROM
					(SELECT
						*, 
						[number] = ROW_NUMBER() OVER (PARTITION BY [TrendlineId] ORDER BY [TrendlineId], [DateIndex] DESC)
					FROM
						(SELECT 
							tb.[TrendlineId], tb.[TrendBreakId] AS [ProductionId], tb.[DateIndex], 0 AS [IsHit] 
						FROM 
							(SELECT 
								tb.* 
							FROM 
								[dbo].[trendBreaks] tb
								INNER JOIN (SELECT * FROM [dbo].[trendlines] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) t 
												ON tb.[TrendlineId] = t.[TrendlineId]
							) tb

						UNION ALL
					
						SELECT 
							th.[TrendlineId], th.[TrendHitId] AS [ProductionId], eg.[EndDateIndex] AS [DateIndex], 1 AS [IsHit] 
						FROM 
							(SELECT 
								th.*
							FROM 
								[dbo].[trendHits] th
								INNER JOIN (SELECT * FROM [dbo].[trendlines] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) t 
												ON th.[TrendlineId] = t.[TrendlineId]
							) th
							LEFT JOIN (SELECT * FROM [dbo].[extremumGroups] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) eg ON th.[ExtremumGroupId] = eg.[ExtremumGroupId]
							) a
					WHERE
						CONCAT([ProductionId], [IsHit]) NOT IN (SELECT DISTINCT CONCAT([ProductionId], [IsHit]) FROM #CurrentAnalysisBreaksAndHits)) z
				WHERE z.[number] = 1;

			END -- [4.3.2]

--SELECT 'DEBUG', '[4.3.2]' AS [4.3.2], '#PrevAnalysisLastBreakOrHit', * FROM #PrevAnalysisLastBreakOrHit;


			-- [4.3.3] Create table with combined records from 4.3.1 and 4.3.2
			BEGIN

				SELECT
					*,
					[number] = ROW_NUMBER() OVER (ORDER BY [TrendlineId], [DateIndex])
				INTO
					#CombinedBreaksAndHits
				FROM
					(SELECT * FROM #CurrentAnalysisBreaksAndHits
					UNION ALL
					SELECT * FROM #PrevAnalysisLastBreakOrHit) a;

			END -- [4.3.3]

----SELECT 'DEBUG', '[4.3.3]' AS [4.3.3], '#CombinedBreaksAndHits', * FROM #CombinedBreaksAndHits;



			-- [4.3.4] Create pairs between two hits or a hit and a break from table #CombinedBreaksAndHits
			BEGIN
			
				DELETE FROM #TrendRanges;

				SELECT 
					cb1.[TrendlineId], cb1.[ProductionId] AS [BaseProductionId], cb1.[IsHit] AS [BaseIsHit], cb1.[DateIndex] AS [BaseDateIndex], cb2.[ProductionId] AS [CounterProductionId], cb2.[IsHit] AS [CounterIsHit], cb2.[DateIndex] AS [CounterDateIndex]
				INTO 
					#TrendRangeSourceData
				FROM 
					#CombinedBreaksAndHits cb1
					INNER JOIN #CombinedBreaksAndHits cb2
					ON  cb1.[TrendlineId] = cb2.[TrendlineId] AND
						cb1.[number] = cb2.[number] - 1;


					
--SELECT 'DEBUG', '[4.3.4]' AS [4.3.4 #CombinedBreaksAndHits], '#CombinedBreaksAndHits', * FROM #CombinedBreaksAndHits;
--SELECT 'DEBUG', '[4.3.4]' AS [4.3.4 #CurrentTrendRanges], '#TrendRanges', * FROM #TrendRanges;
--SELECT 'DEBUG', '[4.3.4]' AS [4.3.4 #TrendRangeSourceData], '#TrendRangeSourceData', * FROM #TrendRangeSourceData;


				INSERT INTO #TrendRanges([TrendlineId], [BaseId], [BaseIsHit], [BaseDateIndex], [CounterId], [CounterIsHit], [CounterDateIndex])
				SELECT
					*
				FROM
					#TrendRangeSourceData


			END -- [4.3.4]

----SELECT 'DEBUG', '[4.3.4]' AS [4.3.4 - Trend ranges after first insert], '#TrendRanges', * FROM #TrendRanges;



			-- [4.3.5] Update info if the given trend range is top or bottom.
			BEGIN

				UPDATE tr
				SET
					[IsPeak] = IIF(eg.[IsPeak] = 1, 1, -1)
				FROM
					#TrendRanges tr
					LEFT JOIN [dbo].[trendHits] th ON (tr.[CounterIsHit] = 0 AND tr.[BaseId] = th.[TrendHitId]) OR (tr.[CounterIsHit] = 1 AND tr.[CounterId] = th.[TrendHitId])
					LEFT JOIN #TrendHits tth ON (tr.[CounterIsHit] = 0 AND tr.[BaseId] = tth.[ProductionId]) OR (tr.[CounterIsHit] = 1 AND tr.[CounterId] = tth.[ProductionId])
					LEFT JOIN [dbo].[extremumGroups] eg ON (th.[ExtremumGroupId] = eg.[ExtremumGroupId]) OR (tth.[ExtremumGroupId] = eg.[ExtremumGroupId])
				WHERE
					eg.[ExtremumGroupId] IS NOT NULL;

			END -- [4.3.5]

----SELECT 'DEBUG', '[4.3.5]' AS [4.3.5 - Trend ranges after IsPeak update], '#TrendRanges', * FROM #TrendRanges;



			-- [4.3.6] Evaluate trend ranges.
			BEGIN


				-- [4.3.6.1] Create data to be used in evaluating function.
				BEGIN

					DECLARE @TrendRangeBasicData AS [dbo].[TrendRangeBasicData];

					SELECT
						tr.[TrendRangeId],
						--tr.[TrendlineId],
						t.[BaseDateIndex] AS [TrendlineStartDateIndex],
						t.[BaseLevel] AS [TrendlineStartLevel],
						t.[Angle] As [TrendlineAngle],
						IIF(tr.[BaseIsHit] = 1, eg.[EndDateIndex] + 1, tr.[BaseDateIndex]) AS [StartIndex],
						IIF(tr.[CounterIsHit] = 1, eg2.[StartDateIndex] - 1, tr.[CounterDateIndex]) AS [EndIndex],
						tr.[IsPeak]
					INTO
						#TrendRangesEvaluationSourceData
					FROM
						#TrendRanges tr
						LEFT JOIN [dbo].[trendHits] th ON tr.[BaseId] = th.[TrendHitId]
						LEFT JOIN [dbo].[extremumGroups] eg ON th.[ExtremumGroupId] = eg.[ExtremumGroupId]
						LEFT JOIN #TrendHits th2 ON tr.[CounterId] = th2.[ProductionId]
						LEFT JOIN [dbo].[extremumGroups] eg2 ON th2.[ExtremumGroupId] = eg2.[ExtremumGroupId]
						LEFT JOIN [dbo].[trendlines] t ON tr.[TrendlineId] = t.[TrendlineId];


----SELECT 'DEBUG', '[4.3.6.0a]' AS [4.3.6.0 - Trend ranges], '#TrendRanges', * FROM #TrendRanges;
----SELECT 'DEBUG', '[4.3.6.0b]' AS [4.3.6.0 - Trend hits], '#TrendHits', * FROM #TrendHits;
----SELECT 'DEBUG', '[4.3.6.0b]' AS [4.3.6.0 - Trend hits PROD], '[dbo].[trendHits]', * FROM [dbo].[trendHits];
----SELECT 'DEBUG', '[4.3.6.0c]' AS [4.3.6.0 - Extremum groups], '[dbo].[extremumGroups]', * FROM [dbo].[extremumGroups];
----SELECT 'DEBUG', '[4.3.6.0d]' AS [4.3.6.0 - Trend lines], '[dbo].[trendlines]', * FROM [dbo].[trendlines];
----SELECT 'DEBUG', '[4.3.6.1]' AS [4.3.6.1 - Trend ranges evaluation source data], '#TrendRangesEvaluationSourceData', * FROM #TrendRangesEvaluationSourceData;


					INSERT INTO @TrendRangeBasicData
					SELECT * FROM #TrendRangesEvaluationSourceData;

				END -- [4.3.6.1]


				-- [4.3.6.2] Update variation data.
				BEGIN

					UPDATE tr
					SET
						[TotalCandles] = v.[TotalCandles],
						[AverageVariation] = v.[TotalVariation] / v.[TotalCandles],
						[ExtremumVariation] = v.[ExtremumVariation],
						[OpenCloseVariation] = v.[OCVariation]
					FROM
						#TrendRanges tr
						LEFT JOIN [dbo].[GetTrendRangesVariations](@assetId, @timeframeId, @TrendRangeBasicData) v ON tr.[TrendRangeId] = v.[TrendRangeId]
				
				END -- [4.3.6.2]


				-- [4.3.6.3] Update cross data.
				BEGIN

					UPDATE tr
					SET
						[ExtremumPriceCrossPenaltyPoints] = v.[ExtremumPriceCrossPenaltyPoints],
						[ExtremumPriceCrossCounter] = v.[ExtremumPriceCrossCounter],
						[OCPriceCrossPenaltyPoints] = v.[OCPriceCrossPenaltyPoints],
						[OCPriceCrossCounter] = v.[OCPriceCrossCounter]
					FROM
						#TrendRanges tr
						LEFT JOIN [dbo].[GetTrendRangesCrossDetails](@assetId, @timeframeId, @TrendRangeBasicData) v ON tr.[TrendRangeId] = v.[TrendRangeId]

				END -- [4.3.6.3]


			END -- [4.3.6]

----SELECT 'DEBUG', '[4.3.6]' AS [4.3.6 - Trend ranges after evaluation], '#TrendRanges', * FROM #TrendRanges;


	
			-- [4.3.7] Move ranges to the production table.
			BEGIN

				INSERT INTO [dbo].[trendRanges]([TrendlineId], [BaseId], [BaseIsHit], [BaseDateIndex], [CounterId], [CounterIsHit], [CounterDateIndex], [IsPeak],
												[ExtremumPriceCrossPenaltyPoints], [ExtremumPriceCrossCounter], [OCPriceCrossPenaltyPoints], [OCPriceCrossCounter],
												[TotalCandles], [AverageVariation], [ExtremumVariation], [OpenCloseVariation], [BaseHitValue], [CounterHitValue])
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
					[CounterHitValue]
				FROM
					#TrendRanges;

			END -- [4.3.7]

----SELECT 'DEBUG', '[4.3.7]' AS [4.3.7 - Trend ranges after export to production table], '[dbo].[trendRanges]', * FROM [dbo].[trendRanges];



			-- [4.3.X] Clean up.
			BEGIN
				DROP TABLE #CurrentAnalysisBreaksAndHits;
				DROP TABLE #PrevAnalysisLastBreakOrHit;
				DROP TABLE #CombinedBreaksAndHits;
				DROP TABLE #TrendRangesEvaluationSourceData;
				DROP TABLE #TrendRangeSourceData;
			END -- [4.3.X]
		


		END -- [4.3]


		-- [4.4] Trendlines
		BEGIN

			UPDATE t
			SET
				t.[IsOpenFromRight] = ct.[IsOpenFromRight],
				t.[EndDateIndex] = ct.[EndDateIndex]
			FROM
				[dbo].[trendlines] t
				LEFT JOIN #ClosedTrendlines ct ON t.[TrendlineId] = ct.[TrendlineId]
			WHERE
				ct.[TrendlineId] IS NOT NULL;

		END -- [4.4]


	END -- [4]




	-- [5] Drop temporary tables.
	BEGIN

		DROP TABLE #TrendBreaks;
		DROP TABLE #TrendHits;
		DROP TABLE #TrendRanges;
		DROP TABLE #Trendlines;
		DROP TABLE #ClosedTrendlines;
		DROP TABLE #Quotes_AssetTimeframe;
		DROP TABLE #Quotes_Iteration;
		DROP TABLE #ExtremumGroups;

	END	-- [5]


--SELECT '[DEBUG]', '[analyzeTrendlinesRightSide]', 'finished';


END

GO
/****** Object:  StoredProcedure [dbo].[findNewExtrema]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE PROC [dbo].[findNewExtrema] @assetId AS INT, @timeframeId AS INT
AS
BEGIN

	DECLARE @lastAnalyzedIndex AS INT = [dbo].[GetLastExtremumAnalysisDate](@assetId, @timeframeId);
	DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);

	IF (@lastQuote > @lastAnalyzedIndex)
	BEGIN
	
		DECLARE @minDistance AS INT = [dbo].[GetExtremumMinDistance]();
		DECLARE @maxDistance AS INT = [dbo].[GetExtremumCheckDistance]();
		DECLARE @firstQuotation AS INT = (SELECT MIN([DateIndex]) FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId);
		DECLARE @startIndex AS INT = [dbo].[MaxValue](@lastAnalyzedIndex - @minDistance + 1, @firstQuotation + @minDistance);
		DECLARE @endIndex AS INT = @lastQuote - @minDistance;


		--Get minimum amount of data required to look for extrema (@startIndex/@endIndex range and @minDistance offset to both sides).
		SELECT
			q.[DateIndex],
			IIF(q.[Close] > q.[Open], q.[Close], q.[Open]) AS [MaxOC],
			IIF(q.[Close] < q.[Open], q.[Close], q.[Open]) AS [MinOC],
			q.[High],
			q.[Low]
		INTO 
			#QuotesForComparing
		FROM
			[dbo].[quotes] q
		WHERE
			q.[AssetId] = @assetId AND
			q.[TimeframeId] = @timeframeId AND
			q.[DateIndex] BETWEEN (@startIndex - @maxDistance) AND @lastQuote;
		CREATE NONCLUSTERED INDEX [ixDateIndex_QuotesForComparing] ON #QuotesForComparing ([DateIndex] ASC);


		--Insert new extrema to the database.
		BEGIN

			DECLARE @ocMaxPrices AS [dbo].[DateIndexPrice];
			DECLARE @ocMinPrices AS [dbo].[DateIndexPrice];
			DECLARE @lowPrices AS [dbo].[DateIndexPrice];
			DECLARE @highPrices AS [dbo].[DateIndexPrice];

			INSERT INTO @ocMaxPrices SELECT [DateIndex], [MaxOC] FROM #QuotesForComparing;
			INSERT INTO @ocMinPrices SELECT [DateIndex], [MinOC] FROM #QuotesForComparing;
			INSERT INTO @lowPrices SELECT [DateIndex], [Low] FROM #QuotesForComparing;
			INSERT INTO @highPrices SELECT [DateIndex], [High] FROM #QuotesForComparing;

			--Peak-by-close
			INSERT INTO [dbo].[extrema]([AssetId], [TimeframeId], [ExtremumTypeId], [DateIndex], [EarlierCounter], [EarlierAmplitude], [EarlierTotalArea], [EarlierAverageArea], [EarlierChange1], [EarlierChange2], [EarlierChange3], [EarlierChange5], [EarlierChange10]) 
			SELECT 
				@assetId, @TimeframeId, 1, a.[DateIndex], COALESCE(a.[EarlierCounter], a.[DateIndex] - @firstQuotation),
				a.[EarlierAmplitude], a.[TotalArea], a.[TotalArea] / COALESCE(a.[EarlierCounter], a.[DateIndex] - @firstQuotation), 
				a.[Delta1], a.[Delta2], a.[Delta3], a.[Delta5], a.[Delta10]
			FROM [dbo].[FindExtremaForSingleExtremumType](@startIndex, @endIndex, 1, @minDistance, @maxDistance, @ocMaxPrices, @lowPrices) a;
		
			--Peak-by-high
			INSERT INTO [dbo].[extrema]([AssetId], [TimeframeId], [ExtremumTypeId], [DateIndex], [EarlierCounter], [EarlierAmplitude], [EarlierTotalArea], [EarlierAverageArea], [EarlierChange1], [EarlierChange2], [EarlierChange3], [EarlierChange5], [EarlierChange10]) 
			SELECT 
				@assetId, @TimeframeId, 2, a.[DateIndex], COALESCE(a.[EarlierCounter], a.[DateIndex] - @firstQuotation),
				a.[EarlierAmplitude], a.[TotalArea], a.[TotalArea] / COALESCE(a.[EarlierCounter], a.[DateIndex] - @firstQuotation), 
				a.[Delta1], a.[Delta2], a.[Delta3], a.[Delta5], a.[Delta10]
			FROM [dbo].[FindExtremaForSingleExtremumType](@startIndex, @endIndex, 1, @minDistance, @maxDistance, @highPrices, @lowPrices) a;

			--Trough-by-close
			INSERT INTO [dbo].[extrema]([AssetId], [TimeframeId], [ExtremumTypeId], [DateIndex], [EarlierCounter], [EarlierAmplitude], [EarlierTotalArea], [EarlierAverageArea], [EarlierChange1], [EarlierChange2], [EarlierChange3], [EarlierChange5], [EarlierChange10]) 
			SELECT 
				@assetId, @TimeframeId, 3, a.[DateIndex], COALESCE(a.[EarlierCounter], a.[DateIndex] - @firstQuotation),
				a.[EarlierAmplitude], a.[TotalArea], a.[TotalArea] / COALESCE(a.[EarlierCounter], a.[DateIndex] - @firstQuotation), 
				a.[Delta1], a.[Delta2], a.[Delta3], a.[Delta5], a.[Delta10]
			FROM [dbo].[FindExtremaForSingleExtremumType](@startIndex, @endIndex, -1, @minDistance, @maxDistance, @ocMinPrices, @highPrices) a;

			--Trough-by-low
			INSERT INTO [dbo].[extrema]([AssetId], [TimeframeId], [ExtremumTypeId], [DateIndex], [EarlierCounter], [EarlierAmplitude], [EarlierTotalArea], [EarlierAverageArea], [EarlierChange1], [EarlierChange2], [EarlierChange3], [EarlierChange5], [EarlierChange10]) 
			SELECT 
				@assetId, @TimeframeId, 4, a.[DateIndex], COALESCE(a.[EarlierCounter], a.[DateIndex] - @firstQuotation),
				a.[EarlierAmplitude], a.[TotalArea], a.[TotalArea] / COALESCE(a.[EarlierCounter], a.[DateIndex] - @firstQuotation), 
				a.[Delta1], a.[Delta2], a.[Delta3], a.[Delta5], a.[Delta10]
			FROM [dbo].[FindExtremaForSingleExtremumType](@startIndex, @endIndex, -1, @minDistance, @maxDistance, @lowPrices, @highPrices) a;

		END


		--Clean-up
		BEGIN
			DROP TABLE #QuotesForComparing;
		END
			
	END	


END


GO
/****** Object:  StoredProcedure [dbo].[findNewTrendlines]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO














CREATE PROC [dbo].[findNewTrendlines] @assetId AS INT, @timeframeId AS INT
AS
BEGIN

	DECLARE @lastAnalyzedIndex AS INT = [dbo].[GetTrendlinesAnalysisLastQuotationIndex](@assetId, @timeframeId);
	DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);
	DECLARE @lastExtremumGroupId AS INT = [dbo].[GetTrendlinesAnalysisLastExtremumGroupId](@assetId, @timeframeId);
	DECLARE @trendlineCheckDistance AS INT = [dbo].[GetTrendlineCheckDistance]();
	DECLARE @extremumMinDistance AS INT = [dbo].[GetExtremumMinDistance]();

	IF (@lastQuote > @lastAnalyzedIndex)
	BEGIN


		--Create table with extremum groups for pairing.
		SELECT
			*
		INTO
			#ExtremumGroupsForPairing
		FROM
			[dbo].[extremumGroups] eg
		WHERE
			eg.[ExtremumGroupId] > @lastExtremumGroupId - @trendlineCheckDistance - @extremumMinDistance;
		

		--Select all extrema group without trendlines.
		SELECT
			*
		INTO
			#UnprocessedExtremumGroups
		FROM
			#ExtremumGroupsForPairing eg
		WHERE
			eg.[ExtremumGroupId] > @lastExtremumGroupId;
		

		--Get price levels for extrema pairing.
		DECLARE @minPrice AS FLOAT = (SELECT [dbo].[MinValue](MIN([ExtremumPriceLevel]), MIN([OCPriceLevel])) FROM #ExtremumGroupsForPairing);
		DECLARE @maxPrice AS FLOAT = (SELECT [dbo].[MaxValue](MAX([ExtremumPriceLevel]), MAX([OCPriceLevel])) FROM #ExtremumGroupsForPairing);
		DECLARE @stepFactor AS FLOAT = [dbo].[GetAssetCalculatingTrendlineStepFactor](@assetId);
		SELECT * INTO #PossiblePriceLevels FROM [dbo].[GetTrendlineExtremaPairingPriceLevels](@minPrice, @maxPrice, @stepFactor);



		-- Price levels and DateIndex for specific extrema.
		SELECT
			eg.[ExtremumGroupId],
			pl.[Level],
			IIF((eg.[IsPeak] = IIF(pl.[Level] <= eg.[MiddlePriceLevel], 1, -1)), eg.[MasterDateIndex], eg.[SlaveDateIndex]) AS [DateIndex]
		INTO
			#PriceLevelForExtremumGroups
		FROM
			(SELECT *, IIF([IsPeak] = 1, [OCPriceLevel], [ExtremumPriceLevel]) AS [Min], IIF([IsPeak] = 1, [ExtremumPriceLevel], [OCPriceLevel]) AS [Max] FROM #ExtremumGroupsForPairing) eg
			LEFT JOIN #PossiblePriceLevels pl
			ON pl.[level] BETWEEN [Min] AND [Max];

		-- Select prospective extremum group pairs.
		SELECT
			egfp.[ExtremumGroupId] AS [BaseExtremumGroupId],
			ueg.[ExtremumGroupId] AS [CounterExtremumGroupId]
		INTO
			#ExtremumGroupPairs
		FROM
			#UnprocessedExtremumGroups ueg
			INNER JOIN #ExtremumGroupsForPairing egfp
			ON ueg.[StartDateIndex] - egfp.[EndDateIndex] <= @trendlineCheckDistance AND ueg.[EndDateIndex] - egfp.[StartDateIndex] >= @extremumMinDistance;
			

		-- Create trendlines.
		INSERT INTO [dbo].[trendlines]([AssetId], [TimeframeId], [BaseExtremumGroupId], [BaseDateIndex], [BaseLevel], [CounterExtremumGroupId], [CounterDateIndex], [CounterLevel], [Angle], [CandlesDistance])
		SELECT 
			@assetId,
			@timeframeId,
			eg.[BaseExtremumGroupId],
			pl1.[DateIndex] AS [BaseDateIndex],
			pl1.[level] AS [BaseLevel],
			eg.[CounterExtremumGroupId],
			pl2.[DateIndex] AS [CounterDateIndex],
			pl2.[level] AS [CounterLevel], 
			(pl1.[level] - pl2.[level]) / (pl1.[DateIndex] - pl2.[DateIndex]) AS [Angle],
			pl2.[DateIndex] - pl1.[DateIndex] AS [CandlesDistance]
		FROM
			#ExtremumGroupPairs eg
			LEFT JOIN #PriceLevelForExtremumGroups pl1 ON eg.[BaseExtremumGroupId] = pl1.[ExtremumGroupId]
			LEFT JOIN #PriceLevelForExtremumGroups pl2 ON eg.[CounterExtremumGroupId] = pl2.[ExtremumGroupId]
		--WHERE
		--	pl1.[DateIndex] = 40 AND
		--	pl1.[Level] = 104.70 AND
		--	pl2.[DateIndex] = 53 AND
		--	pl2.[Level] = 104.78;


		--Clean-up
		BEGIN
			DROP TABLE #UnprocessedExtremumGroups;
			DROP TABLE #ExtremumGroupsForPairing;
			DROP TABLE #PossiblePriceLevels;
			DROP TABLE #PriceLevelForExtremumGroups;
			DROP TABLE #ExtremumGroupPairs;
		END

	END

END


GO
/****** Object:  StoredProcedure [dbo].[insertMissingQuotations]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[insertMissingQuotations] @assetId AS INT, @timeframeId AS INT
AS
BEGIN

	DECLARE @minIndex AS INT = (SELECT MIN([DateIndex]) FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId);
	DECLARE @maxIndex AS INT = (SELECT MAX([DateIndex]) FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId);
	DECLARE @firstMissing AS INT;

	SELECT * INTO #TempQuotes FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;

	SELECT 
		d.[DateIndex], d.[TimeframeId]
	INTO
		#MissingQuotations
	FROM 
		#TempQuotes q
		RIGHT JOIN (SELECT * FROM [dbo].[dates] WHERE [TimeframeId] = 4 AND [DateIndex] BETWEEN @minIndex AND @maxIndex) d
		ON q.[DateIndex] = d.[DateIndex]
	WHERE
		q.[DateIndex] IS NULL

	SET @firstMissing = (SELECT MIN([DateIndex]) FROM #MissingQuotations);

	SELECT
		m.[DateIndex],
		MAX(q.[DateIndex]) AS [PreviousExisting]
	INTO
		#MissingExistingPairs
	FROM
		#MissingQuotations m
		LEFT JOIN (SELECT * FROM #TempQuotes WHERE [DateIndex] >= (@firstMissing - 1)) q
		ON m.[DateIndex] > q.[DateIndex]
	GROUP BY
		m.[DateIndex]

	INSERT INTO [dbo].[quotes]([AssetId], [TimeframeId], [DateIndex], [Open], [High], [Low], [Close], [Volume])
	SELECT
		@assetId AS [AssetId],
		@timeframeId AS [TimeframeId],
		mep.[DateIndex] AS [DateIndex],
		q.[Close] AS [Open],
		q.[Close] AS [High],
		q.[Close] AS [Low],
		q.[Close] AS [Close],
		0 AS [Volume]
	FROM
		#MissingExistingPairs mep
		LEFT JOIN #TempQuotes q
		ON mep.[PreviousExisting] = q.[DateIndex];


	--Clean up
	BEGIN

		DROP TABLE #TempQuotes;
		DROP TABLE #MissingQuotations;
		DROP TABLE #MissingExistingPairs;

	END

END


GO
/****** Object:  StoredProcedure [dbo].[populateDateTable]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[processExtrema]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[processExtrema] @assetId AS INT, @timeframeId AS INT
AS
BEGIN
	
	EXEC [dbo].[findNewExtrema] @assetId = @assetId, @timeframeId = @timeframeId;
	EXEC [dbo].[analyzeExtrema] @assetId = @assetId, @timeframeId = @timeframeId;
	EXEC [dbo].[updateExtremaGroups] @assetId = @assetId, @timeframeId = @timeframeId;

	--Update timestamp.
	BEGIN

		DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);

		UPDATE [dbo].[timestamps] 
		SET [ExtremaLastAnalyzedIndex] = @lastQuote
		WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId
		IF @@ROWCOUNT=0
			INSERT INTO [dbo].[timestamps]([AssetId], [TimeframeId], [ExtremaLastAnalyzedIndex]) 
			VALUES (@assetId, @timeframeId, @lastQuote);
		
	END

END


GO
/****** Object:  StoredProcedure [dbo].[processTrendlines]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[processTrendlines] @assetId AS INT, @timeframeId AS INT
AS
BEGIN
	
	EXEC [dbo].[findNewTrendlines] @assetId = @assetId, @timeframeId = @timeframeId;
	EXEC [dbo].[analyzeTrendlinesLeftSide] @assetId = @assetId, @timeframeId = @timeframeId;
	EXEC [dbo].[analyzeTrendlinesRightSide] @assetId = @assetId, @timeframeId = @timeframeId;
	EXEC [dbo].[updateTrendEvaluations] @assetId = @assetId, @timeframeId = @timeframeId;


	--Update timestamp.
	BEGIN

		DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);
		DECLARE @lastExtremumGroup AS INT = (SELECT MAX([ExtremumGroupId]) FROM [dbo].[extremumGroups] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId);

		UPDATE [dbo].[timestamps] 
		SET 
			[TrendlinesAnalysisLastQuotationIndex] = @lastQuote,
			[TrendlinesAnalysisLastExtremumGroupId] = @lastExtremumGroup
		WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId
		IF @@ROWCOUNT=0
			INSERT INTO [dbo].[timestamps]([AssetId], [TimeframeId], [TrendlinesAnalysisLastQuotationIndex]) 
			VALUES (@assetId, @timeframeId, @lastQuote);
		
	END

END


GO
/****** Object:  StoredProcedure [dbo].[removeAllTables]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[test_addQuoteFromRawH1]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[test_addQuoteFromRawH1] @counter AS INTEGER
AS
BEGIN

	DECLARE @items AS [dbo].[QuotesTransferTable];
	DECLARE @assetId AS INT = 1;
	DECLARE @timeframeId AS INT = 4;
	DECLARE @lastDate AS DATETIME;
	
	SET @lastDate = (SELECT
					MAX(d.[Date]) AS [LastQuoteDate]
				FROM 
					(SELECT * FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) q
					LEFT JOIN (SELECT * FROM [dbo].[dates] WHERE [TimeframeId] = @timeframeId) d
					ON q.[DateIndex] = d.[DateIndex]);
	
	INSERT INTO @items
	SELECT TOP (@counter)
		d.*,
		0 AS [Volume]
	FROM 
		[dbo].[RawH1Data] d
	WHERE
		d.[Date] > COALESCE(@lastDate, CAST('1970-01-01' AS DATETIME))
	ORDER BY
		d.[Date] ASC;

	EXEC [dbo].[addNewQuote] @assetId = @assetId, @timeframeId = @timeframeId, @quotes = @items;

END


GO
/****** Object:  StoredProcedure [dbo].[updateExtremaGroups]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[updateExtremaGroups] @assetId AS INT, @timeframeId AS INT
AS
BEGIN

	DECLARE @lastAnalyzedIndex AS INT = [dbo].[GetLastExtremumAnalysisDate](@assetId, @timeframeId);
	DECLARE @minDistance AS INT = [dbo].[GetExtremumMinDistance]();
	DECLARE @maxDistance AS INT = [dbo].[GetExtremumCheckDistance]();
	DECLARE @startIndex AS INT = @lastAnalyzedIndex - @maxDistance;
	DECLARE @masterSlaveMaxDistance AS INT = @minDistance - 1;
	
	
	-- Prepare extrema and quotes sub-tables.
	SELECT *
	INTO #ExtremaForMasterExtremumAnalysis
	FROM [dbo].[extrema] e
	WHERE
		e.[AssetId] = @assetId AND
		e.[TimeframeId] = @timeframeId AND
		e.[DateIndex] >= @startIndex - @minDistance AND 
		e.[Value] > 50;

	SELECT *
	INTO #ExtremumGroupsForMasterExtremumAnalysis
	FROM [dbo].[extremumGroups] eg
	WHERE
		eg.[AssetId] = @assetId AND
		eg.[TimeframeId] = @timeframeId AND
		eg.[EndDateIndex] >= @startIndex - @minDistance;

	SELECT 
		q.[DateIndex],
		[dbo].MaxValue(q.[Open], q.[Close]) AS [MaxOC],
		[dbo].MinValue(q.[Open], q.[Close]) AS [MinOC],
		q.[High],
		q.[Low]
	INTO #QuotesForMasterExtremumAnalysis 
	FROM [dbo].[quotes] q
	WHERE 
		q.[AssetId] = @assetId AND 
		q.[TimeframeId] = @timeframeId AND 
		q.[DateIndex] >= @startIndex - @minDistance;


	--Find all extrema without group.
	SELECT
		e.*
	INTO
		#ExtremaWithoutGroups
	FROM
		#ExtremaForMasterExtremumAnalysis e
		LEFT JOIN #ExtremumGroupsForMasterExtremumAnalysis eg
		ON e.[ExtremumId] = eg.[MasterExtremumId] OR e.[ExtremumId] = eg.[SlaveExtremumId]
	WHERE
		eg.[ExtremumGroupId] IS NULL;


	--Match extrema without group with other extrema.
	SELECT
		ROW_NUMBER() OVER (ORDER BY a.[MasterExtremumId]) AS [Row],
		a.[MasterExtremumId],
		a.[SlaveExtremumId]
	INTO
		#ExtremaPairs
	FROM
		(SELECT DISTINCT
				IIF(ewg.[ExtremumTypeId] % 2 = 1, ewg.[ExtremumId], COALESCE(emea.[ExtremumId], ewg.[ExtremumId])) AS [MasterExtremumId],
				IIF(ewg.[ExtremumTypeId] % 2 = 1, COALESCE(emea.[ExtremumId], ewg.[ExtremumId]), ewg.[ExtremumId]) AS [SlaveExtremumId]
			FROM
				#ExtremaWithoutGroups ewg
				LEFT JOIN #ExtremaForMasterExtremumAnalysis emea
				ON 
					SIGN(ewg.[ExtremumTypeId] - 2.5) = SIGN(emea.[ExtremumTypeId] - 2.5) AND
					ABS(ewg.[DateIndex] - emea.[DateIndex]) BETWEEN 0 AND @masterSlaveMaxDistance) a;


	SELECT
		ewg.[ExtremumId],
		ep.[Row],
		ABS(ep.[MasterExtremumId] - ep.[SlaveExtremumId]) AS [Dif]
	INTO
		#ExtremaWithoutGroupsWithDiff
	FROM
		#ExtremaWithoutGroups ewg
		LEFT JOIN #ExtremaPairs ep
		ON ewg.[ExtremumId] = ep.[MasterExtremumId] OR ewg.[ExtremumId] = ep.[SlaveExtremumId];


	SELECT
		IIF(b.[ExtremumTypeId] <= 2, 1, -1) AS [IsPeak],
		b.[ExtremumId] AS [MasterExtremumId],
		c.[ExtremumId] AS [SlaveExtremumId],
		b.[DateIndex] AS [MasterDateIndex],
		c.[DateIndex] AS [SlaveDateIndex],
		IIF(b.[DateIndex] < c.[DateIndex], b.[DateIndex], c.[DateIndex]) AS [StartDateIndex],
		IIF(b.[DateIndex] < c.[DateIndex], c.[DateIndex], b.[DateIndex]) AS [EndDateIndex]
	INTO
		#NewExtremumGroups_WithoutQuotes
	FROM
		(SELECT DISTINCT
			ep.*
		FROM
			#ExtremaWithoutGroupsWithDiff a
			INNER JOIN (SELECT ewgwd.[ExtremumId], MAX(ewgwd.[Dif]) AS [MaxDif] FROM #ExtremaWithoutGroupsWithDiff ewgwd GROUP BY ewgwd.[ExtremumId]) b
			ON a.[ExtremumId] = b.[ExtremumId] AND a.[Dif] = b.[MaxDif]
			LEFT JOIN #ExtremaPairs ep
			ON a.[Row] = ep.[Row]) a
		LEFT JOIN #ExtremaForMasterExtremumAnalysis b ON a.[MasterExtremumId] = b.[ExtremumId]
		LEFT JOIN #ExtremaForMasterExtremumAnalysis c ON a.[SlaveExtremumId] = c.[ExtremumId]



	SELECT
		neg.*,
		IIF(neg.[IsPeak] = 1, q1.[MaxOC], q1.[MinOC]) AS [OCPriceLevel],
		IIF(neg.[IsPeak] = 1, q2.[High], q2.[Low]) AS [ExtremumPriceLevel],
		IIF(neg.[IsPeak] = 1, q1.[High], q1.[Low]) AS [MiddlePriceLevel]
	INTO
		#NewExtremumGroups_WithQuotes
	FROM
		#NewExtremumGroups_WithoutQuotes neg
		LEFT JOIN #QuotesForMasterExtremumAnalysis q1
		ON neg.[MasterDateIndex] = q1.[DateIndex]
		LEFT JOIN #QuotesForMasterExtremumAnalysis q2
		ON neg.[SlaveDateIndex] = q2.[DateIndex];
		


	--Remove duplicated extrema from DB table.
	DELETE e
	FROM
		[dbo].[ExtremumGroups] e
		LEFT JOIN #NewExtremumGroups_WithQuotes neg
		ON e.[MasterExtremumId] = neg.[MasterExtremumId] OR e.[MasterExtremumId] = neg.[SlaveExtremumId] OR e.[SlaveExtremumId] = neg.[MasterExtremumId] OR e.[SlaveExtremumId] = neg.[SlaveExtremumId]
	WHERE
		neg.[MasterExtremumId] IS NOT NULL;


	--Add new extrema groups to the DB table.
	INSERT INTO [dbo].[ExtremumGroups] ([AssetId], [TimeframeId], [IsPeak], [MasterExtremumId], [SlaveExtremumId], [MasterDateIndex], [SlaveDateIndex], [StartDateIndex], [EndDateIndex], [OCPriceLevel], [ExtremumPriceLevel], [MiddlePriceLevel])
	SELECT @assetId, @timeframeId, neg.* FROM #NewExtremumGroups_WithQuotes neg;


	--Clean-up
	BEGIN
		DROP TABLE #ExtremaForMasterExtremumAnalysis;
		DROP TABLE #ExtremumGroupsForMasterExtremumAnalysis;
		DROP TABLE #QuotesForMasterExtremumAnalysis;
		DROP TABLE #ExtremaWithoutGroups;
		DROP TABLE #ExtremaPairs;
		DROP TABLE #ExtremaWithoutGroupsWithDiff;
		DROP TABLE #NewExtremumGroups_WithoutQuotes;
		DROP TABLE #NewExtremumGroups_WithQuotes;
	END

END


GO
/****** Object:  StoredProcedure [dbo].[updateTrendEvaluations]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[updateTrendEvaluations] @assetId AS INT, @timeframeId AS INT
AS
BEGIN

	DECLARE @lastAnalyzedIndex AS INT = [dbo].[GetTrendlinesAnalysisLastQuotationIndex](@assetId, @timeframeId);
	DECLARE @extremumCheckDistance AS INT = [dbo].[GetExtremumCheckDistance]();
	DECLARE @maxDeviationFromTrendline AS FLOAT = 0.001;
	DECLARE @trendHitEvaluation_DistanceShare AS FLOAT = 0.4;

	-- [1] Update TrendHits table.
	BEGIN
		
		-- [1.1] Select trend hits to be updated.
		SELECT
			th.[TrendHitId],
			th.[TrendlineId],
			eg.[ExtremumGroupId],
			e.[ExtremumId],
			e.[DateIndex] AS [ExtremumDateIndex],
			IIF(e.[DateIndex] = eg.[MasterDateIndex], 1, 0) AS [IsMasterExtremum],
			e.[Value] AS [ExtremumValue],
			t.[Angle] * (e.[DateIndex] - t.[BaseDateIndex]) + t.[BaseLevel] AS [TrendlineLevel],
			IIF(eg.[IsPeak] = -1, q.[Low], q.[High]) AS [ExtremumPriceLevel],
			IIF(eg.[IsPeak] = IIF(q.[Close] > q.[Open], 1, 0), q.[Close], q.[Open]) AS [OCPriceLevel]
		INTO
			#FilteredTrendHits
		FROM
			[dbo].[trendHits] th
			LEFT JOIN [dbo].[trendlines] t ON th.[TrendlineId] = t.[TrendlineId]
			LEFT JOIN (SELECT * FROM [dbo].[extremumGroups] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) eg ON th.[ExtremumGroupId] = eg.[ExtremumGroupId]
			LEFT JOIN (SELECT * FROM [dbo].[extrema] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) e ON (eg.[MasterExtremumId] = e.[ExtremumId] AND e.[ExtremumTypeId] IN (1, 3)) OR (eg.[SlaveExtremumId] = e.[ExtremumId] AND e.[ExtremumTypeId] IN (2, 4))
			LEFT JOIN (SELECT * FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId) q ON e.[DateIndex] = q.[DateIndex]
		WHERE 
			t.[AssetId] = @assetId AND 
			t.[TimeframeId] = @timeframeId AND 
			eg.[EndDateIndex] >= (@lastAnalyzedIndex - @extremumCheckDistance);

		-- [1.2] Create table with trend hit values.
		SELECT 
			  c.[TrendHitId]
			, c.[PointsForDistance] + c.[PointsForValue] AS [TrendHitValue]
			, [number] = ROW_NUMBER() OVER(PARTITION BY c.[TrendHitId] ORDER BY c.[TrendHitId] ASC, (c.[PointsForDistance] + c.[PointsForValue]) DESC)
		INTO 
			#TrendHitsValues
		FROM
			(SELECT
				b.*,
				b.[Gap] / b.[PriceLevel] AS [RelativeGap],
				(@maxDeviationFromTrendline - (b.[Gap] / b.[PriceLevel])) * 100000 * @trendHitEvaluation_DistanceShare AS [PointsForDistance],
				(1.0 - @trendHitEvaluation_DistanceShare) * b.[ExtremumValue] AS [PointsForValue]
			FROM
				(SELECT
					a.*,
					IIF([IsMasterExtremum] = 1, ABS(a.[TrendlineLevel] - a.[ExtremumPriceLevel]), ABS(a.[TrendlineLevel] - a.[OCPriceLevel])) AS [Gap],
					IIF([IsMasterExtremum] = 1, a.[ExtremumPriceLevel], a.[OCPriceLevel]) AS [PriceLevel]
				FROM #FilteredTrendHits a
				) b
			) c

		-- [1.3] Update destination table with values.
		UPDATE th
		SET th.[Value] = thv.[TrendHitValue]
		FROM
			[dbo].[trendHits] th
			LEFT JOIN (SELECT * FROM #TrendHitsValues WHERE [number] = 1) thv ON th.[TrendHitId] = thv.[TrendHitId]

	END


	-- [X] Clean up.
	BEGIN
		DROP TABLE #FilteredTrendHits;
		DROP TABLE #TrendHitsValues;
	END


END



GO
/****** Object:  Trigger [dbo].[Trigger_Assets_Delete]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
		DELETE FROM [dbo].[adx] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
		DELETE FROM [dbo].[assets] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);	

GO
/****** Object:  Trigger [dbo].[Trigger_Assets_Update]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Trigger [dbo].[Trigger_Currencies_Delete]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Trigger_Currencies_Delete] ON [dbo].[currencies] INSTEAD OF DELETE
AS
BEGIN
	DELETE FROM [dbo].[assets] WHERE [BaseCurrencyId] IN (SELECT [CurrencyId] FROM deleted)
	DELETE FROM [dbo].[assets] WHERE [CounterCurrencyId] IN (SELECT [CurrencyId] FROM deleted)
	DELETE FROM [dbo].[currencies] WHERE [CurrencyId] IN (SELECT [CurrencyId] FROM deleted)
END

GO
/****** Object:  Trigger [dbo].[Trigger_Currencies_Update]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Trigger_Currencies_Update] ON [dbo].[currencies] AFTER UPDATE
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
/****** Object:  Trigger [dbo].[Trigger_Dates_Delete]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE TRIGGER [dbo].[Trigger_Dates_Delete] ON [dbo].[dates] INSTEAD OF DELETE
	AS

		SET NOCOUNT ON
		DELETE FROM [dbo].[quotes] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);
		DELETE FROM [dbo].[extrema] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);
		DELETE FROM [dbo].[trendlines] WHERE [BaseDateIndex] IN (SELECT [DateIndex] FROM deleted);
		DELETE FROM [dbo].[trendlines] WHERE [CounterDateIndex] IN (SELECT [DateIndex] FROM deleted);
		DELETE FROM [dbo].[adx] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);
		DELETE FROM [dbo].[dates] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);

GO
/****** Object:  Trigger [dbo].[Trigger_Extrema_Delete]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Trigger_Extrema_Delete] ON [dbo].[extrema]
INSTEAD OF DELETE
AS
	SET NOCOUNT ON
	DELETE FROM [dbo].[extremumGroups] WHERE [MasterExtremumId] IN (SELECT [ExtremumId] FROM deleted);
	DELETE FROM [dbo].[extremumGroups] WHERE [SlaveExtremumId] IN (SELECT [ExtremumId] FROM deleted);
	DELETE FROM [dbo].[extrema] WHERE [ExtremumId] IN (SELECT [ExtremumId] FROM deleted);

GO
/****** Object:  Trigger [dbo].[Trigger_ExtremumGroups_Delete]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Trigger [dbo].[Trigger_ExtremumTypes_Delete]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Trigger_ExtremumTypes_Delete] ON [dbo].[extremumTypes]
INSTEAD OF DELETE
AS
	SET NOCOUNT ON
	DELETE FROM [dbo].[extrema] WHERE [ExtremumTypeId] IN (SELECT [ExtremumTypeId] FROM deleted);
	DELETE FROM [dbo].[extremumTypes] WHERE [ExtremumTypeId] IN (SELECT [ExtremumTypeId] FROM deleted);


GO
/****** Object:  Trigger [dbo].[Trigger_Timeframes_Delete]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
		DELETE FROM [dbo].[adx] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
		DELETE FROM [dbo].[timeframes] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);

GO
/****** Object:  Trigger [dbo].[Trigger_Trendlines_Delete]    Script Date: 2018-08-25 14:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
USE [master]
GO
ALTER DATABASE [fx] SET  READ_WRITE 
GO
