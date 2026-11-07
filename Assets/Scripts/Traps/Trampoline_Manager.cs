using Players;
using UnityEngine;

namespace Traps
{
    public class Trampoline_Manager : MonoBehaviour
    {
        [SerializeField] private float PushForce;
        [SerializeField] private float DisableControlTimer;
        private Transform TrampolineTransform;
        private Animator TrampolineAnimator;

        private void Awake()
        {
            TrampolineTransform = GetComponent<Transform>();
            if (TrampolineTransform.rotation != Quaternion.identity) DisableControlTimer *= 5;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var Player = other.GetComponent<Player_Controller>();
            if (Player == null) return;

            Player.PushPlayer(transform.up * PushForce, DisableControlTimer);
            TrampolineAnimator = GetComponent<Animator>();
            TrampolineAnimator.SetTrigger("IsActive");
        }
    }
}