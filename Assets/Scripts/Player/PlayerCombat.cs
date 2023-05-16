using System;
using Combat.Weapons;
using Misc;
using Ultimates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Requirements")]
        [SerializeField] private Weapon _weapon;
        [SerializeField] private Ultimate _ultimate;
        
        private void OnValidate()
        {
            this.CheckIfNull(_weapon);
            this.CheckIfNull(_ultimate);
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _weapon.TryUse();
            }
        }
        
        public void OnUltimate(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _ultimate.TryUse();
            }
        }
    }
}