using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class HeroesPool : MonoSingleton<HeroesPool>
    {
        [Header("Requirements")]
        [SerializeField] private Hero _playerPrefab;
        [SerializeField] private Hero _botPrefab;
        
        [Header("Settings")]
        [SerializeField] [Min(0)] private int _botsCount = 2;
        
        public List<Hero> Heroes { get; private set; } = new ();
        
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
        
        public void RemoveHero(Component sender, object data)
        {
            if (sender.TryGetComponent(out Hero hero) && Heroes.Contains(hero))
            {
                Heroes.Remove(hero);
                Destroy(hero.gameObject);
            }

            if (hero.TryGetComponent(out PlayerMovement movement))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}