using UnityEngine;
using UdonSharp;
using VRC.SDKBase;

namespace UnityRepoExample.VRChat
{
    /// <summary>
    /// VRChat/UdonSharp 用のインタラクション スクリプト
    /// UdonAnalyzer によって Udon 互換性がチェックされます
    /// </summary>
    public class VRChatInteraction : UdonSharpBehaviour
    {
        [Header("インタラクション設定")]
        [SerializeField] private string interactionText = "インタラクトしてください";
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private ParticleSystem effectParticles;
        
        [Header("アニメーション")]
        [SerializeField] private Animator targetAnimator;
        [SerializeField] private string triggerParameterName = "Triggered";
        
        // Udon では static は使用できません（UdonAnalyzer が警告する可能性があります）
        private bool isInteracted = false;
        private float lastInteractionTime;
        private VRCPlayerApi localPlayer;
        
        // マテリアルプロパティブロック（パフォーマンス最適化）
        private MaterialPropertyBlock materialPropertyBlock;
        private Renderer targetRenderer;
        
        /// <summary>
        /// Udon の Start イベント
        /// </summary>
        void Start()
        {
            // ローカルプレイヤーの取得
            localPlayer = Networking.LocalPlayer;
            
            // MaterialPropertyBlock の初期化
            materialPropertyBlock = new MaterialPropertyBlock();
            targetRenderer = GetComponent<Renderer>();
            
            // 必要なコンポーネントの検証
            ValidateComponents();
            
            // 初期化
            ResetInteraction();
        }
        
        /// <summary>
        /// プレイヤーがインタラクトした際に呼び出されます
        /// VRChat の Interact イベント
        /// </summary>
        public override void Interact()
        {
            // インタラクションのクールダウンチェック
            if (Time.time - lastInteractionTime < 1.0f)
            {
                return;
            }
            
            // インタラクション実行
            PerformInteraction();
            
            // 最後のインタラクション時間を記録
            lastInteractionTime = Time.time;
        }
        
        /// <summary>
        /// インタラクションを実行します
        /// </summary>
        private void PerformInteraction()
        {
            // 状態を切り替え
            isInteracted = !isInteracted;
            
            // オーディオを再生
            PlayInteractionSound();
            
            // パーティクルエフェクトを再生
            PlayParticleEffect();
            
            // アニメーションをトリガー
            TriggerAnimation();
            
            // ネットワーク同期（Udon では SendCustomNetworkEvent を使用）
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(OnInteractionSynced));
            
            // ログ出力（デバッグ用）
            Debug.Log($"インタラクション実行: {gameObject.name} by {(localPlayer != null ? localPlayer.displayName : "Unknown")}");
        }
        
        /// <summary>
        /// ネットワーク同期されたインタラクション処理
        /// </summary>
        public void OnInteractionSynced()
        {
            // 全プレイヤーで実行される処理
            UpdateVisualState();
        }
        
        /// <summary>
        /// 視覚的な状態を更新します（MaterialPropertyBlock使用でパフォーマンス最適化）
        /// </summary>
        private void UpdateVisualState()
        {
            // MaterialPropertyBlock を使用してマテリアルの色変更
            if (targetRenderer != null && materialPropertyBlock != null)
            {
                materialPropertyBlock.SetColor("_Color", isInteracted ? Color.green : Color.white);
                targetRenderer.SetPropertyBlock(materialPropertyBlock);
            }
        }
        
        /// <summary>
        /// インタラクション音を再生します
        /// </summary>
        private void PlayInteractionSound()
        {
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
        
        /// <summary>
        /// パーティクルエフェクトを再生します
        /// </summary>
        private void PlayParticleEffect()
        {
            if (effectParticles != null)
            {
                effectParticles.Play();
            }
        }
        
        /// <summary>
        /// アニメーションをトリガーします
        /// </summary>
        private void TriggerAnimation()
        {
            if (targetAnimator != null && !string.IsNullOrEmpty(triggerParameterName))
            {
                targetAnimator.SetTrigger(triggerParameterName);
            }
        }
        
        /// <summary>
        /// コンポーネントの検証を行います
        /// </summary>
        private void ValidateComponents()
        {
            if (audioSource == null)
            {
                Debug.LogWarning($"{gameObject.name}: AudioSource が設定されていません。");
            }
            
            if (effectParticles == null)
            {
                Debug.LogWarning($"{gameObject.name}: ParticleSystem が設定されていません。");
            }
            
            if (targetAnimator == null)
            {
                Debug.LogWarning($"{gameObject.name}: Animator が設定されていません。");
            }
        }
        
        /// <summary>
        /// インタラクション状態をリセットします
        /// </summary>
        private void ResetInteraction()
        {
            isInteracted = false;
            lastInteractionTime = 0f;
            UpdateVisualState();
        }
        
        /// <summary>
        /// プレイヤーがトリガー範囲に入った際の処理
        /// </summary>
        /// <param name="other">入ったプレイヤー</param>
        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            if (player.isLocal)
            {
                // ローカルプレイヤーが範囲に入った場合の処理
                ShowInteractionPrompt(true);
            }
        }
        
        /// <summary>
        /// プレイヤーがトリガー範囲から出た際の処理
        /// </summary>
        /// <param name="other">出たプレイヤー</param>
        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            if (player.isLocal)
            {
                // ローカルプレイヤーが範囲から出た場合の処理
                ShowInteractionPrompt(false);
            }
        }
        
        /// <summary>
        /// インタラクションプロンプトの表示/非表示
        /// </summary>
        /// <param name="show">表示するかどうか</param>
        private void ShowInteractionPrompt(bool show)
        {
            // UI プロンプトの表示制御（実装は省略）
            Debug.Log($"インタラクションプロンプト: {(show ? "表示" : "非表示")}");
        }
    }
}