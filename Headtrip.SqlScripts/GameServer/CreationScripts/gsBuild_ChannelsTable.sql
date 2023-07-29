﻿USE HeadtripGameServer;

DROP TABLE IF EXISTS Channels;
GO
CREATE TABLE Channels (
	ChannelId UNIQUEIDENTIFIER NOT NULL UNIQUE,
	UeServiceId UNIQUEIDENTIFIER NOT NULL,
	ZoneName NVARCHAR(255) NOT NULL,
	ConnectionString NVARCHAR(MAX) NOT NULL,
	IsAvailable BIT NOT NULL DEFAULT 0

	
	CONSTRAINT [PK_Channels_ChannelId] PRIMARY KEY NONCLUSTERED (ChannelId)
);
GO


