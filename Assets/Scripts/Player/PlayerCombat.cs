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
                _ultimate.Use();
            }
        }
    }
}