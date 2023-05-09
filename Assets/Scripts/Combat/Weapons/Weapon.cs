using UnityEngine;

namespace Combat.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        protected bool CanBeUsed = true;

        public bool IsUsing { get; set; }
        
        public void TryUse() {
            if (CanBeUsed is false)
            {
                return;
            }

            IsUsing = true;
            Use();
        }
        
        protected abstract void Use();
    }
}