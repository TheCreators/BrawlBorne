using Combat.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ultimates
{
    public class DualGunUltimate : MonoBehaviour
    {
        [Header("Requirements")]
        [SerializeField] private Weapon _weapon;

        public void OnUltimate(InputAction.CallbackContext context)
        {
            if (context.performed is true)
            {
                _weapon.TryUse();
            }
        }
    }
}
