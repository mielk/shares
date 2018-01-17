USE [fx];

BEGIN TRANSACTION

GO

--Drop procedures
IF OBJECT_ID('findNewExtrema','P') IS NOT NULL DROP PROC [dbo].[findNewExtrema];
IF OBJECT_ID('processExtrema','P') IS NOT NULL DROP PROC [dbo].[processExtrema];
IF OBJECT_ID('analyzeExtrema','P') IS NOT NULL DROP PROC [dbo].[analyzeExtrema];


--Drop functions
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLastExtremumAnalysisDate]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetLastExtremumAnalysisDate]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLastQuote]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetLastQuote]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExtremumCheckDistance]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetExtremumCheckDistance]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExtremumMinDistance]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetExtremumMinDistance]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FindExtremaForSingleExtremumType]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[FindExtremaForSingleExtremumType]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExtremaWithEvaluationOpened]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetExtremaWithEvaluationOpened]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewExtrema]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetNewExtrema]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalculateExtremaRightSideProperties]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[CalculateExtremaRightSideProperties]

--Types
--IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'DateIndexPrice') DROP TYPE [dbo].[DateIndexPrice];
--IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'IdAndDateIndex') DROP TYPE [dbo].[IdAndDateIndex];

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
						@isPeak * COALESCE(ex2.[DateIndex] - MIN(bp3.[DateIndex]), ex2.[TotalLater]) AS [LaterCounter],
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


CREATE PROC [dbo].[analyzeExtrema] @assetId AS INT, @timeframeId AS INT
AS
BEGIN

	DECLARE @lastAnalyzedIndex AS INT = [dbo].[GetLastExtremumAnalysisDate](@assetId, @timeframeId);
	DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);
	DECLARE @maxDistance AS INT = [dbo].[GetExtremumCheckDistance]();

	IF (@lastQuote > @lastAnalyzedIndex)
	BEGIN
		
		--Create temporary table with all evaluation-opened tables.
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
				LEFT JOIN (
					SELECT * FROM [dbo].[CalculateExtremaRightSideProperties](1, @maxDistance, @peaksByClose, @ocMaxPrices, @lowPrices)
					UNION ALL
					SELECT * FROM [dbo].[CalculateExtremaRightSideProperties](1, @maxDistance, @peaksByHigh, @highPrices, @lowPrices)
					UNION ALL
					SELECT * FROM [dbo].[CalculateExtremaRightSideProperties](-1, @maxDistance, @troughsByClose, @ocMinPrices, @highPrices)
					UNION ALL
					SELECT * FROM [dbo].[CalculateExtremaRightSideProperties](-1, @maxDistance, @troughsByLow, @lowPrices, @highPrices)
				) a
				ON e.[ExtremumId] = a.[ExtremumId]

		END


		--Clean up
		BEGIN
			DROP TABLE #EvaluationOpenedExtrema;
			DROP TABLE #QuotesForComparing;
		END

			
	END

END
GO



CREATE PROC [dbo].[processExtrema] @assetId AS INT, @timeframeId AS INT
AS
BEGIN
	
	EXEC [dbo].[findNewExtrema] @assetId = @assetId, @timeframeId = @timeframeId;
	EXEC [dbo].[analyzeExtrema] @assetId = @assetId, @timeframeId = @timeframeId;

	--PRINT @lastAnalyzedIndex;
	--PRINT @lastQuote;
	--PRINT @checkDistance;
	--PRINT @minDistance;

END

GO





EXEC [dbo].[processExtrema] @assetId = 1, @timeframeId = 4;

SELECT * FROM [dbo].[extrema];

ROLLBACK TRANSACTION;