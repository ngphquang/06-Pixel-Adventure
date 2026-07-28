using System;
using System.Collections;
using UnityEngine;

public class Saw_Manager : MonoBehaviour
{
    [SerializeField] private float SawMoveSpeed;
    [SerializeField] private float SawCoolDown = 0.5f;
    [SerializeField] private Transform[] Waypoints;
    private Vector2[] WaypointsPosition;
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
        transform.position = WaypointsPosition[0];
    }

    private void UpdateWaypointsInfo()
    {
        WaypointsPosition = new Vector2[Waypoints.Length];
        for (var i = 0; i < Waypoints.Length; i++) { WaypointsPosition[i] = Waypoints[i].transform.position; }
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
            Vector2.MoveTowards(transform.position, WaypointsPosition[WaypointIndex],
                SawMoveSpeed * Time.deltaTime);
    }

    private void UpdateWaypointIndex()
    {
        if (!(Vector2.Distance(transform.position, WaypointsPosition[WaypointIndex]) <= Mathf.Epsilon)) return;
        if (WaypointIndex == WaypointsPosition.Length - 1 || WaypointIndex == 0)
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