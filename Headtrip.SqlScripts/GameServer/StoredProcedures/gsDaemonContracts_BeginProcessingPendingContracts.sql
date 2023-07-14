USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsDaemonContracts_BeginProcessingPendingContracts]
GO

CREATE PROCEDURE [dbo].[gsDaemonContracts_BeginProcessingPendingContracts]
	@DaemonId UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE DaemonContracts
	SET
		IsProcessing = 1
	WHERE
		CurrentDaemonId = @DaemonId AND
		TargetDaemonId IS NOT NULL AND
		IsProcessing = 0 AND
		TransformingDaemonId IS NULL

	SELECT * FROM DaemonContracts
	WHERE
		IsProcessing = 1 AND
		TransformingDaemonId IS NULL AND
		CurrentDaemonId = @DaemonId

END

