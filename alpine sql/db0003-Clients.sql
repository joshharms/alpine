CREATE TABLE [dbo].[Clients] (
    [id] nvarchar(128)  NOT NULL,
    [secret] nvarchar(max)  NOT NULL,
	[name] nvarchar(max) NOT NULL,
	[applicationType] int NOT NULL,
	[active] bit NOT NULL,
	[refreshTokenLifetime] int NOT NULL,
	[allowedOrigin] nvarchar(128) NOT NULL,

);

ALTER TABLE dbo.Clients
	ADD CONSTRAINT PK_Clients
	PRIMARY KEY (id)

ALTER SCHEMA [identity] TRANSFER OBJECT::dbo.Clients;