USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsDaemonContracts_EndProcessingPendingContracts]
GO

CREATE PROCEDURE [dbo].[gsDaemonContracts_EndProcessingPendingContracts]
	@DaemonId UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON;


	DELETE FROM DaemonContracts
	WHERE
		CurrentDaemonId = @DaemonId AND
		TargetChannelId IS NOT NULL AND
		IsProcessing = 1

END

