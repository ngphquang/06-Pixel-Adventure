using System.Collections;
using UnityEngine;

namespace Traps
{
    public class Firetrap_Manager : MonoBehaviour
    {
        [SerializeField] private float FireCoolDown;
        [SerializeField] private Firetrap_Button_Manager FireButton;
        private Animator FiretrapAnimator;
        private CapsuleCollider2D FiretrapCollider;
        private bool IsActive;

        private void Awake()
        {
            FiretrapAnimator = GetComponent<Animator>();
            FiretrapCollider = GetComponent<CapsuleCollider2D>();
        }

        private void Start()
        {
            if (FireButton == null) return;
            SetFire(true);
        }

        private void SetFire(bool active)
        {
            FiretrapAnimator.SetBool("IsActive", active);
            FiretrapCollider.enabled = active;
            IsActive = active;
        }

        public void SwitchOffFire()
        {
            if (!IsActive) return;
            StartCoroutine(FireCoroutine());
        }

        private IEnumerator FireCoroutine()
        {
            SetFire(false);
            yield return new WaitForSeconds(FireCoolDown);
            SetFire(true);
        }
    }
}