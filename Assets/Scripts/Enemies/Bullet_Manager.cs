using Players;
using UnityEngine;

namespace Enemies
{
    public class Bullet_Manager : MonoBehaviour
    {
        [SerializeField] private float BulletSpeed;
        [SerializeField] private string GroundLayerName = "Ground";
        private Rigidbody2D BulletsRigidBody;

        private void Awake()
        {
            BulletsRigidBody = GetComponent<Rigidbody2D>();
        }

        // ReSharper disable once InvertIf
        private void OnTriggerEnter2D(Collider2D collider)
        {
            var player = collider.GetComponent<Player_Controller>();
            if (player != null)
            {
                player.PlayerKnockBack(gameObject.transform);
                Destroy(gameObject);
            }
            else if (collider.gameObject.layer == LayerMask.NameToLayer(GroundLayerName)) Destroy(gameObject);
        }

        public void BulletsFly(float direction)
        {
            BulletsRigidBody.linearVelocityX = BulletSpeed * direction * -1;
        }

        public void DestroyBullets(float time)
        {
            Destroy(gameObject, time);
        }

        public void BulletFlip(float direction)
        {
            transform.localScale = new Vector2(direction * -1f, transform.localScale.y);
        }
    }
}