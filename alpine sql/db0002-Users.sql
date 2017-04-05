CREATE TABLE [dbo].[Users] (
	[id] uniqueidentifier  NOT NULL,
	[clusterId] int IDENTITY(1,1) NOT NULL,
	[active] bit NOT NULL,
	[email] nvarchar(256) NOT NULL,
	[password] nvarchar(max) NOT NULL,
	[passwordLastUpdated] datetime NOT NULL,
	[accessFailedCount] int NOT NULL,
	[firstName] nvarchar(100) NOT NULL,
	[lastName] nvarchar(100) NOT NULL,
	[phoneNumber] varchar(20) NULL,
	[roleId] int NOT NULL,
	[companyId] int NULL,
	[avatar] uniqueidentifier NULL,
);

ALTER TABLE dbo.Users
	ADD CONSTRAINT PK_Users
	PRIMARY KEY NONCLUSTERED (id)

CREATE UNIQUE CLUSTERED INDEX CIX_Users ON dbo.Users(clusterId)

ALTER SCHEMA [identity] TRANSFER OBJECT::dbo.Users;  