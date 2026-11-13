using Players;
using UnityEngine;

namespace Traps
{
    public class Firetrap_Button_Manager : MonoBehaviour
    {
        private Animator FiretrapButtonAnimator;
        private Firetrap_Manager Firetrap;

        private void Awake()
        {
            FiretrapButtonAnimator = GetComponent<Animator>();
            Firetrap = GetComponentInParent<Firetrap_Manager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<Player_Controller>();
            if (player == null) return;
            FiretrapButtonAnimator.SetTrigger("IsActive");
            Firetrap.SwitchOffFire();
        }
    }
}