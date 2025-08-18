-- Sample data for AccessRequest table
-- 25 realistic access request records for testing

INSERT INTO AccessRequest
    (RequestCode, UserName, JobNumber, CycleNumber, ActivityCode, ApplicationName, Workstation, UtcCreatedAt)
VALUES
    ('550e8400-e29b-41d4-a716-446655440001', 'john.smith', 100001, 1, 'JOB.OPEN', 'JobPro', 'WS-DEV-001', '2025-08-10 08:30:00'),
    ('550e8400-e29b-41d4-a716-446655440002', 'jane.doe', 100001, 1, 'JOB.OPEN', 'JobPro', 'WS-DEV-002', '2025-08-10 09:15:00'),
    ('550e8400-e29b-41d4-a716-446655440003', 'mike.johnson', 100002, 1, 'JOB.OPEN', 'JobPro', 'WS-PROD-001', '2025-08-10 10:45:00'),
    ('550e8400-e29b-41d4-a716-446655440004', 'sarah.wilson', 100002, 2, 'JOB.OPEN', 'JobPro', 'WS-ADMIN-001', '2025-08-10 11:20:00'),
    ('550e8400-e29b-41d4-a716-446655440005', 'david.brown', 100003, 1, 'JOB.OPEN', 'JobPro', 'WS-DEV-003', '2025-08-10 13:10:00'),

    ('550e8400-e29b-41d4-a716-446655440006', 'lisa.garcia', 100003, 1, 'JOB.OPEN', 'JobPro', 'WS-DEV-004', '2025-08-10 14:30:00'),
    ('550e8400-e29b-41d4-a716-446655440007', 'robert.taylor', 100004, 1, 'JOB.OPEN', 'JobPro', 'WS-PROD-002', '2025-08-10 15:45:00'),
    ('550e8400-e29b-41d4-a716-446655440008', 'emily.davis', 100004, 2, 'JOB.OPEN', 'JobPro', 'WS-AUDIT-001', '2025-08-11 08:00:00'),
    ('550e8400-e29b-41d4-a716-446655440009', 'chris.miller', 100005, 1, 'JOB.OPEN', 'JobPro', 'WS-DB-001', '2025-08-11 09:30:00'),
    ('550e8400-e29b-41d4-a716-446655440010', 'amanda.moore', 100005, 1, 'JOB.OPEN', 'JobPro', 'WS-MON-001', '2025-08-11 10:15:00'),

    ('550e8400-e29b-41d4-a716-446655440011', 'kevin.white', 100006, 1, 'JOB.OPEN', 'PagePro', 'WS-JOB.OPEN-002', '2025-08-11 11:45:00'),
    ('550e8400-e29b-41d4-a716-446655440012', 'michelle.clark', 100006, 2, 'JOB.OPEN', 'PagePro', 'WS-RPT-001', '2025-08-11 13:20:00'),
    ('550e8400-e29b-41d4-a716-446655440013', 'brian.lewis', 100007, 1, 'JOB.OPEN', 'PagePro', 'WS-ETL-001', '2025-08-11 14:10:00'),
    ('550e8400-e29b-41d4-a716-446655440014', 'stephanie.hall', 100007, 1, 'JOB.OPEN', 'PagePro', 'WS-BATCH-001', '2025-08-11 15:30:00'),
    ('550e8400-e29b-41d4-a716-446655440015', 'jason.young', 100008, 1, 'JOB.OPEN', 'PagePro', 'WS-LOG-001', '2025-08-12 08:45:00'),

    ('550e8400-e29b-41d4-a716-446655440016', 'nicole.king', 100008, 2, 'JOB.OPEN', 'PagePro', 'WS-SYNC-001', '2025-08-12 09:20:00'),
    ('550e8400-e29b-41d4-a716-446655440017', 'ryan.scott', 100009, 1, 'JOB.OPEN', 'PagePro', 'WS-METRICS-001', '2025-08-12 10:35:00'),
    ('550e8400-e29b-41d4-a716-446655440018', 'kimberly.green', 100009, 1, 'JOB.OPEN', 'PagePro', 'WS-SYS-001', '2025-08-12 11:50:00'),
    ('550e8400-e29b-41d4-a716-446655440019', 'daniel.adams', 100010, 1, 'JOB.OPEN', 'DeploPageProyTool', 'WS-DEPLOY-001', '2025-08-12 13:15:00'),
    ('550e8400-e29b-41d4-a716-446655440020', 'laura.baker', 100010, 2, 'JOB.OPEN', 'PagePro', 'WS-STATUS-001', '2025-08-12 14:40:00'),

    ('550e8400-e29b-41d4-a716-446655440021', 'andrew.nelson', 100011, 1, 'JOB.OPEN', 'RenderServer', 'WS-MIGRATE-001', '2025-08-12 15:25:00'),
    ('550e8400-e29b-41d4-a716-446655440022', 'rachel.carter', 100011, 1, 'JOB.OPEN', 'RenderServer', 'WS-ARCHIVE-001', '2025-08-13 08:10:00'),
    ('550e8400-e29b-41d4-a716-446655440023', 'matthew.parker', 100012, 1, 'JOB.OPEN', 'RenderServer', 'WS-SEC-001', '2025-08-13 09:55:00'),
    ('550e8400-e29b-41d4-a716-446655440024', 'jessica.evans', 100012, 2, 'JOB.OPEN', 'RenderServer', 'WS-BACKUP-001', '2025-08-13 11:30:00'),
    ('550e8400-e29b-41d4-a716-446655440025', 'joshua.torres', 100013, 1, 'JOB.OPEN', 'RenderServer', 'WS-NET-001', '2025-08-13 13:00:00');

-- Verify the inserted records
SELECT COUNT(*) AS TotalRecords
FROM AccessRequest;

-- Sample queries to test the data
SELECT TOP 5
    *
FROM AccessRequest
ORDER BY UtcCreatedAt DESC;

SELECT JobNumber, COUNT(*) AS RequestCount
FROM AccessRequest
GROUP BY JobNumber
ORDER BY JobNumber;

SELECT ActivityCode, COUNT(*) AS ActivityCount
FROM AccessRequest
WHERE ActivityCode IS NOT NULL
GROUP BY ActivityCode
ORDER BY ActivityCount DESC;
