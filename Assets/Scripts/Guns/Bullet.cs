using UnityEngine;

namespace Guns
{
    /// <summary>
    /// Destroys the bullet after <see cref="_lifeTime"/> seconds.
    /// Moves the bullet forward at <see cref="_speed"/> units per second.
    /// Detects collision with any object and destroys the bullet.
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _lifeTime = 10f;

        private Vector3 _oldPosition;


        private void Start()
        {
            Destroy(gameObject, _lifeTime);
        }

        private void Update()
        {
            _oldPosition = transform.position;

            transform.Translate(Vector3.forward * (_speed * Time.deltaTime));

            DetectCollision();
        }

        private void DetectCollision()
        {
            if (Physics.Linecast(_oldPosition, transform.position, out _))
            {
                Destroy(gameObject);
            }
        }
    }
}
