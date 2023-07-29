USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUeServiceLatencyRecords_GetLatencyRecordsForTransformableServerTransferRequests]
GO

CREATE PROCEDURE [dbo].[gsUeServiceLatencyRecords_GetLatencyRecordsForTransformableServerTransferRequests]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM UeServiceLatencyRecords
	WHERE
		AccountId IN (SELECT AccountId FROM UeServiceServerTransferRequests WHERE TargetUeServiceId IS NULL);


END

