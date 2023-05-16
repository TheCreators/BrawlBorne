using Combat;
using Misc;
using UnityEngine;

namespace Ultimates
{
    public class HealingUltimate : Ultimate
    {
        [SerializeField, Range(0, 100)] private float _healingPointsPercent = 50f;
        
        private Health _health;
        
        private void Awake()
        {
            _health = this.GetComponentInParentWithNullCheck<Health>();
        }

        protected override void Use()
        {
            _health.Heal(_healingPointsPercent);
            Invoke(nameof(SetCanBeUsedToTrue), _cooldown);
        }
    }
}
