USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsDaemon_GetOrCreateDaemonByNickname];
GO

CREATE PROCEDURE [dbo].[gsDaemon_GetOrCreateDaemonByNickname]
	@Nickname nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP(1) * FROM Daemons WHERE Nickname = @Nickname;
	IF @@ROWCOUNT = 0
		INSERT INTO Daemons (Nickname) OUTPUT inserted.*
		VALUES (@Nickname);
		
END

