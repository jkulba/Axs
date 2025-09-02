#!/usr/bin/pwsh
# Helper script used to start the development environment on a given port number.
# Usage: .\run.ps1 <port-number>

param(
    [int]$port = "5101"
)

# Start the development server
if (-not $port) {
    Write-Host "Usage: ./run.ps1 -port <port-number>"
    exit 1
}

# Set the ASP.NET Core environment
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:ASPNETCORE_URLS = "http://localhost:$port"

Write-Host "Starting development server on port $port in $env:ASPNETCORE_ENVIRONMENT mode"

# Start the development server in a new process
Start-Process -FilePath "dotnet" -ArgumentList "watch --project ./src/Client/ --urls http://localhost:$port" -NoNewWindow -Wait