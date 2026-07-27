using UnityEngine;

public class Checkpoint_Manager : MonoBehaviour
{
    private Animator CheckpointAnimator;
    private bool IsActive;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var Player = other.GetComponent<Player_Controller>();
        if (Player == null || IsActive) return;
        CheckpointAnimator = GetComponent<Animator>();
        ActivateCheckpoint();
    }

    void ActivateCheckpoint()
    {
        IsActive = true;
        print("Active");
        CheckpointAnimator.SetTrigger("IsActive");
        Game_Manager.GameManagerInstance.UpdateRespawnCheckpoint(gameObject.transform);
    }
}