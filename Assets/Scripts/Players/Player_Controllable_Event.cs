using UnityEngine;

namespace Players
{
    public class Player_Controllable_Event : MonoBehaviour
    {
        private Player_Controller Player;

        private void Awake()
        {
            Player = GetComponentInParent<Player_Controller>();
        }

        public void FinishRespawn()
        {
            Player.CanControlPLayer(true);
        }
    }
}