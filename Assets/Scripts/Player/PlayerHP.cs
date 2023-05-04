using Guns;
using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerHP : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private Bullet _bullet;

    [Header("Settings")]
    [SerializeField, Min(0)] private float _healthPoints = 100f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && _bullet != null)
        {
            if (collision.gameObject.CompareTag("Damagable"))
            {
                print(_bullet.damage);
                _healthPoints -= _bullet.damage;
            }
        }

        if (_healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}