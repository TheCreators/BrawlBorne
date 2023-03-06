using UnityEngine;
using UnityEngine.InputSystem;

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