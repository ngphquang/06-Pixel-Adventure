using UnityEngine;

namespace Enemies
{
    public class Snail_Manager : Enemies_Manager
    {
        [SerializeField] private Transform ShellSpawner;
        private bool HadSpawned;

        protected override void Update()
        {
            base.Update();
            HandleFlip();
            if (IsDead) ShellSpawn();
        }

        private void HandleFlip()
        {
            if (!IsGroundDetected) return;
            if (IsGroundInFrontDetected && !IsWallDetected) return;
            EnemiesRigidBody.linearVelocity = Vector2.zero;
            IdleTimer = IdleDuration;
            FlipSprite();
        }

        {

            EnemiesRigidBody.linearVelocityX = MoveSpeed * FacingDirection;
        }

        private void ShellSpawn()
        {
            if (HadSpawned) return;
            HadSpawned = true;
        }
    }
}