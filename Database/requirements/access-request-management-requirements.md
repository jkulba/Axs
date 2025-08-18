# Access Request Management Requirements

## Overview
The system must provide robust access request management to track and process user requests for system access in a controlled and auditable manner.

## Requirements

• **REQ-AR-001**: The system shall have an **AccessRequest** table so that user requests for system access can be tracked and managed

• **REQ-AR-002**: The system shall generate unique request codes (GUIDs) for each access request to ensure global uniqueness

• **REQ-AR-003**: The system shall capture job numbers and cycle numbers to associate access requests with specific work contexts

• **REQ-AR-004**: The system shall track the requesting user, application, version, and machine information for audit purposes

• **REQ-AR-005**: The system shall record UTC timestamps for when access requests are created

## Database Implementation
- Primary table: `AccessRequest`
- Key fields: `RequestId`, `RequestCode`, `UserName`, `JobNumber`, `CycleNumber`, `ActivityCode`, `Application`, `Version`, `Machine`, `UtcCreatedAt`
- Constraints: Primary key on `RequestId`, unique index on `RequestCode`
