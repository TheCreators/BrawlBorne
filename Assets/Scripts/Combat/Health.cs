using Misc;
using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour, IDamageable
    {
        [Header("Settings")]
        [SerializeField, Min(0)] private float _healthPoints = 100f;

        public void TakeDamage(float damage)
        {
            _healthPoints -= damage;

            if (_healthPoints <= 0)
            {
                HeroesPool.Instance.RemoveHero(gameObject);
                Destroy(gameObject);
            }
        }
    }
}