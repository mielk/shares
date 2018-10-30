USE [stock];

BEGIN TRANSACTION
	
	
	-- [1] Create table with indexes and foreign keys.
	BEGIN

		CREATE TABLE [dbo].[macd] (
			[AssetId] [int] NOT NULL,
			[TimeframeId] [int] NOT NULL,
			[DateIndex] [int] NOT NULL,
			[MA12] [float] NULL,
			[EMA12] [float] NULL,
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


		ALTER TABLE [dbo].[macd]  WITH CHECK ADD  CONSTRAINT [FK_Macd_DateIndex] FOREIGN KEY([DateIndex], [TimeframeId])
		REFERENCES [dbo].[dates] ([DateIndex], [TimeframeId])


		CREATE NONCLUSTERED INDEX [ixAssetId_Macd] ON [dbo].[macd] ([AssetId] ASC) 
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE UNIQUE NONCLUSTERED INDEX [ixAssetTimeframeDateIndex_Macd] ON [dbo].[macd] ([AssetId] ASC, [TimeframeId] ASC, [DateIndex] ASC)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixDateIndex_Macd] ON [dbo].[macd] ([DateIndex] ASC)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixTimeframe_Macd] ON [dbo].[macd] ([TimeframeId] ASC)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


	END


	-- [2] Alter triggers to include [adx] table.
	GO

	ALTER TRIGGER [dbo].[Trigger_Assets_Delete] ON [dbo].[assets] INSTEAD OF DELETE
	AS

		SET NOCOUNT ON
		DELETE FROM [dbo].[timestamps] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
		DELETE FROM [dbo].[quotes] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
		DELETE FROM [dbo].[quotesOutOfDate] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
		DELETE FROM [dbo].[extrema] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
		DELETE FROM [dbo].[extremumGroups] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
		DELETE FROM [dbo].[trendlines] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
		DELETE FROM [dbo].[macd] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
		DELETE FROM [dbo].[adx] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
		DELETE FROM [dbo].[assets] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);	
	GO

	ALTER TRIGGER [dbo].[Trigger_Timeframes_Delete] ON [dbo].[timeframes] INSTEAD OF DELETE
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
		DELETE FROM [dbo].[macd] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
		DELETE FROM [dbo].[timeframes] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	GO

	ALTER TRIGGER [dbo].[Trigger_Dates_Delete] ON [dbo].[dates] INSTEAD OF DELETE
	AS

		SET NOCOUNT ON
		DELETE FROM [dbo].[quotes] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);
		DELETE FROM [dbo].[extrema] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);
		DELETE FROM [dbo].[trendlines] WHERE [BaseDateIndex] IN (SELECT [DateIndex] FROM deleted);
		DELETE FROM [dbo].[trendlines] WHERE [CounterDateIndex] IN (SELECT [DateIndex] FROM deleted);
		DELETE FROM [dbo].[adx] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);
		DELETE FROM [dbo].[macd] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);
		DELETE FROM [dbo].[dates] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);
	GO


	-- Commented to avoid deadlock --
	--[3a] Create necessary User-Defined data types
	--IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'SourceForEmaCalculation') DROP TYPE [dbo].[SourceForEmaCalculation];
	--GO

	--CREATE TYPE [dbo].[SourceForEmaCalculation] AS TABLE(
	--	[DateIndex] [int] NOT NULL,
	--	[Close] [float] NULL,
	--	[Ema] [float] NULL,
	--	[Number] [int] NOT NULL
	--); 

	
	--[3b] Create helper functions.
	IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLastMacdAnalysisDate]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetLastMacdAnalysisDate];

	GO 

	CREATE FUNCTION [dbo].[GetLastMacdAnalysisDate](@assetId AS INT, @timeframeId AS INT)
	RETURNS INT
	AS
	BEGIN
		DECLARE @index AS INT;
		SELECT @index = [MacdLastAnalyzedIndex] FROM [dbo].[timestamps] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;
		RETURN IIF(@index IS NULL, 0, @index);
	END

	GO


	IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLastQuote]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetLastQuote]

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



	IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetFirstQuote]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetFirstQuote]

	GO

	CREATE FUNCTION [dbo].[GetFirstQuote](@assetId AS INT, @timeframeId AS INT)
	RETURNS INT
	AS
	BEGIN
		DECLARE @index AS INT;
		SELECT @index = MIN([DateIndex]) FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId AND [IsComplete] = 1;
		RETURN IIF(@index IS NULL, 0, @index);
	END

	GO


	IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalculateEma]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[CalculateEma]
	GO

	CREATE FUNCTION [dbo].[CalculateEma](
		@emaLength AS INT,
		@values AS [dbo].[SourceForEmaCalculation] READONLY
	)
	RETURNS @result TABLE(
						[DateIndex] [int] NOT NULL,
						[Close] [float] NOT NULL,
						[Ema] [float] NULL,
						[factor] [float] NULL,
						[prevEma] [float] NULL,
						[Number] [int] NOT NULL)
	AS
	BEGIN

		DECLARE @minNumber AS INT = (SELECT MIN([Number]) FROM @values WHERE [Ema] IS NULL);
		DECLARE @maxNumber AS INT = (SELECT MAX([Number]) FROM @values WHERE [Ema] IS NULL);
		DECLARE @i AS INT = @minNumber;
		DECLARE @factor AS FLOAT = 2.0 / (@emaLength + 1);

		--INSERT @result SELECT * FROM @values;
		INSERT INTO @result([DateIndex], [Close], [Ema], [Number])
		SELECT * FROM @values;

		WHILE @i <= @maxNumber
		BEGIN
			
			DECLARE @prevEma AS FLOAT = (SELECT [Ema] FROM @result WHERE [number] = @i - 1);
			DECLARE @close AS FLOAT = (SELECT [Close] FROM @result WHERE [number] = @i);
			
			UPDATE @result
			SET 
				[Ema] = @factor * (@close - @prevEma) + @prevEma
				, [factor] = @factor
				, [prevEma] = @prevEma
			WHERE [number] = @i;

			SET @i = @i + 1;

		END;
			
		RETURN

	END

	GO



	-- [4] Creating controlling procedure for processing ADX.
	IF OBJECT_ID('processMacd','P') IS NOT NULL DROP PROC [dbo].[processMacd];
	GO

	CREATE PROC [dbo].[processMacd] @assetId AS INT, @timeframeId AS INT, @debugMode AS INT
	AS
	BEGIN
	
		IF (@debugMode = 1)
		BEGIN
			PRINT ' ____________________________________';
			PRINT '* PROC STARTED [processMacd]';
			PRINT '*      @assetId: ' + CAST(@assetId AS NVARCHAR(255));
			PRINT '*      @timeframeId: ' + CAST(@timeframeId AS NVARCHAR(255));
			PRINT '';
			PRINT '';
			PRINT '[*] MACD last analyzed index (before): ' + CAST([dbo].[GetLastMacdAnalysisDate](@AssetId, @TimeframeId) AS NVARCHAR(255));
		END

		EXEC [dbo].[calculateMacd] @assetId = @assetId, @timeframeId = @timeframeId;

		--Update timestamp.
		BEGIN
			
			DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);

			UPDATE [dbo].[timestamps] 
			SET [MacdLastAnalyzedIndex] = @lastQuote
			WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId
			IF @@ROWCOUNT=0
				INSERT INTO [dbo].[timestamps]([AssetId], [TimeframeId], [MacdLastAnalyzedIndex]) 
				VALUES (@assetId, @timeframeId, @lastQuote);
			
		END

		IF (@debugMode = 1)
		BEGIN
			PRINT '[*] MACD last analyzed index (after): ' + CAST([dbo].[GetLastMacdAnalysisDate](@AssetId, @TimeframeId) AS NVARCHAR(255));
			PRINT '';
			PRINT '[#] MACD data after process:';
			SELECT * FROM [dbo].[macd] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;
			PRINT '';
			PRINT '* PROC ENDED [processAdx]';
			PRINT ' ____________________________________';
			PRINT '';
			PRINT '';
			PRINT '';
		END


	END

	GO



	-- [5] Creating function for actual analyzing MACD.
	IF OBJECT_ID('calculateMacd','P') IS NOT NULL DROP PROC [dbo].[calculateMacd];
	GO

	CREATE PROC [dbo].[calculateMacd] @assetId AS INT, @timeframeId AS INT
	AS
	BEGIN
		DECLARE @shortEma AS INT = 12;
		DECLARE @longEma AS INT = 26;
		DECLARE @signalLine AS INT = 9;
		DECLARE @lastAnalyzedIndex AS INT = [dbo].[GetLastMacdAnalysisDate](@assetId, @timeframeId);
		DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);
		DECLARE @firstQuote AS INT = [dbo].[GetFirstQuote](@assetId, @timeframeId);
		DECLARE @sourceData AS [dbo].[SourceForEmaCalculation];


		IF (@lastQuote > @lastAnalyzedIndex)
		BEGIN
			
			-- [1] Filter quotes and MACDs necessary for analysis.
			BEGIN

				SELECT 
					q.[DateIndex],
					q.[Close]
				INTO
					#Quotes
				FROM
					[dbo].[quotes] q
				WHERE
					q.[AssetId] = @assetId AND 
					q.[TimeframeId] = @timeframeId AND
					q.[DateIndex] > (@lastAnalyzedIndex - @longEma);

				SELECT 
					m.*
				INTO
					#CurrentMacdData
				FROM
					[dbo].[macd] m
				WHERE
					m.[AssetId] = @assetId AND 
					m.[TimeframeId] = @timeframeId AND
					m.[DateIndex] > (@lastAnalyzedIndex - @longEma);

			END


			-- [2] Add short-term EMA
			BEGIN

				-- [2.1] Append MA short
				SELECT
					a.*,
					IIF([DateIndex] <= (@shortEma + @firstQuote - 1), a.[MaShort], NULL) AS [EmaShort]
				INTO
					#WithEmaShort
				FROM
					(SELECT 
						q1.[DateIndex],
						q1.[Close],
						AVG(q2.[Close]) AS [MaShort]
					FROM 
						(SELECT * FROM #Quotes WHERE [DateIndex] >= @lastAnalyzedIndex) q1
						LEFT JOIN #Quotes q2
							ON q2.[DateIndex] BETWEEN q1.[DateIndex] - @shortEma + 1 AND q1.[DateIndex]
					GROUP BY
						q1.[DateIndex], q1.[Close]) a

				-- [2.2] Calculate EMA short for quotations > @shortEma.
				DELETE FROM @sourceData;
				INSERT INTO @sourceData
				SELECT
					m.[DateIndex],
					m.[Close],
					COALESCE(c.[Ema12], m.[EmaShort]) AS [EmaShort],
					[number] = ROW_NUMBER() OVER(ORDER BY m.[DateIndex] ASC)
				FROM
					(SELECT * FROM #WithEmaShort WHERE [DateIndex] >= (@shortEma + @firstQuote - 1)) m
					LEFT JOIN #CurrentMacdData c ON m.[DateIndex] = c.[DateIndex]

				UPDATE wes
				SET
					wes.[EmaShort] = ce.[Ema]
				FROM
					#WithEmaShort wes
					LEFT JOIN [dbo].[CalculateEma](@shortEma, @sourceData) ce ON wes.[DateIndex] = ce.[DateIndex]
				WHERE
					wes.[EmaShort] IS NULL;
				
			END


			-- [3] Add long-term EMA
			BEGIN

				-- [3.1] Append MA long
				SELECT
					a.*,
					IIF([DateIndex] <= (@longEma + @firstQuote - 1), a.[MaLong], NULL) AS [EmaLong]
				INTO
					#WithEmaLong
				FROM
					(SELECT 
						q1.[DateIndex],
						q1.[Close],
						AVG(q2.[Close]) AS [MaLong]
					FROM 
						(SELECT * FROM #Quotes WHERE [DateIndex] >= @lastAnalyzedIndex) q1
						LEFT JOIN #Quotes q2
							ON q2.[DateIndex] BETWEEN q1.[DateIndex] - @longEma + 1 AND q1.[DateIndex]
					GROUP BY
						q1.[DateIndex], q1.[Close]) a

				-- [3.2] Calculate EMA long for quotations > @longEma.
				DELETE FROM @sourceData;
				INSERT INTO @sourceData
				SELECT
					m.[DateIndex],
					m.[Close],
					COALESCE(c.[Ema26], m.[EmaLong]) AS [EmaLong],
					[number] = ROW_NUMBER() OVER(ORDER BY m.[DateIndex] ASC)
				FROM
					(SELECT * FROM #WithEmaLong WHERE [DateIndex] >= (@longEma + @firstQuote - 1)) m
					LEFT JOIN #CurrentMacdData c ON m.[DateIndex] = c.[DateIndex]

				UPDATE wel
				SET
					wel.[EmaLong] = ce.[Ema]
				FROM
					#WithEmaLong wel
					LEFT JOIN [dbo].[CalculateEma](@longEma, @sourceData) ce ON wel.[DateIndex] = ce.[DateIndex]
				WHERE
					wel.[EmaLong] IS NULL;
				

			END


			-- [4] Add signal line
			BEGIN

				-- [4.1] Calculate signal line for quotations > @signalLine.
				DELETE FROM @sourceData;
				INSERT INTO @sourceData
				SELECT
					q.[DateIndex],
					COALESCE(c.[MACDLine], wes.[EmaShort] - wel.[EmaLong]) AS [Macd],
					COALESCE(c.[SignalLine], IIF(q.[DateIndex] <= (@signalLine + @firstQuote - 1), 0, NULL)) AS [Signal],
					[number] = ROW_NUMBER() OVER(ORDER BY q.[DateIndex] ASC)
				FROM
					(SELECT * FROM #Quotes WHERE [DateIndex] >= @lastAnalyzedIndex) q
					LEFT JOIN #WithEmaShort wes ON q.[DateIndex] = wes.[DateIndex]
					LEFT JOIN #WithEmaLong wel ON q.[DateIndex] = wel.[DateIndex]
					LEFT JOIN #CurrentMacdData c ON q.[DateIndex] = c.[DateIndex];

				SELECT 
					ce.[DateIndex],
					ce.[Close] AS [Macd],
					ce.[Ema] AS [Signal]
				INTO 
					#WithSignal
				FROM 
					[dbo].[CalculateEma](@signalLine, @sourceData) ce;

			END


			-- [5] Update MACD table.
			BEGIN
				
				INSERT INTO [dbo].[macd]([AssetId], [TimeframeId], [DateIndex], [MA12], [EMA12], [MA26], [EMA26], [MACDLine], [SignalLine], [Histogram])
				SELECT
					@assetId,
					@timeframeId,
					q.[DateIndex],
					wes.[MaShort],
					wes.[EmaShort],
					wel.[MaLong],
					wel.[EmaLong],
					--0, 0, 0
					ws.[Macd],
					ws.[Signal],
					ws.[Macd] - ws.[Signal]
				FROM
					(SELECT * FROM #Quotes WHERE [DateIndex] > @lastAnalyzedIndex) q 
					LEFT JOIN #WithEmaShort wes ON q.[DateIndex] = wes.[DateIndex]
					LEFT JOIN #WithEmaLong wel ON q.[DateIndex] = wel.[DateIndex]
					LEFT JOIN #WithSignal ws ON q.[DateIndex] = ws.[DateIndex];
				
			END


			SELECT * FROM [dbo].[macd];


			--Clean up
			BEGIN
				DROP TABLE #Quotes;
				DROP TABLE #CurrentMacdData;
				DROP TABLE #WithEmaShort;
				DROP TABLE #WithEmaLong;
				DROP TABLE #WithSignal;
			END

		END

	END


GO


COMMIT TRANSACTION