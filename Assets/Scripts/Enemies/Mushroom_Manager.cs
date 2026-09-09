using UnityEngine;

public class Mushroom_Manager : Enemies_Manager
{
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
}