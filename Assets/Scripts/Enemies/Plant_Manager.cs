using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plant_Manager : Enemies_Manager
{
    [Header("Bullet")] [SerializeField] private Transform BulletsShooter;
    [SerializeField] private Bullet_Manager[] BulletsArray;
    [SerializeField] private float BulletShootRate;
    private float Timer = 0f;

    protected override void Awake()
    {
        base.Awake();
        UpdateBullets();
    }

    protected override void Update()
    {
        base.Update();
        Timer += Time.deltaTime;
    }

    private void UpdateBullets()
    {
        var BulletsList = new List<Bullet_Manager>(GetComponentsInChildren<Bullet_Manager>());
        if (BulletsArray.Length == BulletsList.Count) return;

        BulletsArray = new Bullet_Manager[BulletsList.Count];
        for (var i = 0; i < BulletsArray.Length; i++) { BulletsArray[i] = BulletsList[i]; }
    }

    protected override void CollisionCheck()
    {
        base.CollisionCheck();
        if (IsPlayerDetected) ShootBullet();
    }

    protected override void HandleAnimation()
    {
        base.HandleAnimation();
        EnemiesAnimator.SetBool("IsPlayerDetected", IsPlayerDetected);
    }

    private void ShootBullet()
    {
        if (BulletsArray.Length == 0 || !(Timer >= BulletShootRate)) return;

        var Bullets = Instantiate(BulletsArray[Random.Range(0, 3)], BulletsShooter.position,
            Quaternion.identity);
        Bullets.AddComponent<Bullet_Manager>();
        Bullets.BulletsFly();
        Bullets.DestroyBullets();
        Timer -= BulletShootRate;
    }
}