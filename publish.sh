#!/usr/bin/env bash

source ./CONFIG.inc

VERSIONFILE=$PACKAGE.version
REMOTE_DIR="${TARGET_CMS_PATH}ModuleManager/WatchDog/"
REMOTE_CONTENT_DIR="${TARGET_CMS_PATH}ModuleManager/WatchDog/"

scp -i $SSH_ID "./GameData/$TARGETDIR/$VERSIONFILE" $SITE:$TARGET_CONTENT_PATH
ssh ${SITE} "mkdir -p ${REMOTE_DIR}"
scp -i $SSH_ID "./GameData/$TARGETDIR/README.md" $SITE:${REMOTE_DIR}index.md
scp -i $SSH_ID "./KNOWN_ISSUES.md" $SITE:${REMOTE_DIR}
#scp -i $SSH_ID "./PR_material/banner.jpg" $SITE:/${TARGET_CONTENT_PATH}PR_material/banner.jpg
