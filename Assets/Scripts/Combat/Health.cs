using Events;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] [ProgressBar("Health", nameof(_maxHealthPoints), EColor.Green)]
        private float _healthPointsBar = 100f;

        [SerializeField] [Min(0)] [OnValueChanged(nameof(OnHealthChangedUsingInspector))]
        private float _healthPoints = 100f;

        [SerializeField] [Min(0)] [OnValueChanged(nameof(OnMaxHealthChangedUsingInspector))]
        private float _maxHealthPoints = 100f;

        [SerializeField]
        private bool _isInvulnerable;

        [SerializeField] [BoxGroup(Group.Events)] [Required]
        private GameEvent _onDeath;

        [SerializeField] [BoxGroup(Group.Events)] [Required]
        private GameEvent _onHealthChanged;

        [SerializeField] [BoxGroup(Group.Events)] [Required]
        private GameEvent _onMaxHealthChanged;

        private float HealthPoints
        {
            get => _healthPoints;
            set
            {
                if (value < 0)
                {
                    _healthPoints = 0;
                }
                else if (value > MaxHealthPoints)
                {
                    _healthPoints = MaxHealthPoints;
                }
                else
                {
                    _healthPoints = value;
                }

                _healthPointsBar = HealthPoints;
                _onHealthChanged.Raise(this, HealthPoints);

                bool isDead = HealthPoints <= 0;
                if (isDead)
                {
                    _onDeath.Raise(this, null);
                }
            }
        }

        private float MaxHealthPoints
        {
            get => _maxHealthPoints;
            set
            {
                if (value < 0)
                {
                    _maxHealthPoints = 0;
                }
                else
                {
                    _maxHealthPoints = value;

                    if (HealthPoints > MaxHealthPoints)
                    {
                        HealthPoints = MaxHealthPoints;
                    }
                }

                _onMaxHealthChanged.Raise(this, MaxHealthPoints);
            }
        }

        private void OnValidate()
        {
            this.CheckIfNull(_onDeath, _onHealthChanged);
        }

        private void Start()
        {
            _onHealthChanged.Raise(this, HealthPoints);
            _onMaxHealthChanged.Raise(this, MaxHealthPoints);
        }

        public void TakeDamage(float damage)
        {
            if (_isInvulnerable)
            {
                return;
            }

            HealthPoints -= damage;
        }

        public void Heal(float healedPointsPercent)
        {
            HealthPoints += MaxHealthPoints * healedPointsPercent / 100;
        }

        public void IncreaseMaxHealthWithCurrentHealth(float increasePercent)
        {
            float increaseCoefficient = 1 + increasePercent / 100;
            MaxHealthPoints *= increaseCoefficient;
            HealthPoints *= increaseCoefficient;
        }

        private void OnHealthChangedUsingInspector() => HealthPoints = _healthPoints;

        private void OnMaxHealthChangedUsingInspector() => MaxHealthPoints = _maxHealthPoints;
    }
}