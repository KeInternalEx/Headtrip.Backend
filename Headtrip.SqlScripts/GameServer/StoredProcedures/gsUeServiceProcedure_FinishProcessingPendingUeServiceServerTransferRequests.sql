USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUnrealServiceServerTransferRequests_FinishProcessingPendingServerTransferRequests]
GO

CREATE PROCEDURE [dbo].[gsUnrealServiceServerTransferRequests_FinishProcessingPendingServerTransferRequests]
	@UnrealServiceId UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON;

	-- TODO: NEED TO UPDATE GAME SESSION OBJECT FOR EACH ServerTransferRequest'S ACCOUNT TO SET THEIR CURRENT CHANNEL ID

	DELETE FROM UnrealServiceServerTransferRequests
	WHERE
		CurrentUnrealServiceId = @UnrealServiceId AND
		TargetChannelId IS NOT NULL AND
		IsProcessing = 1

END

