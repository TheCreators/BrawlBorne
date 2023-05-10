using UnityEngine;

namespace Misc
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private Transform _shootAt;
        
        public Vector3 ShootAt => _shootAt.position;
    }
}