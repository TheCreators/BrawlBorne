using Combat;
using Combat.Weapons;
using UnityEngine;

namespace Bot
{
    public class BotCombat : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        
        private GameObject _target;
        public GameObject Target
        {
            get => _target;
            set
            {
                if (value == null) return;
                _target = value;
                
                var shootAt = _target.GetComponentInChildren<ShootAt>();
                TargetShootPosition = shootAt == null ? Target.transform.position : shootAt.transform.position;
            }
        }

        public Vector3 TargetShootPosition {get; private set; }

        private void Awake()
        {
            Target = gameObject;
        }


        public void AimAndTryUseWeapon()
        {
            var lookDirection = TargetShootPosition - transform.position;
            transform.rotation = Quaternion.LookRotation(lookDirection);
            _weapon.TryUse();
        }
    }
}