using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class HeroesPool : MonoSingleton<HeroesPool>
    {
        [Header("Requirements")]
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _botPrefab;
        
        [Header("Settings")]
        [SerializeField] [Min(0)] private int _botsCount = 2;

        public List<GameObject> Heroes { get; private set; } = new ();
        
        private void Start()
        {
            Heroes.Add(Instantiate(_playerPrefab));
            
            for (var i = 0; i < _botsCount; i++)
            {
                // Random position in circle
                var randomPosition = transform.position + Random.insideUnitSphere * 100f;
                Vector3 spawnPosition = new Vector3(randomPosition.x, 0, randomPosition.z);
                Heroes.Add(Instantiate(_botPrefab, spawnPosition, Quaternion.identity));
            }
        }
        
        public void RemoveHero(GameObject hero)
        {
            Heroes.Remove(hero);
        }
    }
}