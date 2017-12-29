USE [shares];


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

BEGIN TRANSACTION;

DECLARE @shareId AS INT;
SET @shareId = 1;

SELECT DISTINCT TOP 100 [BaseId], [CounterId] INTO #ExtremaPairs FROM [dbo].[trendlines] t WHERE [ShareId] = @shareId


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
             IF @Counter = 1
             BEGIN
                        PRINT 'BaseId' + CHAR(9) + 'CounterId'
                        PRINT '------------------------------------'
             END
 
             --PRINT CURRENT RECORD.
             PRINT CAST(@BaseId AS VARCHAR(10)) + CHAR(9) + CHAR(9) + CHAR(9) + CAST(@CounterId AS VARCHAR(10))
    
             --INCREMENT COUNTER.
             SET @Counter = @Counter + 1
 
             --FETCH THE NEXT RECORD INTO THE VARIABLES.
			 FETCH NEXT FROM ExtremaCursor INTO @BaseId, @CounterId;

      END
 
--CLOSE THE CURSOR.
CLOSE ExtremaCursor
DEALLOCATE ExtremaCursor

END
GO

ROLLBACK TRANSACTION;

--exec [dbo].[evaluateTrendlines] @shareId = 1, @baseExtremumId = 7078, @counterExtremumId = 7079

--begin transaction;
--
--UPDATE t
--SET 
--	[ShowOnChart] = 1,
--	[StartDateIndex] = [BaseStartIndex] - 3,
--	[EndDateIndex] = [CounterStartIndex] + 3
--FROM
--	[dbo].[Trendlines] t
--	LEFT JOIN
--	(SELECT 
--		[BaseId], [CounterId], MIN([Id]) AS [MinId]
--	FROM
--		[dbo].[Trendlines]
--	GROUP BY
--		[BaseId], [CounterId]) x
--	ON t.[Id] = x.[MinId]
--WHERE
--	x.[MinId] IS NOT NULL;
--
--commit transaction;
