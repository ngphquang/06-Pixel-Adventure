using UnityEngine;

namespace Enemies
{
    public class Trunk_Manager : Enemies_Manager
    {
        [Header("Trunk")] [SerializeField] private Transform BulletsShooter;
        [SerializeField] private Bullet_Manager[] BulletsArray;
        [SerializeField] private float BulletShootRate;
        [SerializeField] private float BulletsLiveTime;
        private float LastTimeAttacked;
        private bool CanAttack;

        protected override void Update()
        {
            base.Update();
            CheckBool();
            TrunkRun();
            HandleFlip();
            Attack();
        }

        private void CheckBool()
        {
            CanMove = !IsPlayerDetected && IsGroundDetected && IsGroundInFrontDetected;
            CanAttack = Time.time >= LastTimeAttacked + BulletShootRate;
        }

        private void TrunkRun()
        {
            if (!IsGroundDetected || !CanMove || IdleTimer > 0)
            {
                return;
            }

            EnemiesRigidBody.linearVelocityX = MoveSpeed * FacingDirection;
        }

        private void HandleFlip()
        {
            if (!IsGroundDetected) return;
            if (IsGroundInFrontDetected && !IsWallDetected) return;

            EnemiesRigidBody.linearVelocity = Vector2.zero;
            CanMove = false;
            IdleTimer = IdleDuration;
            FlipSprite();
        }

        private void Attack()
        {
            if (!IsPlayerDetected || !CanAttack) return;
            LastTimeAttacked = Time.time;
            EnemiesAnimator.SetTrigger("IsPlayerDetected");
        }

        private void ShootBullet()
        {
            var Bullets = Instantiate(BulletsArray[Random.Range(0, BulletsArray.Length)],
                BulletsShooter.position, Quaternion.identity);
            Bullets.BulletFlip(transform.localScale.x);
            Bullets.BulletsFly(Mathf.Sign(transform.localScale.x));
            Bullets.DestroyBullets(BulletsLiveTime);
        }
    }
}