using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class HeroesPool : MonoSingleton<HeroesPool>
    {
        [Header("Requirements")] [SerializeField]
        private Hero _playerPrefab;

        public List<Hero> Heroes { get; private set; } = new();


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

            else if (Heroes.Count == 1)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            }
        }


        public void SetHeroes(List<Hero> heroes)
        {
            Heroes.Add(Instantiate(_playerPrefab));

            Heroes.AddRange(heroes);
        }
    }
}
