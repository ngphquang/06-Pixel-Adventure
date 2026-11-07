using UnityEngine;

namespace Enemies
{
    public class Shell_Manager : Enemies_Manager
    {
        [Header("Shell")] [SerializeField] private Shell_Manager Shell;
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

            EnemiesRigidBody.linearVelocityX = MoveSpeed * transform.localScale.x * -1f;
        }

        protected override void HandleAnimation()
        {
            // base.HandleAnimation();
            if (IsWallDetected) EnemiesAnimator.SetTrigger("IsHit");
        }

        private void ShellSpawn()
        {
            if (HadSpawned || !Shell) return;
            var shell = Instantiate(Shell, transform.position, Quaternion.identity);
            shell.ShellFlip(transform.localScale.x);
            HadSpawned = true;
        }

        public void ShellFlip(float direction)
        {
            transform.localScale = new Vector2(direction, transform.localScale.y);
        }
    }
}