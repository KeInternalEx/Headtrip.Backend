USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUnrealServiceProc_BeginProcessingPendingServerTransferRequests]
GO

CREATE PROCEDURE [dbo].[gsUnrealServiceProc_BeginProcessingPendingServerTransferRequests]
	@UnrealServiceId UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE UnrealServiceServerTransferRequests
	SET
		IsProcessing = 1
	OUTPUT inserted.*
	WHERE
		CurrentUnrealServiceId = @UnrealServiceId AND
		TargetChannelId IS NOT NULL AND
		IsProcessing = 0

END

