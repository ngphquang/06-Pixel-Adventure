using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour

{
    [Header("Movement")] [SerializeField] private float PlayerSpeed = 1f;
    [SerializeField] private float PlayerJumpForce = 1f;
    private Rigidbody2D PlayerRigidBody;
    private PlayerInput PlayerInput;
    private Animator PlayerAnimator;
    private Vector2 MoveInput;

    [Header("Collision")] [SerializeField] private float GroundCheckDistance;
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
        IsRunning();
        IsGrounded();
        PlayerRun();
        FlipSprite();
    }

    bool IsRunning()
    {
        return Mathf.Abs(MoveInput.x) > Mathf.Epsilon;
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, GroundCheckDistance, GroundLayer);
    }

    void FlipSprite()
    {
        if (IsRunning())
        {
            transform.localScale = new Vector2(Mathf.Sign(MoveInput.x), transform.localScale.y);
        }
    }

    void OnMove(InputValue inputValue)
    {
        MoveInput = inputValue.Get<Vector2>();
    }

    void OnJump()
    {
        if (IsGrounded())
        {
            PlayerRigidBody.linearVelocityY += PlayerJumpForce;
        }
    }

    void PlayerRun()
    {
        PlayerRigidBody.linearVelocityX = MoveInput.x * PlayerSpeed;
        PlayerAnimator.SetFloat("VelocityX", Mathf.Abs(PlayerRigidBody.linearVelocityX));
    }
}