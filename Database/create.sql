SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;

-- Check if database exists before creating it
IF NOT EXISTS (SELECT name
FROM master.dbo.sysdatabases
WHERE name = 'axs')
BEGIN
	-- Create a new database only if it doesn't exist
	CREATE DATABASE axs;
	PRINT 'Database axs created.';
END
ELSE
BEGIN
	PRINT 'Database axs already exists.';
END;

-- Switch to the newly created database
USE axs;

IF  EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[dbo].[AccessGroup]') AND type in (N'U'))
DROP TABLE [dbo].[AccessGroup];

CREATE TABLE [dbo].[AccessGroup]
(
	[GroupId] [int] IDENTITY(1,1) NOT NULL,
	[GroupCode] [uniqueidentifier] NOT NULL,
	[GroupName] [nvarchar](255) NULL,
	[UtcExpirationDate] [datetime2](7) NULL,
	[UtcCreatedAt] [datetime2](7) NULL,
	[CreatedByNum] [nvarchar](50) NULL
) ON [PRIMARY]
;
ALTER TABLE [dbo].[AccessGroup] ADD PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
;
CREATE UNIQUE NONCLUSTERED INDEX [IX_AccessGroup_GroupCode] ON [dbo].[AccessGroup]
(
	[GroupCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
;
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Stores access groups for profile authorization' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccessGroup'
;


IF  EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[dbo].[AccessRequestHistory]') AND type in (N'U'))
DROP TABLE [dbo].[AccessRequestHistory]
;
CREATE TABLE [dbo].[AccessRequestHistory]
(
	[AccessRequestHistoryId] [int] IDENTITY(1,1) NOT NULL,
	[Operation] [int] NOT NULL,
	[RequestId] [int] NOT NULL,
	[RequestCode] [uniqueidentifier] NOT NULL,
	[EmployeeNum] [varchar](12) NOT NULL,
	[UserName] [varchar](12) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[JobNumber] [int] NOT NULL,
	[CycleNumber] [int] NOT NULL,
	[JobSiteCode] [varchar](12) NULL,
	[JobManufacturingSiteCode] [varchar](12) NULL,
	[ApproverNum] [varchar](12) NULL,
	[ApprovalStatus] [int] NOT NULL,
	[UtcCreatedAt] [datetime] NULL,
	[CreatedByNum] [varchar](12) NULL,
	[UtcUpdatedAt] [datetime] NULL,
	[UpdatedByNum] [varchar](12) NULL,
	[AccessExpiresAt] [datetime] NULL
) ON [PRIMARY]
;
ALTER TABLE [dbo].[AccessRequestHistory] ADD PRIMARY KEY CLUSTERED 
(
	[AccessRequestHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
;

IF  EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[dbo].[AccessRequest]') AND type in (N'U'))
DROP TABLE [dbo].[AccessRequest]
;
CREATE TABLE [dbo].[AccessRequest]
(
	[RequestId] [int] IDENTITY(1,1) NOT NULL,
	[RequestCode] [uniqueidentifier] NOT NULL,
	[EmployeeNum] [varchar](12) NOT NULL,
	[UserName] [varchar](12) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[JobNumber] [int] NOT NULL,
	[CycleNumber] [int] NOT NULL,
	[JobSiteCode] [varchar](12) NULL,
	[JobManufacturingSiteCode] [varchar](12) NULL,
	[ApproverNum] [varchar](12) NULL,
	[ApprovalStatus] [int] NOT NULL,
	[UtcCreatedAt] [datetime] NULL,
	[CreatedByNum] [varchar](12) NULL,
	[UtcUpdatedAt] [datetime] NULL,
	[UpdatedByNum] [varchar](12) NULL,
	[AccessExpiresAt] [datetime] NULL
) ON [PRIMARY]
;
ALTER TABLE [dbo].[AccessRequest] ADD  CONSTRAINT [PK_AccessRequest] PRIMARY KEY CLUSTERED 
(
	[RequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
;
SET ANSI_PADDING ON
;
CREATE UNIQUE NONCLUSTERED INDEX [Index_EmployeeNum] ON [dbo].[AccessRequest]
(
	[EmployeeNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
;
CREATE UNIQUE NONCLUSTERED INDEX [Index_RequestCode] ON [dbo].[AccessRequest]
(
	[RequestCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
;
SET ANSI_NULLS ON
;
SET QUOTED_IDENTIFIER ON
;

CREATE TRIGGER [dbo].[TR_AccessRequestHistory] ON [dbo].[AccessRequest]
FOR insert, update, delete
AS
IF EXISTS (SELECT 0
FROM deleted)
BEGIN
	IF EXISTS (SELECT 0
	FROM inserted)
    BEGIN
		-- Update request, Operation 2
		INSERT INTO [AccessRequestHistory]
		SELECT 2, *
		FROM inserted
	END
    ELSE
    BEGIN
		-- Delete request, Operation 3
		INSERT INTO [AccessRequestHistory]
		SELECT 3, *
		FROM deleted
	END
END
ELSE
BEGIN
	-- Insert request, Operation 1
	INSERT INTO [AccessRequestHistory]
	SELECT 1, *
	FROM inserted
END
;
ALTER TABLE [dbo].[AccessRequest] ENABLE TRIGGER [TR_AccessRequestHistory]
;
