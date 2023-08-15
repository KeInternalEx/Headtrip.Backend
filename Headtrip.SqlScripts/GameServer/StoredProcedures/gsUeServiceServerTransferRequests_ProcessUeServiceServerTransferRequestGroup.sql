USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUnrealServiceServerTransferRequests_ProcessUnrealServiceServerTransferRequestGroup]
GO

CREATE PROCEDURE [dbo].[gsUnrealServiceServerTransferRequests_ProcessUnrealServiceServerTransferRequestGroup]
	@UnrealServiceServerTransferRequestIds VARCHAR(MAX),
	@UnrealServiceId UniqueIdentifier,
	@UnrealServiceServerTransferRequestGroupId UniqueIdentifier,
	@ZoneName NVARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;

	
	UPDATE UnrealServiceServerTransferRequests
	SET
		TargetUnrealServiceId = @UnrealServiceId,
		ServerTransferRequestGroupId = @UnrealServiceServerTransferRequestGroupId
	WHERE
		UnrealServiceServerTransferRequestId IN (SELECT CAST([Value] AS UniqueIdentifier) FROM STRING_SPLIT(@UnrealServiceServerTransferRequestIds, ','));


END

