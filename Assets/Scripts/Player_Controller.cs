using System;
using System.Collections;
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
    private float KnockBackDelayTimer = 1f;
    [SerializeField] private Vector2 KnockBackForce;
    private bool IsKnockedBack;

    [Header("Collision")] [SerializeField] private float GroundCheckDistance;
    [SerializeField] private float WallCheckingDistance;
    [SerializeField] private LayerMask GroundLayer;

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
        IsGrounded();
        IsWallDetected();
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

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, GroundCheckDistance, GroundLayer);
    }

    private bool IsWallDetected()
    {
        return Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(transform.localScale.x),
            WallCheckingDistance, GroundLayer);
    }

    private void LandingManager()
    {
        if (!IsGrounded()) { return; }

        CanDoubleJump = true;
        AttemptBufferJump();
        if (PlayerRigidBody.linearVelocityY == 0) { HasJumped = false; }
    }

    private void AirborneManager()
    {
        if (IsGrounded() || IsWallDetected()) { return; }

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

    public void PlayerKnockBack()
    {
        if (IsKnockedBack) { return; }

        StartCoroutine(KnockBackCoroutine());
        PlayerAnimator.SetTrigger("IsKnockBack");
        PlayerRigidBody.linearVelocity =
            new Vector2(KnockBackForce.x * Mathf.Sign(transform.localScale.x) * -1, KnockBackForce.y);
        StopCoroutine(KnockBackCoroutine());
    }

    private IEnumerator KnockBackCoroutine()
    {
        IsKnockedBack = true;
        yield return new WaitForSeconds(KnockBackDelayTimer);
        IsKnockedBack = false;
    }

    public void PlayerDie()
    {
        Destroy(gameObject);
        IsDead = true;
        GameObject deadVFX = Instantiate(this.DeadVFX, transform.position, Quaternion.identity);
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

        if (IsGrounded() || CanCoyoteJump())
        {
            PlayerJump();
            CancelCoyoteJump();
        }
        else if (!IsGrounded() && !IsWallDetected() && !CanDoubleJump) { RequestBufferJump(); }
        else if (IsWallDetected() && !IsGrounded()) { PlayerWallJump(); }
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
        PlayerAnimator.SetBool("IsGrounded", IsGrounded());
        PlayerAnimator.SetBool("IsWallDetected", IsWallDetected());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x, transform.position.y - GroundCheckDistance));
        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x + WallCheckingDistance, transform.position.y));
    }
}