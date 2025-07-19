using UnityEngine;

namespace UnityRepoExample
{
    /// <summary>
    /// プレイヤーの移動とアニメーションを制御するコンポーネント
    /// Unity.Analyzers と StyleCop.Analyzers のサンプルとして作成
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("移動設定")]
        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private float jumpForce = 10.0f;
        
        [Header("コンポーネント参照")]
        [SerializeField] private Rigidbody playerRigidbody;
        [SerializeField] private Animator playerAnimator;
        
        // プライベートフィールド
        private Vector3 movementDirection;
        private bool isGrounded;
        private bool isJumping;
        
        // Unity のコールバック（Microsoft.Unity.Analyzers が最適化を提案する可能性があります）
        private void Start()
        {
            // Rigidbody コンポーネントが設定されていない場合は取得
            if (playerRigidbody == null)
            {
                playerRigidbody = GetComponent<Rigidbody>();
            }
            
            // Animator コンポーネントが設定されていない場合は取得
            if (playerAnimator == null)
            {
                playerAnimator = GetComponent<Animator>();
            }
        }
        
        private void Update()
        {
            // 入力の取得
            GetMovementInput();
            
            // ジャンプ入力の処理
            HandleJumpInput();
            
            // アニメーションの更新
            UpdateAnimations();
        }
        
        private void FixedUpdate()
        {
            // 物理演算を使用した移動
            ApplyMovement();
        }
        
        /// <summary>
        /// プレイヤーの移動入力を取得します
        /// </summary>
        private void GetMovementInput()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            movementDirection = new Vector3(horizontal, 0, vertical).normalized;
        }
        
        /// <summary>
        /// ジャンプ入力を処理します
        /// </summary>
        private void HandleJumpInput()
        {
            if (Input.GetButtonDown("Jump") && isGrounded && !isJumping)
            {
                Jump();
            }
        }
        
        /// <summary>
        /// プレイヤーをジャンプさせます
        /// </summary>
        private void Jump()
        {
            if (playerRigidbody != null)
            {
                playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isJumping = true;
                isGrounded = false;
            }
        }
        
        /// <summary>
        /// 移動を適用します（FixedUpdate で呼び出される）
        /// </summary>
        private void ApplyMovement()
        {
            if (playerRigidbody != null && movementDirection.magnitude > 0.1f)
            {
                Vector3 movement = movementDirection * moveSpeed * Time.fixedDeltaTime;
                playerRigidbody.MovePosition(transform.position + movement);
            }
        }
        
        /// <summary>
        /// アニメーションパラメータを更新します
        /// </summary>
        private void UpdateAnimations()
        {
            if (playerAnimator != null)
            {
                playerAnimator.SetFloat("Speed", movementDirection.magnitude);
                playerAnimator.SetBool("IsGrounded", isGrounded);
                playerAnimator.SetBool("IsJumping", isJumping);
            }
        }
        
        /// <summary>
        /// 地面との衝突判定
        /// </summary>
        /// <param name="other">衝突したコライダー</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground"))
            {
                isGrounded = true;
                isJumping = false;
            }
        }
        
        /// <summary>
        /// 地面から離れる際の処理
        /// </summary>
        /// <param name="other">離れたコライダー</param>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }
        
        /// <summary>
        /// 移動速度を設定します（外部から呼び出し可能）
        /// </summary>
        /// <param name="newSpeed">新しい移動速度</param>
        public void SetMoveSpeed(float newSpeed)
        {
            if (newSpeed >= 0)
            {
                moveSpeed = newSpeed;
            }
            else
            {
                Debug.LogWarning("移動速度は0以上である必要があります。");
            }
        }
        
        /// <summary>
        /// 現在の移動速度を取得します
        /// </summary>
        /// <returns>現在の移動速度</returns>
        public float GetMoveSpeed()
        {
            return moveSpeed;
        }
    }
}