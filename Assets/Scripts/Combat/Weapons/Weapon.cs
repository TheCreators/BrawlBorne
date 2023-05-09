using UnityEngine;
using UnityEngine.Events;

namespace Combat.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected UnityEvent _onUse;
        
        protected bool CanBeUsed = true;

        public void TryUse() {
            if (CanBeUsed is false)
            {
                return;
            }

            _onUse.Invoke();
            Use();
        }
        
        protected abstract void Use();
    }
}