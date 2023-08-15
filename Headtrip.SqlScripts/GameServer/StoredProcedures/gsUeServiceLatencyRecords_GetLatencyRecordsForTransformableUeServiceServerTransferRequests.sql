USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUnrealServiceLatencyRecords_GetLatencyRecordsForTransformableServerTransferRequests]
GO

CREATE PROCEDURE [dbo].[gsUnrealServiceLatencyRecords_GetLatencyRecordsForTransformableServerTransferRequests]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM UnrealServiceLatencyRecords
	WHERE
		AccountId IN (SELECT AccountId FROM UnrealServiceServerTransferRequests WHERE TargetUnrealServiceId IS NULL);


END

