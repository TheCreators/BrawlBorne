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
                Heroes.Add(Instantiate(_botPrefab));
            }
            
            Debug.Log($"Heroes count: {GetHashCode()}");
        }
        
        private void OnDestroy()
        {
            foreach (var hero in Heroes)
            {
                Destroy(hero);
            }
        }
    }
}