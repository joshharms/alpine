
CREATE TABLE [dbo].[] (
    [id] uniqueidentifier  NOT NULL,
	[clusterId] int IDENTITY(1,1) NOT NULL,
    [] int  NOT NULL,
	[] nvarchar(50) NOT NULL,
	[] nvarchar(300) NOT NULL,
	[] nvarchar(300) NOT NULL,
	[] varchar(15) NOT NULL,
	[] uniqueidentifier NOT NULL,
);
GO

ALTER TABLE dbo.
	ADD CONSTRAINT PK_
	PRIMARY KEY NONCLUSTERED (id)

CREATE UNIQUE CLUSTERED INDEX CIX_TABLENAME ON dbo.(clusterId)