using System.Collections;
using Bot;
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
        private BotMovement _botMovement;
        private bool _isPlayer = false;

        private void Start()
        {
            if (TryGetComponent(out _playerMovement))
            {
                _isPlayer = true;
            }
            else if (TryGetComponent(out _botMovement))
            {
                _isPlayer = false;
            }
            else
            {
                throw new MissingComponentException("Missing PlayerMovement or BotMovement component");
            }
        }

        public override void Use()
        {
            StartCoroutine(HittingRoutine());
        }

        private IEnumerator HittingRoutine()
        {
            float previousSpeed = _isPlayer ? _playerMovement.WalkSpeed : _botMovement.WalkSpeed;

            if (_isPlayer)
            {
                _playerMovement.WalkSpeed = _speed;
            }
            else
            {
                _botMovement.WalkSpeed = _speed;
            }

            int count = 0;
            while (_timeBetweenHits * count <= _ultimateDuration)
            {
                Hit();
                yield return new WaitForSeconds(_timeBetweenHits);
                count += 1;
            }

            if (_isPlayer)
            {
                _playerMovement.WalkSpeed = previousSpeed;
            }
            else
            {
                _botMovement.WalkSpeed = previousSpeed;
            }
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