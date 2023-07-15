USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsDaemonProc_ProcessDaemonContractGroup]
GO

CREATE PROCEDURE [dbo].[gsDaemonProc_ProcessDaemonContractGroup]
	@DaemonContractIds VARCHAR(MAX),
	@DaemonId UniqueIdentifier,
	@ZoneName NVARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;

	
	UPDATE DaemonContracts
	SET
		TargetDaemonId = @DaemonId
	WHERE
		DaemonContractId IN (SELECT CAST([Value] AS UniqueIdentifier) FROM STRING_SPLIT(@DaemonContractIds, ','));

	DELETE TOP(1) FROM DaemonCLaims
	WHERE
		DaemonId = @DaemonId AND
		ZoneName = @ZoneName;


END

