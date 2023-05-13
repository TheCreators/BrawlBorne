using UnityEngine;

namespace Misc
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private Transform _shootAt;
        [SerializeField] private string _name;
        
        public Vector3 ShootAt => _shootAt.position;
        
        public string Name => _name;
    }
}