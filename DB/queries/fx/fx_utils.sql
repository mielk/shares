USE [fx];


BEGIN TRANSACTION

	--Drop existing version of functions
	BEGIN
		IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MinValue]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[MinValue]
		IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MaxValue]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))  DROP FUNCTION [dbo].[MaxValue]
	END

	GO

	--MIN.VALUE
	CREATE FUNCTION [dbo].[MinValue](@a AS FLOAT, @b AS FLOAT)
	RETURNS FLOAT
	AS
	BEGIN
		RETURN IIF (@a < @b, @a, @b);
	END

	GO

	--MAX.VALUE
	CREATE FUNCTION [dbo].[MaxValue](@a AS FLOAT, @b AS FLOAT)
	RETURNS FLOAT
	AS
	BEGIN
		RETURN IIF (@a > @b, @a, @b);
	END

	GO

COMMIT TRANSACTION