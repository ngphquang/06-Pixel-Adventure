using Players;
using UnityEngine;

namespace Checkpoints
{
    public class Checkpoint_Manager : MonoBehaviour
    {
        private Animator CheckpointAnimator;
        private bool IsActive;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsActive && !Game_Manager.GameManagerInstance.CheckpointsCanBeReactivated) return;

            var Player = other.GetComponent<Player_Controller>();
            if (Player == null) return;
            CheckpointAnimator = GetComponent<Animator>();
            ActivateCheckpoint();
        }

        void ActivateCheckpoint()
        {
            IsActive = true;
            CheckpointAnimator.SetTrigger("IsActive");
            Game_Manager.GameManagerInstance.UpdateRespawnCheckpoint(gameObject.transform);
        }
    }
}