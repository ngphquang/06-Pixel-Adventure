using System;
using System.Collections;
using Players;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    private UI_In_Game_Manager UIInGameManager;

    [Header("Levels Manager")] [SerializeField]
    private int CurrentLevelIndex;

    [SerializeField] private float LevelTimer;
    private int NextLevelIndex;

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
        CurrentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        NextLevelIndex = CurrentLevelIndex + 1;
        UIInGameManager = UI_In_Game_Manager.Instance;
        CollectFruitsInfo();
    }

    private void Update()
    {
        LevelTimer += Time.deltaTime;
        UIInGameManager.UpdateTimerUI(LevelTimer);
    }

    private void CollectFruitsInfo()
    {
        var allFruitsOnThisLevel = FindObjectsByType<Fruit_Manager>();
        TotalFruitsAmount = allFruitsOnThisLevel.Length;
        UIInGameManager.UpdateFruitsUI(FruitsCollected, TotalFruitsAmount);
    }

    public void AddFruits()
    {
        FruitsCollected++;
        UIInGameManager.UpdateFruitsUI(FruitsCollected, TotalFruitsAmount);
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

    private static void LoadTheEndScene()
    {
        SceneManager.LoadScene("End Scene");
    }

    private void LoadTheNextLevel()
    {
        SceneManager.LoadScene(NextLevelIndex);
    }

    public void LevelFinished()
    {
        SaveLevelProgression();
        LoadNextScene();
    }

    private void SaveLevelProgression()
    {
        PlayerPrefs.SetInt($"NextLevelIndex {NextLevelIndex} Unlocked", 1);
        if (!NoMoreLevel())
        {
            PlayerPrefs.SetInt("ContinueLevelIndex", NextLevelIndex);
        }
    }

    private void LoadNextScene()
    {
        if (NoMoreLevel())
        {
            LoadTheEndScene();
        }
        else
        {
            UIInGameManager.FadeEffectManager.ScreenFadeEffect(1, 1f, LoadTheNextLevel);
        }
    }

    private bool NoMoreLevel()
    {
        return CurrentLevelIndex + 2 == SceneManager.sceneCountInBuildSettings;
        //+2 because we don't count the Main Menu and the End Scene
    }
}