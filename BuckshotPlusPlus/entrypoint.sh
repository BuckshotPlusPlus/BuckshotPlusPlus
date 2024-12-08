#!/bin/bash

CLONE_DIR="local_repo"

if [[ -z "$GITHUB_REPO" || -z "$GITHUB_TOKEN" ]]; then
  echo "Les variables GITHUB_REPO et GITHUB_TOKEN doivent être définies."
  exit 1
fi

git clone "https://${GITHUB_TOKEN}@github.com/${GITHUB_REPO}" $CLONE_DIR

if [ -n "$MAIN_BPP_PATH" ]; then
    echo "Using main.bpp at $MAIN_BPP_PATH"
else
    echo "MAIN_BPP_PATH not specified"
fi

exec ./BuckshotPlusPlus ./local_repo/$MAIN_BPP_PATH
