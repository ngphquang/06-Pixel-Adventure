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
            var Player = collider.GetComponent<Player_Controller>();
            if (Player != null)
            {
                Player.PlayerDie();
                Game_Manager.GameManagerInstance.RespawnPlayer();
            }

            var Enemies = GetComponent<Enemies_Manager>();
            if (Enemies != null)
            {
                Enemies.EnemiesDie();
            }
        }
    }
}