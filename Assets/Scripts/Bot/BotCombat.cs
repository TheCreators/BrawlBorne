using Combat;
using Combat.Weapons;
using JetBrains.Annotations;
using Misc;
using Ultimates;
using UnityEngine;

namespace Bot
{
    public class BotCombat : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        [SerializeField] private Ultimate _ultimate;

        public void Shoot(Hero hero)
        {
            var lookDirection = hero.ShootAt - transform.position;
            transform.rotation = Quaternion.LookRotation(lookDirection);
            _weapon.TryUse();
            _ultimate.Use();
            
            Debug.DrawRay(transform.position, lookDirection, Color.red);
        }
    }
}