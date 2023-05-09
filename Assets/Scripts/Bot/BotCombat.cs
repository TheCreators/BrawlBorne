using Combat;
using Combat.Weapons;
using Ultimates;
using UnityEngine;

namespace Bot
{
    public class BotCombat : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        [SerializeField] private Ultimate _ultimate;
        
        private GameObject _target;
        public GameObject Target
        {
            get => _target;
            set
            {
                _target = value;
                
                if (_target is null)
                {
                    TargetShootPosition = null;
                    return;
                }
                
                var shootAt = _target.GetComponentInChildren<ShootAt>();
                TargetShootPosition = shootAt == null ? Target.transform.position : shootAt.transform.position;
            }
        }

        public Vector3? TargetShootPosition {get; private set; }

        private void Awake()
        {
            Target = gameObject;
        }


        public void AimAndTryUseWeapon()
        {
            if (TargetShootPosition is null) return;
            
            var lookDirection = TargetShootPosition!.Value - transform.position;
            transform.rotation = Quaternion.LookRotation(lookDirection);
            _weapon.TryUse();
            _ultimate.Use();
        }
    }
}