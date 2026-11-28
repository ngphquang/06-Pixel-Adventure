using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UI_In_Game_Manager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI TimerText;
        [SerializeField] private TextMeshProUGUI FruitsText;
        public static UI_In_Game_Manager Instance;
        public Fade_Effect_Manager FadeEffectManager { get; private set; } //Readonly

        private void Awake()
        {
            Instance = this;
            FadeEffectManager = GetComponentInChildren<Fade_Effect_Manager>();
        }

        private void Start()
        {
            FadeEffectManager.ScreenFadeEffect(0, 1f);
        }

        public void UpdateFruitsUI(int collectedFruits, int totalFruits)
        {
            FruitsText.text = $"{collectedFruits}/{totalFruits}";
        }

        public void UpdateTimerUI(float timer)
        {
            TimerText.text = $"{timer:00}s"; //== ToString("00")
        }
    }
}