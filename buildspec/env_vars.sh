#!/usr/bin/env bash

set -e

echo "AWS_DEFAULT_REGION ${AWS_DEFAULT_REGION}"
echo "AWS_REGION ${AWS_REGION}"
echo "CODEBUILD_BUILD_ARN ${CODEBUILD_BUILD_ARN}"
echo "CODEBUILD_BUILD_ID ${CODEBUILD_BUILD_ID}"
echo "CODEBUILD_BUILD_IMAGE ${CODEBUILD_BUILD_IMAGE}"
echo "CODEBUILD_BUILD_SUCCEEDING ${CODEBUILD_BUILD_SUCCEEDING}"
echo "CODEBUILD_INITIATOR ${CODEBUILD_INITIATOR}"
echo "CODEBUILD_KMS_KEY_ID ${CODEBUILD_KMS_KEY_ID}"
echo "CODEBUILD_LOG_PATH ${CODEBUILD_LOG_PATH}"
echo "CODEBUILD_RESOLVED_SOURCE_VERSION ${CODEBUILD_RESOLVED_SOURCE_VERSION}"
echo "CODEBUILD_SOURCE_REPO_URL ${CODEBUILD_SOURCE_REPO_URL}"
echo "CODEBUILD_SOURCE_VERSION ${CODEBUILD_SOURCE_VERSION}"
echo "CODEBUILD_SRC_DIR ${CODEBUILD_SRC_DIR}"
echo "CODEBUILD_START_TIME ${CODEBUILD_START_TIME}"
echo "HOME ${HOME}"
echo "GITSHA ${GITSHA}"
echo "REPO ${REPO}"
echo "GIT_BRANCH ${GIT_BRANCH}"
echo "GIT_AUTHOR_EMAIL ${GIT_AUTHOR_EMAIL}"
echo "GIT_AUTHOR_NAME ${GIT_AUTHOR_NAME}"
echo "GIT_COMMIT_MESSAGE ${GIT_COMMIT_MESSAGE}"
echo "JOB_ID ${JOB_ID}"
echo "LAMBDASHARP_TIER ${TIER}"
echo "LAMBDASHARP_CLIPROFILE ${CLI_PROFILE}"