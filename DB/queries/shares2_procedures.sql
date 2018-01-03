USE [shares2];

BEGIN TRANSACTION

--Create [GenerateWeeklyPrices] procedure.
GO
IF OBJECT_ID('ShowQuotesErrors','P') IS NOT NULL DROP PROC [dbo].[ShowQuotesErrors];
GO

CREATE PROCEDURE [dbo].[ShowQuotesErrors]
AS

BEGIN
	
	--Display all quotes with any price undefined
	SELECT
		'UndefinedPrice', *
	FROM
		[dbo].[quotes] 
	WHERE 
		[Open] IS NULL OR
		[Low] IS NULL OR
		[High] IS NULL OR
		[Close] IS NULL;

	--Display quotations with 0 Volume and [Close] greater than [High]
	SELECT
		'Null volume and not all prices equal', *
	FROM [dbo].[quotes] 
	WHERE
		(COALESCE([Volume], 0) = 0) AND
		([Close] <> [High] OR
		[Close] <> [Low] OR
		[Close] <> [Open]);

	--Display quotations with [High] price not being the highest one 
	SELECT
		'High price is not the highest one ', *
	FROM [dbo].[quotes] 
	WHERE
		[Close] > [High] OR
		[Low] > [High] OR
		[Open] > [High];

	--Display quotations with [Low] price not being the lowest one
	SELECT
		'Low price is not the lowest', *
	FROM [dbo].[quotes] 
	WHERE
		[Close] < [Low] OR
		[High] < [Low] OR
		[Open] < [Low];


END

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

EXEC [dbo].[ShowQuotesErrors];

ROLLBACK TRANSACTION