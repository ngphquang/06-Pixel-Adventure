using UnityEngine;

public class Chicken_Manager : Enemies_Manager
{
    [Header("Chicken")] [SerializeField] private float AggressiveDuration;
    [SerializeField] private float DetectionRange;
    [SerializeField] private float FollowFlipDelay;
    private float AggressiveTimer;
    private bool PlayerDetected;
    private bool CanFlip = true;

    protected override void Awake()
    {
        base.Awake();
        CanMove = false;
    }

    protected override void Update()
    {
        if (IsDead) return;
        base.Update();
        HandleAnimation();
        CollisionCheck();
        ChickenRun();
        HandleFlip();
        FollowPlayer();
        AggressiveTimer -= Time.deltaTime;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void FollowPlayer()
    {
        if (!CanMove) return;
        var PlayerX = Player.transform.position.x;
        var PlayerIsBehind = PlayerX < transform.position.x && FacingRight ||
                             PlayerX > transform.position.x && !FacingRight;
        if (!PlayerIsBehind || !CanFlip) return;
        CanFlip = false;
        Invoke(nameof(FlipSprite), FollowFlipDelay);
    }

    protected override void FlipSprite()
    {
        base.FlipSprite();
        CanFlip = true;
    }

    private void HandleFlip()
    {
        if (!IsGroundDetected) return;
        if (IsGroundInFrontDetected && !IsWallDetected) return;

        EnemiesRigidBody.linearVelocity = Vector2.zero;
        CanMove = false;
        FlipSprite();
    }

    protected override void CollisionCheck()
    {
        base.CollisionCheck();
        PlayerDetected = Physics2D.Raycast(transform.position,
            Vector2.left * Mathf.Sign(transform.localScale.x), DetectionRange, PlayerLayer);

        if (PlayerDetected)
        {
            CanMove = true;
            AggressiveTimer = AggressiveDuration;
        }

        if (AggressiveTimer < 0) { CanMove = false; }
    }

    private void HandleAnimation()
    {
        EnemiesAnimator.SetFloat("VelocityX", EnemiesRigidBody.linearVelocityX);
    }

    private void ChickenRun()
    {
        if (!IsGroundDetected || !CanMove) { return; }

        EnemiesRigidBody.linearVelocityX = MoveSpeed * FacingDirection;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position,
            new Vector2((transform.position.x - DetectionRange) * Mathf.Sign(transform.localScale.x),
                transform.position.y));
    }
}