@echo off
REM Unity用静的解析とフォーマットチェック
REM このスクリプトはコミット前に自動実行され、コード品質を保証します

echo [Unity Static Analysis] フォーマットチェックを実行中...

REM フォーマッタで自動修正し、修正が出たらコミット中断
dotnet format . --verify-no-changes
IF %ERRORLEVEL% NEQ 0 (
  echo [ERROR] コードフォーマットの問題が見つかりました。
  echo 以下のコマンドでフォーマットを修正してください:
  echo   dotnet format .
  echo その後、再度コミットしてください。
  exit /b 1
)

echo [Unity Static Analysis] 静的解析を実行中...

REM 静的解析で警告=エラー扱い
dotnet build . -c Release -warnaserror
IF %ERRORLEVEL% NEQ 0 (
  echo [ERROR] 静的解析で問題が見つかりました。
  echo 警告やエラーを修正してから再度コミットしてください。
  exit /b 1
)

echo [Unity Static Analysis] すべてのチェックが完了しました。コミットを続行します。
exit /b 0