using UnityEngine;

public class Damage_Trigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<Player_Controller>();
        if (player != null) player.PlayerKnockBack(gameObject.transform);
        // Game_Manager.GameManagerInstance.RespawnPlayer();
    }
}