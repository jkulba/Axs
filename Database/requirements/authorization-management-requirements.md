# Authorization Management Requirements

## Overview
The system must provide fine-grained authorization management capabilities to control individual user permissions at the activity level for specific job contexts.

## Requirements

• **REQ-AUTH-001**: The system shall have an **Authorization** table so that individual user permissions can be granted and managed at the activity level

• **REQ-AUTH-002**: The system shall support job-based authorization where users can have different permissions for different job numbers

• **REQ-AUTH-003**: The system shall enforce unique combinations of job number, user ID, and activity ID to prevent duplicate authorization records

• **REQ-AUTH-004**: The system shall track who created and last updated authorization records for audit trail purposes

• **REQ-AUTH-005**: The system shall store authorization decisions as boolean values (IsAuthorized) for clear permission states

## Database Implementation
- Primary table: `Authorization`
- Key fields: `AuthorizationId`, `JobNumber`, `UserId`, `ActivityId`, `IsAuthorized`, `UtcCreatedAt`, `CreatedByNum`, `UtcUpdatedAt`, `UpdatedByNum`
- Constraints: Primary key on `AuthorizationId`, unique index on combination of `JobNumber`, `UserId`, and `ActivityId`
- Foreign keys: References `Activity` table
