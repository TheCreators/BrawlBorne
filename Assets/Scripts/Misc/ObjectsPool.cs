using System.Collections.Generic;
using System.Linq;
using Environment;
using Events;
using Heroes;
using Heroes.Player;
using Models;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class ObjectsPool : MonoSingleton<ObjectsPool>
    {
        [SerializeField] [BoxGroup(Group.Settings)]
        private bool _autoFillOnStart;

        [SerializeField] [BoxGroup(Group.Events)]
        private GameEvent _onHeroesAmountChanged;

        private Hero _playerPrefab;
        private Vector3 _playerSpawnPosition;

        public List<Hero> Heroes { get; private set; } = new();

        [ShowNativeProperty]
        private int HeroesAmount => Heroes.Count;

        public List<Crate> Crates { get; private set; } = new();

        [ShowNativeProperty]
        private int CratesAmount => Crates.Count;

        public List<Boost> Boosts { get; private set; } = new();

        [ShowNativeProperty]
        private int BoostsAmount => Boosts.Count;

        private void OnValidate()
        {
            this.CheckIfNull(_onHeroesAmountChanged);
        }

        private void Start()
        {
            _onHeroesAmountChanged.Raise(this, Heroes.Count);

            if (_autoFillOnStart)
            {
                AutoFill();
            }
        }

        public void SetPlayerPrefab(Player playerPrefab)
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

        public void SetPlayerSpawnPosition(Vector3 spawnPosition)
        {
            _playerSpawnPosition = spawnPosition;
        }

        public void InstantiatePlayer()
        {
            if (_playerPrefab == null)
            {   
                Debug.LogError("Player prefab is not set!");
                return;
            }
            
            Heroes.Add(Instantiate(_playerPrefab, _playerSpawnPosition, Quaternion.identity));
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

            if (hero is Player)
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
            }
        }

        [Button]
        private void AutoFill()
        {
            Heroes = FindObjectsOfType<Hero>().ToList();
            Crates = FindObjectsOfType<Crate>().ToList();
            Boosts = FindObjectsOfType<Boost>().ToList();
        }
    }
}