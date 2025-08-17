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
END
GO

-- Switch to the newly created database
USE axs;
GO

-- Drop and recreate Activity table
IF EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[dbo].[Activity]') AND type in (N'U'))
DROP TABLE [dbo].[Activity]
GO

CREATE TABLE [dbo].[Activity]
(
    [ActivityId] [int] IDENTITY(1,1) NOT NULL,
    [ActivityCode] [nvarchar](50) NOT NULL,
    [ActivityName] [nvarchar](100) NOT NULL,
    [Description] [nvarchar](500) NULL,
    [IsActive] [bit] NOT NULL DEFAULT 1
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Activity] ADD CONSTRAINT [PK_Activity] PRIMARY KEY CLUSTERED 
(
    [ActivityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Activity_ActivityCode] ON [dbo].[Activity]
(
    [ActivityCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

-- Drop and recreate UserGroup table
IF EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[dbo].[UserGroup]') AND type in (N'U'))
DROP TABLE [dbo].[UserGroup]
GO

CREATE TABLE [dbo].[UserGroup]
(
    [GroupId] [int] IDENTITY(1,1) NOT NULL,
    [GroupName] [nvarchar](100) NOT NULL,
    [Description] [nvarchar](500) NULL,
    [GroupOwner] [nvarchar](50) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserGroup] ADD CONSTRAINT [PK_UserGroup] PRIMARY KEY CLUSTERED 
(
    [GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

-- Drop and recreate AccessRequest table
IF EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[dbo].[AccessRequest]') AND type in (N'U'))
DROP TABLE [dbo].[AccessRequest]
GO

CREATE TABLE [dbo].[AccessRequest]
(
    [RequestId] [int] IDENTITY(1,1) NOT NULL,
    [RequestCode] [uniqueidentifier] NOT NULL,
    [UserName] [nvarchar](50) NOT NULL,
    [JobNumber] [int] NOT NULL,
    [CycleNumber] [int] NOT NULL,
    [ActivityCode] [nvarchar](50) NULL,
    [ApplicationName] [nvarchar](50) NULL,
    [Workstation] [nvarchar](50) NULL,
    [UtcCreatedAt] [datetime] NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AccessRequest] ADD CONSTRAINT [PK_AccessRequest] PRIMARY KEY CLUSTERED 
(
    [RequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [Index_RequestCode] ON [dbo].[AccessRequest]
(
    [RequestCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

-- Drop and recreate Authorization table (must be created after Activity table for foreign key)
IF EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[dbo].[Authorization]') AND type in (N'U'))
DROP TABLE [dbo].[Authorization]
GO

CREATE TABLE [dbo].[Authorization]
(
    [AuthorizationId] [int] IDENTITY(1,1) NOT NULL,
    [JobNumber] [int] NOT NULL,
    [UserId] [nvarchar](50) NOT NULL,
    [ActivityId] [int] NOT NULL,
    [IsAuthorized] [bit] NOT NULL,
    [UtcCreatedAt] [datetime] NOT NULL,
    [CreatedByNum] [nvarchar](50) NULL,
    [UtcUpdatedAt] [datetime] NULL,
    [UpdatedByNum] [nvarchar](50) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Authorization] ADD CONSTRAINT [PK_Authorization] PRIMARY KEY CLUSTERED 
(
    [AuthorizationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Authorization_JobNumber_UserId_ActivityId] ON [dbo].[Authorization]
(
    [JobNumber] ASC,
    [UserId] ASC,
    [ActivityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

-- Drop and recreate UserGroupMember table (must be created after UserGroup table for foreign key)
IF EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[dbo].[UserGroupMember]') AND type in (N'U'))
DROP TABLE [dbo].[UserGroupMember]
GO

CREATE TABLE [dbo].[UserGroupMember]
(
    [GroupId] [int] NOT NULL,
    [UserId] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserGroupMember] ADD CONSTRAINT [PK_UserGroupMember] PRIMARY KEY CLUSTERED 
(
    [GroupId] ASC,
    [UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

-- Drop and recreate GroupAuthorization table (must be created after UserGroup and Activity tables for foreign keys)
IF EXISTS (SELECT *
FROM sys.objects
WHERE object_id = OBJECT_ID(N'[dbo].[GroupAuthorization]') AND type in (N'U'))
DROP TABLE [dbo].[GroupAuthorization]
GO

CREATE TABLE [dbo].[GroupAuthorization]
(
    [AuthorizationId] [int] IDENTITY(1,1) NOT NULL,
    [JobNumber] [int] NOT NULL,
    [GroupId] [int] NOT NULL,
    [ActivityId] [int] NOT NULL,
    [IsAuthorized] [bit] NOT NULL,
    [UtcCreatedAt] [datetime] NOT NULL,
    [CreatedByNum] [nvarchar](50) NULL,
    [UtcUpdatedAt] [datetime] NULL,
    [UpdatedByNum] [nvarchar](50) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GroupAuthorization] ADD CONSTRAINT [PK_GroupAuthorization] PRIMARY KEY CLUSTERED 
(
    [AuthorizationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_GroupAuthorization_JobNumber_GroupId_ActivityId] ON [dbo].[GroupAuthorization]
(
    [JobNumber] ASC,
    [GroupId] ASC,
    [ActivityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

-- Add Foreign Key Constraints
ALTER TABLE [dbo].[Authorization] ADD CONSTRAINT [FK_Authorization_Activity]
FOREIGN KEY([ActivityId])
REFERENCES [dbo].[Activity] ([ActivityId])
ON DELETE NO ACTION
GO

ALTER TABLE [dbo].[UserGroupMember] ADD CONSTRAINT [FK_UserGroupMember_UserGroup]
FOREIGN KEY([GroupId])
REFERENCES [dbo].[UserGroup] ([GroupId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[GroupAuthorization] ADD CONSTRAINT [FK_GroupAuthorization_UserGroup]
FOREIGN KEY([GroupId])
REFERENCES [dbo].[UserGroup] ([GroupId])
ON DELETE NO ACTION
GO

ALTER TABLE [dbo].[GroupAuthorization] ADD CONSTRAINT [FK_GroupAuthorization_Activity]
FOREIGN KEY([ActivityId])
REFERENCES [dbo].[Activity] ([ActivityId])
ON DELETE NO ACTION
GO

PRINT 'Database schema updated successfully.'
GO
