using Players;
using UnityEngine;

namespace Checkpoints
{
    public class Startpoint_Manager : MonoBehaviour
    {
        private Animator StartpointAnimator;

        private void OnTriggerExit2D(Collider2D other)
        {
            var Player = other.GetComponent<Player_Controller>();
            if (Player == null) return;
            StartpointAnimator = GetComponent<Animator>();
            StartpointAnimator.SetTrigger("IsActive");
        }
    }
}