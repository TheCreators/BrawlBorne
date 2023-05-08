using Combat.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Requirements")]
        [SerializeField] private Weapon _weapon;

        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _weapon.TryUse();
            }
        }
    }
}