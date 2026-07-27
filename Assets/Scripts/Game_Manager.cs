using System.Collections;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager GameManagerInstance;
    [Header("Player")] [SerializeField] public GameObject MainPlayer;
    [SerializeField] public Player_Controller Player;
    private GameObject PlayerGameObject;
    [SerializeField] private Transform RespawnPoint;
    [SerializeField] private float RespawnDelay = 1f;

    [Header("Fruits")] [SerializeField] private bool RandomFruits = true;
    private int FruitsCollected;

    private void Awake()
    {
        if (GameManagerInstance == null) GameManagerInstance = this;
        else Destroy(gameObject);
    }

    public void AddFruits()
    {
        FruitsCollected++;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(RespawnDelay);
        var player = Instantiate(MainPlayer, RespawnPoint.position, Quaternion.identity);
        Player = player.GetComponent<Player_Controller>();
    }

    public void UpdateRespawnCheckpoint(Transform checkpoint)
    {
        RespawnPoint = checkpoint;
    }

    public int GetFruitsCollected()
    {
        return FruitsCollected;
    }

    public bool GetRandomFruits()
    {
        return RandomFruits;
    }
}