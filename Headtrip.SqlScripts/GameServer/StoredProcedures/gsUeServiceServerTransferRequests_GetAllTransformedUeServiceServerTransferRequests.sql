USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUnrealServiceServerTransferRequests_GetAllTransformedUnrealServiceServerTransferRequests]
GO

CREATE PROCEDURE [dbo].[gsUnrealServiceServerTransferRequests_GetAllTransformedUnrealServiceServerTransferRequests]
	@UnrealServiceId UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM UnrealServiceServerTransferRequests
	WHERE
		TargetUnrealServiceId = @UnrealServiceId AND
		TargetChannelId IS NULL AND
		IsProcessing = 0;

END

