using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class Level_Selection_Manager : MonoBehaviour
    {
        [SerializeField] private Level_Buttons_Manager LevelButtonsPrefab;
        [SerializeField] private Transform LevelButtonsParent;
        [SerializeField] private bool[] LevelUnlocked;

        private void Start()
        {
            LoadLevelsInfo();
            CreateLevelButtons();
        }

        private void CreateLevelButtons()
        {
            var levelAmount = SceneManager.sceneCountInBuildSettings - 1;
            for (var i = 1; i < levelAmount; i++)
            {
                if (!IsLevelUnlocked(i)) return;

                var newButton = Instantiate(LevelButtonsPrefab, LevelButtonsParent);
                newButton.SetUpLevelButton(i);
            }
        }

        private bool IsLevelUnlocked(int levelIndex)
        {
            return LevelUnlocked[levelIndex];
        }

        private void LoadLevelsInfo()
        {
            var levelAmount = SceneManager.sceneCountInBuildSettings - 1;

            LevelUnlocked = new bool[levelAmount];
            for (var i = 0; i < levelAmount; i++)
            {
                var levelUnlocked = PlayerPrefs.GetInt($"NextLevelIndex {i} Unlocked", 0) == 1;
                if (levelUnlocked) LevelUnlocked[i] = true;
            }

            LevelUnlocked[1] = true;
        }
    }
}