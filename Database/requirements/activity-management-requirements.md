# Activity Management Requirements

## Overview
The system must provide comprehensive activity management capabilities to define and control the various activities that users can perform within the system.

## Requirements

• **REQ-ACT-001**: The system shall have an **Activity** table so that a prescriptive list of system activities are managed in the database

• **REQ-ACT-002**: The system shall uniquely identify each activity with both an auto-generated ActivityId and a user-defined ActivityCode

• **REQ-ACT-003**: The system shall support activity deactivation through an IsActive flag rather than deletion to maintain historical integrity

• **REQ-ACT-004**: The system shall enforce unique activity codes to prevent duplicate activity definitions

## Database Implementation
- Primary table: `Activity`
- Key fields: `ActivityId`, `ActivityCode`, `ActivityName`, `Description`, `IsActive`
- Constraints: Primary key on `ActivityId`, unique index on `ActivityCode`
