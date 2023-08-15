USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUnrealServices_GetAllUnrealServices];
GO

CREATE PROCEDURE [dbo].[gsUnrealServices_GetAllUnrealServices]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM UnrealServices;
		
END

