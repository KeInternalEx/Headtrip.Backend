USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsDaemonClaims_GetAllDaemonClaims]
GO

CREATE PROCEDURE [dbo].[gsDaemonClaims_GetAllDaemonClaims]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM DaemonClaims;

END

