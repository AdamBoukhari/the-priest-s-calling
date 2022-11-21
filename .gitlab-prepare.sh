#!/bin/bash
# Stops on error
set -e

# Determine the version of Unity to use to build the project. Keep it in an environment for the next stages. 
echo UNITY_VERSION=$(cat $UNITY_DIR/ProjectSettings/ProjectVersion.txt | grep "m_EditorVersion:.*" | awk '{print $2}') | tee prepare.env