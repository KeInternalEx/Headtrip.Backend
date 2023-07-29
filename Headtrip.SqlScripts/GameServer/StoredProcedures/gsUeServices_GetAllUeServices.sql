USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUeServices_GetAllUeServices];
GO

CREATE PROCEDURE [dbo].[gsUeServices_GetAllUeServices]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM UeServices;
		
END

