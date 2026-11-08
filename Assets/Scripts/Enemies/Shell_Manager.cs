using Unity.Cinemachine;
using UnityEngine;

namespace Enemies
{
    public class Shell_Manager : Enemies_Manager
    {
        [Header("Shell")] private bool CanShell = true;
        private bool HadSpawned;

        protected override void Awake()
        {
            base.Awake();
            CanMove = false;
        }

        protected override void Update()
        {
            base.Update();
            ShellRun();
            HandleFlip();
        }

        public override void EnemiesDie()
        {
            if (CanShell)
            {
                CanMove = true;
                CanShell = false;
                EnemiesAnimator.SetTrigger("IsHit");
            }
            else base.EnemiesDie();
        }

        private void HandleFlip()
        {
            if (!IsGroundDetected || !IsWallDetected) return;
            EnemiesRigidBody.linearVelocity = Vector2.zero;
            FlipSprite();
        }

        private void ShellRun()
        {
            if (!IsGroundDetected || !CanMove || IsWallDetected) return;

            EnemiesRigidBody.linearVelocityX = MoveSpeed * transform.localScale.x * -1f;
        }

        protected override void HandleAnimation()
        {
            // base.HandleAnimation();
            if (IsWallDetected && !CanShell && IsGroundDetected) EnemiesAnimator.SetTrigger("IsHit");
        }

        public void ShellFlip(float direction)
        {
            transform.localScale = new Vector2(direction, transform.localScale.y);
        }
    }
}