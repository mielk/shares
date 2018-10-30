USE [stock];

BEGIN TRANSACTION

SET NOCOUNT ON;

EXEC [dbo].[ClearDatabaseForTests];
EXEC [dbo].[RunIterations] @AssetId = 1, @TimeframeId = 6, @IterationCounter = 20, @IterationStep = 10, @DebugMode = 0, @RunAdx = 0, @RunMacd = 0;
EXEC [dbo].[RunIterations] @AssetId = 1, @TimeframeId = 6, @IterationCounter = 1, @IterationStep = 10, @DebugMode = 1, @RunAdx = 1, @RunMacd = 1;


--ROLLBACK TRANSACTION;
COMMIT TRANSACTION;