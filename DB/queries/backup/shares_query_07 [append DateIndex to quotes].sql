USE [shares];

GO

BEGIN TRANSACTION;

--SELECT * INTO [dbo].[quotesTemp] FROM [dbo].[quotes];

GO

UPDATE q
SET q.[DateIndex] = d.[Id]
FROM 
	[dbo].[quotes] q
	LEFT JOIN [dbo].[dates] d
	ON q.[Date] = d.[Date]


--ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
