USE HeadtripGameServer;

DROP TABLE IF EXISTS DaemonNumbers;
GO
CREATE TABLE DaemonNumbers (
	Number INT NOT NULL IDENTITY

	CONSTRAINT [PK_DaemonNumbers_Number] PRIMARY KEY CLUSTERED (Number)
);
GO

DECLARE @i int = 0;
WHILE @i < 256
BEGIN
INSERT INTO DaemonNumbers DEFAULT VALUES;
SET @i = @i + 1;
END

GO

