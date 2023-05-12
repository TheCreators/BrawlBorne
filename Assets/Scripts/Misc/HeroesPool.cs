using System.Collections.Generic;
using UnityEngine;

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
        }

        public void GetHeroes(List<Hero> heroes)
        {
            Heroes.Add(Instantiate(_playerPrefab));

            Heroes.AddRange(heroes);
        }
    }
}