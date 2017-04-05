CREATE TABLE [dbo].[Roles] (
	[id] int IDENTITY(1,1)  NOT NULL,
	[name] nvarchar(256) NOT NULL,
);

ALTER TABLE dbo.Roles
	ADD CONSTRAINT PK_Roles
	PRIMARY KEY (id)

ALTER SCHEMA [identity] TRANSFER OBJECT::dbo.Roles;