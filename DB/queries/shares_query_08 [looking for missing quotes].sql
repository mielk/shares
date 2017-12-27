USE [shares];

BEGIN TRANSACTION

--SELECT * INTO [dbo].[quotes_temp] FROM [dbo].[quotes];

GO

SELECT * INTO #TempFirstQuotes FROM [dbo].[firstQuotes] WHERE fq IS NOT NULL;

------

SELECT
	d.[Id] AS [DateIndex],
	d.[Date],
	fqs.[Id] AS [ShareId]
INTO 
	#TempRequiredQuotes
FROM
	--(SELECT * FROM [dbo].[dates] WHERE [Date] < DATEADD(day, -1, GETDATE())) d,
	(SELECT * FROM [dbo].[dates] WHERE [Date] < '2017-12-09') d,
	(SELECT * FROM #TempFirstQuotes) fqs

WHERE
	d.[Date] >= fqs.[fq];

------

SELECT
	trq.*
INTO
	#TempMissingQuotes
FROM
	#TempRequiredQuotes trq
	LEFT JOIN
	[dbo].[quotes] q
	ON trq.[ShareId] = q.[ShareId] AND trq.[DateIndex] = q.[DateIndex]
WHERE
	q.[DateIndex] IS NULL;

------


--pętla
DECLARE @missingCounter INT, @message as VARCHAR(100)
SET @missingCounter = (SELECT COUNT(*) FROM #TempMissingQuotes);
SET @message = CONCAT('Still missing: ', @missingCounter);


WHILE (@missingCounter > 0)
BEGIN
	SELECT 
		tmq.[ShareId],
		tmq.[Date],
		q.[Close] AS [Open],
		q.[Close] AS [Low],
		q.[Close] AS [High],
		q.[Close] AS [Close],
		q.[AdjClose] AS [AdjClose],
		0 AS [Volume],
		CURRENT_TIMESTAMP AS [CreatedDate],
		tmq.[DateIndex] AS [DateIndex]
	INTO
		#TempGeneratedQuotes
	FROM 
		#TempMissingQuotes tmq
		LEFT JOIN
		[dbo].[quotes] q
		ON tmq.[ShareId] = q.[ShareId] AND tmq.[DateIndex] = (q.[DateIndex] + 1)
	WHERE
		q.[Close] IS NOT NULL;

	----

	DELETE tmq
	FROM #TempMissingQuotes tmq
		LEFT JOIN #TempGeneratedQuotes tgq
		ON tmq.[DateIndex] = tgq.[DateIndex] AND tmq.[ShareId] = tgq.[ShareId]
	WHERE 
		tgq.[ShareId] IS NOT NULL

	------

	INSERT INTO [dbo].[quotes]([ShareId], [Date], [Open], [Low], [High], [Close], [AdjClose], [Volume], [CreatedDate], [DateIndex])
	SELECT * FROM #TempGeneratedQuotes;

	DROP TABLE #TempGeneratedQuotes;

	SET @missingCounter = (SELECT COUNT(*) FROM #TempMissingQuotes);
	SELECT * FROM #TempMissingQuotes;
	SET @message = CONCAT('Still missing: ', @missingCounter);
	RAISERROR( @message, 10,1) WITH NOWAIT

END

--select shareid, count(*) from #TempMissingQuotes group by shareid;

DROP TABLE #TempFirstQuotes;
DROP TABLE #TempRequiredQuotes;
DROP TABLE #TempMissingQuotes;

COMMIT TRANSACTION
--ROLLBACK TRANSACTION



--select * from quotes where shareid = 1739;