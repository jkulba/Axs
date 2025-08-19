#!/bin/bash

# Helper script used to start the development environment on a given port number.
# Usage: ./run.sh [port-number] [environment]
# Example: ./run.sh 7001 Development

set -euo pipefail  # Exit on error, undefined variables, and pipe failures

# Default values
DEFAULT_PORT=7001
DEFAULT_ENVIRONMENT="Development"
DEFAULT_PROJECT_PATH="./src/Api/"

# Function to display usage information
usage() {
    echo "Usage: $0 [port-number] [environment]"
    echo "  port-number: Port to run the server on (default: $DEFAULT_PORT)"
    echo "  environment: ASP.NET Core environment (default: $DEFAULT_ENVIRONMENT)"
    echo ""
    echo "Examples:"
    echo "  $0                    # Run on port $DEFAULT_PORT in $DEFAULT_ENVIRONMENT"
    echo "  $0 8080               # Run on port 8080 in $DEFAULT_ENVIRONMENT"
    echo "  $0 7001 Production    # Run on port 7001 in Production"
    echo ""
    echo "Environment options: Development, Staging, Production, Testing"
    exit 1
}

# Function to check if port is available
check_port() {
    local port=$1
    if command -v lsof >/dev/null 2>&1; then
        if lsof -Pi :$port -sTCP:LISTEN -t >/dev/null 2>&1; then
            echo "Error: Port $port is already in use"
            echo "To find what's using the port: lsof -i :$port"
            exit 1
        fi
    elif command -v netstat >/dev/null 2>&1; then
        if netstat -tuln | grep -q ":$port "; then
            echo "Error: Port $port is already in use"
            echo "To find what's using the port: netstat -tuln | grep :$port"
            exit 1
        fi
    fi
}

# Function to validate environment
validate_environment() {
    local env=$1
    case $env in
        Development|Staging|Production|Testing)
            return 0
            ;;
        *)
            echo "Error: Invalid environment '$env'"
            echo "Valid environments: Development, Staging, Production, Testing"
            exit 1
            ;;
    esac
}

# Function to check if dotnet is installed
check_dotnet() {
    if ! command -v dotnet >/dev/null 2>&1; then
        echo "Error: .NET CLI (dotnet) is not installed or not in PATH"
        echo "Please install .NET SDK from https://dotnet.microsoft.com/download"
        exit 1
    fi
}

# Function to cleanup on script exit
cleanup() {
    echo ""
    echo "Cleaning up..."
    # Kill any background processes started by this script
    if [[ -n ${DOTNET_PID:-} ]]; then
        echo "Stopping .NET process (PID: $DOTNET_PID)..."
        kill $DOTNET_PID 2>/dev/null || true
    fi
}

# Function to handle signals
signal_handler() {
    echo ""
    echo "Received interrupt signal. Shutting down gracefully..."
    cleanup
    exit 0
}

# Parse command line arguments
PORT=${1:-$DEFAULT_PORT}
ENVIRONMENT=${2:-$DEFAULT_ENVIRONMENT}

# Show help if requested
if [[ "${1:-}" == "-h" ]] || [[ "${1:-}" == "--help" ]]; then
    usage
fi

# Validate port number
if ! [[ "$PORT" =~ ^[0-9]+$ ]] || [ "$PORT" -lt 1 ] || [ "$PORT" -gt 65535 ]; then
    echo "Error: Invalid port number '$PORT'. Must be between 1 and 65535."
    exit 1
fi

# Validate environment
validate_environment "$ENVIRONMENT"

# Check if dotnet is available
check_dotnet

# Check if project exists
if [[ ! -d "$DEFAULT_PROJECT_PATH" ]]; then
    echo "Error: Project directory '$DEFAULT_PROJECT_PATH' not found"
    exit 1
fi

# Check if port is available
check_port "$PORT"

# Set up signal handlers for graceful shutdown
trap signal_handler SIGINT SIGTERM

# Set environment variables
export ASPNETCORE_ENVIRONMENT="$ENVIRONMENT"
export ASPNETCORE_URLS="http://localhost:$PORT"

# Additional environment variables for development
if [[ "$ENVIRONMENT" == "Development" ]]; then
    export DOTNET_ENVIRONMENT="Development"
    export ASPNETCORE_DETAILEDERRORS="true"
fi

echo "Starting development server..."
echo "  Port: $PORT"
echo "  Environment: $ENVIRONMENT"
echo "  URLs: $ASPNETCORE_URLS"
echo "  Project: $DEFAULT_PROJECT_PATH"
echo ""
echo "Press Ctrl+C to stop the server"
echo ""

# Start the development server
if [[ "$ENVIRONMENT" == "Development" ]]; then
    # Use dotnet watch for development with hot reload
    echo "Starting with hot reload (dotnet watch)..."
    dotnet run --project "$DEFAULT_PROJECT_PATH" --urls "http://localhost:$PORT" &
else
    # Use regular dotnet run for other environments
    echo "Starting without hot reload (dotnet run)..."
    dotnet run --project "$DEFAULT_PROJECT_PATH" --urls "http://localhost:$PORT" &
fi

# Store the process ID for cleanup
DOTNET_PID=$!

# Wait for the background process to finish or for signal
wait $DOTNET_PID

# If we reach here, the process exited normally
echo "Server stopped."
