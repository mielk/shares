use [stock];
GO

IF OBJECT_ID('RunIteration','P') IS NOT NULL DROP PROC [dbo].[RunIteration];
IF OBJECT_ID('RunIterations','P') IS NOT NULL DROP PROC [dbo].[RunIterations];
GO

CREATE PROC [dbo].[RunIterations] (
	@AssetId AS INT,
	@TimeframeId AS INT,
	@IterationCounter AS INT, 
	@IterationStep AS INT, 
	@DebugMode AS INT, 
	@RunAdx AS INT,
	@RunMacd AS INT
) AS
BEGIN

	DECLARE @iteration AS INT = 1;

	EXEC [dbo].[SetDebugMode] @value = @DebugMode;

	IF (@DebugMode = 1) 
	BEGIN
		PRINT '';
		PRINT '';
		PRINT '--------------------------------------------------------------------------------------';
		PRINT '[RUN ITERATIONS - START]'
		PRINT '    AssetId: ' + CAST(@AssetId AS NVARCHAR(255));
		PRINT '    TimeframeId: ' + CAST(@TimeframeId AS NVARCHAR(255));
		PRINT '    IterationCounter: ' + CAST(@IterationCounter AS NVARCHAR(255));
		PRINT '    IterationStep: ' + CAST(@IterationStep AS NVARCHAR(255));
		PRINT '    DebugMode: ' + CAST(@DebugMode AS NVARCHAR(255));
		PRINT '    RunAdx: ' + CAST(@RunAdx AS NVARCHAR(255));
		PRINT '    RunMacd: ' + CAST(@RunMacd AS NVARCHAR(255));
		PRINT '--------------------------------------------------------------------------------------';
		PRINT '';
		PRINT '';
	END


	WHILE (@iteration <= @IterationCounter)
	BEGIN

		SET NOCOUNT ON;

		IF (@DebugMode = 1) 
		BEGIN
			PRINT '<Iteration: ' + CAST(@iteration AS NVARCHAR(255)) + '> ==================================';
			PRINT '';
			PRINT '   <Parameters before>';
			PRINT '       First price: ' + CAST([dbo].[GetFirstQuote](@AssetId, @TimeframeId) AS NVARCHAR(255));
			PRINT '       Last price: ' + CAST([dbo].[GetLastQuote](@AssetId, @TimeframeId) AS NVARCHAR(255));
			PRINT '       Extrema last analyzed index: ' + CAST([dbo].[GetLastExtremumAnalysisDate](@AssetId, @TimeframeId) AS NVARCHAR(255));
			PRINT '       Trendlines - last quotation index: ' + CAST([dbo].[GetTrendlinesAnalysisLastQuotationIndex](@AssetId, @TimeframeId) AS NVARCHAR(255));
			PRINT '       Trendlines - last extremum group id: ' + CAST([dbo].[GetTrendlinesAnalysisLastExtremumGroupId](@AssetId, @TimeframeId) AS NVARCHAR(255));
			PRINT '       ADX last analyzed index: ' + CAST([dbo].[GetLastAdxAnalysisDate](@AssetId, @TimeframeId) AS NVARCHAR(255));
			PRINT '       MACD last analyzed index: ' + CAST([dbo].[GetLastMacdAnalysisDate](@AssetId, @TimeframeId) AS NVARCHAR(255));
			PRINT '       --------------------------';
			PRINT '       Asset calculating trendlines step factor: ' + CAST([dbo].[GetAssetCalculatingTrendlineStepFactor](@AssetId) AS NVARCHAR(255));
			PRINT '       Extremum minimum distance: ' + CAST([dbo].[GetExtremumMinDistance]() AS NVARCHAR(255));
			PRINT '       Extremum check distance: ' + CAST([dbo].[GetExtremumCheckDistance]() AS NVARCHAR(255));
			PRINT '       Trendline check distance: ' + CAST([dbo].[GetTrendlineCheckDistance]() AS NVARCHAR(255));
			PRINT '   </Parameters before>';
			PRINT '';
			PRINT '------------------------------------------------------------------------------------';
			PRINT '';
		END

		-- Adding quotations to proper table.
		EXEC [dbo].[test_addQuoteFromRawD1] @assetId = @AssetId, @timeframeId = @TimeframeId, @counter = @IterationStep, @debugMode = @DebugMode;

		---- ADX
		--IF (@RunAdx = 1) EXEC [dbo].[processAdx] @assetId = @AssetId, @timeframeId = @TimeframeId, @debugMode = 0;
		
		---- MACD
		--IF (@RunMacd = 1) EXEC [dbo].[processMacd] @assetId = @AssetId, @timeframeId = @TimeframeId, @debugMode = 0;

		-- Extrema
		EXEC [dbo].[processExtrema] @assetId = @AssetId, @timeframeId = @TimeframeId, @debugMode = @DebugMode;

		-- Trendlines
		EXEC [dbo].[processTrendlines] @assetId = @AssetId, @timeframeId = @TimeframeId, @debugMode = @DebugMode;

		IF (@DebugMode = 1) 
		BEGIN
			PRINT '';
			PRINT '';
			PRINT '';
			PRINT '';
			PRINT '';
			PRINT '';
			PRINT '';
			PRINT '';
			PRINT '';
			PRINT '';
		END


		SET @iteration = @iteration + 1;


	END	

END