-- INSERT 10 sample AccessGroup records
INSERT INTO [dbo].[AccessGroup] ([GroupCode], [GroupName], [UtcExpirationDate], [UtcCreatedAt], [CreatedByNum])
VALUES 
    (NEWID(), 'Admin Access Group', '2025-12-31 23:59:59', GETUTCDATE(), 'SYS001'),
    (NEWID(), 'Developer Access Group', '2025-12-31 23:59:59', GETUTCDATE(), 'SYS001'),
    (NEWID(), 'QA Testing Group', '2025-10-31 23:59:59', GETUTCDATE(), 'MGR001'),
    (NEWID(), 'Production Support', '2026-06-30 23:59:59', GETUTCDATE(), 'MGR001'),
    (NEWID(), 'Data Analyst Group', '2025-09-30 23:59:59', GETUTCDATE(), 'DBA001'),
    (NEWID(), 'Security Team', '2026-12-31 23:59:59', GETUTCDATE(), 'SEC001'),
    (NEWID(), 'Contractor Access', '2025-08-31 23:59:59', GETUTCDATE(), 'HR001'),
    (NEWID(), 'Manager Override', '2026-03-31 23:59:59', GETUTCDATE(), 'MGR002'),
    (NEWID(), 'Temporary Access', '2025-09-15 23:59:59', GETUTCDATE(), 'TMP001'),
    (NEWID(), 'Audit Team', '2025-11-30 23:59:59', GETUTCDATE(), 'AUD001');