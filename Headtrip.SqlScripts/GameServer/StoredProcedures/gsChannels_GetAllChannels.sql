USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsChannels_GetAllChannels];
GO

CREATE PROCEDURE [dbo].[gsChannels_GetAllChannels]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM Channels;
		
END

