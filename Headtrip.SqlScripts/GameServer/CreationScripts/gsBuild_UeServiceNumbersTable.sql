USE HeadtripGameServer;

DROP TABLE IF EXISTS UeServiceNumbers;
GO
CREATE TABLE UeServiceNumbers (
	Number INT NOT NULL IDENTITY

	CONSTRAINT [PK_UeServiceNumbers_Number] PRIMARY KEY CLUSTERED (Number)
);
GO

DECLARE @i int = 0;
WHILE @i < 256
BEGIN
INSERT INTO UeServiceNumbers DEFAULT VALUES;
SET @i = @i + 1;
END

GO

