USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUnrealServiceServerTransferRequests_GetAllTransformableUnrealServiceServerTransferRequests]
GO

CREATE PROCEDURE [dbo].[gsUnrealServiceServerTransferRequests_GetAllTransformableUnrealServiceServerTransferRequests]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM UnrealServiceServerTransferRequests
	WHERE
		TargetUnrealServiceId IS NULL AND
		TargetChannelId IS NULL AND
		IsProcessing = 0;

END

