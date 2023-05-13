using System;
using Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ultimates
{
    [RequireComponent(typeof(Health))]
    public class HealingUltimate : Ultimate
    {
        [SerializeField, Range(0, 100)] private float _healingPointsPercent = 50f;
        
        private Health _health;
        void Start()
        {
            _health = GetComponent<Health>();
        }

        private void Awake()
        {
            Invoke(nameof(SetCanBeUsedToTrue), _cooldown);
        }

        public override void Use()
        {
            if (_canBeUsed is false) return;
            _onUse.Raise(this, null);
            _health.Heal(_healingPointsPercent);
            _canBeUsed = false;
            Invoke(nameof(SetCanBeUsedToTrue), _cooldown);
        }
    }
}
