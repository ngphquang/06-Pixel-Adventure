using UnityEngine;

namespace Enemies
{
    public class Shell_Manager : Enemies_Manager
    {
        [Header("Shell")] [SerializeField] private GameObject Shell;
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

            EnemiesRigidBody.linearVelocityX = MoveSpeed * FacingDirection;
        }

        protected override void HandleAnimation()
        {
            // base.HandleAnimation();
            if (IsWallDetected) EnemiesAnimator.SetTrigger("IsHit");
        }

        private void ShellSpawn()
        {
            if (HadSpawned || !Shell) return;
            Instantiate(Shell, transform.position, Quaternion.identity);
            HadSpawned = true;
        }
    }
}