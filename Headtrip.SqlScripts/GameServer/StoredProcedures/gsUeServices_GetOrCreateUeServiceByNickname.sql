USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUeServices_GetOrCreateUeServiceByNickname];
GO

CREATE PROCEDURE [dbo].[gsUeServices_GetOrCreateUeServiceByNickname]
	@Nickname nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP(1) * FROM UeServices WHERE Nickname = @Nickname;
	IF @@ROWCOUNT = 0
		INSERT INTO UeServices (Nickname) OUTPUT inserted.*
		VALUES (@Nickname);
		
END

