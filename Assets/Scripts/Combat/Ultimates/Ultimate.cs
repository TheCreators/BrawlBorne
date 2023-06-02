using Events;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Ultimates
{
    public abstract class Ultimate : MonoBehaviour
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Min(0)]
        protected float _cooldown = 10f;
        
        [SerializeField] [BoxGroup(Group.Events)] [Required]
        protected GameEvent _onUse;

        [SerializeField] [BoxGroup(Group.Events)] [Required]
        protected GameEvent _onReady;

        [ShowNonSerializedField]
        private bool _canBeUsed;

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