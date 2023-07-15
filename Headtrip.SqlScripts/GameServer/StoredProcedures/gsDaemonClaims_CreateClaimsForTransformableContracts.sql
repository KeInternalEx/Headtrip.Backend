USE [HeadtripGameServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [dbo].[gsDaemonClaims_CreateClaimsForTransformableContracts]
GO

CREATE PROCEDURE [dbo].[gsDaemonClaims_CreateClaimsForTransformableContracts]
	@DaemonId UniqueIdentifier,
	@NumberOfCreatableInstances int
AS
BEGIN
	SET NOCOUNT ON;


	INSERT INTO DaemonClaims OUTPUT inserted.*
	SELECT TOP (@NumberOfCreatableInstances)
		[ClaimsAggragate].ZoneName,
		@DaemonId,
		[ClaimsAggragate].NumberOfContracts
	FROM (
		SELECT
			ZoneName,
			CEILING(([Aggregate].NumberOfContracts - [Aggregate].NumberOfClaims * [Aggregate].SoftPlayerCap) / [Aggregate].SoftPlayerCap) AS NumberOfClaimsRequired,
			[Aggregate].SoftPlayerCap AS NumberOfContracts
		FROM (
			SELECT
				[Contract].ZoneName,
				CAST(COUNT([Contract].DaemonContractId) AS Float) AS NumberOfContracts,
				CAST((SELECT COUNT([Claim].ZoneName) FROM DaemonClaims AS [Claim] WHERE [Claim].ZoneName = [Contract].ZoneName) AS Float) AS NumberOfClaims,
				CAST((SELECT TOP 1 [Zone].SoftPlayerCap FROM Zones AS [Zone] WHERE [Zone].ZoneName = [Contract].ZoneName) AS Float) AS SoftPlayerCap
			FROM DaemonContracts AS [Contract]
			WHERE
				[Contract].TargetChannelId IS NULL AND
				[Contract].TargetDaemonId IS NULL
			GROUP BY [Contract].ZoneName
		) AS [Aggregate]
		WHERE 
			[Aggregate].NumberOfContracts > [Aggregate].SoftPlayerCap * [Aggregate].NumberOfClaims
	) AS [ClaimsAggragate]
	INNER JOIN DaemonNumbers AS [Index]
		ON [Index].Number < [ClaimsAggragate].NumberOfClaimsRequired;

END

