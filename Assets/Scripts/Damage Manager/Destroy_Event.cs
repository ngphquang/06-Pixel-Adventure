using UnityEngine;

namespace Damage_Manager
{
    public class Destroy_Event : MonoBehaviour
    {
        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}