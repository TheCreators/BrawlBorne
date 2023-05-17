using System;
using Combat.Weapons;
using Misc;
using Ultimates;
using UnityEngine;

namespace Bot
{
    public class BotCombat : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        [SerializeField] private Ultimate _ultimate;

        private void OnValidate()
        {
            this.CheckIfNull(_weapon);
            this.CheckIfNull(_ultimate);
        }

        public void Shoot(Hero hero)
        {
            Shoot(hero.ShootAt - transform.position);
        }

        public void Shoot(Component component)
        {
            Shoot(component.transform.position - transform.position);
        }
        
        private void Shoot(Vector3 lookDirection)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
            _weapon.TryUse();
            _ultimate.TryUse();

            Debug.DrawRay(transform.position, lookDirection, Color.red);
        }
    }
}