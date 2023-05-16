using Events;
using Misc;
using Models;
using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour, IDamageable
    {
        [Header("Settings")]
        [SerializeField, Min(0)] private float _maxHealthPoints = 100f;
        [SerializeField, Min(0)] private float _healthPoints = 100f;
        
        [SerializeField] private GameEvent _onDeath;
        [SerializeField] private GameEvent _onHealthChanged;
        
        private void OnValidate()
        {
            this.CheckIfNull(_onDeath, _onHealthChanged);
        }

        private void Start()
        {
            _healthPoints = _maxHealthPoints;
            _onHealthChanged.Raise(this, new HealthAmount(_healthPoints, _maxHealthPoints));
        }

        public void TakeDamage(float damage)
        {
            _healthPoints -= damage;
            _onHealthChanged.Raise(this, new HealthAmount(_healthPoints, _maxHealthPoints));

            if (_healthPoints <= 0)
            {
                _onDeath.Raise(this, null);
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
            
            _onHealthChanged.Raise(this, new HealthAmount(_healthPoints, _maxHealthPoints));
        }

        public void IncreaseMaxHealth(float increasePercent)
        {
            _maxHealthPoints = _maxHealthPoints * (1 + increasePercent / 100);
            float increasedCurrentHealth = _healthPoints * (1 + increasePercent / 100);
            if (increasedCurrentHealth >= _maxHealthPoints)
            {
                _healthPoints = _maxHealthPoints;
            }
            else
            {
                _healthPoints = increasedCurrentHealth;
            }
            
            _onHealthChanged.Raise(this, new HealthAmount(_healthPoints, _maxHealthPoints));
        }
    }
}