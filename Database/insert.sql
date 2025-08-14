-- Sample data for AccessRequest table
-- 25 realistic access request records for testing

INSERT INTO AccessRequests (RequestCode, UserName, JobNumber, CycleNumber, ActivityCode, Application, Version, Machine, UtcCreatedAt) VALUES 
('550e8400-e29b-41d4-a716-446655440001', 'john.smith', 100001, 1, 'READ', 'DataViewer', '2.1.0', 'WS-DEV-001', '2025-08-10 08:30:00'),
('550e8400-e29b-41d4-a716-446655440002', 'jane.doe', 100001, 1, 'WRITE', 'DataEditor', '1.5.2', 'WS-DEV-002', '2025-08-10 09:15:00'),
('550e8400-e29b-41d4-a716-446655440003', 'mike.johnson', 100002, 1, 'READ', 'ReportViewer', '3.0.1', 'WS-PROD-001', '2025-08-10 10:45:00'),
('550e8400-e29b-41d4-a716-446655440004', 'sarah.wilson', 100002, 2, 'ADMIN', 'SystemAdmin', '4.2.0', 'WS-ADMIN-001', '2025-08-10 11:20:00'),
('550e8400-e29b-41d4-a716-446655440005', 'david.brown', 100003, 1, 'READ', 'LogViewer', '1.8.3', 'WS-DEV-003', '2025-08-10 13:10:00'),

('550e8400-e29b-41d4-a716-446655440006', 'lisa.garcia', 100003, 1, 'WRITE', 'ConfigEditor', '2.4.1', 'WS-DEV-004', '2025-08-10 14:30:00'),
('550e8400-e29b-41d4-a716-446655440007', 'robert.taylor', 100004, 1, 'EXECUTE', 'ProcessRunner', '5.1.0', 'WS-PROD-002', '2025-08-10 15:45:00'),
('550e8400-e29b-41d4-a716-446655440008', 'emily.davis', 100004, 2, 'READ', 'AuditViewer', '1.2.5', 'WS-AUDIT-001', '2025-08-11 08:00:00'),
('550e8400-e29b-41d4-a716-446655440009', 'chris.miller', 100005, 1, 'WRITE', 'DatabaseTool', '3.7.2', 'WS-DB-001', '2025-08-11 09:30:00'),
('550e8400-e29b-41d4-a716-446655440010', 'amanda.moore', 100005, 1, 'READ', 'MonitorDash', '2.9.0', 'WS-MON-001', '2025-08-11 10:15:00'),

('550e8400-e29b-41d4-a716-446655440011', 'kevin.white', 100006, 1, 'ADMIN', 'UserManager', '1.6.4', 'WS-ADMIN-002', '2025-08-11 11:45:00'),
('550e8400-e29b-41d4-a716-446655440012', 'michelle.clark', 100006, 2, 'READ', 'ReportGen', '4.3.1', 'WS-RPT-001', '2025-08-11 13:20:00'),
('550e8400-e29b-41d4-a716-446655440013', 'brian.lewis', 100007, 1, 'WRITE', 'DataImport', '2.0.8', 'WS-ETL-001', '2025-08-11 14:10:00'),
('550e8400-e29b-41d4-a716-446655440014', 'stephanie.hall', 100007, 1, 'EXECUTE', 'BatchProcessor', '3.5.0', 'WS-BATCH-001', '2025-08-11 15:30:00'),
('550e8400-e29b-41d4-a716-446655440015', 'jason.young', 100008, 1, 'READ', 'LogAnalyzer', '1.4.7', 'WS-LOG-001', '2025-08-12 08:45:00'),

('550e8400-e29b-41d4-a716-446655440016', 'nicole.king', 100008, 2, 'WRITE', 'ConfigSync', '2.2.3', 'WS-SYNC-001', '2025-08-12 09:20:00'),
('550e8400-e29b-41d4-a716-446655440017', 'ryan.scott', 100009, 1, 'READ', 'MetricsView', '5.0.2', 'WS-METRICS-001', '2025-08-12 10:35:00'),
('550e8400-e29b-41d4-a716-446655440018', 'kimberly.green', 100009, 1, 'ADMIN', 'SystemConfig', '3.1.9', 'WS-SYS-001', '2025-08-12 11:50:00'),
('550e8400-e29b-41d4-a716-446655440019', 'daniel.adams', 100010, 1, 'EXECUTE', 'DeployTool', '4.6.1', 'WS-DEPLOY-001', '2025-08-12 13:15:00'),
('550e8400-e29b-41d4-a716-446655440020', 'laura.baker', 100010, 2, 'READ', 'StatusBoard', '1.7.4', 'WS-STATUS-001', '2025-08-12 14:40:00'),

('550e8400-e29b-41d4-a716-446655440021', 'andrew.nelson', 100011, 1, 'WRITE', 'DataMigrate', '2.8.0', 'WS-MIGRATE-001', '2025-08-12 15:25:00'),
('550e8400-e29b-41d4-a716-446655440022', 'rachel.carter', 100011, 1, 'READ', 'ArchiveView', '3.4.2', 'WS-ARCHIVE-001', '2025-08-13 08:10:00'),
('550e8400-e29b-41d4-a716-446655440023', 'matthew.parker', 100012, 1, 'ADMIN', 'SecurityMgr', '5.2.1', 'WS-SEC-001', '2025-08-13 09:55:00'),
('550e8400-e29b-41d4-a716-446655440024', 'jessica.evans', 100012, 2, 'EXECUTE', 'BackupRunner', '2.5.6', 'WS-BACKUP-001', '2025-08-13 11:30:00'),
('550e8400-e29b-41d4-a716-446655440025', 'joshua.torres', 100013, 1, 'READ', 'NetworkMon', '4.1.3', 'WS-NET-001', '2025-08-13 13:00:00');

-- Verify the inserted records
SELECT COUNT(*) AS TotalRecords FROM AccessRequests;

-- Sample queries to test the data
SELECT TOP 5 * FROM AccessRequests ORDER BY UtcCreatedAt DESC;

SELECT JobNumber, COUNT(*) AS RequestCount 
FROM AccessRequests 
GROUP BY JobNumber 
ORDER BY JobNumber;

SELECT ActivityCode, COUNT(*) AS ActivityCount 
FROM AccessRequests 
WHERE ActivityCode IS NOT NULL
GROUP BY ActivityCode 
ORDER BY ActivityCount DESC;
