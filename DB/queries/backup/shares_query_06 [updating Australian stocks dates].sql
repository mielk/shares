use [shares]

go


BEGIN TRANSACTION

DELETE FROM [dbo].[quotes] WHERE [Close] IS NULL

GO

--Updating Australian stocks dates
--UPDATE [dbo].[quotes]
--SET [Date] = DATEADD(day, 1, [Date])
--WHERE 
----([ShareId] BETWEEN 1119 AND 1215) AND
--[ShareId] = 1130 and
--([Date] BETWEEN '2005-01-01' AND '2005-03-27' OR
--[Date] BETWEEN '2005-10-30' AND '2006-04-02' OR
--[Date] BETWEEN '2006-10-29' AND '2007-03-25' OR
--[Date] BETWEEN '2007-10-28' AND '2008-04-06' OR
--[Date] BETWEEN '2008-10-05' AND '2009-04-05' OR
--[Date] BETWEEN '2009-10-04' AND '2010-04-04' OR
--[Date] BETWEEN '2010-10-03' AND '2011-04-03' OR
--[Date] BETWEEN '2011-10-09' AND '2012-04-01' OR
--[Date] BETWEEN '2012-10-07' AND '2013-03-31' OR
--[Date] BETWEEN '2013-10-06' AND '2014-04-06' OR
--[Date] BETWEEN '2014-10-05' AND '2015-04-05' OR
--[Date] BETWEEN '2015-10-04' AND '2016-03-27' OR
--[Date] BETWEEN '2016-10-02' AND '2017-04-02' OR
--[Date] BETWEEN '2017-10-01' AND '2018-04-02')

GO


--Delete empty quotations
DELETE
FROM [quotes] 
WHERE
	[Open] = [Close] AND
	[Low] = [Close] AND
	[High] = [Close] AND
	[Volume] = 0;

GO


--Delete quotations with 0 Volume and [Close] greater than [High]
DELETE
FROM [dbo].[quotes] 
WHERE
	[Volume] = 0 AND
	[Close] > [High];



--Printuje wszystkie notowania, które wykraczają poza zakres zdefiniowanych dat.
SELECT
	*
FROM
	(SELECT * FROM [dbo].[quotes]) q
	LEFT JOIN
	(SELECT * FROM [dbo].[dates] WHERE [Date] < DATEADD(day, -1, GETDATE())) d
	ON d.[Date] = q.[Date]
WHERE
	d.[Date] IS NULL


--Sprawdza ile jest notowań przypisanych do każdej daty
--SELECT * FROM dates 
--WHERE [Date] IN 
--(SELECT
--	d.[Date]

--FROM
--	(SELECT * FROM [dbo].[dates] WHERE [Date] < DATEADD(day, -1, GETDATE())) d
--	LEFT JOIN
--	(SELECT
--		[Date],
--		COUNT(*) AS [Counter]
--	FROM [dbo].[quotes]
--	group by [date]) x
--	ON d.[Date] = x.[Date]

--WHERE
--	x.[Counter] < 100 OR x.[Counter] IS NULL);


--Usuwa wszystkie z nullowymi wartościami.
DELETE FROM [dbo].[quotes] WHERE [Close] IS NULL;


ROLLBACK TRANSACTION
--COMMIT TRANSACTION

SELECT * from QUOTES WHERE SHAREID = 1119