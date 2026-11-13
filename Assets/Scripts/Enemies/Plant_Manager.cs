using UnityEngine;

namespace Enemies
{
    public class Plant_Manager : Enemies_Manager
    {
        [Header("Plant")] [SerializeField] private Transform BulletsShooter;
        [SerializeField] private Bullet_Manager[] BulletsArray;
        [SerializeField] private float BulletShootRate;
        [SerializeField] private float BulletsLiveTime;
        [SerializeField] private bool FacingLeft;
        private float ShootDirection;
        private float LastTimeAttacked;
        private bool CanAttack;

        protected override void Awake()
        {
            base.Awake();
            ShootDirection = FacingLeft ? -1f : 1f;
        }

        private void Start()
        {
            transform.localScale = new Vector2(ShootDirection, transform.localScale.y);
        }

        // ReSharper disable once InvertIf
        protected override void Update()
        {
            base.Update();
            CanAttack = Time.time >= LastTimeAttacked + BulletShootRate;
            if (IsPlayerDetected && CanAttack) Attack();
        }

        private void Attack()
        {
            LastTimeAttacked = Time.time;
            EnemiesAnimator.SetTrigger("IsPlayerDetected");
        }

        protected override void HandleAnimation()
        {
            // base.HandleAnimation();
        }

        private void ShootBullet()
        {
            var bullets = Instantiate(BulletsArray[Random.Range(0, BulletsArray.Length)],
                BulletsShooter.position, Quaternion.identity);
            bullets.BulletsFly(ShootDirection);
            bullets.DestroyBullets(BulletsLiveTime);
        }
    }
}