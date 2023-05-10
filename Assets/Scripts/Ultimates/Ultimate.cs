using System;
using UnityEngine;

namespace Ultimates
{
    public abstract class Ultimate : MonoBehaviour
    {
        [SerializeField] protected bool _canBeUsed = false;
        [SerializeField, Min(0)] protected float _cooldown = 10f;
        public abstract void Use();

        protected void SetCanBeUsedToTrue()
        {
            _canBeUsed = true;
        }
    }
}