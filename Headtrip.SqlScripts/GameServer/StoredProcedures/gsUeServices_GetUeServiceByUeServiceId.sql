USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUnrealServices_GetUnrealServiceByUnrealServiceId];
GO

CREATE PROCEDURE [dbo].[gsUnrealServices_GetUnrealServiceByUnrealServiceId]
	@UnrealServiceId UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP(1) * FROM UnrealServices WHERE UnrealServiceId = @UnrealServiceId;
		
END

