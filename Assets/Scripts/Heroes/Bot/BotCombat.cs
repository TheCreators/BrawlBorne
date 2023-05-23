﻿using Combat.Weapons;
using Misc;
using NaughtyAttributes;
using Ultimates;
using UnityEngine;

namespace Heroes.Bot
{
    public class BotCombat : MonoBehaviour
    {
        [SerializeField] [Required] [ShowAssetPreview]
        private Weapon _weapon;

        [SerializeField] [Required] [ShowAssetPreview]
        private Ultimate _ultimate;

        private void OnValidate()
        {
            this.CheckIfNull(_weapon);
            this.CheckIfNull(_ultimate);
        }

        public void Shoot(Hero hero, bool useUltimate = true)
        {
            Shoot(hero.ShootAt - transform.position, useUltimate);
        }

        public void Shoot(Component component, bool useUltimate = true)
        {
            Shoot(component.transform.position - transform.position, useUltimate);
        }

        private void Shoot(Vector3 lookDirection, bool useUltimate)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);

            if (_weapon is BallisticGun)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection + new Vector3(0, 2f, 0));
            }

            _weapon.TryUse();

            if (useUltimate)
            {
                _ultimate.TryUse();
            }

            Debug.DrawRay(transform.position, lookDirection, Color.red);
        }
    }
}