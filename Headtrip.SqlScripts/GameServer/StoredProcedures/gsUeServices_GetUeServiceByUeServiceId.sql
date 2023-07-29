USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUeServices_GetUeServiceByUeServiceId];
GO

CREATE PROCEDURE [dbo].[gsUeServices_GetUeServiceByUeServiceId]
	@UeServiceId UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP(1) * FROM UeServices WHERE UeServiceId = @UeServiceId;
		
END

