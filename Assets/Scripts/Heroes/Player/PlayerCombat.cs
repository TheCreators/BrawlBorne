using Combat.Weapons;
using Misc;
using NaughtyAttributes;
using Ultimates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Heroes.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] [Required] [ShowAssetPreview]
        private Weapon _weapon;

        [SerializeField] [Required] [ShowAssetPreview]
        private Ultimate _ultimate;

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