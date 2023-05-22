using Combat.Weapons;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Ultimates
{
    public class WeaponUpgradeUltimate : Ultimate
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Required]
        private Weapon _weapon;

        protected override void OnValidate()
        {
            this.CheckIfNull(_weapon);

            base.OnValidate();
        }

        protected override void Use()
        {
            _weapon.TryUse();
            Invoke(nameof(SetCanBeUsedToTrue), _cooldown);
        }
    }
}