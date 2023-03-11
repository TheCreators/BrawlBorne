using UnityEngine;

namespace Guns
{
    public abstract class Gun : MonoBehaviour
    {
        [Header("Requirements")]
        [SerializeField] protected GameObject _bullet;
        [SerializeField] protected Transform _shootingDirection;

        public abstract void Shoot();
    }
}