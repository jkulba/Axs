# Access Database Management System

This directory contains the complete database management system for the Access application, which handles access requests within the ProFile document management system. The system is designed to run on **Debian-based Linux distributions** (tested on Linux Mint Debian Edition) using **Podman** as the container runtime.

## System Requirements

- **Operating System**: Debian-based Linux distribution (Ubuntu, Linux Mint Debian Edition, etc.)
- **Container Runtime**: Podman (required dependency)
- **Database**: Microsoft SQL Server 2022 (containerized)
- **Optional**: Python 3 with pyodbc for database validation

## Database Architecture

### Database: AccessDb

The Access database contains the following tables:

#### AccessRequest Table
- **Purpose**: Contains access requests for jobs within the ProFile document management system
- **Key Features**:
  - Employee information (number, username, name, email)
  - Job details (job number, cycle number, site codes)
  - Approval workflow (approver, status, timestamps)
  - Access expiration tracking
- **Unique Constraints**: Employee number and request code
- **Primary Key**: RequestId (auto-increment)

#### AccessRequestHistory Table
- **Purpose**: Audit trail for all CRUD operations on AccessRequest table
- **Key Features**:
  - Tracks INSERT (Operation 1), UPDATE (Operation 2), and DELETE (Operation 3) operations
  - Maintains complete historical record of all changes
  - Automatically populated via database trigger

#### AccessGroup Table
- **Purpose**: Stores access groups for profile authorization
- **Key Features**:
  - Group management with unique codes and names
  - Expiration date tracking
  - Creation audit trail

### Database Trigger
- **TR_AccessRequestHistory**: Automatically logs all changes to AccessRequest table into AccessRequestHistory

## Container Management Scripts

### manage.sh - Primary Container Management Tool

The `manage.sh` script is the main tool for managing the SQL Server container. It provides the following functions:

#### Commands

```bash
./manage.sh start     # Start the SQL Server container
./manage.sh stop      # Stop and remove the SQL Server container
./manage.sh status    # Show container status
./manage.sh health    # Check SQL Server health and connectivity
```

#### Technical Details

- **Container Name**: `mssql-ultra3`
- **Image**: `mcr.microsoft.com/mssql/server:2022-latest`
- **Network**: Host networking mode
- **Data Persistence**: `/home/jim/.mssql-ultra3/mssql-data`
- **Port**: 1433 (standard SQL Server port)
- **Authentication**: SA account with configured password

#### Features

- **Automatic Restart**: Container configured with `--restart=unless-stopped`
- **Health Monitoring**: Built-in health check using sqlcmd
- **Data Persistence**: Volume mounting for database persistence
- **User Permissions**: Proper ownership setup for data directory

### setup.sh - Database Initialization

The `setup.sh` script initializes the database by executing the `create.sql` script.

#### Functionality
- Copies `create.sql` into the container
- Executes the SQL script using sqlcmd
- Provides success/failure feedback
- Creates the complete database schema

### sql-exec.sh - Generic SQL Script Executor

A flexible utility for executing any SQL script against the containerized database.

#### Usage
```bash
./sql-exec.sh <sql-script-file>
```

#### Features
- Parameter validation and file existence checking
- Automatic script copying to container
- SQL execution with proper error handling
- Exit code reporting

## Database Schema Scripts

### create.sql - Complete Database Schema

The `create.sql` script contains the complete database schema definition:

#### What it creates:
1. **AccessDb Database**: Main application database
2. **AccessGroup Table**: Access group management
3. **AccessRequest Table**: Primary access request tracking
4. **AccessRequestHistory Table**: Audit trail table
5. **Database Trigger**: Automatic history logging
6. **Indexes**: Performance optimization indexes
7. **Constraints**: Data integrity enforcement

#### Key Features:
- **Idempotent Design**: Safe to run multiple times
- **Drop and Recreate**: Ensures clean schema deployment
- **Comprehensive Indexing**: Optimized for common query patterns
- **Audit Trail**: Automatic change tracking

### Insert.sql - Sample Data

Provides realistic sample data for testing and development:
- 25 sample access requests with varied states
- Different approval statuses (Pending, Approved, Rejected, Expired)
- Realistic employee and job information
- Time-based data distribution

## Validation and Testing

### validate.py - Database Connectivity Validator

Python script for validating database connectivity and configuration.

#### Dependencies
- `pyodbc` Python module
- Microsoft ODBC Driver for SQL Server

#### Features
- Connection string validation
- Database accessibility testing
- Driver installation guidance
- Error diagnostics

## Container Configuration

### Podman Requirements

The system relies on Podman for container management:

- **User Mode**: Runs in rootless mode where possible
- **Network**: Host networking for simplified connectivity
- **Storage**: Persistent volume mounting
- **Security**: SELinux-compatible volume labeling (`:Z` flag)

### Security Considerations

- SA password is stored in script files (consider environment variables for production)
- Container runs with elevated privileges for SQL Server requirements
- Data directory permissions are managed automatically

## Quick Start Guide

1. **Start the Database Container**:
   ```bash
   ./manage.sh start
   ```

2. **Verify Container Health**:
   ```bash
   ./manage.sh health
   ```

3. **Initialize the Database**:
   ```bash
   ./setup.sh
   ```

4. **Load Sample Data**:
   ```bash
   ./sql-exec.sh Insert.sql
   ```

5. **Validate Setup** (optional):
   ```bash
   python3 validate.py
   ```

## Troubleshooting

### Common Issues

1. **Container Won't Start**: Check Podman installation and user permissions
2. **Health Check Fails**: Verify SQL Server is fully initialized (may take 30-60 seconds)
3. **Permission Errors**: Ensure data directory has correct ownership
4. **Connection Issues**: Verify container is running and port 1433 is accessible

### Log Locations

- Container logs: `podman logs mssql-ultra3`
- SQL Server logs: Inside container at `/var/opt/mssql/log/`
- Application logs: Can be enabled by uncommenting log redirection in manage.sh

## Development Notes

- Scripts are designed for Debian-based systems
- Podman is used instead of Docker for better security and rootless operation
- Container name `mssql-ultra3` can be customized by modifying the scripts
- Database schema supports multi-site manufacturing environments
- Approval workflow supports multiple approval states and approver assignments

## File Structure Summary

```
Database/
├── manage.sh        # Container lifecycle management
├── setup.sh         # Database initialization
├── sql-exec.sh      # Generic SQL script executor
├── create.sql       # Complete database schema
├── Insert.sql       # Sample data for testing
├── validate.py      # Connectivity validation
└── README.md        # This documentation
```
