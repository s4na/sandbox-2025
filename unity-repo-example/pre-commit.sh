#!/bin/bash
# Unity用静的解析とフォーマットチェック
# このスクリプトはコミット前に自動実行され、コード品質を保証します

set -e

echo "[Unity Static Analysis] フォーマットチェックを実行中..."

# フォーマッタで自動修正し、修正が出たらコミット中断
if ! dotnet format . --verify-no-changes; then
  echo "[ERROR] コードフォーマットの問題が見つかりました。"
  echo "以下のコマンドでフォーマットを修正してください:"
  echo "  dotnet format ."
  echo "その後、再度コミットしてください。"
  exit 1
fi

echo "[Unity Static Analysis] 静的解析を実行中..."

# 静的解析で警告=エラー扱い
if ! dotnet build . -c Release -warnaserror; then
  echo "[ERROR] 静的解析で問題が見つかりました。"
  echo "警告やエラーを修正してから再度コミットしてください。"
  exit 1
fi

echo "[Unity Static Analysis] すべてのチェックが完了しました。コミットを続行します。"
exit 0