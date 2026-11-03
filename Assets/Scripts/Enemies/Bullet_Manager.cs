using System;
using UnityEngine;

public class Bullet_Manager : MonoBehaviour
{
    [SerializeField] private float BulletSpeed;
    [SerializeField] private float BulletsLiveTime;
    private Rigidbody2D BulletsRigidBody;

    private void Awake()
    {
        BulletsRigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player_Controller>();
        if (player == null) return;

        player.PlayerKnockBack(gameObject.transform);
    }

    public void BulletsFly()
    {
        BulletsRigidBody.linearVelocityX = BulletSpeed * Mathf.Sign(transform.localScale.x) * -1;
    }

    public void DestroyBullets()
    {
        Destroy(gameObject, BulletsLiveTime);
    }
}