-- Sample data for AccessRequest table
-- Insert 25 realistic records with varied states and information
USE AccessDb;
GO

-- Set the date to a fixed point for reproducible results
DECLARE @Now DATETIME = GETUTCDATE();
DECLARE @30DaysAgo DATETIME = DATEADD(DAY, -30, @Now);
DECLARE @60DaysAgo DATETIME = DATEADD(DAY, -60, @Now);
DECLARE @90DaysAgo DATETIME = DATEADD(DAY, -90, @Now);
DECLARE @FutureDate DATETIME = DATEADD(DAY, 30, @Now);

INSERT INTO [dbo].[AccessRequest]
    (
    [RequestCode],
    [EmployeeNum],
    [UserName],
    [FirstName],
    [LastName],
    [Email],
    [JobNumber],
    [CycleNumber],
    [JobSiteCode],
    [JobManufacturingSiteCode],
    [ApproverNum],
    [ApprovalStatus],
    [UtcCreatedAt],
    [CreatedByNum],
    [UtcUpdatedAt],
    [UpdatedByNum],
    [AccessExpiresAt]
    )
VALUES
    -- Approved requests
    (NEWID(), '100001', 'jsmith', 'John', 'Smith', 'john.smith@example.com', 10001, 2, 'NYC', 'NYC-MFG', '900001', 2, @60DaysAgo, '900001', @30DaysAgo, '900001', @FutureDate),
    (NEWID(), '100002', 'mjohnson', 'Michael', 'Johnson', 'michael.johnson@example.com', 10002, 1, 'CHI', 'CHI-MFG', '900002', 2, @60DaysAgo, '900001', @30DaysAgo, '900002', @FutureDate),
    (NEWID(), '100003', 'lwilliams', 'Linda', 'Williams', 'linda.williams@example.com', 10003, 3, 'LAX', 'LAX-MFG', '900002', 2, @60DaysAgo, '900002', @30DaysAgo, '900002', @FutureDate),
    (NEWID(), '100004', 'bjones', 'Barbara', 'Jones', 'barbara.jones@example.com', 10004, 1, 'BOS', 'BOS-MFG', '900001', 2, @60DaysAgo, '900002', @30DaysAgo, '900001', @FutureDate),
    (NEWID(), '100005', 'rbrown', 'Robert', 'Brown', 'robert.brown@example.com', 10005, 2, 'DFW', 'DFW-MFG', '900003', 2, @60DaysAgo, '900003', @30DaysAgo, '900003', @FutureDate),

    -- Rejected requests
    (NEWID(), '100006', 'mdavis', 'Mary', 'Davis', 'mary.davis@example.com', 10006, 2, 'SFO', 'SFO-MFG', '900003', 3, @90DaysAgo, '900003', @60DaysAgo, '900003', NULL),
    (NEWID(), '100007', 'jmiller', 'James', 'Miller', 'james.miller@example.com', 10007, 1, 'SEA', 'SEA-MFG', '900002', 3, @90DaysAgo, '900002', @60DaysAgo, '900002', NULL),
    (NEWID(), '100008', 'pwilson', 'Patricia', 'Wilson', 'patricia.wilson@example.com', 10008, 3, 'DEN', 'DEN-MFG', '900001', 3, @90DaysAgo, '900001', @60DaysAgo, '900001', NULL),

    -- Pending requests
    (NEWID(), '100009', 'tmoore', 'Thomas', 'Moore', 'thomas.moore@example.com', 10009, 1, 'ATL', 'ATL-MFG', '900002', 1, @30DaysAgo, '900001', NULL, NULL, NULL),
    (NEWID(), '100010', 'jtaylor', 'Jennifer', 'Taylor', 'jennifer.taylor@example.com', 10010, 2, 'MIA', 'MIA-MFG', '900003', 1, @30DaysAgo, '900002', NULL, NULL, NULL),
    (NEWID(), '100011', 'dthomas', 'David', 'Thomas', 'david.thomas@example.com', 10011, 3, 'MSP', 'MSP-MFG', '900001', 1, @30DaysAgo, '900003', NULL, NULL, NULL),
    (NEWID(), '100012', 'sharris', 'Susan', 'Harris', 'susan.harris@example.com', 10012, 1, 'PHL', 'PHL-MFG', '900002', 1, @30DaysAgo, '900001', NULL, NULL, NULL),
    (NEWID(), '100013', 'dmartin', 'Daniel', 'Martin', 'daniel.martin@example.com', 10013, 2, 'CLT', 'CLT-MFG', '900003', 1, @30DaysAgo, '900002', NULL, NULL, NULL),

    -- Recently approved requests
    (NEWID(), '100014', 'nwhite', 'Nancy', 'White', 'nancy.white@example.com', 10014, 1, 'LAS', 'LAS-MFG', '900001', 2, @60DaysAgo, '900003', @Now, '900001', @FutureDate),
    (NEWID(), '100015', 'clewis', 'Charles', 'Lewis', 'charles.lewis@example.com', 10015, 3, 'MCO', 'MCO-MFG', '900002', 2, @60DaysAgo, '900001', @Now, '900002', @FutureDate),
    (NEWID(), '100016', 'kclark', 'Karen', 'Clark', 'karen.clark@example.com', 10016, 2, 'IAD', 'IAD-MFG', '900003', 2, @60DaysAgo, '900002', @Now, '900003', @FutureDate),

    -- Expired requests
    (NEWID(), '100017', 'rwalker', 'Richard', 'Walker', 'richard.walker@example.com', 10017, 1, 'BNA', 'BNA-MFG', '900001', 4, @90DaysAgo, '900003', @60DaysAgo, '900001', @30DaysAgo),
    (NEWID(), '100018', 'ehall', 'Elizabeth', 'Hall', 'elizabeth.hall@example.com', 10018, 3, 'AUS', 'AUS-MFG', '900002', 4, @90DaysAgo, '900001', @60DaysAgo, '900002', @30DaysAgo),
    (NEWID(), '100019', 'jallen', 'Joseph', 'Allen', 'joseph.allen@example.com', 10019, 2, 'MSY', 'MSY-MFG', '900003', 4, @90DaysAgo, '900002', @60DaysAgo, '900003', @30DaysAgo),

    -- Additional varied requests
    (NEWID(), '100020', 'eyoung', 'Emma', 'Young', 'emma.young@example.com', 10020, 1, 'PDX', 'PDX-MFG', '900001', 2, @30DaysAgo, '900003', @Now, '900001', @FutureDate),
    (NEWID(), '100021', 'nking', 'Noah', 'King', 'noah.king@example.com', 10021, 2, 'SAN', 'SAN-MFG', '900002', 1, @30DaysAgo, '900001', NULL, NULL, NULL),
    (NEWID(), '100022', 'owilson', 'Olivia', 'Wilson', 'olivia.wilson@example.com', 10022, 3, 'MCI', 'MCI-MFG', '900003', 3, @60DaysAgo, '900002', @30DaysAgo, '900003', NULL),
    (NEWID(), '100023', 'lscott', 'Liam', 'Scott', 'liam.scott@example.com', 10023, 1, 'SLC', 'SLC-MFG', '900001', 4, @90DaysAgo, '900003', @30DaysAgo, '900001', @30DaysAgo),
    (NEWID(), '100024', 'cmurphy', 'Charlotte', 'Murphy', 'charlotte.murphy@example.com', 10024, 2, 'CLE', 'CLE-MFG', '900002', 2, @60DaysAgo, '900001', @30DaysAgo, '900002', @FutureDate),
    (NEWID(), '100025', 'bnelson', 'Benjamin', 'Nelson', 'benjamin.nelson@example.com', 10025, 3, 'RDU', 'RDU-MFG', '900003', 1, @Now, '900002', NULL, NULL, NULL);

-- Output message with count of records inserted
PRINT 'Inserted 25 sample records into AccessRequest table.';

-- Optionally, verify the inserted data with a simple query
-- SELECT COUNT(*) AS TotalRecords FROM AccessRequest;
-- SELECT ApprovalStatus, COUNT(*) AS StatusCount FROM AccessRequest GROUP BY ApprovalStatus;