USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUeServiceServerTransferRequests_FinishProcessingPendingServerTransferRequests]
GO

CREATE PROCEDURE [dbo].[gsUeServiceServerTransferRequests_FinishProcessingPendingServerTransferRequests]
	@UeServiceId UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON;

	-- TODO: NEED TO UPDATE GAME SESSION OBJECT FOR EACH ServerTransferRequest'S ACCOUNT TO SET THEIR CURRENT CHANNEL ID

	DELETE FROM UeServiceServerTransferRequests
	WHERE
		CurrentUeServiceId = @UeServiceId AND
		TargetChannelId IS NOT NULL AND
		IsProcessing = 1

END

