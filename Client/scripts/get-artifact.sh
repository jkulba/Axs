#!/bin/bash

# Check if config file exists
CONFIG_FILE=".cfg"
if [ ! -f "$CONFIG_FILE" ]; then
    echo "Error: Configuration file '$CONFIG_FILE' not found"
    exit 1
fi

# Source the config file
source "$CONFIG_FILE"

# Verify required config values
if [ -z "$REPO_NAME" ] || [ -z "$ARTIFACT_NAME" ]; then
    echo "Error: REPO_NAME and ARTIFACT_NAME must be set in $CONFIG_FILE"
    exit 1
fi

# Check if a run ID was provided
if [ $# -ne 1 ]; then
    echo "Usage: $0 <run_id>"
    echo "Example: $0 14585861320"
    exit 1
fi

# Store the run ID from command line argument
RUN_ID=$1

# Create download directory if it doesn't exist
DOWNLOAD_DIR="$(pwd)/$RUN_ID"
mkdir -p $DOWNLOAD_DIR

# Download the artifact using GitHub CLI
echo "Downloading artifact for run ID: $RUN_ID"
echo "Repository: $REPO_NAME"
echo "Artifact: $ARTIFACT_NAME"

gh run download $RUN_ID \
    --repo $REPO_NAME \
    --name $ARTIFACT_NAME \
    --dir $DOWNLOAD_DIR

# Check if download was successful
if [ $? -eq 0 ]; then
    echo "Successfully downloaded artifact to: $DOWNLOAD_DIR"
else
    echo "Failed to download artifact"
    exit 1
fi