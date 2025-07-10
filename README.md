# sandbox-2025

## Claude GitHub Actions の権限設定

このリポジトリでは、`@claude` メンションによるGitHub Actionsの実行を特定のユーザーのみに制限しています。

### 設定方法

1. **リポジトリオーナー**は自動的に実行権限を持ちます

2. **その他のユーザーに権限を付与する場合**：
   - リポジトリの Settings → Secrets and variables → Actions → Variables タブに移動
   - `CLAUDE_ALLOWED_USERS` という名前で新しい変数を作成
   - 値にはカンマ区切りでGitHubユーザー名を入力（例: `user1,user2,user3`）

### 動作の仕組み

`.github/workflows/claude.yml` では以下の条件をチェックしています：
- コメントを投稿したユーザーがリポジトリオーナーである
- または、`CLAUDE_ALLOWED_USERS` 変数に含まれるユーザーである

これにより、許可されていないユーザーが `@claude` でメンションしてもワークフローは実行されません。