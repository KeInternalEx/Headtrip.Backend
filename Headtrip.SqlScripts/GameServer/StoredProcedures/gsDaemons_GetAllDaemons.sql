﻿USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsDaemons_GetAllDaemons];
GO

CREATE PROCEDURE [dbo].[gsDaemons_GetAllDaemons]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM Daemons;
		
END
