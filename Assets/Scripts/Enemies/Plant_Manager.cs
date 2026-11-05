using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        UpdateBullets();
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

    private void UpdateBullets()
    {
        BulletsArray = new Bullet_Manager[GetComponentsInChildren<Bullet_Manager>().Length];
        for (var i = 0; i < BulletsArray.Length; i++)
        {
            BulletsArray[i] = GetComponentsInChildren<Bullet_Manager>()[i];
        }
    }

    protected override void HandleAnimation()
    {
        // base.HandleAnimation();
    }

    private void ShootBullet()
    {
        var Bullets = Instantiate(BulletsArray[Random.Range(0, BulletsArray.Length)],
            BulletsShooter.position, Quaternion.identity);
        Bullets.AddComponent<Bullet_Manager>();
        Bullets.BulletsFly(ShootDirection);
        Bullets.DestroyBullets(BulletsLiveTime);
    }
}