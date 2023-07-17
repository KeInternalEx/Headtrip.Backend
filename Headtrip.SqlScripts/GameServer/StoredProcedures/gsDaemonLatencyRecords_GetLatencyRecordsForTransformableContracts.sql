USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsDaemonLatencyRecords_GetLatencyRecordsForTransformableContracts]
GO

CREATE PROCEDURE [dbo].[gsDaemonLatencyRecords_GetLatencyRecordsForTransformableContracts]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM DaemonLatencyRecords
	WHERE
		AccountId IN (SELECT AccountId FROM DaemonContracts WHERE TargetDaemonId IS NULL);


END

