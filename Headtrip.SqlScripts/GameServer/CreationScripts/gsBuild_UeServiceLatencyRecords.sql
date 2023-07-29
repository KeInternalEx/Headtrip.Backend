USE HeadtripGameServer;

DROP TABLE IF EXISTS UeServiceLatencyRecords;
GO
CREATE TABLE UeServiceLatencyRecords (
	UeServiceId UNIQUEIDENTIFIER NOT NULL,
	AccountId UNIQUEIDENTIFIER NOT NULL,
	Latency FLOAT NOT NULL DEFAULT 0


	INDEX IX_UeServiceLatencyRecords_UeServiceId NONCLUSTERED (UeServiceId),
	INDEX IX_UeServiceLatencyRecords_AccountId NONCLUSTERED (AccountId)
);
GO


