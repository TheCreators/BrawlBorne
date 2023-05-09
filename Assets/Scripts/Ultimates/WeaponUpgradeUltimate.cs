using Combat.Weapons;
using UnityEngine;

namespace Ultimates
{
    public class WeaponUpgradeUltimate : Ultimate
    {
        [Header("Requirements")]
        [SerializeField] private Weapon _weapon;

        public override void Use()
        {
            _weapon.TryUse();
        }
    }
}