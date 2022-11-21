#!/bin/bash

# Stops the script on error.
set -e

# Build script header.
echo "Building for $BUILD_TARGET"

# Create build folder.
export BUILD_PATH=$UNITY_DIR/Builds/$BUILD_TARGET/
set -x
mkdir -p $BUILD_PATH
set +x

# Call Unity to build the project. This expects a license and a "BuildCommand.cs" class.
${UNITY_EXECUTABLE:-xvfb-run --auto-servernum --server-args='-screen 0 640x480x24' unity-editor} \
  -projectPath $UNITY_DIR \
  -quit \
  -batchmode \
  -nographics \
  -buildTarget $BUILD_TARGET \
  -customBuildTarget $BUILD_TARGET \
  -customBuildName $BUILD_NAME \
  -customBuildPath $BUILD_PATH \
  -executeMethod BuildCommand.PerformBuild \
  -logFile /dev/stdout

# Check if build succeeded and report that.
UNITY_EXIT_CODE=$?
if [ $UNITY_EXIT_CODE -eq 0 ]; then
  echo "Run succeeded, no failures occurred";
elif [ $UNITY_EXIT_CODE -eq 2 ]; then
  echo "Run succeeded, some tests failed";
elif [ $UNITY_EXIT_CODE -eq 3 ]; then
  echo "Run failure (other failure)";
else
  echo "Unexpected exit code $UNITY_EXIT_CODE";
fi

# Fail job if build folder is empty.
ls -la $BUILD_PATH
[ -n "$(ls -A $BUILD_PATH)" ]
