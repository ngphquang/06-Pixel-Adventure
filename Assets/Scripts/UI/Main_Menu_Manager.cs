using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class Main_Menu_Manager : MonoBehaviour
    {
        public string SceneName;

        public void NewGame()
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}