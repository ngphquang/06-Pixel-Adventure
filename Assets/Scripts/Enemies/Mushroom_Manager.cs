using UnityEngine;

namespace Enemies
{
    public class Mushroom_Manager : Enemies_Manager
    {
        protected override void Update()
        {
            base.Update();
            MushroomRun();
            HandleFlip();
        }

        private void HandleFlip()
        {
            if (!IsGroundDetected) return;
            if (IsGroundInFrontDetected && !IsWallDetected) return;
            EnemiesRigidBody.linearVelocity = Vector2.zero;
            IdleTimer = IdleDuration;
            FlipSprite();
        }

        private void MushroomRun()
        {
            if (IdleTimer > 0 || !IsGroundDetected)
            {
                return;
            }

            EnemiesRigidBody.linearVelocityX = MoveSpeed * FacingDirection;
        }
    }
}