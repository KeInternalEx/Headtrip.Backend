USE HeadtripGameServer;

DROP TABLE IF EXISTS UnrealServices;
GO
CREATE TABLE UnrealServices (
	UnrealServiceId UNIQUEIDENTIFIER NOT NULL,
	Nickname NVARCHAR(MAX) NOT NULL,
	ServerAddress NVARCHAR(MAX) NOT NULL,
	NumberOfFreeEntries INT NOT NULL DEFAULT 0,
	IsSuperUnrealService BIT NOT NULL DEFAULT 0


	CONSTRAINT [PK_UnrealServices_UnrealServiceId] PRIMARY KEY NONCLUSTERED (UnrealServiceId)
);
GO


