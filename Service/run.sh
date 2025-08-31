#!/bin/bash

# Set default port if not provided
PORT=${PORT:-5001}

# Set the ASPNETCORE_URLS environment variable
export ASPNETCORE_URLS="http://localhost:$PORT"

# Set development environment
export ASPNETCORE_ENVIRONMENT="Development"

echo "Starting API on port $PORT..."
echo "URL: $ASPNETCORE_URLS"
echo "Environment: $ASPNETCORE_ENVIRONMENT"
echo ""

# Change to the API project directory and run it
dotnet run --project src/Api --urls http://localhost:$PORT
