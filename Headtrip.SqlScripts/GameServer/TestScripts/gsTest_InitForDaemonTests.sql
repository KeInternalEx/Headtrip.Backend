use HeadtripGameServer;

DELETE FROM Zones;
DELETE FROM UeServiceClaims;
DELETE FROM UeServiceServerTransferRequests;
DELETE FROM UeServices;

INSERT INTO Zones (
	ZoneName,
	LevelName,
	SoftPlayerCap,
HardPlayerCap) VALUES
	('TestZone',
	'TestLevel',
	50,
	75);

GO

INSERT INTO Zones (
	ZoneName,
	LevelName,
	SoftPlayerCap,
HardPlayerCap) VALUES
	('TestZone2',
	'TestLevel2',
	10,
	15);

GO

INSERT INTO Zones (
	ZoneName,
	LevelName,
	SoftPlayerCap,
HardPlayerCap) VALUES
	('TestZone3',
	'TestLevel3',
	30,
	45);

GO


INSERT INTO UeServiceServerTransferRequests(
	UeServiceServerTransferRequestId,
	CurrentChannelId,
	CurrentUeServiceId,
	TargetChannelId,
	TargetUeServiceId,
	AccountId,
	PartyId,
	ZoneName)
VALUES(
newid(),
	newid(),
	newid(),
	null,
	null,
	newid(),
	null,
	'TestZone');
GO 230


INSERT INTO UeServiceServerTransferRequests(
	UeServiceServerTransferRequestId,
	CurrentChannelId,
	CurrentUeServiceId,
	TargetChannelId,
	TargetUeServiceId,
	AccountId,
	PartyId,
	ZoneName)
VALUES(
newid(),
	newid(),
	newid(),
	null,
	null,
	newid(),
	null,
	'TestZone2');
GO 171

INSERT INTO UeServiceServerTransferRequests(
	UeServiceServerTransferRequestId,
	CurrentChannelId,
	CurrentUeServiceId,
	TargetChannelId,
	TargetUeServiceId,
	AccountId,
	PartyId,
	ZoneName)
VALUES(
newid(),
	newid(),
	newid(),
	null,
	null,
	newid(),
	null,
	'TestZone3');
GO 3

