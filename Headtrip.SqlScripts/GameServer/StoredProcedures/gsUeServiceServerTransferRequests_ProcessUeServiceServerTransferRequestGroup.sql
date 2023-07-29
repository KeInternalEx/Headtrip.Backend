USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUeServiceServerTransferRequests_ProcessUeServiceServerTransferRequestGroup]
GO

CREATE PROCEDURE [dbo].[gsUeServiceServerTransferRequests_ProcessUeServiceServerTransferRequestGroup]
	@UeServiceServerTransferRequestIds VARCHAR(MAX),
	@UeServiceId UniqueIdentifier,
	@UeServiceServerTransferRequestGroupId UniqueIdentifier,
	@ZoneName NVARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;

	
	UPDATE UeServiceServerTransferRequests
	SET
		TargetUeServiceId = @UeServiceId,
		ServerTransferRequestGroupId = @UeServiceServerTransferRequestGroupId
	WHERE
		UeServiceServerTransferRequestId IN (SELECT CAST([Value] AS UniqueIdentifier) FROM STRING_SPLIT(@UeServiceServerTransferRequestIds, ','));


END

