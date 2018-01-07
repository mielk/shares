USE [fx];

BEGIN TRANSACTION

GO
IF OBJECT_ID('addNewQuote','P') IS NOT NULL DROP PROC [dbo].[addNewQuote];
IF OBJECT_ID('test_addQuoteFromRawH1','P') IS NOT NULL DROP PROC [dbo].[test_addQuoteFromRawH1];
GO

CREATE PROC [dbo].[addNewQuote] @assetId AS INT, @timeframe AS INT, @date AS DATETIME, @open AS FLOAT, @high AS FLOAT, @low AS FLOAT, @close AS FLOAT, @volume AS FLOAT
AS
BEGIN

	DECLARE @dateIndex AS INT = (SELECT [DateIndex] FROM [dbo].[dates] WHERE [Timeframe] = @timeframe AND [Date] = @date);

	INSERT INTO [dbo].[quotes]([AssetId], [Timeframe], [DateIndex], [Open], [High], [Low], [Close], [Volume], [IsComplete])
	SELECT @assetId, @timeframe, @dateIndex, @open, @high, @low, @close, @volume, 1;

END

GO

CREATE PROC [dbo].[test_addQuoteFromRawH1]
AS
BEGIN

	DECLARE @assetId AS INT = 1;
	DECLARE @timeframe AS INT = 4;
	DECLARE @lastDate AS DATETIME;
	DECLARE @date AS DATETIME;
	DECLARE @open AS FLOAT;
	DECLARE @high AS FLOAT;
	DECLARE @low AS FLOAT;
	DECLARE @close AS FLOAT;
	DECLARE @volume AS INT;
	
	SET @lastDate = (SELECT
					MAX(d.[Date]) AS [LastQuoteDate]
				FROM 
					(SELECT * FROM [dbo].[quotes] WHERE [AssetId] = @assetId AND [Timeframe] = @timeframe) q
					LEFT JOIN (SELECT * FROM [dbo].[dates] WHERE [Timeframe] = @timeframe) d
					ON q.[DateIndex] = d.[DateIndex]);
	
	SELECT TOP 1
		*
	INTO
		#NextRow
	FROM 
		[dbo].[RawH1Data] d
	WHERE
		d.[Date] > COALESCE(@lastDate, '1970-01-01')
	ORDER BY
		d.[Date] ASC;


	SET @date = (SELECT [Date] FROM #NextRow);
	SET @open = (SELECT [Open] FROM #NextRow);
	SET @high = (SELECT [High] FROM #NextRow);
	SET @low = (SELECT [Low] FROM #NextRow);
	SET @close = (SELECT [Close] FROM #NextRow);
	SET @volume = 0;

	EXEC [dbo].[addNewQuote] @assetId = @assetId, @timeframe = @timeframe, @date = @date, @open = @open, @high = @high, @low = @low, @close = @close, @volume = @volume

	DROP TABLE #NextRow;


END

GO

EXEC [dbo].[test_addQuoteFromRawH1];
EXEC [dbo].[test_addQuoteFromRawH1];
EXEC [dbo].[test_addQuoteFromRawH1];
EXEC [dbo].[test_addQuoteFromRawH1];
EXEC [dbo].[test_addQuoteFromRawH1];

COMMIT TRANSACTION;