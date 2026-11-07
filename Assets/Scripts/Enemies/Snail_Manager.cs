using UnityEngine;

namespace Enemies
{
    public class Snail_Manager : Enemies_Manager
    {
        [Header("Snail")] [SerializeField] private GameObject Shell;
        [SerializeField] private Transform ShellSpawner;
        private bool HadSpawned;

        protected override void Update()
        {
            base.Update();
            MushroomRun();
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

        private void MushroomRun()
        {
            if (IdleTimer > 0 || !IsGroundDetected)
            {
                return;
            }

            EnemiesRigidBody.linearVelocityX = MoveSpeed * FacingDirection;
        }

        private void ShellSpawn()
        {
            if (HadSpawned) return;
            Instantiate(Shell, ShellSpawner.position, Quaternion.identity);
            HadSpawned = true;
        }
    }
}