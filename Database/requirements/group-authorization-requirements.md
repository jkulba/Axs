# Group-Based Authorization Requirements

## Overview
The system must support group-based authorization to enable efficient permission management by granting access rights to entire user groups rather than managing individual user permissions.

## Requirements

• **REQ-GA-001**: The system shall have a **GroupAuthorization** table so that permissions can be granted to entire user groups rather than individual users

• **REQ-GA-002**: The system shall enforce unique combinations of job number, group ID, and activity ID for group authorizations

• **REQ-GA-003**: The system shall maintain separate audit trails for group-level authorization changes

• **REQ-GA-004**: The system shall prevent deletion of activities and groups that are referenced in authorization records

## Database Implementation
- Primary table: `GroupAuthorization`
- Key fields: `AuthorizationId`, `JobNumber`, `GroupId`, `ActivityId`, `IsAuthorized`, `UtcCreatedAt`, `CreatedByNum`, `UtcUpdatedAt`, `UpdatedByNum`
- Constraints: Primary key on `AuthorizationId`, unique index on combination of `JobNumber`, `GroupId`, and `ActivityId`
- Foreign keys: References both `UserGroup` and `Activity` tables with NO ACTION on delete
