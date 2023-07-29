USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUeServiceServerTransferRequests_GetAllTransformedUeServiceServerTransferRequests]
GO

CREATE PROCEDURE [dbo].[gsUeServiceServerTransferRequests_GetAllTransformedUeServiceServerTransferRequests]
	@UeServiceId UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM UeServiceServerTransferRequests
	WHERE
		TargetUeServiceId = @UeServiceId AND
		TargetChannelId IS NULL AND
		IsProcessing = 0;

END

