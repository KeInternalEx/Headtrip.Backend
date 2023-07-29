USE HeadtripGameServer;

DROP TABLE IF EXISTS UeServiceServerTransferRequests;
GO

CREATE TABLE UeServiceServerTransferRequests (
	UeServiceServerTransferRequestId UNIQUEIDENTIFIER NOT NULL,
	CurrentChannelId UNIQUEIDENTIFIER NOT NULL,
	CurrentUeServiceId UNIQUEIDENTIFIER NOT NULL,
	TargetChannelId UNIQUEIDENTIFIER NULL,
	TargetUeServiceId UNIQUEIDENTIFIER NULL,
	ServerTransferRequestGroupId UNIQUEIDENTIFIER NULL,
	AccountId UNIQUEIDENTIFIER NOT NULL,
	PartyId UNIQUEIDENTIFIER NULL,
	ZoneName NVARCHAR(255) NOT NULL,
	CharacterLevel INT NOT NULL,
	IsProcessing BIT NOT NULL DEFAULT 0

	CONSTRAINT [PK_UeServiceServerTransferRequests_UeServiceServerTransferRequestId] PRIMARY KEY NONCLUSTERED (UeServiceServerTransferRequestId),
	INDEX [IX_UeServiceServerTransferRequests_CurrentChannelId] NONCLUSTERED (CurrentChannelId)
);
GO


