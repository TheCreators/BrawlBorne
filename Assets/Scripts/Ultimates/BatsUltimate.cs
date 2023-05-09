using Combat.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ultimates
{
    public class BatsUltimate : Ultimate
    {
        [Header("Requirements")]
        [SerializeField] private Weapon _weapon;

        public override void Use()
        {
            _weapon.TryUse();
        }
    }
}
