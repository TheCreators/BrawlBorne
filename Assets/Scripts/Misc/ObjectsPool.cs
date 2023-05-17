using System.Collections.Generic;
using Environment;
using Events;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class ObjectsPool : MonoSingleton<ObjectsPool>
    {
        [SerializeField] private GameEvent _onHeroesAmountChanged;
        [SerializeField] private bool _autoFill = false;

        private Hero _playerPrefab;

        public List<Hero> Heroes { get; private set; } = new();
        public List<Crate> Crates { get; private set; } = new();
        public List<Boost> Boosts { get; private set; } = new();
        
        private void OnValidate()
        {
            this.CheckIfNull(_onHeroesAmountChanged);
        }
        
        private void Start()
        {
            _onHeroesAmountChanged.Raise(this, Heroes.Count);
            
            if (_autoFill)
            {
                Heroes.AddRange(FindObjectsOfType<Hero>());
                Crates.AddRange(FindObjectsOfType<Crate>());
                Boosts.AddRange(FindObjectsOfType<Boost>());
            }
        }

        public void SetPlayerPrefab(Hero playerPrefab)
        {
            _playerPrefab = playerPrefab;
        }
        
        public void SetHeroes(IEnumerable<Hero> heroes)
        {
            Heroes.AddRange(heroes);
            _onHeroesAmountChanged.Raise(this, Heroes.Count);
        }
        
        public void SetCrates(IEnumerable<Crate> crates)
        {
            Crates.AddRange(crates);
        }
        
        public void InstantiatePlayer()
        {
            Heroes.Add(Instantiate(_playerPrefab));
            _onHeroesAmountChanged.Raise(this, Heroes.Count);
        }
        
        public void AddBoost(Boost boost)
        {
            Boosts.Add(boost);
        }

        public void RemoveHero(Component sender, object data)
        {
            if (sender.TryGetComponent(out Hero hero) && Heroes.Contains(hero))
            {
                _onHeroesAmountChanged.Raise(this, Heroes.Count - 1);
                Heroes.Remove(hero);
                Destroy(hero.gameObject);
            }

            if (hero.TryGetComponent(out PlayerMovement _))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (Heroes.Count == 1)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            }
        }
        
        public void RemoveProp(Component sender, object data)
        {
            if (sender.TryGetComponent(out Crate crate) && Crates.Contains(crate))
            {
                Crates.Remove(crate);
                Destroy(crate.gameObject);
                return;
            }
            
            if (sender.TryGetComponent(out Boost boost) && Boosts.Contains(boost))
            {
                Boosts.Remove(boost);
                Destroy(boost.gameObject);
                return;
            }
        }
    }
}