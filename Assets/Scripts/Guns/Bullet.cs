using System.Collections;
using System.Collections.Generic;
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
        [Header("Settings")]
        [SerializeField] private float _speed = 20f;
        [SerializeField] private float _lifeTime = 1f;
        [SerializeField] private int _damageSpeedCoeff = 0;
        [SerializeField] public float damage = 5f;

        private Vector3 _oldPosition;
        private float _downSpeed;


        private void Start()
        {
            _downSpeed = damage * (1 / _lifeTime) * _damageSpeedCoeff;
            Destroy(gameObject, _lifeTime);
        }

        private void Update()
        {
            _oldPosition = transform.position;
            transform.Translate(Vector3.forward * (_speed * Time.deltaTime));

            damage = Mathf.MoveTowards(damage, 0, _downSpeed * Time.deltaTime);

            //DetectCollision();
        }

        /*protected void DetectCollision()
        {
            if (Physics.Linecast(_oldPosition, transform.position, out _))
            {
                Destroy(gameObject);
            }
        }*/

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }
    }
}
