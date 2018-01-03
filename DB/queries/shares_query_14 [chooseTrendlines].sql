USE [shares];


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--Remove procedure if already exists
IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[AnalyzeShare]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [dbo].[AnalyzeShare]
END

GO

CREATE PROCEDURE [dbo].[AnalyzeShare] @shareId INT, @fromScratch BIT

AS

	SELECT DISTINCT [BaseId], [CounterId] INTO #ExtremaPairs FROM [dbo].[trendlines] t WHERE [ShareId] = @shareId --AND [StartDateIndex] IS NULL

	--PREPARE TABLES
	BEGIN
		DELETE FROM [dbo].[TrendRanges];
		DELETE FROM [dbo].[trendlinesBreaks];
		DELETE FROM [dbo].[TrendlinesHits];
		DELETE FROM [dbo].[trendlinesByHits];
	END

	BEGIN

		--DECLARE THE VARIABLES FOR HOLDING DATA.
		DECLARE @BaseId INT, @CounterId INT;

		--DECLARE AND SET COUNTER.
		DECLARE @Counter INT;
		SET @Counter = 1;

		--DECLARE THE CURSOR FOR A QUERY.
		DECLARE ExtremaCursor CURSOR READ_ONLY
		FOR
		SELECT [BaseId], [CounterId]
		FROM #ExtremaPairs;

		--OPEN CURSOR.
		OPEN ExtremaCursor;
 
		--FETCH THE RECORD INTO THE VARIABLES.
		FETCH NEXT FROM ExtremaCursor INTO @BaseId, @CounterId;

			  --LOOP UNTIL RECORDS ARE AVAILABLE.
			  WHILE @@FETCH_STATUS = 0
			  BEGIN

					--PRINT CURRENT RECORD.
					--SELECT
					--	@Counter, @BaseId, @CounterId
					--PRINT CAST(@Counter AS VARCHAR(10)) + CHAR(9) + CHAR(9) + CHAR(9) + CAST(@BaseId AS VARCHAR(10)) + CHAR(9) + CHAR(9) + CHAR(9) + CAST(@CounterId AS VARCHAR(10))
			
					--CALCULATE TRENDLINES FOR THE CURRENT PAIR
					EXEC [dbo].[evaluateTrendlines] @shareId = @shareId, @baseExtremumId = @BaseId, @counterExtremumId = @CounterId

					--INCREMENT COUNTER.
					SET @Counter = @Counter + 1
 
					--FETCH THE NEXT RECORD INTO THE VARIABLES.
					FETCH NEXT FROM ExtremaCursor INTO @BaseId, @CounterId;

			  END
 
		--CLOSE THE CURSOR.
		CLOSE ExtremaCursor
		DEALLOCATE ExtremaCursor

	END

	--Clean up
	BEGIN

		DROP TABLE #ExtremaPairs;

	END