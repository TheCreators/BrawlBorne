using Events;
using Misc;
using UnityEngine;

namespace Ultimates
{
    public abstract class Ultimate : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, Min(0)] protected float _cooldown = 10f;
        
        [Header("Events")]
        [SerializeField] protected GameEvent _onUse;
        [SerializeField] protected GameEvent _onReady;

        private bool _canBeUsed = false;
        
        protected virtual void OnValidate()
        {
            this.CheckIfNull(_onUse, _onReady);
        }
        
        private void Start()
        {
            Invoke(nameof(SetCanBeUsedToTrue), _cooldown);
        }

        protected abstract void Use();
        
        protected virtual bool CanBeUsedExtraCondition => true;
        
        public void TryUse()
        {
            if (_canBeUsed is false || CanBeUsedExtraCondition is false)
            {
                return;
            }
            
            _canBeUsed = false;
            _onUse.Raise(this, null);
            Use();
        }

        protected void SetCanBeUsedToTrue()
        {
            _canBeUsed = true;
            _onReady.Raise(this, null);
        }
    }
}