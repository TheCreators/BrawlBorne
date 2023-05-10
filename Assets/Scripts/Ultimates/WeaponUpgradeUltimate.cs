using System;
using Combat.Weapons;
using UnityEngine;

namespace Ultimates
{
    public class WeaponUpgradeUltimate : Ultimate
    {
        [Header("Requirements")]
        [SerializeField] private Weapon _weapon;

        private void Awake()
        {
            Invoke(nameof(SetCanBeUsedToTrue), _cooldown);
        }

        public override void Use()
        {
            if (_canBeUsed is false) return;
            _weapon.TryUse();
            _canBeUsed = false;
            Invoke(nameof(SetCanBeUsedToTrue), _cooldown);
        }
    }
}