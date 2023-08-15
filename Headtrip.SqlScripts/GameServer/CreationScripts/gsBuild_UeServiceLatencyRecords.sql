USE HeadtripGameServer;

DROP TABLE IF EXISTS UnrealServiceLatencyRecords;
GO
CREATE TABLE UnrealServiceLatencyRecords (
	UnrealServiceId UNIQUEIDENTIFIER NOT NULL,
	AccountId UNIQUEIDENTIFIER NOT NULL,
	Latency FLOAT NOT NULL DEFAULT 0


	INDEX IX_UnrealServiceLatencyRecords_UnrealServiceId NONCLUSTERED (UnrealServiceId),
	INDEX IX_UnrealServiceLatencyRecords_AccountId NONCLUSTERED (AccountId)
);
GO


