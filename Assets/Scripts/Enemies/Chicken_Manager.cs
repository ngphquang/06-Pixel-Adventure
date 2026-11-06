using UnityEngine;

public class Chicken_Manager : Enemies_Manager
{
    [Header("Chicken")] [SerializeField] private float AggressiveDuration;
    [SerializeField] private float FollowFlipDelay;
    private float AggressiveTimer;
    private bool CanFlip = true;

    protected override void Awake()
    {
        base.Awake();
        CanMove = false;
    }

    protected override void Update()
    {
        base.Update();
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
        if (IsPlayerDetected)
        {
            CanMove = true;
            AggressiveTimer = AggressiveDuration;
        }

        if (AggressiveTimer < 0) { CanMove = false; }
    }

    private void ChickenRun()
    {
        if (!IsGroundDetected || !CanMove) { return; }

        EnemiesRigidBody.linearVelocityX = MoveSpeed * FacingDirection;
    }
}