#!/bin/bash

# Check if SQL script parameter is provided
if [ $# -eq 0 ]; then
    echo "Usage: $0 <sql-script-file>"
    echo "Example: $0 create.sql"
    exit 1
fi

# Get the SQL script filename from the first parameter
SQL_SCRIPT_FILE="$1"

# Check if the SQL script file exists
if [ ! -f "$SQL_SCRIPT_FILE" ]; then
    echo "Error: SQL script file '$SQL_SCRIPT_FILE' not found."
    exit 1
fi

# Define container name, username, and password for SQL Server INSIDE the container
CONTAINER_NAME="mssql-ultra3"
USERNAME="sa"  # Replace with your SQL Server username inside the container
PASSWORD="P@ssword92"  # Replace with your SQL Server password inside the container

# Path to the SQL script file inside the container
SQL_SCRIPT="/tmp/$(basename "$SQL_SCRIPT_FILE")"

# Copy the SQL script into the container and then execute
echo "Copying SQL script '$SQL_SCRIPT_FILE' to the container..."
podman cp "$SQL_SCRIPT_FILE" "$CONTAINER_NAME:$SQL_SCRIPT"

# Construct the sqlcmd command to run inside the container
SQLCMD_COMMAND="/opt/mssql-tools18/bin/sqlcmd -S localhost -U '$USERNAME' -P '$PASSWORD' -N -C -i '$SQL_SCRIPT'"

echo "Executing SQL script inside the container..."
podman exec -it "$CONTAINER_NAME" bash -c "$SQLCMD_COMMAND"

# Get the exit code from the last executed command inside the container
EXIT_CODE=$?

# Check the exit code
if [ $EXIT_CODE -eq 0 ]; then
  echo "Sql executed successfully inside the container."
else
  echo "An error occurred during Sql execution inside the container. Check the output above for details (it's from within the container)."
fi
