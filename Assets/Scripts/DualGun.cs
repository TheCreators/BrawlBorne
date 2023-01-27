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
    [SerializeField] private int _bulletsPerShot = 1;
    [SerializeField] private float _timeBetweenBullets = 0.25f;
    [SerializeField] private float _bulletsSpread = 0.5f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(Shoot());
        }
    }
    
    private IEnumerator Shoot()
    {
        int positionShiftAmount = 1;
        for (int i = 0; i < _bulletsPerShot; i++)
        {
            Vector3 spawnPosition = transform.position + 
                                    transform.forward + 
                                    _bulletsSpread * positionShiftAmount * transform.right;
            
            Instantiate(_bullet, spawnPosition, _camera.rotation);
            
            positionShiftAmount *= -1;
            yield return new WaitForSeconds(_timeBetweenBullets);
        }
    }
}
