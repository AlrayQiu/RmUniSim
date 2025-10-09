#!/bin/bash

# 获取脚本所在目录的绝对路径
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

# 设置目标目录为脚本所在目录的上一级
SPARSE_PATH="com.unity.robotics.ros-tcp-connector"
REPO_URL="https://github.com/Unity-Technologies/ros-tcp-connector.git"
TAG="v0.7.1"
TARGET_DIR="$(realpath "$SCRIPT_DIR/..")/ros-tcp-connector_$TAG"

echo "📁 克隆到目录: $TARGET_DIR"
echo "🔖 指定版本标签: $TAG"

# 克隆仓库但不检出内容
git clone --filter=blob:none --no-checkout "$REPO_URL" "$TARGET_DIR"

# 进入目标目录
cd "$TARGET_DIR" || exit 1

# 获取所有 tag
git fetch --tags

# 检出指定 tag（注意：tag 是一个 commit 的引用）
git checkout "tags/$TAG"

# 启用 sparse checkout
git sparse-checkout init --cone
git sparse-checkout set "$SPARSE_PATH"

echo "✅ 已成功拉取文件夹：$SPARSE_PATH @ $TAG 到 $TARGET_DIR"
