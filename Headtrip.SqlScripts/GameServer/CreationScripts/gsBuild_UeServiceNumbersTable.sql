USE HeadtripGameServer;

DROP TABLE IF EXISTS UnrealServiceNumbers;
GO
CREATE TABLE UnrealServiceNumbers (
	Number INT NOT NULL IDENTITY

	CONSTRAINT [PK_UnrealServiceNumbers_Number] PRIMARY KEY CLUSTERED (Number)
);
GO

DECLARE @i int = 0;
WHILE @i < 256
BEGIN
INSERT INTO UnrealServiceNumbers DEFAULT VALUES;
SET @i = @i + 1;
END

GO

