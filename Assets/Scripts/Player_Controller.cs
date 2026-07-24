using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour

{
    [Header("Movement")] [SerializeField] private float PlayerSpeed = 1f;
    [SerializeField] private float PlayerJumpForce = 1f;
    [SerializeField] private float WallSlideFriction = 0.5f;
    private bool JustJumped;
    private bool CanDoubleJump;
    private Rigidbody2D PlayerRigidBody;
    private PlayerInput PlayerInput;
    private Animator PlayerAnimator;
    private Vector2 MoveInput;

    [Header("Buffer & Coyote Jump")] [SerializeField]
    private float BufferJumpWindow = 0.25f;
    private float BufferJumpActivated = -1f;
    [SerializeField] private float CoyoteJumpWindow = 0.5f;
    private float CoyoteJumpActivated = -1f;

    [Header("Wall Jump Interactions")] [SerializeField]
    private Vector2 WallJumpForce;

    [Header("Knock Back")] [SerializeField]
    private float KnockBackDelayTimer = 1f;
    [SerializeField] private Vector2 KnockBackForce;
    private bool IsKnockedBack;

    [Header("Collision")] [SerializeField] private float GroundCheckDistance;
    [SerializeField] private float WallCheckingDistance;
    [SerializeField] private LayerMask GroundLayer;

    private void Start()
    {
        PlayerAnimator = GetComponentInChildren<Animator>();
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        PlayerInput = GetComponent<PlayerInput>();
        PlayerInput.actions.Enable();
    }

    private void FixedUpdate()
    {
        if (IsKnockedBack) { return; }

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

        AttemptBufferJump();
        if (PlayerRigidBody.linearVelocityY == 0)
        {
            CanDoubleJump = true;
            JustJumped = false;
        }
    }

    private void AirborneManager()
    {
        if (IsGrounded() || IsWallDetected()) { return; }

        else if (PlayerRigidBody.linearVelocityY < 0 && !JustJumped) { RequestCoyoteJump(); }
    }

    private void FlipSprite()
    {
        if (IsRunning()) { transform.localScale = new Vector2(Mathf.Sign(MoveInput.x), transform.localScale.y); }
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

    private void PlayerKnockBack()
    {
        if (IsKnockedBack) { return; }

        StartCoroutine(KnockBackCoroutine());
        PlayerAnimator.SetTrigger("KnockBack");
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

        var CanCoyoteJump = Time.time < (CoyoteJumpActivated + CoyoteJumpWindow);
        if (IsGrounded() || CanCoyoteJump) { PlayerJump(); }
        else if (!IsGrounded() && !IsWallDetected() && !CanDoubleJump) { RequestBufferJump(); }
        else if (IsWallDetected() && !IsGrounded()) { PlayerWallJump(); }
        else if (CanDoubleJump)
        {
            PlayerJump();
            CanDoubleJump = false;
        }

        CancelCoyoteJump();
        JustJumped = true;
    }

    private void PlayerJump()
    {
        PlayerRigidBody.linearVelocityY = PlayerJumpForce;
    }

    private void PlayerWallJump()
    {
        CanDoubleJump = true;
        PlayerRigidBody.linearVelocity =
            new Vector2(WallJumpForce.x * Mathf.Sign(transform.localScale.x) * -1, WallJumpForce.y);
    }

    private void RequestBufferJump()
    {
        BufferJumpActivated = Time.time;
    }

    private void AttemptBufferJump()
    {
        if (!(Time.time < (BufferJumpActivated + BufferJumpWindow))) { return; }

        BufferJumpActivated = Time.time - 1f;
        PlayerJump();
    }

    private void RequestCoyoteJump()
    {
        CoyoteJumpActivated = Time.time;
    }

    private void CancelCoyoteJump()
    {
        CoyoteJumpActivated = Time.time - 1f;
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