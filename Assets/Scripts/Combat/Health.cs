using System;
using Misc;
using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour, IDamageable
    {
        [Header("Settings")]
        [SerializeField, Min(0)] private float _maxHealthPoints = 100f;
        [SerializeField, Min(0)] private float _healthPoints = 100f;

        public float GetHealth { get; private set; }

        private void Start()
        {
            _healthPoints = _maxHealthPoints;
            GetHealth = _healthPoints;
        }

        public void TakeDamage(float damage)
        {
            _healthPoints -= damage;
            GetHealth = _healthPoints;

            if (_healthPoints <= 0)
            {
                HeroesPool.Instance.RemoveHero(gameObject);
                Destroy(gameObject);
            }
        }

        public void Heal(float healedPointsPercent)
        {
            float healedValue = _maxHealthPoints * healedPointsPercent / 100;
            if (_healthPoints + healedValue < _maxHealthPoints)
            {
                _healthPoints += healedValue;
            }
            else
            {
                _healthPoints = _maxHealthPoints;
            }
            GetHealth = _healthPoints;
        }
    }
}