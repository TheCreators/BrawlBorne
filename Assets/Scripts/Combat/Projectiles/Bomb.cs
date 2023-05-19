using Misc;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat.Projectiles
{
    [RequireComponent(typeof(AudioSource))]
    public class Bomb : RigidbodyProjectile
    {
        [SerializeField] private AudioClip _explosionSound;
        [SerializeField] private GameObject _explosion;
        [SerializeField] private GameObject _objectToDestroy;
        
        private AudioSource _audioSource;
        private float _timeToExplode = 3f;
        private float _explosionRadius = 5f;

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
            float timeToExplode,
            float explosionRadius)
        {
            base.Init(damage, hitLayers, sender);
            _timeToExplode = timeToExplode;
            _explosionRadius = explosionRadius;

            Invoke(nameof(Explode), _timeToExplode);
        }

        private void Explode()
        {
            Instantiate(_explosion, transform.position, _explosion.transform.rotation);
            _audioSource.PlayOneShot(_explosionSound);
            
            var colliders = new Collider[20];
            int count = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, colliders, HitLayers);

            for (int i = 0; i < count; i++)
            {
                if (colliders[i].gameObject.TryGetComponent(out IDamageable damageable) && 
                    (Sender.IsDestroyed() || colliders[i].gameObject != Sender.gameObject))
                {
                    damageable.TakeDamage(Damage);
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