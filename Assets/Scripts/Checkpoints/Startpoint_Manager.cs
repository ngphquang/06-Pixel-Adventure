using Players;
using UnityEngine;

namespace Checkpoints
{
    public class Startpoint_Manager : MonoBehaviour
    {
        private Animator StartpointAnimator;

        private void OnTriggerExit2D(Collider2D other)
        {
            var player = other.GetComponent<Player_Controller>();
            if (player == null) return;
            StartpointAnimator = GetComponent<Animator>();
            StartpointAnimator.SetTrigger("IsActive");
        }
    }
}