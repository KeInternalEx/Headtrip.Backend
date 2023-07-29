USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUeServiceServerTransferRequests_GetAllTransformableUeServiceServerTransferRequests]
GO

CREATE PROCEDURE [dbo].[gsUeServiceServerTransferRequests_GetAllTransformableUeServiceServerTransferRequests]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM UeServiceServerTransferRequests
	WHERE
		TargetUeServiceId IS NULL AND
		TargetChannelId IS NULL AND
		IsProcessing = 0;

END

