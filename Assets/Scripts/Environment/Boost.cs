using Combat;
using Combat.Weapons;
using Events;
using Misc;
using UnityEngine;

namespace Environment
{
    public class Boost : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _healthIncreasePercent = 30f;
        [SerializeField, Min(0)] private float _damageIncreasePercent = 30f;
        [SerializeField] private GameEvent _onCollected;
        
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
            
            hero.GetComponent<Health>()?.IncreaseMaxHealth(_healthIncreasePercent);

            foreach (Weapon weapon in hero.GetComponentsInChildren<Weapon>())
            {
                weapon.IncreaseDamage(_damageIncreasePercent);
            }
            
            _onCollected.Raise(this, null);
        }
    }
}
