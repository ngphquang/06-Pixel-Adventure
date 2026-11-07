using System.Collections;
using UnityEngine;

namespace Enemies
{
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
        [SerializeField] protected Transform GroundChecker;
        [SerializeField] protected LayerMask PlayerLayer;
        protected Transform Player;
        protected bool IsGroundInFrontDetected;
        protected bool IsGroundDetected;
        protected bool CanMove = true;
        protected bool IsWallDetected;
        protected bool FacingRight;

        [Header("Player Detection")] [SerializeField]
        protected float DetectionRange;

        protected bool IsPlayerDetected;

        [Header("Death")] [SerializeField] private float DeathImpactForce;
        [SerializeField] private float DeathTorque;
        [SerializeField] private float DeathGravity;
        [SerializeField] private float DestroyTime;
        private Collider2D[] EnemiesColliders;
        protected bool IsDead;

        protected virtual void Awake()
        {
            EnemiesAnimator = GetComponent<Animator>();
            EnemiesRigidBody = GetComponent<Rigidbody2D>();
            EnemiesColliders = GetComponentsInChildren<Collider2D>();
            IsDead = false;
        }

        private void Start()
        {
            InvokeRepeating(nameof(UpdatePlayerRef), 0, 1);
        }

        private void UpdatePlayerRef()
        {
            if (Player == null)
            {
                Player = Game_Manager.GameManagerInstance.Player.transform;
            }
        }

        protected virtual void Update()
        {
            if (IsDead) return;
            CollisionCheck();
            HandleAnimation();
            IdleTimer -= Time.deltaTime;
        }

        protected virtual void HandleAnimation()
        {
            EnemiesAnimator.SetFloat("VelocityX", EnemiesRigidBody.linearVelocityX);
        }

        protected virtual void CollisionCheck()
        {
            IsGroundInFrontDetected = Physics2D.Raycast(GroundChecker.position,
                Vector2.down, GroundCheckDistance, GroundLayer);
            IsGroundDetected = Physics2D.Raycast(transform.position,
                Vector2.down, GroundCheckDistance, GroundLayer);
            IsWallDetected = Physics2D.Raycast(GroundChecker.position,
                Vector2.left * Mathf.Sign(transform.localScale.x), WallCheckDistance, GroundLayer);
            IsPlayerDetected = Physics2D.Raycast(GroundChecker.position,
                Vector2.left * Mathf.Sign(transform.localScale.x), DetectionRange, PlayerLayer);
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
            EnemiesAnimator.SetTrigger("IsHit");
            EnemiesRigidBody.AddForce(new Vector2(0, DeathImpactForce));
            EnemiesRigidBody.freezeRotation = false;
            EnemiesRigidBody.AddTorque(DeathTorque);
            EnemiesRigidBody.gravityScale = DeathGravity;
            foreach (var collider2D in EnemiesColliders)
            {
                collider2D.enabled = false;
            }

            StartCoroutine(DestroyEnemies());
        }

        public bool ReturnDead()
        {
            return IsDead;
        }

        private IEnumerator DestroyEnemies()
        {
            yield return new WaitForSeconds(DestroyTime);
            Destroy(gameObject);
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.DrawLine(GroundChecker.position,
                new Vector2(GroundChecker.position.x, GroundChecker.position.y - GroundCheckDistance));
            Gizmos.DrawLine(transform.position,
                new Vector2(transform.position.x, GroundChecker.position.y - GroundCheckDistance));
            Gizmos.DrawLine(GroundChecker.position,
                new Vector2(GroundChecker.position.x + WallCheckDistance, GroundChecker.position.y));
            Gizmos.DrawLine(GroundChecker.position,
                new Vector2((GroundChecker.position.x - DetectionRange) * Mathf.Sign(transform.localScale.x),
                    GroundChecker.position.y));
        }
    }
}