USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsDaemons_GetDaemonByDaemonId];
GO

CREATE PROCEDURE [dbo].[gsDaemons_GetDaemonByDaemonId]
	@DaemonId UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP(1) * FROM Daemons WHERE DaemonId = @DaemonId;
		
END

