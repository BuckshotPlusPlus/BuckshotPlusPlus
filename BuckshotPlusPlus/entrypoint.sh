#!/bin/bash

# Get repository URL from environment variable
REPO_URL="${GITHUB_REPO_URL}"
REPO_FILE="${BPP_MAIN_FILE:-main.bpp}"

# Check if ./web directory already exists
if [ -d "./web" ]; then
    echo "Directory ./web already exists. Removing it first..."
    rm -rf ./web
fi

# Clone the repository
echo "Cloning repository from $REPO_URL into ./web..."
if git clone "$REPO_URL" ./web; then
    echo "Repository cloned successfully!"
else
    echo "Failed to clone repository. Exiting..."
    exit 1
fi

ls ./web/

# Execute your program
exec ./BuckshotPlusPlus /app/web/$REPO_FILE

