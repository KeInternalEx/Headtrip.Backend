USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsUnrealServices_GetOrCreateUnrealServiceByNickname];
GO

CREATE PROCEDURE [dbo].[gsUnrealServices_GetOrCreateUnrealServiceByNickname]
	@Nickname nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP(1) * FROM UnrealServices WHERE Nickname = @Nickname;
	IF @@ROWCOUNT = 0
		INSERT INTO UnrealServices (Nickname) OUTPUT inserted.*
		VALUES (@Nickname);
		
END

