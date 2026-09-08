using UnityEngine;

public class Mushroom_Manager : Enemies_Manager
{
    private BoxCollider2D EnemiesCollider;

    protected override void Awake()
    {
        base.Awake();
        EnemiesCollider = GetComponent<BoxCollider2D>();
    }

    protected override void Update()
    {
        if (IsDead) return;
        base.Update();
        HandleAnimation();
        CollisionCheck();
        MushroomRun();
        HandleFlip();
    }

    private void HandleFlip()
    {
        if (!IsGroundDetected) return;
        if (IsGroundInFrontDetected && !IsWallDetected) return;
        FlipSprite();
        IdleTimer = IdleDuration;
        EnemiesRigidBody.linearVelocity = Vector2.zero;
    }

    private void HandleAnimation()
    {
        EnemiesAnimator.SetFloat("VelocityX", EnemiesRigidBody.linearVelocityX);
    }

    private void MushroomRun()
    {
        if (IdleTimer > 0 || !IsGroundDetected) { return; }

        EnemiesRigidBody.linearVelocityX = MoveSpeed * FacingDirection;
    }

    public override void EnemiesDie()
    {
        base.EnemiesDie();
        EnemiesCollider.enabled = false;
    }
}