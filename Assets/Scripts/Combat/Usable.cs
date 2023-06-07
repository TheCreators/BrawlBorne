using System.Collections.Generic;
using Heroes;
using Misc;
using UnityEngine;

namespace Combat
{
    public abstract class Usable : MonoBehaviour
    {
        protected Hero Owner;
        
        protected virtual void Awake()
        {
            Owner = this.GetComponentInParentWithNullCheck<Hero>();
        }
        
        public abstract void TryUse(IEnumerator<Quaternion> aimRotations);
        
    }
}