using System;
using NUnit.Framework;
using UnityEngine;

public class Endpoint_Manager : MonoBehaviour
{
    private Animator EndpointAnimator;
    private bool IsActive;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var Player = other.GetComponent<Player_Controller>();
        if (Player == null || IsActive) return;
        EndpointAnimator = GetComponent<Animator>();
        EndpointAnimator.SetTrigger("IsActive");
        IsActive = true;
        print("Win");
    }
}