using UnityEngine;

public class Enemies_Manager : MonoBehaviour
{
    [Header("Movement")] [SerializeField] protected float MoveSpeed = 5f;
    [SerializeField] protected float IdleDuration = 0.5f;
    protected float IdleTimer;
    protected int FacingDirection = -1;
    protected Animator EnemiesAnimator;
    protected Rigidbody2D EnemiesRigidBody;

    [Header("Collision")] [SerializeField] protected float GroundCheckDistance = 1.1f;
    [SerializeField] protected float WallCheckDistance = 1.1f;
    [SerializeField] protected LayerMask GroundLayer;
    [SerializeField] protected Transform GroundCheck;
    protected bool IsGroundDetected;
    protected bool IsGroundInFrontDetected;
    protected bool IsWallDetected;
    private bool FacingRight;

    protected virtual void Awake()
    {
        EnemiesAnimator = GetComponent<Animator>();
        EnemiesRigidBody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        IdleTimer -= Time.deltaTime;
    }

    protected void CollisionCheck()
    {
        IsGroundInFrontDetected = Physics2D.Raycast(GroundCheck.position,
            Vector2.down, GroundCheckDistance, GroundLayer);
        IsGroundDetected = Physics2D.Raycast(transform.position,
            Vector2.down, GroundCheckDistance, GroundLayer);
        IsWallDetected = Physics2D.Raycast(GroundCheck.position,
            Vector2.right * Mathf.Sign(transform.localScale.x), WallCheckDistance, GroundLayer);
    }

    protected void HandleFlip(float xValue)
    {
        if (xValue < 0 && FacingRight || xValue > 0 && !FacingRight) { FlipSprite(); }
    }

    protected void FlipSprite()
    {
        FacingDirection *= -1;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        FacingRight = !FacingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(GroundCheck.position,
            new Vector2(GroundCheck.position.x, GroundCheck.position.y - GroundCheckDistance));
        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x, GroundCheck.position.y - GroundCheckDistance));
        Gizmos.DrawLine(GroundCheck.position,
            new Vector2(GroundCheck.position.x + WallCheckDistance, GroundCheck.position.y));
    }
}