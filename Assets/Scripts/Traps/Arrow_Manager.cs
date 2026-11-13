using UnityEngine;

namespace Traps
{
    public class Arrow_Manager : Trampoline_Manager
    {
        [Header("Arrows")] [SerializeField] private float RotationSpeed = 120f;
        [SerializeField] private bool RotateRight;
        private int RotateDirection = -1;

        [Header("Additional Info")] [SerializeField]
        private float InstantiateDelay;

        [SerializeField] private float ScaleUpSpeed = 10f;
        [SerializeField] private Vector3 TargetScale;

        private void Start()
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        private void Update()
        {
            ArrowScaleUp();
            ArrowRotation();
        }

        private void ArrowScaleUp()
        {
            if (transform.localScale.x < TargetScale.x)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, TargetScale, ScaleUpSpeed * Time.deltaTime);
            }
        }

        private void ArrowRotation()
        {
            RotateDirection = RotateRight ? 1 : -1;
            transform.Rotate(0, 0, RotationSpeed * RotateDirection * Time.deltaTime);
        }

        private void InstantiateArrow()
        {
            var arrowPrefab = Game_Manager.GameManagerInstance.ArrowPrefab;
            Game_Manager.GameManagerInstance.CreateObject(arrowPrefab, transform, InstantiateDelay);
            Destroy(gameObject);
        }
    }
}