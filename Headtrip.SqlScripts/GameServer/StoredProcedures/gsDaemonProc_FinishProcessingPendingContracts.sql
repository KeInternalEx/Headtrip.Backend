USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsDaemonContracts_FinishProcessingPendingContracts]
GO

CREATE PROCEDURE [dbo].[gsDaemonContracts_FinishProcessingPendingContracts]
	@DaemonId UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON;

	-- TODO: NEED TO UPDATE GAME SESSION OBJECT FOR EACH CONTRACT'S ACCOUNT TO SET THEIR CURRENT CHANNEL ID

	DELETE FROM DaemonContracts
	WHERE
		CurrentDaemonId = @DaemonId AND
		TargetChannelId IS NOT NULL AND
		IsProcessing = 1

END

