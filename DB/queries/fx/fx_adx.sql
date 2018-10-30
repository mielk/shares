USE [stock];

BEGIN TRANSACTION
	
	
	---- [1] Create table with indexes and foreign keys.
	--BEGIN

	--	CREATE TABLE [dbo].[adx] (
	--		[AssetId] [int] NOT NULL,
	--		[TimeframeId] [int] NOT NULL,
	--		[DateIndex] [int] NOT NULL,
	--		[TR] [float] NULL,
	--		[DM1Pos] [float] NULL,
	--		[DM1Neg] [float] NULL,
	--		[TR14] [float] NULL,
	--		[DM14Pos] [float] NULL,
	--		[DM14Neg] [float] NULL,
	--		[DI14Pos] [float] NULL,
	--		[DI14Neg] [float] NULL,
	--		[DI14Diff] [float] NULL,
	--		[DI14Sum] [float] NULL,
	--		[DX] [float] NULL,
	--		[ADX] [float] NULL,
	--		[DaysUnder20] [int] DEFAULT NULL,
	--		[DaysUnder15] [int] DEFAULT NULL,
	--		[Cross20] [float] NULL,
	--		[DeltaDIPos] [float] NULL,
	--		[DeltaDINeg] [float] NULL,
	--		[DeltaADX] [float] NULL,
	--		[DIPosDirection3D] [int] DEFAULT NULL,
	--		[DIPosDirection2D] [int] DEFAULT NULL,
	--		[DINegDirection3D] [int] DEFAULT NULL,
	--		[DINegDirection2D] [int] DEFAULT NULL,
	--		[ADXDirection3D] [int] DEFAULT NULL,
	--		[ADXDirection2D] [int] DEFAULT NULL,
	--		[DIPosDirectionChanged] [int] DEFAULT NULL,
	--		[DINegDirectionChanged] [int] DEFAULT NULL,
	--		[ADXDirectionChanged] [int] DEFAULT NULL,
	--		[DIDifference] [float] NULL,
	--		[DILinesCrossing] [int] DEFAULT NULL,
	--		[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Adx_CreatedDate]  DEFAULT (GETDATE())
	--	);


	--	ALTER TABLE [dbo].[adx]  WITH CHECK ADD  CONSTRAINT [FK_Adx_AssetId] FOREIGN KEY([AssetId])
	--	REFERENCES [dbo].[assets] ([AssetId])


	--	ALTER TABLE [dbo].[adx]  WITH CHECK ADD  CONSTRAINT [FK_Adx_DateIndex] FOREIGN KEY([DateIndex], [TimeframeId])
	--	REFERENCES [dbo].[dates] ([DateIndex], [TimeframeId])


	--	CREATE NONCLUSTERED INDEX [ixAssetId_Adx] ON [dbo].[adx] ([AssetId] ASC) 
	--	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	--	CREATE UNIQUE NONCLUSTERED INDEX [ixAssetTimeframeDateIndex_Adx] ON [dbo].[adx] ([AssetId] ASC, [TimeframeId] ASC, [DateIndex] ASC)
	--	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	--	CREATE NONCLUSTERED INDEX [ixDateIndex_Adx] ON [dbo].[adx] ([DateIndex] ASC)
	--	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	--	CREATE NONCLUSTERED INDEX [ixTimeframe_Adx] ON [dbo].[adx] ([TimeframeId] ASC)
	--	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


	--END


	---- [2] Alter triggers to include [adx] table.
	--GO

	--ALTER TRIGGER [dbo].[Trigger_Assets_Delete] ON [dbo].[assets] INSTEAD OF DELETE
	--AS

	--	SET NOCOUNT ON
	--	DELETE FROM [dbo].[timestamps] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
	--	DELETE FROM [dbo].[quotes] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
	--	DELETE FROM [dbo].[quotesOutOfDate] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
	--	DELETE FROM [dbo].[extrema] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
	--	DELETE FROM [dbo].[extremumGroups] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
	--	DELETE FROM [dbo].[trendlines] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
	--	DELETE FROM [dbo].[adx] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);
	--	DELETE FROM [dbo].[assets] WHERE [AssetId] IN (SELECT [AssetId] FROM deleted);	
	--GO

	--ALTER TRIGGER [dbo].[Trigger_Timeframes_Delete] ON [dbo].[timeframes] INSTEAD OF DELETE
	--AS

	--	SET NOCOUNT ON
	--	DELETE FROM [dbo].[timestamps] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	--	DELETE FROM [dbo].[dates] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	--	DELETE FROM [dbo].[quotes] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	--	DELETE FROM [dbo].[quotesOutOfDate] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	--	DELETE FROM [dbo].[extrema] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	--	DELETE FROM [dbo].[extremumGroups] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	--	DELETE FROM [dbo].[trendlines] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	--	DELETE FROM [dbo].[adx] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	--	DELETE FROM [dbo].[timeframes] WHERE [TimeframeId] IN (SELECT [TimeframeId] FROM deleted);
	--GO

	--ALTER TRIGGER [dbo].[Trigger_Dates_Delete] ON [dbo].[dates] INSTEAD OF DELETE
	--AS

	--	SET NOCOUNT ON
	--	DELETE FROM [dbo].[quotes] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);
	--	DELETE FROM [dbo].[extrema] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);
	--	DELETE FROM [dbo].[trendlines] WHERE [BaseDateIndex] IN (SELECT [DateIndex] FROM deleted);
	--	DELETE FROM [dbo].[trendlines] WHERE [CounterDateIndex] IN (SELECT [DateIndex] FROM deleted);
	--	DELETE FROM [dbo].[adx] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);
	--	DELETE FROM [dbo].[dates] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);
	--GO


	-- Commented to avoid deadlock --

	--[3a] Create necessary User-Defined data types
	--IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'SourceForTrAvg') DROP TYPE [dbo].[SourceForTrAvg];
	--GO

	--CREATE TYPE [dbo].[SourceForTrAvg] AS TABLE(
	--	[DateIndex] [int] NOT NULL,
	--	[Tr] [float] NULL,
	--	[Avg] [float] NULL,
	--	[Number] [int] NOT NULL
	--); 

	
	--[3b] Create helper functions.
	IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLastAdxAnalysisDate]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[GetLastAdxAnalysisDate]

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


	IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalculateTrAvgOrSum]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[CalculateTrAvgOrSum]
	GO
	CREATE FUNCTION [dbo].[CalculateTrAvgOrSum](
		@startIndex AS INT,
		@countAverage AS BIT,
		@dataSampleSize AS INT,
		@values AS [dbo].[SourceForTrAvg] READONLY
	)
	RETURNS @result TABLE(
						[DateIndex] [int] NOT NULL,
						[Tr] [float] NULL,
						[Avg] [float] NULL,
						[Number] [int] NOT NULL)
	AS
	BEGIN

		DECLARE @minNumber AS INT = (SELECT [Number] FROM @values WHERE [DateIndex] = @startIndex);
		DECLARE @maxNumber AS INT = (SELECT MAX([Number]) FROM @values WHERE [Avg] IS NULL);
		DECLARE @i AS INT = @minNumber;

		INSERT @result SELECT * FROM @values;


		WHILE @i <= @maxNumber
		BEGIN
			
			DECLARE @prevAvg AS FLOAT = (SELECT [Avg] FROM @result WHERE [number] = @i - 1);
			DECLARE @avg AS FLOAT;
			
			IF (@prevAvg IS NOT NULL)
				BEGIN
					SET @avg = (@prevAvg * (@dataSampleSize - 1))/@dataSampleSize + ((SELECT [Tr] FROM @result WHERE [number] = @i)/IIF(@countAverage = 1, @dataSampleSize, 1));
				END
			ELSE
				BEGIN
					SET @avg = (SELECT
									IIF(b.[Counter] = @dataSampleSize, IIF(@countAverage = 1, b.[Sum] / b.[Counter], b.[Sum]), NULL)
								FROM
									(SELECT
										COUNT(a.[Tr]) AS [Counter],
										SUM(a.[Tr]) AS [Sum]
									FROM
										(SELECT
											r.[Tr]
										FROM
											@result r
										WHERE
											r.[Number] BETWEEN @i - @dataSampleSize + 1 AND @i
											AND r.[Tr] IS NOT NULL
										) a
									) b
								);
				END
			
			UPDATE @result
			SET [Avg] = @avg
			WHERE [number] = @i;

			SET @i = @i + 1;
		END;

		RETURN
		
	END

	GO
	




	-- [4] Creating controlling procedure for processing ADX.
	IF OBJECT_ID('processAdx','P') IS NOT NULL DROP PROC [dbo].[processAdx];
	GO

	CREATE PROC [dbo].[processAdx] @assetId AS INT, @timeframeId AS INT
	AS
	BEGIN
		PRINT ' ____________________________________';
		PRINT '* PROC STARTED [processAdx]';
		PRINT '*      @assetId: ' + CAST(@assetId AS NVARCHAR(255));
		PRINT '*      @timeframeId: ' + CAST(@timeframeId AS NVARCHAR(255));
		PRINT '';
		PRINT '';

		PRINT '[*] ADX last analyzed index (before): ' + CAST([dbo].[GetLastAdxAnalysisDate](@AssetId, @TimeframeId) AS NVARCHAR(255));

		EXEC [dbo].[calculateAdx] @assetId = @assetId, @timeframeId = @timeframeId;

		--Update timestamp.
		BEGIN
			
			DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);

			UPDATE [dbo].[timestamps] 
			SET [AdxLastAnalyzedIndex] = @lastQuote
			WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId
			IF @@ROWCOUNT=0
				INSERT INTO [dbo].[timestamps]([AssetId], [TimeframeId], [AdxLastAnalyzedIndex]) 
				VALUES (@assetId, @timeframeId, @lastQuote);
			
			IF ([dbo].[GetDebugMode]() = 1)
			BEGIN
				PRINT '[*] ADX last analyzed index (after): ' + CAST([dbo].[GetLastAdxAnalysisDate](@AssetId, @TimeframeId) AS NVARCHAR(255));
				PRINT '';
				PRINT '[#] ADX data after process:';
				SELECT * FROM [dbo].[adx] WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId;
				PRINT '';
				PRINT '* PROC ENDED [processAdx]';
				PRINT ' ____________________________________';
				PRINT '';
				PRINT '';
				PRINT '';

			END

		END

	END

	GO



	-- [5] Creating function for actual analyzing ADX.
	IF OBJECT_ID('calculateAdx','P') IS NOT NULL DROP PROC [dbo].[calculateAdx];
	GO

	CREATE PROC [dbo].[calculateAdx] @assetId AS INT, @timeframeId AS INT
	AS
	BEGIN

		DECLARE @analysisPeriod AS INT = 14;
		DECLARE @lastAnalyzedIndex AS INT = [dbo].[GetLastAdxAnalysisDate](@assetId, @timeframeId);
		DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);
		DECLARE @firstQuote AS INT = [dbo].[GetFirstQuote](@assetId, @timeframeId);
		DECLARE @firstForAnalysis AS INT = [dbo].[MaxValue](@firstQuote, COALESCE(@lastAnalyzedIndex + 1, 1));

		IF (@lastQuote > @lastAnalyzedIndex)
		BEGIN
			
			-- [1] Filter quotes and ADXs necessary for analysis.
			SELECT 
				q.*
			INTO
				#Quotes
			FROM
				[dbo].[quotes] q
			WHERE
				q.[AssetId] = @assetId AND 
				q.[TimeframeId] = @timeframeId AND
				q.[DateIndex] > (@lastAnalyzedIndex - @analysisPeriod);

			SELECT 
				a.*
			INTO
				#CurrentAdxData
			FROM
				[dbo].[adx] a
			WHERE
				a.[AssetId] = @assetId AND 
				a.[TimeframeId] = @timeframeId AND
				a.[DateIndex] > (@lastAnalyzedIndex - @analysisPeriod);


			-- [2] Add [TR], [+DM1] and [-DM1]
			SELECT 
				q1.[DateIndex],
				COALESCE(cad.[tr], IIF(q1.[DateIndex] IS NOT NULL, [dbo].[MaxValue](q1.[High] - q1.[Low], [dbo].[MaxValue](ABS(q1.[High] - q2.[Close]), ABS(q1.[Low] - q2.[Close]))), 0)) AS [tr],
				COALESCE(cad.[dm1Pos], IIF(q2.[DateIndex] IS NULL, NULL, IIF(q1.[High] - q2.[High] > q2.[Low] - q1.[Low], [dbo].[MaxValue](q1.[High] - q2.[High], 0), 0))) AS [dm1Pos],
				COALESCE(cad.[dm1Neg], IIF(q2.[DateIndex] IS NULL, NULL, IIF(q1.[High] - q2.[High] < q2.[Low] - q1.[Low], [dbo].[MaxValue](q2.[Low] - q1.[Low], 0), 0))) AS [dm1Neg]
			INTO
				#AdxFirstStep
			FROM 
				#Quotes q1
				LEFT JOIN #Quotes q2 ON q1.[DateIndex] = q2.[DateIndex] + 1
				LEFT JOIN #CurrentAdxData cad ON q1.[DateIndex] = cad.[DateIndex];


			-- [3] Append other data.
			BEGIN

				DECLARE @adxFirstStepData AS [dbo].[SourceForTrAvg];

				-- [3.1] Append [TR14]
				BEGIN

					DELETE FROM @adxFirstStepData;
					INSERT INTO @adxFirstStepData
					SELECT
						afs.[DateIndex],
						afs.[tr],
						a.[TR14],
						[number] = ROW_NUMBER() OVER(ORDER BY afs.[DateIndex] ASC)
					FROM
						#AdxFirstStep afs
						LEFT JOIN #CurrentAdxData a ON afs.[DateIndex] = a.[DateIndex]

					SELECT
						*
					INTO
						#Tr14
					FROM
						[dbo].[CalculateTrAvgOrSum](@firstForAnalysis, 0, @analysisPeriod, @adxFirstStepData)

				END



				-- [3.2] Append [+DM14]
				BEGIN
					DELETE FROM @adxFirstStepData;
					INSERT INTO @adxFirstStepData
					SELECT
						afs.[DateIndex],
						afs.[dm1Pos],
						a.[DM14Pos],
						[number] = ROW_NUMBER() OVER(ORDER BY afs.[DateIndex] ASC)
					FROM
						#AdxFirstStep afs
						LEFT JOIN #CurrentAdxData a ON afs.[DateIndex] = a.[DateIndex]

					SELECT
						*
					INTO
						#Dm14Positive
					FROM
						[dbo].[CalculateTrAvgOrSum](@firstForAnalysis, 0, @analysisPeriod, @adxFirstStepData);
				END

				-- [3.3] Append [-DM14]
				BEGIN
					DELETE FROM @adxFirstStepData;
					INSERT INTO @adxFirstStepData
					SELECT
						afs.[DateIndex],
						afs.[dm1Neg],
						a.[DM14Neg],
						[number] = ROW_NUMBER() OVER(ORDER BY afs.[DateIndex] ASC)
					FROM
						#AdxFirstStep afs
						LEFT JOIN #CurrentAdxData a ON afs.[DateIndex] = a.[DateIndex]

					SELECT
						*
					INTO
						#Dm14Negative
					FROM
						[dbo].[CalculateTrAvgOrSum](@firstForAnalysis, 0, @analysisPeriod, @adxFirstStepData);
				END

				-- [3.4] Combine all data fetched above into next step table.
				BEGIN

					SELECT
						  b.*
						, 100 * ([Di14Diff]/[Di14Sum]) AS [DX]
					INTO
						#AdxSecondStep
					FROM
						(SELECT
							a.*,
							ABS([Di14Positive] - [Di14Negative]) AS [Di14Diff],
							[Di14Positive] + [Di14Negative] AS [Di14Sum]
						FROM
							(SELECT
								afs.*,
								t.[Avg] AS [Tr14],
								dp.[Avg] AS [Dm14Positive],
								dn.[Avg] AS [Dm14Negative],
								100 * (dp.[Avg] / t.[Avg]) AS [Di14Positive],
								100 * (dn.[Avg] / t.[Avg]) AS [Di14Negative]
							FROM
								#AdxFirstStep afs
								LEFT JOIN #Tr14 t ON afs.[DateIndex] = t.[DateIndex]
								LEFT JOIN #Dm14Positive dp ON afs.[DateIndex] = dp.[DateIndex]
								LEFT JOIN #Dm14Negative dn ON afs.[DateIndex] = dn.[DateIndex]
							) a
						) b

				END

				-- [3.5] Drop unnecessary tables.
				BEGIN
					DROP TABLE #Tr14;
					DROP TABLE #Dm14Positive;
					DROP TABLE #Dm14Negative;
				END

			END


			-- [4] Append ADX
			BEGIN

				DELETE FROM @adxFirstStepData;
				INSERT INTO @adxFirstStepData
				SELECT
					ass.[DateIndex],
					ass.[DX],
					a.[ADX],
					[number] = ROW_NUMBER() OVER(ORDER BY ass.[DateIndex] ASC)
				FROM
					#AdxSecondStep ass
					LEFT JOIN #CurrentAdxData a ON ass.[DateIndex] = a.[DateIndex]

				SELECT
					*
				INTO
					#FinalAdx
				FROM
					[dbo].[CalculateTrAvgOrSum](@firstForAnalysis, 1, @analysisPeriod, @adxFirstStepData);
				
				
			END


			-- [5] Insert ADX data into destination table.
			BEGIN

				INSERT INTO [dbo].[adx]([AssetId], [TimeframeId], [DateIndex], [TR], [DM1Pos], [DM1Neg], [TR14], [DM14Pos], [DM14Neg], [DI14Pos], [DI14Neg], [DI14Diff], [DI14Sum], [DX], [ADX])
				SELECT 
					@assetId, @timeframeId, a.*
				FROM
					(SELECT
						  ass.*
						, fa.[avg] AS [ADX]
					FROM
						(SELECT * FROM #AdxSecondStep WHERE [DateIndex] > @lastAnalyzedIndex) ass
						LEFT JOIN #FinalAdx fa
							ON ass.[DateIndex] = fa.[DateIndex]) a;

			END

			--Clean up
			BEGIN
				DROP TABLE #Quotes;
				DROP TABLE #CurrentAdxData;
				DROP TABLE #AdxFirstStep;
				DROP TABLE #AdxSecondStep;
				DROP TABLE #FinalAdx;
			END


		END

	END

GO


COMMIT TRANSACTION