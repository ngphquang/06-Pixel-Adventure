using Players;
using UnityEngine;

namespace Damage_Manager
{
    public class Damage_Trigger : MonoBehaviour
    {
        [SerializeField] private bool IsEnemy;
        private Collider2D EnemiesCollider;

        private void Awake()
        {
            EnemiesCollider = GetComponentInParent<Collider2D>();
        }

        private void Update()
        {
            if (!EnemiesCollider.enabled && IsEnemy) Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<Player_Controller>();
            if (player != null) player.PlayerKnockBack(gameObject.transform);
            // Game_Manager.GameManagerInstance.RespawnPlayer();
        }
    }
}