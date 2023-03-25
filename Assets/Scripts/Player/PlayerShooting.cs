using Guns;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerShooting : MonoBehaviour
    {
        [Header("Requirements")]
        [SerializeField] private Gun _gun;

        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _gun.Shoot();
            }
        }
    }
}