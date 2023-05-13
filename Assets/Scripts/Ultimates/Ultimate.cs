using System;
using Events;
using UnityEngine;

namespace Ultimates
{
    public abstract class Ultimate : MonoBehaviour
    {
        [SerializeField] protected bool _canBeUsed = false;
        [SerializeField, Min(0)] protected float _cooldown = 10f;
        [SerializeField] protected GameEvent _onUse;
        [SerializeField] protected GameEvent _onReady;

        public abstract void Use();

        protected void SetCanBeUsedToTrue()
        {
            _canBeUsed = true;
            _onReady.Raise(this, null);
        }
    }
}