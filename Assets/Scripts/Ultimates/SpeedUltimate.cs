using System.Collections;
using Combat;
using Player;
using UnityEngine;

namespace Ultimates
{
    public class SpeedUltimate : Ultimate
    {
        [SerializeField] private LayerMask _hitLayers;
    
        [SerializeField, Range(0, 20)] private float _ultimateDuration = 7f;
        [SerializeField, Range(0, 50)] private float _speed = 20f;
        [SerializeField, Range(0, 10)] private float _timeBetweenHits = 1f;
        [SerializeField, Min(0)] private float _damagePerHit = 4f;
        [SerializeField, Min(0)] private float _hitRadius = 3f;
    
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            _playerMovement = GetComponentInParent<PlayerMovement>();
            if (_playerMovement is null) Debug.LogError("PlayerMovement is null for " + gameObject.name);
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
