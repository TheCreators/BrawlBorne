using System.Collections;
using Combat;
using Heroes.Player;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Ultimates
{
    public class SpeedUltimate : Ultimate
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Range(0, 20)]
        private float _ultimateDuration = 7f;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(0, 50)]
        private float _speed = 20f;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(0, 10)]
        private float _timeBetweenHits = 1f;
        
        [SerializeField] [BoxGroup(Group.Hit)]
        private LayerMask _hitLayers;
        
        [SerializeField] [BoxGroup(Group.Hit)] [Min(0)]
        private float _damagePerHit = 4f;

        [SerializeField] [BoxGroup(Group.Hit)] [Min(0)]
        private float _hitRadius = 3f;

        private PlayerMovement _playerMovement;

        protected override void OnValidate()
        {
            this.CheckIfNull(_hitLayers);

            base.OnValidate();
        }

        private void Awake()
        {
            _playerMovement = this.GetComponentInParentWithNullCheck<PlayerMovement>();
        }

        protected override void Use()
        {
            StartCoroutine(HittingRoutine());
        }

        private IEnumerator HittingRoutine()
        {
            float previousSpeed = _playerMovement.WalkSpeed;
            _playerMovement.WalkSpeed = _speed;

            int count = 0;
            while (_timeBetweenHits * count <= _ultimateDuration)
            {
                Hit();
                yield return new WaitForSeconds(_timeBetweenHits);
                count += 1;
            }

            _playerMovement.WalkSpeed = previousSpeed;

            Invoke(nameof(SetCanBeUsedToTrue), _cooldown);
        }

        private void Hit()
        {
            var colliders = new Collider[20];
            int count = Physics.OverlapSphereNonAlloc(transform.position, _hitRadius, colliders, _hitLayers);

            for (int i = 0; i < count; i++)
            {
                if (colliders[i].gameObject.TryGetComponent(out IDamageable damageable) && colliders[i].gameObject != gameObject)
                {
                    damageable.TakeDamage(_damagePerHit);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _hitRadius);
        }
    }
}