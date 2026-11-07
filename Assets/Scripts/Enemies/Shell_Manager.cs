using UnityEngine;

namespace Enemies
{
    public class Shell_Manager : Enemies_Manager
    {
        [SerializeField] private bool CanShell;
        private bool HadSpawned;

        protected override void Update()
        {
            base.Update();
            ShellRun();
            HandleFlip();
            if (IsDead) ShellSpawn();
        }

        private void HandleFlip()
        {
            if (!IsGroundDetected) return;
            if (IsGroundInFrontDetected && !IsWallDetected) return;
            EnemiesRigidBody.linearVelocity = Vector2.zero;
            FlipSprite();
        }

        private void ShellRun()
        {
            if (!IsGroundDetected || !CanShell)
            {
                return;
            }

        }

        protected override void HandleAnimation()
        {
            // base.HandleAnimation();
            if (IsWallDetected) EnemiesAnimator.SetTrigger("IsHit");
        }

        private void ShellSpawn()
        {
            if (HadSpawned || !Shell) return;
            HadSpawned = true;
        }
    }
}