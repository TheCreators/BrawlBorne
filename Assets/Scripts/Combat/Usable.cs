using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public abstract class Usable : MonoBehaviour
    {
        public abstract void TryUse(IEnumerator<Quaternion> aimRotations);
    }
}