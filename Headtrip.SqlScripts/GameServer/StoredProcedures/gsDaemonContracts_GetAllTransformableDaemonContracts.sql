﻿USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsDaemonContracts_GetAllTransformableDaemonContracts]
GO

CREATE PROCEDURE [dbo].[gsDaemonContracts_GetAllTransformableDaemonContracts]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM DaemonContracts
	WHERE
		TransformingDaemonId IS NULL;

END
