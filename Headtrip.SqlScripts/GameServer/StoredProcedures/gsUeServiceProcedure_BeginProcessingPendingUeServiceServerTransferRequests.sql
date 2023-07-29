USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUeServiceProc_BeginProcessingPendingServerTransferRequests]
GO

CREATE PROCEDURE [dbo].[gsUeServiceProc_BeginProcessingPendingServerTransferRequests]
	@UeServiceId UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE UeServiceServerTransferRequests
	SET
		IsProcessing = 1
	OUTPUT inserted.*
	WHERE
		CurrentUeServiceId = @UeServiceId AND
		TargetChannelId IS NOT NULL AND
		IsProcessing = 0

END

