using System;
using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour, IDamageable
    {
        [Header("Settings")]
        [SerializeField, Min(0)] private float _maxHealthPoints = 100f;
        [SerializeField, Min(0)] private float _healthPoints = 100f;

        private void Start()
        {
            _healthPoints = _maxHealthPoints;
        }

        public void TakeDamage(float damage)
        {
            _healthPoints -= damage;

            if (_healthPoints <= 0)
            {
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
        }
    }
}