using System;
using Enemies;
using Players;
using UnityEngine;

namespace Traps
{
    public class DeadZone : MonoBehaviour
    {
        // ReSharper disable once InvertIf
        private void OnTriggerEnter2D(Collider2D collider)
        {
            var player = collider.GetComponent<Player_Controller>();
            if (player != null)
            {
                player.PlayerDie();
                Game_Manager.GameManagerInstance.RespawnPlayer();
            }

            var enemies = GetComponent<Enemies_Manager>();
            if (enemies != null)
            {
                enemies.EnemiesDie();
            }
        }
    }
}