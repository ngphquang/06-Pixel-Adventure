using System.Collections;
using UnityEngine;

public class Rino_Manager : Enemies_Manager
{
    [Header("Rino")] [SerializeField] private Vector2 WallHitForce;
    [SerializeField] private float WallHitTimer;
    [SerializeField] private float MaxSpeed = 20f;
    [SerializeField] private float SpeedUpScale = 1.5f;
    private float OriginalSpeed;

    protected override void Awake()
    {
        base.Awake();
        CanMove = false;
        OriginalSpeed = MoveSpeed;
    }

    protected override void Update()
    {
        if (IsDead) return;
        base.Update();
        RinoRun();
    }

    private void HandleFlip()
    {
        if (!IsGroundDetected) return;

        EnemiesRigidBody.linearVelocity = Vector2.zero;
        CanMove = false;
        FlipSprite();
    }

    protected override void CollisionCheck()
    {
        base.CollisionCheck();
        if (!IsGroundInFrontDetected) HandleFlip();
        else if (IsPlayerDetected && IsGroundDetected) CanMove = true;
        else if (IsWallDetected && IsGroundDetected) WallHit();
    }

    private void WallHit()
    {
        StopAllCoroutines();
        EnemiesAnimator.SetBool("IsWallHit", true);
        CanMove = false;
        MoveSpeed = OriginalSpeed;
        EnemiesRigidBody.AddForce(new Vector2(WallHitForce.x * FacingDirection * -1, WallHitForce.y));
        StartCoroutine(ResetWallHit());
    }

    private IEnumerator ResetWallHit()
    {
        yield return new WaitForSeconds(WallHitTimer);
        EnemiesAnimator.SetBool("IsWallHit", false);
        FlipSprite();
    }

    private void RinoRun()
    {
        if (!IsGroundDetected || !CanMove) return;
        MoveSpeed = MoveSpeed >= MaxSpeed ? MaxSpeed : MoveSpeed + SpeedUpScale * Time.deltaTime;
        EnemiesRigidBody.linearVelocityX = MoveSpeed * FacingDirection;
    }
}