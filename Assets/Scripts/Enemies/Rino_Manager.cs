using System.Collections;
using UnityEngine;

public class Rino_Manager : Enemies_Manager
{
    [Header("Rino")] [SerializeField] private float DetectionRange;
    [SerializeField] private Vector2 WallHitForce;
    [SerializeField] private float WallHitTimer;
    [SerializeField] private float MaxSpeed = 20f;
    [SerializeField] private float SpeedUpScale = 1.5f;
    private float OriginalSpeed;
    private bool PlayerDetected;
    private bool CanFlip;

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
        CollisionCheck();
        RinoRun();
        HandleFlip();
    }

    private void HandleFlip()
    {
        if (!IsGroundDetected || !CanFlip) return;

        EnemiesRigidBody.linearVelocity = Vector2.zero;
        CanMove = false;
        CanFlip = true;
        FlipSprite();
    }

    protected override void CollisionCheck()
    {
        EnemiesAnimator.SetFloat("VelocityX", EnemiesRigidBody.linearVelocityX);
        base.CollisionCheck();
        PlayerDetected = Physics2D.Raycast(transform.position,
            Vector2.left * Mathf.Sign(transform.localScale.x), DetectionRange, PlayerLayer);

        if (PlayerDetected) CanMove = true;
        else if (!IsGroundInFrontDetected && !IsWallDetected) NoGroundInFront();
        else if (IsWallDetected && IsGroundInFrontDetected) WallHit();
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
        if (!IsGroundDetected || !CanMove || !IsGroundInFrontDetected) return;
        MoveSpeed += SpeedUpScale * Time.deltaTime;
        if (MoveSpeed >= MaxSpeed) MoveSpeed = MaxSpeed;
        EnemiesRigidBody.linearVelocityX = MoveSpeed * FacingDirection;
    }

    private void NoGroundInFront()
    {
        if (!IsGroundDetected) return;
        EnemiesRigidBody.linearVelocity = Vector2.zero;
        CanMove = false;
        MoveSpeed = OriginalSpeed;
        FlipSprite();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position,
            new Vector2((transform.position.x - DetectionRange) * Mathf.Sign(transform.localScale.x),
                transform.position.y));
    }
}