using Combat;
using Combat.Weapons;
using Events;
using Heroes;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Environment
{
    public class Boost : MonoBehaviour
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Min(0)]
        private float _healthIncreasePercent = 30f;

        [SerializeField] [BoxGroup(Group.Settings)] [Min(0)]
        private float _damageIncreasePercent = 30f;

        [SerializeField] [BoxGroup(Group.Events)] [Required]
        private GameEvent _onCollected;

        private void OnValidate()
        {
            this.CheckIfNull(_onCollected);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Hero hero) is false)
            {
                return;
            }

            hero.GetComponent<Health>()?.IncreaseMaxHealthWithCurrentHealth(_healthIncreasePercent);

            foreach (Weapon weapon in hero.GetComponentsInChildren<Weapon>())
            {
                weapon.IncreaseDamage(_damageIncreasePercent);
            }

            _onCollected.Raise(this, null);
        }
    }
}