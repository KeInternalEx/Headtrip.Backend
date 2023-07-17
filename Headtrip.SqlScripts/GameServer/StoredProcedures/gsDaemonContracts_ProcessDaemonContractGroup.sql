USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsDaemonContracts_ProcessDaemonContractGroup]
GO

CREATE PROCEDURE [dbo].[gsDaemonContracts_ProcessDaemonContractGroup]
	@DaemonContractIds VARCHAR(MAX),
	@DaemonId UniqueIdentifier,
	@DaemonContractGroupId UniqueIdentifier,
	@ZoneName NVARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;

	
	UPDATE DaemonContracts
	SET
		TargetDaemonId = @DaemonId,
		ContractGroupId = @DaemonContractGroupId
	WHERE
		DaemonContractId IN (SELECT CAST([Value] AS UniqueIdentifier) FROM STRING_SPLIT(@DaemonContractIds, ','));


END

