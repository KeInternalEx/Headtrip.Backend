USE HeadtripGameServer;

DROP TABLE IF EXISTS UnrealServiceServerTransferRequests;
GO

CREATE TABLE UnrealServiceServerTransferRequests (
	UnrealServiceServerTransferRequestId UNIQUEIDENTIFIER NOT NULL,
	CurrentChannelId UNIQUEIDENTIFIER NOT NULL,
	CurrentUnrealServiceId UNIQUEIDENTIFIER NOT NULL,
	TargetChannelId UNIQUEIDENTIFIER NULL,
	TargetUnrealServiceId UNIQUEIDENTIFIER NULL,
	ServerTransferRequestGroupId UNIQUEIDENTIFIER NULL,
	AccountId UNIQUEIDENTIFIER NOT NULL,
	PartyId UNIQUEIDENTIFIER NULL,
	ZoneName NVARCHAR(255) NOT NULL,
	CharacterLevel INT NOT NULL,
	IsProcessing BIT NOT NULL DEFAULT 0

	CONSTRAINT [PK_UnrealServiceServerTransferRequests_UnrealServiceServerTransferRequestId] PRIMARY KEY NONCLUSTERED (UnrealServiceServerTransferRequestId),
	INDEX [IX_UnrealServiceServerTransferRequests_CurrentChannelId] NONCLUSTERED (CurrentChannelId)
);
GO


