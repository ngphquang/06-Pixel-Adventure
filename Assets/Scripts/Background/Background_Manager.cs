using System;
using UnityEngine;

namespace Background
{
    public enum BackgroundType
    {
        Blue,
        Brown,
        Grey,
        Green,
        Pink,
        Purple,
        Yellow
    }

    [RequireComponent(typeof(MeshRenderer))]
    public class Background_Manager : MonoBehaviour
    {
        [Header("Background Movement")] [SerializeField]
        private Vector2 MovingDirection;

        [Header("Colors")] [SerializeField] private BackgroundType BackgroundType;
        [SerializeField] private Texture2D[] Texture2D;
        private MeshRenderer MeshRenderer;

        private void Start()
        {
            MeshRenderer = GetComponent<MeshRenderer>();
            UpdateBackgroundTexture();
        }

        private void Update()
        {
            MeshRenderer.material.mainTextureOffset += MovingDirection * Time.deltaTime;
        }

        [ContextMenu("Update Background Texture")]
        private void UpdateBackgroundTexture()
        {
            if (MeshRenderer == null)
            {
                MeshRenderer = GetComponent<MeshRenderer>();
            }

            MeshRenderer.material.mainTexture = Texture2D[Convert.ToInt32(BackgroundType)];
        }
    }
}