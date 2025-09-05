#!/usr/bin/pwsh
# Define paths
$solutionPath = "./Client.sln"
$distDir = "./dist"
$project = "./src/Client/Client.csproj"

# Clean artifacts directory
if (Test-Path $distDir) {
    Remove-Item $distDir -Recurse -Force
}
New-Item -ItemType Directory -Path $distDir | Out-Null

if (Test-Path "./src/Client/bin" ) {
    Remove-Item "./src/Client/bin" -Recurse -Force
}
if (Test-Path "./src/Client/obj" ) {
    Remove-Item "./src/Client/obj" -Recurse -Force
}

# Clean solution
dotnet clean $solutionPath

# Publish Client
dotnet publish $project -c Development -o dist /p:RunAOTCompilation=true

# Write-Host "Volcano Data Web Application packaged"