using Combat.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ultimates
{
    public class ShotgunUltimate : MonoBehaviour
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
