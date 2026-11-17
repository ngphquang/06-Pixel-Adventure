using System;
using UnityEngine;

namespace Players
{
    public class Skin_Manager : MonoBehaviour
    {
        public static Skin_Manager Instance;
        public int ChosenSkinIndex;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void SetSkinIndex(int index)
        {
            ChosenSkinIndex = index;
        }

        public int GetSkinIndex()
        {
            return ChosenSkinIndex;
        }
    }
}