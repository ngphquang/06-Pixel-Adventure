using Players;
using UnityEngine;

namespace UI
{
    public class Skin_Selection_Manager : MonoBehaviour
    {
        [SerializeField] private int CurrentSkinIndex = 0;
        private int MaxSkinIndex;
        [SerializeField] private Animator SkinAnimator;

        private void Awake()
        {
            MaxSkinIndex = SkinAnimator.layerCount - 1;
        }

        public void NextSkin()
        {
            CurrentSkinIndex++;
            if (CurrentSkinIndex > MaxSkinIndex) CurrentSkinIndex = 0;

            UpdateSkin();
        }

        public void PreviousSkin()
        {
            CurrentSkinIndex--;
            if (CurrentSkinIndex < 0) CurrentSkinIndex = MaxSkinIndex;

            UpdateSkin();
        }

        private void UpdateSkin()
        {
            for (var i = 0; i <= MaxSkinIndex; i++)
            {
                SkinAnimator.SetLayerWeight(i, 0);
            }

            SkinAnimator.SetLayerWeight(CurrentSkinIndex, 1);
        }

        public void SelectSkin()
        {
            Skin_Manager.Instance.SetSkinIndex(CurrentSkinIndex);
        }
    }
}