using System.Collections;
using Players;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private int TotalFruitsAmount;

    [Header("Checkpoints")] [SerializeField]
    public bool CheckpointsCanBeReactivated;

    [Header("Traps")] [SerializeField] public GameObject ArrowPrefab;
    [SerializeField] public GameObject PlatformPrefab;

    private void Awake()
    {
        if (GameManagerInstance == null) GameManagerInstance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        CollectFruitsInfo();
    }

    private void CollectFruitsInfo()
    {
        var allFruitsOnThisLevel = FindObjectsByType<Fruit_Manager>();
        TotalFruitsAmount = allFruitsOnThisLevel.Length;
        print("Total fruits amount: " + TotalFruitsAmount);
    }

    public void AddFruits()
    {
        FruitsCollected++;
    }

    public int GetFruitsCollected()
    {
        return FruitsCollected;
    }

    public bool GetRandomFruits()
    {
        return RandomFruits;
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

    public void CreateObject(GameObject prefab, Transform target, float delay = 0)
    {
        StartCoroutine(CreateObjectCoroutine(prefab, target, delay));
    }

    private static IEnumerator CreateObjectCoroutine(GameObject prefab, Transform target, float delay)
    {
        var position = target.position;
        yield return new WaitForSeconds(delay);
        if (!prefab.Equals(null)) Instantiate(prefab, position, Quaternion.identity);
    }

    public void LoadTheEndScene()
    {
        SceneManager.LoadScene("End Scene");
    }
}