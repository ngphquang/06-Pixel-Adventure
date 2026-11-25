using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class Main_Menu_Manager : MonoBehaviour
    {
        public string SceneName;
        private Fade_Effect_Manager FadeEffectManager;
        [SerializeField] private GameObject[] UIElements;
        [SerializeField] private GameObject ContinueButton;

        private void Awake()
        {
            FadeEffectManager = GetComponentInChildren<Fade_Effect_Manager>();
            ContinueButton.SetActive(false);
        }

        private void Start()
        {
            FadeEffectManager.ScreenFadeEffect(0, 1f);

            if (HasLevelProgression()) ContinueButton.SetActive(true);
        }

        public void SwitchUI(GameObject uiEnable)
        {
            foreach (var elements in UIElements)
            {
                elements.SetActive(false);
            }

            uiEnable.SetActive(true);
        }

        public void NewGame()
        {
            FadeEffectManager.ScreenFadeEffect(1, 1f, LoadLevelScene);
        }

        private void LoadLevelScene()
        {
            SceneManager.LoadScene(SceneName);
        }

        private static bool HasLevelProgression()
        {
            return PlayerPrefs.GetInt("ContinueLevelIndex", 0) > 0;
        }

        public void ContinueLevel()
        {
            var levelIndex = PlayerPrefs.GetInt("ContinueLevelIndex", 0);
            SceneManager.LoadScene(levelIndex);
        }
    }
}