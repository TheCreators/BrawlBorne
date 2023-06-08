using System.Collections.Generic;
using Combat.Weapons;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Combat.Ultimates
{
    public class WeaponUpgradeUltimate : Ultimate
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Required]
        private Usable _weapon;
        
        public Usable Weapon => _weapon;

        protected override void OnValidate()
        {
            this.CheckIfNull(_weapon);

            base.OnValidate();
        }

        protected override void Use(IEnumerator<Quaternion> aimRotations)
        {
            _weapon.TryUse(aimRotations);
            Invoke(nameof(SetCanBeUsedToTrue), _cooldown);
        }
    }
}