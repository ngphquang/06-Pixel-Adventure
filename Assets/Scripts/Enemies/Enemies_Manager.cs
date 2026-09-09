using System;
using System.Collections;
using UnityEngine;

public class Enemies_Manager : MonoBehaviour
{
    [Header("Movement")] [SerializeField] protected float MoveSpeed = 5f;
    [SerializeField] protected float IdleDuration = 0.5f;
    protected float IdleTimer;
    protected int FacingDirection = -1;
    protected Animator EnemiesAnimator;
    protected Rigidbody2D EnemiesRigidBody;

    [Header("Collision")] [SerializeField] protected float GroundCheckDistance = 1.1f;
    [SerializeField] protected float WallCheckDistance = 1.1f;
    [SerializeField] protected LayerMask GroundLayer;
    [SerializeField] protected Transform GroundCheck;
    [SerializeField] protected LayerMask PlayerLayer;
    protected Transform Player;
    protected bool IsGroundInFrontDetected;
    protected bool IsGroundDetected;
    protected bool CanMove = true;
    protected bool IsWallDetected;
    protected bool FacingRight;

    [Header("Death")] [SerializeField] private float DeathImpactForce;
    [SerializeField] private float DeathTorque;
    [SerializeField] private float DeathGravity;
    [SerializeField] private float DestroyTime;
    [SerializeField] private GameObject EnemiesDamageArea;
    private Collider2D EnemiesCollider;
    protected bool IsDead;

    protected virtual void Awake()
    {
        EnemiesAnimator = GetComponent<Animator>();
        EnemiesRigidBody = GetComponent<Rigidbody2D>();
        EnemiesCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdatePlayerRef), 0, 1);
    }

    private void UpdatePlayerRef()
    {
        if (Player == null) { Player = Game_Manager.GameManagerInstance.Player.transform; }
    }

    protected virtual void Update()
    {
        IdleTimer -= Time.deltaTime;
    }

    protected virtual void CollisionCheck()
    {
        IsGroundInFrontDetected = Physics2D.Raycast(GroundCheck.position,
            Vector2.down, GroundCheckDistance, GroundLayer);
        IsGroundDetected = Physics2D.Raycast(transform.position,
            Vector2.down, GroundCheckDistance, GroundLayer);
        IsWallDetected = Physics2D.Raycast(GroundCheck.position,
            Vector2.right * Mathf.Sign(transform.localScale.x), WallCheckDistance, GroundLayer);
    }

    protected virtual void FlipSprite()
    {
        FacingDirection *= -1;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        FacingRight = !FacingRight;
    }

    public virtual void EnemiesDie()
    {
        IsDead = true;
        Destroy(EnemiesDamageArea);
        EnemiesAnimator.SetTrigger("IsHit");
        EnemiesRigidBody.AddForce(new Vector2(0, DeathImpactForce));
        EnemiesRigidBody.freezeRotation = false;
        EnemiesRigidBody.AddTorque(DeathTorque);
        EnemiesRigidBody.gravityScale = DeathGravity;
        EnemiesCollider.enabled = false;
        StartCoroutine(DestroyEnemies());
    }

    private IEnumerator DestroyEnemies()
    {
        yield return new WaitForSeconds(DestroyTime);
        Destroy(gameObject);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(GroundCheck.position,
            new Vector2(GroundCheck.position.x, GroundCheck.position.y - GroundCheckDistance));
        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x, GroundCheck.position.y - GroundCheckDistance));
        Gizmos.DrawLine(GroundCheck.position,
            new Vector2(GroundCheck.position.x + WallCheckDistance, GroundCheck.position.y));
    }
}