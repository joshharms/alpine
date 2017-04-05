CREATE TABLE [dbo].[Audiences] (
	[id] uniqueidentifier  NOT NULL,
	[clusterId] int IDENTITY(1,1) NOT NULL,
	[name] nvarchar(256)  NOT NULL,
	[base64Secret] nvarchar(max) NOT NULL,
);

ALTER TABLE dbo.Audiences
	ADD CONSTRAINT PK_Audiences
	PRIMARY KEY NONCLUSTERED (id)

CREATE UNIQUE CLUSTERED INDEX CIX_Users ON dbo.Audiences(clusterId)

ALTER SCHEMA [identity] TRANSFER OBJECT::dbo.Audiences;