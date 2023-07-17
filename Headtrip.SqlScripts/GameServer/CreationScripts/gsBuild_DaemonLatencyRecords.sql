USE HeadtripGameServer;

DROP TABLE IF EXISTS DaemonLatencyRecords;
GO
CREATE TABLE DaemonLatencyRecords (
	DaemonId UNIQUEIDENTIFIER NOT NULL,
	AccountId UNIQUEIDENTIFIER NOT NULL,
	Latency FLOAT NOT NULL DEFAULT 0


	INDEX IX_DaemonLatencyRecords_DaemonId NONCLUSTERED (DaemonId),
	INDEX IX_DaemonLatencyRecords_AccountId NONCLUSTERED (AccountId)
);
GO


