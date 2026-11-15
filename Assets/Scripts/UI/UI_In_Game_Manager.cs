using System;
using UnityEngine;

namespace UI
{
    public class UI_In_Game_Manager : MonoBehaviour
    {
        public static UI_In_Game_Manager Instance;
        public Fade_Effect_Manager FadeEffectManager;

        private void Awake()
        {
            Instance = this;
            FadeEffectManager = GetComponentInChildren<Fade_Effect_Manager>();
        }

        private void Start()
        {
            FadeEffectManager.ScreenFadeEffect(0, 1f);
        }
    }
}