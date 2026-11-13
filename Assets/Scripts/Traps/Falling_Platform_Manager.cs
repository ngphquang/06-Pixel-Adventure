using System.Collections;
using Players;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Traps
{
    public class Falling_Platform_Manager : MonoBehaviour
    {
        [Header("Fly")] [SerializeField] private float PlatformSpeed = 1f;
        [SerializeField] private float TravelDistance;
        [SerializeField] private float RandomRange = 0.5f;
        [SerializeField] private bool RandomStart;
        [SerializeField] private float InstantiateDelay;
        private bool CanMove;
        private int WaypointsIndex;
        private Vector3[] Waypoints;
        private Animator PlatformAnimator;
        private Rigidbody2D PlatformRigidBody;
        private BoxCollider2D[] PlatformCollider;

        [Header("Impact")] [SerializeField] private float ImpactSpeed = 3f;
        [SerializeField] private float ImpactDuration = 0.1f;
        private float ImpactTimer;
        private bool Impacted;

        [Header("Fall")] [SerializeField] private float FallDelay;
        [SerializeField] private float GravityScale = 3.5f;
        [SerializeField] private float LinearDamping = 0.5f;

        private void Start()
        {
            PlatformRigidBody = GetComponent<Rigidbody2D>();
            PlatformAnimator = GetComponent<Animator>();
            PlatformCollider = GetComponents<BoxCollider2D>();

            SetupWaypoints();
            RandomActivatePlatform();
        }

        private void Update()
        {
            MovePlatform();
            ImpactPlatform();
        }

        private void RandomActivatePlatform()
        {
            var randomDelay = RandomStart ? Random.Range(0, RandomRange) : 0f;
            Invoke(nameof(ActivatePlatform), randomDelay);
        }

        private void ActivatePlatform()
        {
            CanMove = true;
        }

        // ReSharper disable once InvertIf
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Impacted) return;
            var player = other.GetComponent<Player_Controller>();
            if (player != null)
            {
                StartCoroutine(FallPlatform());
                ImpactTimer = ImpactDuration;
                Impacted = true;
            }
        }

        private IEnumerator FallPlatform()
        {
            InstantiatePlatform();
            yield return new WaitForSeconds(FallDelay);
            FallHandle();
            yield return new WaitForSeconds(FallDelay * 4);
            Destroy(gameObject);
        }

        private void FallHandle()
        {
            CanMove = false;
            PlatformAnimator.SetTrigger("IsInactive");

            PlatformRigidBody.bodyType = RigidbodyType2D.Dynamic;
            PlatformRigidBody.gravityScale = GravityScale;
            PlatformRigidBody.linearDamping = LinearDamping;

            foreach (var collider in PlatformCollider)
            {
                collider.enabled = false;
            }
        }

        private void SetupWaypoints()
        {
            Waypoints = new Vector3[2];
            var yOffset = TravelDistance / 2;
            Waypoints[0] = transform.position + new Vector3(0, yOffset, 0);
            Waypoints[1] = transform.position - new Vector3(0, yOffset, 0);
        }

        // ReSharper disable once InvertIf
        private void MovePlatform()
        {
            if (!CanMove) return;

            transform.position =
                Vector2.MoveTowards(transform.position, Waypoints[WaypointsIndex],
                    PlatformSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, Waypoints[WaypointsIndex]) <= Mathf.Epsilon)
            {
                WaypointsIndex++;
                if (WaypointsIndex == Waypoints.Length) WaypointsIndex = 0;
            }
        }

        private void ImpactPlatform()
        {
            if (ImpactTimer < 0) return;
            ImpactTimer -= Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position,
                transform.position + Vector3.down * 10, ImpactSpeed * Time.deltaTime);
        }

        private void InstantiatePlatform()
        {
            var platformPrefab = Game_Manager.GameManagerInstance.PlatformPrefab;
            Game_Manager.GameManagerInstance.CreateObject(platformPrefab, transform, InstantiateDelay);
        }
    }
}