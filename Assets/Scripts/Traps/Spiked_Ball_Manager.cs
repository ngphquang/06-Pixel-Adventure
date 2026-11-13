using UnityEngine;

namespace Traps
{
    public class Spiked_Ball_Manager : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D SpikedRigidBody;
        [SerializeField] private float PushForce;

        private void Start()
        {
            var pushVector = new Vector2(PushForce, 0);
            SpikedRigidBody.AddForce(pushVector, ForceMode2D.Impulse);
        }
    }
}