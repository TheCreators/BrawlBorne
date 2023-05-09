using Combat.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ultimates
{
    public class BombUltimate : Ultimate
    {
        [Header("Requirements")]
        [SerializeField] private Weapon _weapon;

        public override void Use()
        {
            _weapon.TryUse();
        }
    }
}
