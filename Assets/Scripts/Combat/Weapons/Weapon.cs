using UnityEngine;

namespace Combat.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        protected bool CanBeUsed = true;
        
        public void TryUse() {
            if (CanBeUsed is false)
            {
                return;
            }

            Use();
        }
        
        protected abstract void Use();
    }
}