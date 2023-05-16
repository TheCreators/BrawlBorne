using System;
using Combat.Weapons;
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

        private void Start()
        {
            if (_weapon is null) Debug.LogError("Weapon is null for " + gameObject.name);
            if (_ultimate is null) Debug.LogError("Ultimate is null for " + gameObject.name);
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