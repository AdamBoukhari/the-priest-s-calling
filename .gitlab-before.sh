#!/bin/bash

# Stops the script on error.
set -e

# Create directories.
set -x                                      # Print commands.
mkdir -p /root/.cache/unity3d               # Create the unity3d cache dir.
mkdir -p /root/.local/share/unity3d/Unity/  # Create the unity3d config dir.
set +x                                      # Stop print commands.

# Various paths.
unity_license_destination=/root/.local/share/unity3d/Unity/Unity_lic.ulf

# Make scripts executable.
chmod +x .gitlab-before.sh .gitlab-prepare.sh .gitlab-build.sh 

# Write the Unity licence file from the $UNITY_LICENSE environment variable.
if [ -n "$UNITY_LICENSE" ]                                                        # Check if $UNITY_LICENSE environment variable exits.
then
    echo "Writing '\$UNITY_LICENSE' to license file ${unity_license_destination}" 
    echo "${UNITY_LICENSE}" | tr -d '\r' > ${unity_license_destination}           # Write license file. Strip Windows line endings in the process.
else
    echo "'\$UNITY_LICENSE' environment variable not found"
    exit 1                                                                        # Stop the build. License not found.
fi
