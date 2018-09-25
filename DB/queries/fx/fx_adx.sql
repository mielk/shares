USE [fx];

BEGIN TRANSACTION
	
	
	-- [1] Create table with indexes and foreign keys.
	BEGIN

		CREATE TABLE [dbo].[adx] (
			[AssetId] [int] NOT NULL,
			[TimeframeId] [int] NOT NULL,
			[DateIndex] [int] NOT NULL,
			[TR] [float] NULL,
			[DM1Pos] [float] NULL,
			[DM1Neg] [float] NULL,
			[TR14] [float] NULL,
			[DM14Pos] [float] NULL,
			[DM14Neg] [float] NULL,
			[DI14Pos] [float] NULL,
			[DI14Neg] [float] NULL,
			[DI14Diff] [float] NULL,
			[DI14Sum] [float] NULL,
			[DX] [float] NULL,
			[ADX] [float] NULL,
			[DaysUnder20] [int] DEFAULT NULL,
			[DaysUnder15] [int] DEFAULT NULL,
			[Cross20] [float] NULL,
			[DeltaDIPos] [float] NULL,
			[DeltaDINeg] [float] NULL,
			[DeltaADX] [float] NULL,
			[DIPosDirection3D] [int] DEFAULT NULL,
			[DIPosDirection2D] [int] DEFAULT NULL,
			[DINegDirection3D] [int] DEFAULT NULL,
			[DINegDirection2D] [int] DEFAULT NULL,
			[ADXDirection3D] [int] DEFAULT NULL,
			[ADXDirection2D] [int] DEFAULT NULL,
			[DIPosDirectionChanged] [int] DEFAULT NULL,
			[DINegDirectionChanged] [int] DEFAULT NULL,
			[ADXDirectionChanged] [int] DEFAULT NULL,
			[DIDifference] [float] NULL,
			[DILinesCrossing] [int] DEFAULT NULL,
			[CreatedDate] [datetime] NOT NULL CONSTRAINT [Default_Adx_CreatedDate]  DEFAULT (GETDATE())
		);


		ALTER TABLE [dbo].[adx]  WITH CHECK ADD  CONSTRAINT [FK_Adx_AssetId] FOREIGN KEY([AssetId])
		REFERENCES [dbo].[assets] ([AssetId])


		ALTER TABLE [dbo].[adx]  WITH CHECK ADD  CONSTRAINT [FK_Adx_DateIndex] FOREIGN KEY([DateIndex], [TimeframeId])
		REFERENCES [dbo].[dates] ([DateIndex], [TimeframeId])


		CREATE NONCLUSTERED INDEX [ixAssetId_Adx] ON [dbo].[adx] ([AssetId] ASC) 
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE UNIQUE NONCLUSTERED INDEX [ixAssetTimeframeDateIndex_Adx] ON [dbo].[adx] ([AssetId] ASC, [TimeframeId] ASC, [DateIndex] ASC)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixDateIndex_Adx] ON [dbo].[adx] ([DateIndex] ASC)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

		CREATE NONCLUSTERED INDEX [ixTimeframe_Adx] ON [dbo].[adx] ([TimeframeId] ASC)
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
		DELETE FROM [dbo].[dates] WHERE [DateIndex] IN (SELECT [DateIndex] FROM deleted);
	GO


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


	IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalculateTrAvg]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[CalculateTrAvg]
	GO
	CREATE FUNCTION [dbo].[CalculateTrAvg](
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

		DECLARE @minNumber AS INT = [dbo].[MaxValue]((SELECT MIN([Number]) FROM @values WHERE [Avg] IS NULL), 2);
		DECLARE @maxNumber AS INT = (SELECT MAX([Number]) FROM @values WHERE [Avg] IS NULL);
		DECLARE @i AS INT = @minNumber;

		INSERT @result SELECT * FROM @values;


		WHILE @i <= @maxNumber
		BEGIN
			
			DECLARE @prevAvg AS FLOAT = (SELECT [Avg] FROM @result WHERE [number] = @i - 1);
			DECLARE @avg AS FLOAT;
			
			IF (@prevAvg IS NOT NULL)
				BEGIN
					SET @avg = (@prevAvg * (@dataSampleSize - 1))/@dataSampleSize + (SELECT [Tr] FROM @result WHERE [number] = @i);
				END
			ELSE
				BEGIN
					SET @avg = (SELECT
									IIF(b.[Counter] = @dataSampleSize, b.[Sum], NULL)
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


		--DECLARE @MaxDate DATETIME

		--SELECT @MaxDate = MAX(OrderDate)
		--FROM Sales.SalesOrderHeader
		--WHERE CustomerID = @CustomerID

		--INSERT @CustomerOrder
		--SELECT a.SalesOrderID, a.CustomerID, a.OrderDate, b.OrderQty
		--FROM Sales.SalesOrderHeader a INNER JOIN Sales.SalesOrderHeader b
		--    ON a.SalesOrderID = b.SalesOrderID
		--    INNER JOIN Production.Product c ON b.ProductID = c.ProductID
		--WHERE a.OrderDate = @MaxDate
		--    AND a.CustomerID = @CustomerID
		RETURN
		
	END

	GO
	




	-- [4] Creating controlling procedure for processing ADX.
	IF OBJECT_ID('processAdx','P') IS NOT NULL DROP PROC [dbo].[processAdx];
	GO

	CREATE PROC [dbo].[processAdx] @assetId AS INT, @timeframeId AS INT
	AS
	BEGIN
	
		EXEC [dbo].[analyzeAdx] @assetId = @assetId, @timeframeId = @timeframeId;

		--Update timestamp.
		BEGIN
			
			DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);

			UPDATE [dbo].[timestamps] 
			SET [AdxLastAnalyzedIndex] = @lastQuote
			WHERE [AssetId] = @assetId AND [TimeframeId] = @timeframeId
			IF @@ROWCOUNT=0
				INSERT INTO [dbo].[timestamps]([AssetId], [TimeframeId], [AdxLastAnalyzedIndex]) 
				VALUES (@assetId, @timeframeId, @lastQuote);
		
		END

	END

	GO



	-- [5] Creating function for actual analyzing ADX.
	IF OBJECT_ID('analyzeAdx','P') IS NOT NULL DROP PROC [dbo].[analyzeAdx];
	GO

	CREATE PROC [dbo].[analyzeAdx] @assetId AS INT, @timeframeId AS INT
	AS
	BEGIN

		DECLARE @analysisPeriod AS INT = 14;
		DECLARE @lastAnalyzedIndex AS INT = [dbo].[GetLastAdxAnalysisDate](@assetId, @timeframeId);
		DECLARE @lastQuote AS INT = [dbo].[GetLastQuote](@assetId, @timeframeId);
		DECLARE @firstQuote AS INT = [dbo].[GetFirstQuote](@assetId, @timeframeId);

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
				#Adx
			FROM
				[dbo].[adx] a
			WHERE
				a.[AssetId] = @assetId AND 
				a.[TimeframeId] = @timeframeId AND
				a.[DateIndex] > (@lastAnalyzedIndex - @analysisPeriod);


			-- [2] Add [TR], [+DM1] and [-DM1]
			SELECT 
				q1.[DateIndex],
				IIF(q1.[DateIndex] IS NOT NULL, [dbo].[MaxValue](q1.[High] - q1.[Low], [dbo].[MaxValue](ABS(q1.[High] - q2.[Close]), ABS(q1.[Low] - q2.[Close]))), 0) AS [tr],
				IIF(q2.[DateIndex] IS NULL, NULL, IIF(q1.[High] - q2.[High] > q2.[Low] - q1.[Low], [dbo].[MaxValue](q1.[High] - q2.[High], 0), 0)) AS [dm1Pos],
				IIF(q2.[DateIndex] IS NULL, NULL, IIF(q1.[High] - q2.[High] < q2.[Low] - q1.[Low], [dbo].[MaxValue](q2.[Low] - q1.[Low], 0), 0)) AS [dm1Neg]
			INTO
				#AdxFirstStep
			FROM 
				#Quotes q1
				LEFT JOIN #Quotes q2
					ON q1.[DateIndex] = q2.[DateIndex] + 1;


			-- [3] Append [TR14], [+DM14] and [-DM14]
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
						LEFT JOIN #Adx a ON afs.[DateIndex] = a.[DateIndex]

					SELECT
						*
					INTO
						#Tr14
					FROM
						[dbo].[CalculateTrAvg](@analysisPeriod, @adxFirstStepData);

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
						LEFT JOIN #Adx a ON afs.[DateIndex] = a.[DateIndex]

					SELECT
						*
					INTO
						#Dm14Positive
					FROM
						[dbo].[CalculateTrAvg](@analysisPeriod, @adxFirstStepData);
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
						LEFT JOIN #Adx a ON afs.[DateIndex] = a.[DateIndex]

					SELECT
						*
					INTO
						#Dm14Negative
					FROM
						[dbo].[CalculateTrAvg](@analysisPeriod, @adxFirstStepData);
				END

				-- [3.4] Combine all data fetched above into next step table.
				BEGIN
					SELECT
						afs.*,
						t.[Avg] AS [Tr14],
						dp.[Avg] AS [Dm14Positive],
						dn.[Avg] AS [Dm14Negative]
					INTO
						#AdxWith14Averages
					FROM
						#AdxFirstStep afs
						LEFT JOIN #Tr14 t ON afs.[DateIndex] = t.[DateIndex]
						LEFT JOIN #Dm14Positive dp ON afs.[DateIndex] = dp.[DateIndex]
						LEFT JOIN #Dm14Negative dn ON afs.[DateIndex] = dn.[DateIndex];

				END

				-- [3.5] Drop unnecessary tables.
				BEGIN
					DROP TABLE #Tr14;
					DROP TABLE #Dm14Positive;
					DROP TABLE #Dm14Negative;
				END

			END


			SELECT * FROM #AdxWith14Averages;


			--Clean up
			BEGIN
				DROP TABLE #Quotes;
				DROP TABLE #Adx;
				DROP TABLE #AdxFirstStep;
				DROP TABLE #AdxWith14Averages;
				--DROP TABLE #EvaluationOpenedExtrema;
				--DROP TABLE #QuotesForComparing;
				--DROP TABLE #UpdatedExtrema;
			END


		END

	END

GO


EXEC [dbo].[test_addQuoteFromRawH1] @counter = 20
EXEC [dbo].[processAdx] @assetId = 1, @timeframeId = 4;


--SELECT 'Timestamps after', * FROM [dbo].[timestamps];

select * from [adx];


ROLLBACK TRANSACTION