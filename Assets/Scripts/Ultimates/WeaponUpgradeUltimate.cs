using Combat.Weapons;
using Misc;
using UnityEngine;

namespace Ultimates
{
    public class WeaponUpgradeUltimate : Ultimate
    {
        [Header("Requirements")]
        [SerializeField] private Weapon _weapon;
        
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