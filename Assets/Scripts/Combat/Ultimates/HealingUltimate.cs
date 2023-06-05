using System.Collections.Generic;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Combat.Ultimates
{
    public class HealingUltimate : Ultimate
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Min(0)]
        private float _healingPointsPercent = 50f;

        private Health _health;

        private void Awake()
        {
            _health = this.GetComponentInParentWithNullCheck<Health>();
        }

        protected override void Use(IEnumerator<Quaternion> aimRotations)
        {
            _health.Heal(_healingPointsPercent);
            Invoke(nameof(SetCanBeUsedToTrue), _cooldown);
        }
    }
}