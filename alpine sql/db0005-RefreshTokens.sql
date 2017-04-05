CREATE TABLE [dbo].[RefreshTokens] (
	[id] nvarchar(128)  NOT NULL,
	[clusterId] int IDENTITY(1,1) NOT NULL,
	[subject] nvarchar(128)  NOT NULL,
	[clientId] nvarchar(128) NOT NULL,
	[issuedUtc] datetime NOT NULL,
	[expiresUtc] datetime NOT NULL,
	[protectedTicket] nvarchar(max) NULL,
);

ALTER TABLE dbo.RefreshTokens
	ADD CONSTRAINT PK_RefreshTokens
	PRIMARY KEY NONCLUSTERED (id)

CREATE UNIQUE CLUSTERED INDEX CIX_RefreshTokens ON dbo.RefreshTokens(clusterId)

ALTER SCHEMA [identity] TRANSFER OBJECT::dbo.RefreshTokens;