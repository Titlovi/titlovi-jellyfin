#!/bin/bash

# Configuration
SOLUTION_NAME="Titlovi.sln"
PLUGIN_NAME="Titlovi.Plugin"
SOURCE_DIR="src/$PLUGIN_NAME"
JELLYFIN_PLUGINS_DIR="/var/lib/jellyfin/plugins"
TARGET_DIR="$JELLYFIN_PLUGINS_DIR/JellyfinTitlovi"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo -e "${YELLOW}Building and deploying $PLUGIN_NAME plugin...${NC}"

# Check if source directory exists
if [ ! -d "$SOURCE_DIR" ]; then
    echo -e "${RED}Error: Source directory '$SOURCE_DIR' not found!${NC}"
    exit 1
fi

# Check if Jellyfin plugins directory exists
if ! sudo test -d "$JELLYFIN_PLUGINS_DIR"; then
    echo -e "${RED}Error: Jellyfin plugins directory '$JELLYFIN_PLUGINS_DIR' not found!${NC}"
    echo "Make sure Jellyfin is installed and the plugins directory exists."
    exit 1
fi

# Stop Jellyfin service
echo "Stopping Jellyfin service..."
sudo systemctl stop jellyfin

# Create target directory if it doesn't exist
if ! sudo test -d "$TARGET_DIR"; then
    echo "Creating target directory: $TARGET_DIR"
    sudo mkdir -p "$TARGET_DIR"
fi

# Build the plugin
echo "Building $PLUGIN_NAME project..."
dotnet publish "$SOLUTION_NAME"

# Copy only DLL files
echo "Copying DLL files to $TARGET_DIR..."
sudo find "$SOURCE_DIR/bin/Release/net9.0/publish/" -name "*.dll" -exec cp {} "$TARGET_DIR/" \;

# Check if any DLLs were found and copied
DLL_COUNT=$(find "$SOURCE_DIR" -name "*.dll" | wc -l)
if [ $DLL_COUNT -eq 0 ]; then
    echo -e "${YELLOW}Warning: No DLL files found in $SOURCE_DIR${NC}"
    echo "Make sure you've built the plugin first."
fi

# Set proper ownership (jellyfin user)
echo "Setting proper ownership..."
sudo chown -R jellyfin:jellyfin "$TARGET_DIR"

# Set proper permissions
sudo chmod -R 755 "$TARGET_DIR"

# Start Jellyfin service
echo "Starting Jellyfin service..."
sudo systemctl start jellyfin

echo -e "${GREEN}Plugin '$PLUGIN_NAME' successfully deployed to $TARGET_DIR${NC}"
