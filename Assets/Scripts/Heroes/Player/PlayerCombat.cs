using System.Collections.Generic;
using Combat;
using Misc;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Heroes.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] [Required] [ShowAssetPreview]
        private Usable _weapon;

        [SerializeField] [Required] [ShowAssetPreview]
        private Usable _ultimate;
        
        [SerializeField] [Required]
        private Transform _aimTransform;

        private void OnValidate()
        {
            this.CheckIfNull(_weapon);
            this.CheckIfNull(_ultimate);
            this.CheckIfNull(_aimTransform);
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _weapon.TryUse(GetAimRotations());
            }
        }

        public void OnUltimate(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _ultimate.TryUse(GetAimRotations());
            }
        }
        
        private IEnumerator<Quaternion> GetAimRotations()
        {
            while (true)
            {
                yield return _aimTransform.rotation;
            }
        }
    }
}