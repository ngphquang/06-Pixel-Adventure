using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class Level_Selection_Manager : MonoBehaviour
    {
        [SerializeField] private Level_Buttons_Manager LevelButtonsPrefab;
        [SerializeField] private Transform LevelButtonsParent;

        private void Start()
        {
            CreateLevelButtons();
        }

        private void CreateLevelButtons()
        {
            var levelAmount = SceneManager.sceneCountInBuildSettings - 1;
            for (var i = 1; i < levelAmount; i++)
            {
                var newButton = Instantiate(LevelButtonsPrefab, LevelButtonsParent);
                newButton.SetUpLevelButton(i);
            }
        }
    }
}