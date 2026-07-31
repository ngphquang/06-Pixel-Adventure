using System;
using UnityEngine;

public class Spiked_Ball_Manager : MonoBehaviour
{
    [SerializeField] private Rigidbody2D SpikedRigidBody;
    [SerializeField] private float PushForce;

    private void Start()
    {
        var PushVector = new Vector2(PushForce, 0);
        SpikedRigidBody.AddForce(PushVector, ForceMode2D.Impulse);
    }
}