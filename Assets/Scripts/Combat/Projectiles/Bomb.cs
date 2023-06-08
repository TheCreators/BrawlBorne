using System;
using Heroes;
using Misc;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

namespace Combat.Projectiles
{
    [RequireComponent(typeof(AudioSource))]
    public class Bomb : RigidbodyProjectile
    {
        [SerializeField] [Required]
        private AudioClip _explosionSound;

        [SerializeField] [Required]
        private GameObject _explosion;

        [SerializeField] [Required]
        private GameObject _objectToDestroy;

        private AudioSource _audioSource;
        private float _explosionRadius = 5f;
        private bool _exploded = false;

        private void OnValidate()
        {
            this.CheckIfNull(_explosionSound);
            this.CheckIfNull(_explosion);
        }

        protected override void Awake()
        {
            _audioSource = this.GetComponentWithNullCheck<AudioSource>();

            base.Awake();
        }

        public void Init(
            float damage,
            LayerMask hitLayers,
            Hero sender,
            float explosionRadius)
        {
            base.Init(damage, hitLayers, sender);
            _explosionRadius = explosionRadius;
        }

        private void OnCollisionEnter()
        {
            Explode();
        }

        private void Explode()
        {
            if (_exploded) return;
            _exploded = true;
            
            Instantiate(_explosion, transform.position, _explosion.transform.rotation);
            _audioSource.PlayOneShot(_explosionSound);

            var colliders = new Collider[20];
            int count = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, colliders, HitLayers);

            for (int i = 0; i < count; i++)
            {
                if (colliders[i].gameObject.TryGetComponent(out IDamageable damageable) &&
                    (Sender.IsDestroyed() || colliders[i].gameObject != Sender.gameObject))
                {
                    damageable.TakeDamage(Damage, Sender);
                }
            }

            Destroy(_objectToDestroy);
            Invoke(nameof(DestroyMyself), _explosionSound.length);
        }

        private void DestroyMyself()
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }
}