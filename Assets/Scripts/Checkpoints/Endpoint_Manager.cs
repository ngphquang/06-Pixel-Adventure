using Players;
using UnityEngine;

namespace Checkpoints
{
    public class Endpoint_Manager : MonoBehaviour
    {
        private Animator EndpointAnimator;
        private bool IsActive;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<Player_Controller>();
            if (player == null || IsActive) return;

            EndpointAnimator = GetComponent<Animator>();
            EndpointAnimator.SetTrigger("IsActive");
            Game_Manager.GameManagerInstance.LoadTheEndScene();
            IsActive = true;
        }
    }
}