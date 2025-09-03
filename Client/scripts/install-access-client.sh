#!/bin/bash

# Check if the script is being run as sudo
if [[ $EUID -ne 0 ]]; then
   echo "This script must be run as sudo."
   exit 1
fi

# Check if the first parameter is provided
if [ -z "$1" ]; then
    echo "Usage: $0 <source>"
    exit 1
fi

SOURCE_DIR=$1
TARGET_DIR="/opt/access-client"

# Remove the existing SOURCE_DIR in the target location if it exists
if [ -d "$TARGET_DIR/$(basename $SOURCE_DIR)" ]; then
    echo "Removing existing directory: $TARGET_DIR/$(basename $SOURCE_DIR)"
    rm -rf "$TARGET_DIR/$(basename $SOURCE_DIR)"
fi

# Copy SOURCE_DIR to /opt/access-client
cp -r "$SOURCE_DIR" "$TARGET_DIR"

# Remove the existing symbolic link if it exists
if [ -L "$TARGET_DIR/client" ]; then
    rm "$TARGET_DIR/client"
fi

# Create the symbolic link
ln -s "$TARGET_DIR/$(basename $SOURCE_DIR)" "$TARGET_DIR/client"

# Confirm symbolic link creation
echo "Symbolic link created: $SOURCE_DIR"
