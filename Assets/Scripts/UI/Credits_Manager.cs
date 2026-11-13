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
        private bool CreditsSkipped;

        private void Update()
        {
            RectTransform.anchoredPosition += Vector2.up * (ScrollSpeed * Time.deltaTime);
            if (RectTransform.anchoredPosition.y >= CreditsOffScreenPosition)
            {
                GoToMainMenu();
            }
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
                GoToMainMenu();
            }
        }
    }
}