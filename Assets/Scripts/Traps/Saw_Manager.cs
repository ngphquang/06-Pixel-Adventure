using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traps
{
    public class Saw_Manager : MonoBehaviour
    {
        [SerializeField] private float SawMoveSpeed;
        [SerializeField] private float SawCoolDown = 0.5f;
        [SerializeField] private Transform[] WaypointsArray;
        private Vector2[] WaypointsPositionArray;
        private SpriteRenderer SawSpriteRenderer;
        private Animator SawAnimator;
        private int MoveDirection = 1;
        private int WaypointIndex = 1;
        private bool CanMove = true;

        private void Awake()
        {
            SawAnimator = GetComponent<Animator>();
            SawSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            UpdateWaypointsInfo();
            transform.position = WaypointsPositionArray[0];
        }

        private void UpdateWaypointsInfo()
        {
            CheckWaypointsArray();
            WaypointsPositionArray = new Vector2[WaypointsArray.Length];
            for (var i = 0; i < WaypointsArray.Length; i++)
            {
                WaypointsPositionArray[i] = WaypointsArray[i].transform.position;
            }
        }

        private void CheckWaypointsArray()
        {
            var waypointsList = new List<Saw_Waypoints>(GetComponentsInChildren<Saw_Waypoints>());
            if (waypointsList.Count != WaypointsArray.Length)
            {
                WaypointsArray = new Transform[waypointsList.Count];
                for (var i = 0; i < waypointsList.Count; i++)
                {
                    WaypointsArray[i] = waypointsList[i].transform;
                }
            }
        }

        private void Update()
        {
            if (!CanMove) return;
            SawAnimator.SetBool("IsActive", CanMove);
            MoveSaw();
            UpdateWaypointIndex();
        }

        private void MoveSaw()
        {
            transform.position =
                Vector2.MoveTowards(transform.position, WaypointsPositionArray[WaypointIndex],
                    SawMoveSpeed * Time.deltaTime);
        }

        private void UpdateWaypointIndex()
        {
            if (!(Vector2.Distance(transform.position, WaypointsPositionArray[WaypointIndex]) <= Mathf.Epsilon)) return;
            if (WaypointIndex == WaypointsPositionArray.Length - 1 || WaypointIndex == 0)
            {
                MoveDirection *= -1;
                StartCoroutine(CoolDown(SawCoolDown));
            }

            WaypointIndex += MoveDirection;
        }

        private IEnumerator CoolDown(float delay)
        {
            CanMove = false;
            yield return new WaitForSeconds(delay);
            CanMove = true;
            SawSpriteRenderer.flipX = !SawSpriteRenderer.flipX;
        }
    }
}