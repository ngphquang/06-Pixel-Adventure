using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class Credits_Manager : MonoBehaviour
    {
        [SerializeField] private RectTransform RectTransform;
        [SerializeField] private float ScrollSpeed = 200f;
        [SerializeField] private string MainMenuSceneName = "Main Menu";
        [SerializeField] private float CreditsOffScreenPosition;
        private Fade_Effect_Manager FadeEffectManager;
        private bool CreditsSkipped;

        private void Awake()
        {
            FadeEffectManager = GetComponentInChildren<Fade_Effect_Manager>();
        }

        private void Start()
        {
            FadeEffectManager.ScreenFadeEffect(0, 1f);
        }

        private void Update()
        {
            RectTransform.anchoredPosition += Vector2.up * (ScrollSpeed * Time.deltaTime);
            if (RectTransform.anchoredPosition.y >= CreditsOffScreenPosition)
            {
                LevelFinished();
            }
        }

        private void LevelFinished()
        {
            FadeEffectManager.ScreenFadeEffect(1, 1f, GoToMainMenu);
        }

        private void GoToMainMenu()
        {
            SceneManager.LoadScene(MainMenuSceneName);
        }

        public void SkipCredits()
        {
            if (!CreditsSkipped)
            {
                ScrollSpeed *= 10f;
                CreditsSkipped = true;
            }
            else
            {
                LevelFinished();
            }
        }
    }
}