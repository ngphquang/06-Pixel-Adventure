using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class Main_Menu_Manager : MonoBehaviour
    {
        public string SceneName;
        private Fade_Effect_Manager FadeEffectManager;

        private void Awake()
        {
            FadeEffectManager = GetComponentInChildren<Fade_Effect_Manager>();
        }

        private void Start()
        {
            FadeEffectManager.ScreenFadeEffect(0, 1f);
        }

        public void NewGame()
        {
            FadeEffectManager.ScreenFadeEffect(1, 1f, LoadLevelScene);
        }

        private void LoadLevelScene()
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}