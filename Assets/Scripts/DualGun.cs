using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class DualGun : MonoBehaviour
{
    [FormerlySerializedAs("_bulletPrefab")]
    [Header("Requirements")]
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _camera;

    [Header("Settings")]
    [SerializeField] [Min(0)] private int _bulletsPerShot = 6;
    [SerializeField] [Min(0)] private float _timeBetweenBullets = 0.25f;
    [SerializeField] [Min(0)] private float _timeBetweenShots = 0.5f;

    [Header("Spawn Settings")]
    [SerializeField] [Min(0)] private float _bulletsSpread = 0.5f;
    [SerializeField] private float _bulletSpawnDistance = 1f;
    [SerializeField] private float _bulletSpawnHeight = 0.5f;

    private bool _isShooting;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (_isShooting)
        {
            return;
        }

        _isShooting = true;
        StartCoroutine(StartShooting());
        StartCoroutine(StopShootingAfter(_timeBetweenShots));
    }

    private IEnumerator StartShooting()
    {
        int positionShiftAmount = 1;
        for (int i = 0; i < _bulletsPerShot; i++)
        {
            Vector3 spawnPosition = _camera.position + // Position
                                    _camera.forward * _bulletSpawnDistance + // Distance from camera
                                    _camera.up * _bulletSpawnHeight + // Height from camera
                                    _bulletsSpread * positionShiftAmount * transform.right; // Spread (left or right)

            Instantiate(_bullet, spawnPosition, _camera.rotation);

            positionShiftAmount *= -1;
            yield return new WaitForSeconds(_timeBetweenBullets);
        }
    }

    private IEnumerator StopShootingAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _isShooting = false;
    }
}
