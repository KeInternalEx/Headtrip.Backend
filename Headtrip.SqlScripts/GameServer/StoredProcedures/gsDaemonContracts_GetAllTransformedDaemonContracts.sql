USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsDaemonContracts_GetAllTransformedDaemonContracts]
GO

CREATE PROCEDURE [dbo].[gsDaemonContracts_GetAllTransformedDaemonContracts]
	@DaemonId UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM DaemonContracts
	WHERE
		TargetDaemonId = @DaemonId AND
		TargetChannelId IS NULL AND
		IsProcessing = 0;

END

