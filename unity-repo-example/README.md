# Unity リポジトリサンプル - 静的解析 & フォーマット設定

このリポジトリは、Unity/VRChat プロジェクト向けの静的解析ツールとコードフォーマッターの統合例を示しています。Claude Code との連携を前提とした、完全な開発環境セットアップのサンプルです。

## 🛠️ 使用している静的解析ツール

### C# 共通ツール

| ツール | 用途 | 設定ファイル |
|--------|------|--------------|
| `dotnet format` | コードフォーマット自動化 | `.editorconfig` |
| `StyleCop.Analyzers` | コード規約チェック | `.editorconfig` |
| `Microsoft.Unity.Analyzers` | Unity 特有のバグ検出 | `Packages/manifest.json` |

### VRChat/UdonSharp 専用ツール

| ツール | 用途 | 備考 |
|--------|------|------|
| `UdonAnalyzer` | UdonSharp 互換性チェック | パッケージマネージャーで導入 |
| `UdonRabbit.Analyzer` | Udon 限定の問題検出 | 既存コードのクリーンアップに有効 |

## 📁 プロジェクト構成

```
unity-repo-example/
├── .editorconfig                    # フォーマット設定
├── .github/workflows/
│   └── unity-ci.yml                # GitHub Actions CI 設定
├── Assets/
│   └── Scripts/
│       ├── PlayerController.cs     # Unity スタンダードなサンプル
│       └── VRChatInteraction.cs    # VRChat/UdonSharp サンプル
├── Packages/
│   └── manifest.json               # Unity パッケージ管理
├── ProjectSettings/                 # Unity プロジェクト設定
├── pre-commit.bat                   # Windows 用プリコミットフック
├── pre-commit.sh                    # Unix 用プリコミットフック
└── README.md                        # このファイル
```

## 🚀 セットアップ手順

### 1. 必要なソフトウェア

- Unity 2022.3 LTS 以降
- .NET 8.0 SDK
- Git
- 推奨IDE: JetBrains Rider または Visual Studio 2022

### 2. 依存関係のインストール

```bash
# Unity Package Manager で自動的に解決されます
# または手動で以下を実行:
dotnet restore
```

### 3. プリコミットフックの設定

#### Windows の場合:
```cmd
# .git/hooks/pre-commit として pre-commit.bat をコピー
copy pre-commit.bat .git\hooks\pre-commit
```

#### Unix/Linux/Mac の場合:
```bash
# .git/hooks/pre-commit として pre-commit.sh をコピー
cp pre-commit.sh .git/hooks/pre-commit
chmod +x .git/hooks/pre-commit
```

### 4. IDE 設定

#### JetBrains Rider
1. File → Settings → Editor → Code Style → C#
2. "Use .editorconfig files" を有効化
3. NuGet パッケージが自動的に読み込まれます

#### Visual Studio 2022
1. Tools → Options → Text Editor → C# → Code Style
2. "Follow project coding conventions" を有効化
3. Solution Explorer で StyleCop.Analyzers が参照されていることを確認

## 🔍 静的解析の実行

### 手動実行

```bash
# フォーマットチェック
dotnet format . --verify-no-changes

# フォーマット修正
dotnet format .

# 静的解析（警告をエラーとして扱う）
dotnet build . -c Release -warnaserror
```

### 自動実行

- **コミット時**: プリコミットフックが自動実行
- **プッシュ時**: GitHub Actions が CI で実行
- **IDE内**: リアルタイムで警告・エラー表示

## 🎯 Claude Code との連携ワークフロー

### 1. 基本的な流れ

1. Claude に C# コード生成を依頼
2. 生成されたコードをプロジェクトに追加
3. `dotnet format .` でフォーマットを自動修正
4. IDE で警告/エラーを確認・修正
5. プリコミットフックでチェック
6. コミット & プッシュ
7. CI で最終チェック

### 2. Claude への指示例

```markdown
@claude 以下の要件でUnityスクリプトを作成してください：

- プレイヤーの移動制御
- StyleCop.Analyzers の規約に準拠
- Microsoft.Unity.Analyzers の警告なし
- XML ドキュメントコメント付き
```

### 3. エラーが発生した場合

```bash
# フォーマットエラーの場合
dotnet format .

# 静的解析エラーの場合
# IDE で警告箇所を確認し、Claude に修正を依頼:
```

## 📊 CI/CD パイプライン

GitHub Actions による自動チェック：

1. **Code Quality Check**
   - フォーマット検証
   - 静的解析実行
   - 警告をエラーとして扱い

2. **Unity Build Test**
   - Unity でのコンパイル確認
   - ビルド成果物のアーティファクト化

3. **VRChat Compatibility Check**
   - VRChat/Udon 特有の互換性チェック
   - プルリクエスト時のみ実行

## ⚙️ 設定詳細

### .editorconfig の主要設定

```ini
# Unity 向けカスタマイズ
dotnet_diagnostic.IDE0051.severity = none  # Unity リフレクション対応
dotnet_diagnostic.SA1600.severity = none   # ドキュメント必須を緩和
dotnet_diagnostic.SA1309.severity = none   # アンダースコア許可
```

### StyleCop 除外設定

- SA1600: ドキュメント必須（Unity では過剰）
- SA1633: ファイルヘッダー必須（Unity では不要）
- SA1200: using 配置（Unity の自動生成に対応）

### Unity.Analyzers 対応

- 空の `Update()` メソッド検出
- コルーチンの戻り値ミス検出
- Unity 特有のパフォーマンス問題検出

## 🐛 トラブルシューティング

### よくある問題

#### 1. dotnet format が動作しない
```bash
# .NET SDK のインストール確認
dotnet --version

# プロジェクトファイルが見つからない場合
# Unity で一度プロジェクトを開いて .csproj を生成
```

#### 2. StyleCop.Analyzers が効かない
```bash
# Package Manager でパッケージ確認
# Visual Studio で NuGet パッケージマネージャーを確認
```

#### 3. GitHub Actions が失敗する
- Unity License の設定確認
- Secrets 設定の確認
- ワークフローファイルの構文確認

### サポートが必要な場合

1. Unity エディタでコンパイルエラーが無いか確認
2. `dotnet build` でエラーメッセージを確認
3. IDE のエラーリストを確認
4. GitHub Actions のログを確認

## 📝 追加情報

### VRChat 向け追加設定

VRChat SDK を使用する場合：

1. VRChat SDK のインポート
2. UdonSharp のインポート
3. 適切なレイヤー設定
4. VRChat 固有のスクリプト制限への対応

### Claude Code ベストプラクティス

1. **明確な要件指定**: 使用する Unity 機能、VRChat 対応の有無
2. **段階的実装**: 大きな機能は小さな単位に分割
3. **テスト駆動**: 動作確認可能な小さな機能から実装
4. **継続的改善**: 静的解析結果を元にコード品質向上

---

この設定により、Claude Code が生成するコードが自動的にUnity/VRChat の規約に準拠し、高品質なコードベースを維持できます。
