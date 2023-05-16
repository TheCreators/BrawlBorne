using Combat.Weapons;
using UnityEngine;

namespace Ultimates
{
    public class WeaponUpgradeUltimate : Ultimate
    {
        [Header("Requirements")]
        [SerializeField] private Weapon _weapon;

        protected void Awake()
        {
            if (_weapon is null) Debug.LogError("Weapon is null for " + gameObject.name);
        }

        protected override void Use()
        {
            _weapon.TryUse();
            Invoke(nameof(SetCanBeUsedToTrue), _cooldown);
        }
    }
}