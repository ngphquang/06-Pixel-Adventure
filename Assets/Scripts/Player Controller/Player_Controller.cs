using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour

{
    [Header("Movement")] [SerializeField] private float PlayerSpeed = 1f;
    [SerializeField] private float PlayerJumpForce = 1f;
    [SerializeField] private float WallSlideFriction = 0.5f;
    private bool HasJumped;
    private bool CanDoubleJump;
    private CapsuleCollider2D PlayerCollider;
    private Player_Controller PlayerInstance;
    private Rigidbody2D PlayerRigidBody;
    private PlayerInput PlayerInput;
    private Animator PlayerAnimator;
    private Vector2 MoveInput;
    private bool IsDead;
    private bool CanBeControl;
    private float DefaultGravityScale = 1f;

    [Header("Buffer & Coyote Jump")] [SerializeField]
    private float BufferJumpWindow = 0.25f;
    private float BufferJumpActivatedMoment = -1f;
    [SerializeField] private float CoyoteJumpWindow = 0.5f;
    private float CoyoteJumpActivatedMoment = -1f;

    [Header("Wall Jump Interactions")] [SerializeField]
    private Vector2 WallJumpForce;

    [Header("Knock Back")] [SerializeField]
    private float KnockBackDuration = 1f;
    [SerializeField] private Vector2 KnockBackForce;
    private bool IsKnockedBack;

    [Header("Collision")] [SerializeField] private float GroundCheckDistance;
    [SerializeField] private float WallCheckDistance;
    [SerializeField] private LayerMask GroundLayer;
    [Space] [SerializeField] private Transform EnemiesCheckPosition;
    [SerializeField] private float EnemiesCheckRadius;
    [SerializeField] private LayerMask EnemiesLayer;

    [Header("VFX")] [SerializeField] private GameObject DeadVFX;

    private void Awake()
    {
        CheckSingleton();
        PlayerAnimator = GetComponentInChildren<Animator>();
        PlayerCollider = GetComponent<CapsuleCollider2D>();
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        PlayerInput = GetComponent<PlayerInput>();
        PlayerInput.actions.Enable();
    }

    void CheckSingleton()
    {
        if (PlayerInstance == null) PlayerInstance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        IsDead = false;
        DefaultGravityScale = PlayerRigidBody.gravityScale;
        CanControlPLayer(false);
    }

    private void FixedUpdate()
    {
        if (IsKnockedBack || IsDead || !CanBeControl) { return; }

        IsRunning();
        IsGroundDetected();
        IsWallDetected();
        EnemiesDetectionManager();
        PlayerRun();
        PlayerWallSlide();
        FlipSprite();
        AnimationManager();
        LandingManager();
        AirborneManager();
    }

    public void CanControlPLayer(bool controllable)
    {
        if (controllable)
        {
            PlayerRigidBody.gravityScale = DefaultGravityScale;
            PlayerCollider.enabled = true;
            CanBeControl = true;
        }
        else
        {
            PlayerRigidBody.gravityScale = 0f;
            PlayerCollider.enabled = false;
            CanBeControl = false;
        }
    }

    private bool IsRunning()
    {
        return Mathf.Abs(MoveInput.x) > Mathf.Epsilon;
    }

    private bool IsGroundDetected()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, GroundCheckDistance, GroundLayer);
    }

    private bool IsWallDetected()
    {
        return Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(transform.localScale.x),
            WallCheckDistance, GroundLayer);
    }

    private void LandingManager()
    {
        if (!IsGroundDetected()) { return; }

        CanDoubleJump = true;
        AttemptBufferJump();
        if (PlayerRigidBody.linearVelocityY == 0) { HasJumped = false; }
    }

    private void AirborneManager()
    {
        if (IsGroundDetected() || IsWallDetected()) { return; }

        if (PlayerRigidBody.linearVelocityY < 0 && !HasJumped) { RequestCoyoteJump(); }
    }

    private void FlipSprite()
    {
        if (!IsRunning()) { return; }

        transform.localScale = new Vector2(Mathf.Sign(MoveInput.x), transform.localScale.y);
    }

    private void OnMove(InputValue inputValue)
    {
        MoveInput = inputValue.Get<Vector2>();
    }

    private void PlayerRun()
    {
        if (IsWallDetected()) { return; }

        PlayerRigidBody.linearVelocityX = MoveInput.x * PlayerSpeed;
    }

    public void PlayerKnockBack(Transform damagePosition)
    {
        if (IsKnockedBack) return;

        var FacingDirection = 1f;
        if (transform.position.x < damagePosition.position.x) FacingDirection = -1f;

        StartCoroutine(KnockBackCoroutine());
        PlayerRigidBody.linearVelocity = new Vector2(KnockBackForce.x * FacingDirection, KnockBackForce.y);
        StopCoroutine(KnockBackCoroutine());
    }

    private IEnumerator KnockBackCoroutine()
    {
        IsKnockedBack = true;
        PlayerAnimator.SetBool("IsKnockBack", IsKnockedBack);

        yield return new WaitForSeconds(KnockBackDuration);

        IsKnockedBack = false;
        PlayerAnimator.SetBool("IsKnockBack", IsKnockedBack);
    }

    public void PushPlayer(Vector2 force, float duration)
    {
        StartCoroutine(PushPlayerCoroutine(force, duration));
    }

    private IEnumerator PushPlayerCoroutine(Vector2 force, float duration)
    {
        CanBeControl = false;
        PlayerRigidBody.linearVelocity = Vector2.zero;
        PlayerRigidBody.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);
        CanBeControl = true;
    }

    public void PlayerDie()
    {
        IsDead = true;
        Destroy(gameObject);
        Instantiate(DeadVFX, transform.position, Quaternion.identity);
    }

    private void PlayerWallSlide()
    {
        var FastWallSlideFriction = MoveInput.y < 0 ? WallSlideFriction * 2f : WallSlideFriction;

        if (IsWallDetected() && PlayerRigidBody.linearVelocityY < 0)
        {
            PlayerRigidBody.linearVelocityY *= FastWallSlideFriction;
            // CanDoubleJump = false;
        }
    }

    private void OnJump()
    {
        if (IsKnockedBack) { return; }

        if (IsGroundDetected() || CanCoyoteJump())
        {
            PlayerJump();
            CancelCoyoteJump();
        }
        else if (!IsGroundDetected() && !IsWallDetected() && !CanDoubleJump) { RequestBufferJump(); }
        else if (IsWallDetected() && !IsGroundDetected()) { PlayerWallJump(); }
        else if (CanDoubleJump)
        {
            PlayerJump();
            CanDoubleJump = false;
        }

        HasJumped = true;
    }

    private void PlayerJump()
    {
        PlayerRigidBody.linearVelocityY = PlayerJumpForce;
    }

    private void PlayerWallJump()
    {
        CanDoubleJump = true;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        var jumpForce = new Vector2(WallJumpForce.x * Mathf.Sign(transform.localScale.x), WallJumpForce.y);
        PlayerRigidBody.AddRelativeForce(jumpForce);
    }

    private void RequestBufferJump()
    {
        BufferJumpActivatedMoment = Time.time;
    }

    private void AttemptBufferJump()
    {
        if (!(Time.time <= (BufferJumpActivatedMoment + BufferJumpWindow))) { return; }

        PlayerJump();
        BufferJumpActivatedMoment = Time.time - 1f;
    }

    private void RequestCoyoteJump()
    {
        CoyoteJumpActivatedMoment = Time.time;
    }

    private void CancelCoyoteJump()
    {
        CoyoteJumpActivatedMoment = Time.time - 1f;
    }

    private bool CanCoyoteJump()
    {
        return Time.time < (CoyoteJumpActivatedMoment + CoyoteJumpWindow);
    }

    private void AnimationManager()
    {
        PlayerAnimator.SetFloat("VelocityX", Mathf.Abs(PlayerRigidBody.linearVelocityX));
        PlayerAnimator.SetFloat("VelocityY", PlayerRigidBody.linearVelocityY);
        PlayerAnimator.SetBool("IsGrounded", IsGroundDetected());
        PlayerAnimator.SetBool("IsWallDetected", IsWallDetected());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    // ReSharper disable once Unity.PreferNonAllocApi
    private void EnemiesDetectionManager()
    {
        if (PlayerRigidBody.linearVelocityY >= 0 || IsKnockedBack) { return; }

        var colliders =
            Physics2D.OverlapCircleAll(EnemiesCheckPosition.position, EnemiesCheckRadius, EnemiesLayer);
        foreach (var enemy in colliders)
        {
            if (enemy.IsUnityNull()) return;

            var enemiesManager = enemy.GetComponent<Enemies_Manager>();
            enemiesManager.EnemiesDie();
            PlayerJump();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x, transform.position.y - GroundCheckDistance));
        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x + WallCheckDistance, transform.position.y));
        Gizmos.DrawWireSphere(EnemiesCheckPosition.position, EnemiesCheckRadius);
    }
}