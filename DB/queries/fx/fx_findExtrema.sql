USE [fx];

BEGIN TRANSACTION

GO

--Drop procedures
IF OBJECT_ID('analyzeExtrema','P') IS NOT NULL DROP PROC [dbo].[analyzeExtrema];
IF OBJECT_ID('findNewExtrema','P') IS NOT NULL DROP PROC [dbo].[findNewExtrema];

--Drop functions
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLastExtremumAnalysisDate]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetLastExtremumAnalysisDate]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLastQuote]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetLastQuote]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExtremumCheckDistance]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetExtremumCheckDistance]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExtremumMinDistance]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetExtremumMinDistance]

GO

CREATE FUNCTION [dbo].[GetLastExtremumAnalysisDate](@assetId AS INT, @timeframeId AS INT)
RETURNS INT
AS
BEGIN
	DECLARE @index AS INT;
	SELECT @index = [ExtremaLastAnalyzedIndex] FROM [dbo].[timestamps] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;
	RETURN IIF(@index IS NULL, 0, @index);
END

GO

CREATE FUNCTION [dbo].[GetLastQuote](@assetId AS INT, @timeframeId AS INT)
RETURNS INT
AS
BEGIN
	DECLARE @index AS INT;
	SELECT @index = MAX([DateIndex]) FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;
	RETURN IIF(@index IS NULL, 0, @index);
END

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

CREATE FUNCTION [dbo].[GetExtremumMinDistance]()
RETURNS INT
AS
BEGIN
	DECLARE @value AS INT;
	SELECT @value = [SettingValue] FROM [dbo].[settingsNumeric] WHERE [SettingName] = 'ExtremumAnalysisMinDistance';
	RETURN IIF(@value IS NULL, 0, @value);
END

GO



CREATE PROC [dbo].[findNewExtrema] @assetId AS INT, @timeframeId AS INT, @lastAnalyzedIndex AS INT, @lastQuote AS INT
AS
BEGIN

	DECLARE @minDistance AS INT = [dbo].[GetExtremumMinDistance]();
	DECLARE @startIndex AS INT = [dbo].[MaxValue](@lastAnalyzedIndex - @minDistance + 1, 0);
	DECLARE @endIndex AS INT = @lastQuote - @minDistance;

		
	PRINT CONCAT('Let''s analyze extrema | Start index: ', @startIndex, ' | End index: ', @endIndex);
	
	PRINT 'TODO'

END


GO


CREATE PROC [dbo].[analyzeExtrema] @assetId AS INT, @timeframeId AS INT
AS
BEGIN
	
	DECLARE @lastAnalyzedIndex AS INT = [dbo].[GetLastExtremumAnalysisDate](@assetId, @timeframeId);
	DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);
	
	--DECLARE @checkDistance AS INT = [dbo].[GetExtremumCheckDistance]();

	IF (@lastQuote > @lastAnalyzedIndex)
	BEGIN
		
		EXEC [dbo].[findNewExtrema] @assetId = @assetId, @timeframeId = @timeframeId, @lastAnalyzedIndex = @lastAnalyzedIndex, @lastQuote = @lastQuote;

	END

	--PRINT @lastAnalyzedIndex;
	--PRINT @lastQuote;
	--PRINT @checkDistance;
	--PRINT @minDistance;

END

GO


EXEC [dbo].[analyzeExtrema] @assetId = 1, @timeframeId = 4;

ROLLBACK TRANSACTION;