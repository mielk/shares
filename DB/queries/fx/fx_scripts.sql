USE [master]
GO
/****** Object:  Database [fx]    Script Date: 2018-01-06 03:56:14 ******/
CREATE DATABASE [fx]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'fx', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\fx.mdf' , SIZE = 643072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'fx_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\fx_log.ldf' , SIZE = 321088KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
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
/****** Object:  Table [dbo].[assets]    Script Date: 2018-01-06 03:56:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[assets](
	[Id] [int] NOT NULL,
	[Uuid] [nvarchar](36) NOT NULL DEFAULT (newid()),
	[BaseCurrencyId] [int] NOT NULL,
	[CounterCurrencyId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [Default_Campaigns_IsActive]  DEFAULT ((1)),
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Campaigns_CreatedDate]  DEFAULT (getdate()),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [Default_Campaigns_ModifiedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_assets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[currencies]    Script Date: 2018-01-06 03:56:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[currencies](
	[Id] [int] NOT NULL,
	[Uuid] [nvarchar](36) NOT NULL DEFAULT (newid()),
	[Name] [nvarchar](255) NOT NULL,
	[Symbol] [nvarchar](3) NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [Default_Markets_IsActive]  DEFAULT ((1)),
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Markets_CreatedDate]  DEFAULT (getdate()),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [Default_Markets_ModifiedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_currencies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[dates]    Script Date: 2018-01-06 03:56:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dates](
	[DateIndex] [int] NOT NULL DEFAULT ((1)),
	[Timeframe] [int] NOT NULL DEFAULT ((6)),
	[Date] [datetime] NOT NULL,
	[ParentLevelDateIndex] [int] NULL,
 CONSTRAINT [PK_dates] PRIMARY KEY CLUSTERED 
(
	[DateIndex] ASC,
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[errorLogs]    Script Date: 2018-01-06 03:56:14 ******/
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
/****** Object:  Table [dbo].[predefinedNumbers]    Script Date: 2018-01-06 03:56:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[predefinedNumbers](
	[number] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tempData]    Script Date: 2018-01-06 03:56:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tempData](
	[Date] [datetime] NOT NULL,
	[Open] [float] NOT NULL,
	[High] [float] NOT NULL,
	[Low] [float] NOT NULL,
	[Close] [float] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[timeframes]    Script Date: 2018-01-06 03:56:14 ******/
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
/****** Object:  Index [ixBaseCurrency_assets]    Script Date: 2018-01-06 03:56:14 ******/
CREATE NONCLUSTERED INDEX [ixBaseCurrency_assets] ON [dbo].[assets]
(
	[BaseCurrencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixCounterCurrency_assets]    Script Date: 2018-01-06 03:56:14 ******/
CREATE NONCLUSTERED INDEX [ixCounterCurrency_assets] ON [dbo].[assets]
(
	[CounterCurrencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixCurrencies_assets]    Script Date: 2018-01-06 03:56:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixCurrencies_assets] ON [dbo].[assets]
(
	[BaseCurrencyId] ASC,
	[CounterCurrencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [ixName_markets]    Script Date: 2018-01-06 03:56:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixName_markets] ON [dbo].[currencies]
(
	[Name] ASC
)
WHERE ([IsActive]=(1))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDate_dates]    Script Date: 2018-01-06 03:56:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixDate_dates] ON [dbo].[dates]
(
	[Date] ASC,
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDateIndex_dates]    Script Date: 2018-01-06 03:56:14 ******/
CREATE NONCLUSTERED INDEX [ixDateIndex_dates] ON [dbo].[dates]
(
	[DateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDateIndexTimeframe_dates]    Script Date: 2018-01-06 03:56:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ixDateIndexTimeframe_dates] ON [dbo].[dates]
(
	[DateIndex] ASC,
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixParentDateIndex_dates]    Script Date: 2018-01-06 03:56:14 ******/
CREATE NONCLUSTERED INDEX [ixParentDateIndex_dates] ON [dbo].[dates]
(
	[ParentLevelDateIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixTimeframe_dates]    Script Date: 2018-01-06 03:56:14 ******/
CREATE NONCLUSTERED INDEX [ixTimeframe_dates] ON [dbo].[dates]
(
	[Timeframe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixNumber_predefinedNumbers]    Script Date: 2018-01-06 03:56:14 ******/
CREATE NONCLUSTERED INDEX [ixNumber_predefinedNumbers] ON [dbo].[predefinedNumbers]
(
	[number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ixDate_tempData]    Script Date: 2018-01-06 03:56:14 ******/
CREATE NONCLUSTERED INDEX [ixDate_tempData] ON [dbo].[tempData]
(
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[errorLogs] ADD  CONSTRAINT [Default_ErrorLogs_Timestamp]  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[assets]  WITH CHECK ADD  CONSTRAINT [FK_Assets_BaseCurrency] FOREIGN KEY([BaseCurrencyId])
REFERENCES [dbo].[currencies] ([Id])
GO
ALTER TABLE [dbo].[assets] CHECK CONSTRAINT [FK_Assets_BaseCurrency]
GO
ALTER TABLE [dbo].[assets]  WITH CHECK ADD  CONSTRAINT [FK_Assets_CounterCurrency] FOREIGN KEY([CounterCurrencyId])
REFERENCES [dbo].[currencies] ([Id])
GO
ALTER TABLE [dbo].[assets] CHECK CONSTRAINT [FK_Assets_CounterCurrency]
GO
ALTER TABLE [dbo].[dates]  WITH CHECK ADD  CONSTRAINT [FK_dates_timeframe] FOREIGN KEY([Timeframe])
REFERENCES [dbo].[timeframes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[dates] CHECK CONSTRAINT [FK_dates_timeframe]
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
USE [master]
GO
ALTER DATABASE [fx] SET  READ_WRITE 
GO
