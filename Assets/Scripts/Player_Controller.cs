using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour

{
    [Header("Movement")] [SerializeField] private float PlayerSpeed = 1f;
    [SerializeField] private float PlayerJumpForce = 1f;
    [SerializeField] private float WallSlideFriction = 0.5f;
    private bool CanDoubleJump;
    private Rigidbody2D PlayerRigidBody;
    private PlayerInput PlayerInput;
    private Animator PlayerAnimator;
    private Vector2 MoveInput;

    [Header("Wall Jump Interactions")] [SerializeField]
    private Vector2 WallJumpForce;

    [Header("Knock Back")] [SerializeField]
    private float KnockBackDelayTimer = 1f;
    [SerializeField] private Vector2 KnockBackForce;
    private bool IsKnockedBack;
    private bool CanBeKnockBack = true;

    [Header("Collision")] [SerializeField] private float GroundCheckDistance;
    [SerializeField] private float WallCheckingDistance;
    [SerializeField] private LayerMask GroundLayer;

    void Start()
    {
        PlayerAnimator = GetComponentInChildren<Animator>();
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        PlayerInput = GetComponent<PlayerInput>();
        PlayerInput.actions.Enable();
    }

    void FixedUpdate()
    {
        if (IsKnockedBack) { return; }

        IsRunning();
        IsGrounded();
        IsWallDetected();
        PlayerRun();
        PlayerWallSlide();
        FlipSprite();
        AnimationManager();
        AirborneManager();
        LandingManager();
    }

    void OnAttack()
    {
        PlayerKnockBack();
    }

    bool IsRunning()
    {
        return Mathf.Abs(MoveInput.x) > Mathf.Epsilon;
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, GroundCheckDistance, GroundLayer);
    }

    bool IsWallDetected()
    {
        return Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(transform.localScale.x),
            WallCheckingDistance, GroundLayer);
    }

    void LandingManager()
    {
        if (!IsGrounded()) { return; }

        CanDoubleJump = true;
    }

    void AirborneManager()
    {
        if (IsGrounded()) { return; }
    }

    void FlipSprite()
    {
        if (IsRunning()) { transform.localScale = new Vector2(Mathf.Sign(MoveInput.x), transform.localScale.y); }
    }

    void OnMove(InputValue inputValue)
    {
        MoveInput = inputValue.Get<Vector2>();
    }

    void PlayerRun()
    {
        if (IsWallDetected()) return;
        PlayerRigidBody.linearVelocityX = MoveInput.x * PlayerSpeed;
    }

    void PlayerKnockBack()
    {
        if (IsKnockedBack) return;
        StartCoroutine(KnockBackCoroutine());
        PlayerAnimator.SetTrigger("KnockBack");
        PlayerRigidBody.linearVelocity =
            new Vector2(KnockBackForce.x * Mathf.Sign(transform.localScale.x) * -1, KnockBackForce.y);
        StopCoroutine(KnockBackCoroutine());
    }

    IEnumerator KnockBackCoroutine()
    {
        CanBeKnockBack = false;
        IsKnockedBack = true;
        yield return new WaitForSeconds(KnockBackDelayTimer);
        CanBeKnockBack = true;
        IsKnockedBack = false;
    }

    void PlayerWallSlide()
    {
        float LocalWallSlideFriction = MoveInput.y < 0 ? WallSlideFriction * 2f : WallSlideFriction;

        if (IsWallDetected() && PlayerRigidBody.linearVelocityY < 0)
        {
            PlayerRigidBody.linearVelocityY *= LocalWallSlideFriction;
            // CanDoubleJump = false;
        }
    }

    void OnJump()
    {
        if (IsKnockedBack) { return; }
        else if (IsGrounded()) { PlayerRigidBody.linearVelocityY = PlayerJumpForce; }
        else if (IsWallDetected() && !IsGrounded()) { PlayerWallJump(); }
        else if (CanDoubleJump)
        {
            PlayerRigidBody.linearVelocityY = PlayerJumpForce;
            CanDoubleJump = false;
        }
    }

    private void PlayerWallJump()
    {
        CanDoubleJump = true;
        PlayerRigidBody.linearVelocity =
            new Vector2(WallJumpForce.x * Mathf.Sign(transform.localScale.x) * -1, WallJumpForce.y);
    }

    void AnimationManager()
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