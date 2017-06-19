CREATE TABLE [dbo].[UserPasswordResetLinks] (
	[id] uniqueidentifier NOT NULL,
	[guid] uniqueidentifier NOT NULL,
	[linkType] varchar(10) NOT NULL,
	[linkExpiration] datetime NOT NULL,
	[userId] uniqueidentifier NOT NULL,
);
GO

ALTER TABLE dbo.UserPasswordResetLinks
	ADD CONSTRAINT PK_UserPasswordResetLinks
	PRIMARY KEY (id)

ALTER SCHEMA [identity] TRANSFER OBJECT::dbo.UserPasswordResetLinks;  
GO  

ALTER TABLE [identity].[UserPasswordResetLinks]
	ADD FOREIGN KEY (userId)
	REFERENCES [identity].[Users](id)