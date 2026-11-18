using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class Level_Buttons_Manager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI LevelButtonsText;
        public string SceneName;
        private int LevelIndex;

        public void SetUpLevelButton(int newLevelIndex)
        {
            LevelIndex = newLevelIndex;
            SceneName = $"Level {LevelIndex}";
            LevelButtonsText.text = $"Level {LevelIndex}";
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}