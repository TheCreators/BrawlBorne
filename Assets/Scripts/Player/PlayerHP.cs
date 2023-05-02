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
    [SerializeField, Min(0)] private int _healthPoints = 100;

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == _bullet.gameObject)
        {
            _healthPoints -= _bullet._damage;
        }

        if (_healthPoints <= 0)
        {
            Destroy(transform.gameObject);
        }
    }
}
