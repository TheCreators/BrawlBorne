using System;
using System.Collections;
using Events;
using Misc;
using UnityEngine;

namespace Combat.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [Header("Cells")] 
        [SerializeField, Min(1)] private int _maxCells = 3;
        [SerializeField, Min(0)] private int _currentCells = 3;
        [SerializeField, Min(0)] private float _refillTime = 2f;
        
        [Header("Events")]
        [SerializeField] protected GameEvent _onUse;
        [SerializeField] protected GameEvent _onCellsAmountChanged;
        
        [Header("Damage")]
        [SerializeField, Min(0)] protected float _damage = 5f;
        
        [Header("Hit Layers")]
        [SerializeField] protected LayerMask _hitLayers;

        protected bool CanBeUsed = true;
        private bool _isRefilling = false;

        protected virtual void OnValidate()
        {
            this.CheckIfNull(_onUse, _onCellsAmountChanged);
            this.CheckIfNull(_hitLayers);
        }

        private void Start()
        {
            _onCellsAmountChanged.Raise(this, _currentCells);
        }

        protected abstract void Use();

        public void TryUse()
        {
            if (CanBeUsed is false || _currentCells <= 0)
            {
                return;
            }

            _currentCells--;
            _onCellsAmountChanged.Raise(this, _currentCells);
            if (_isRefilling is false)
            {
                StartCoroutine(Refill());
            }
            
            _onUse.Raise(this, null);
            Use();
        }
        
        private IEnumerator Refill()
        {
            _isRefilling = true;

            while (_currentCells < _maxCells)
            {
                yield return new WaitForSeconds(_refillTime);
                _currentCells++;
                _onCellsAmountChanged.Raise(this, _currentCells);
            }

            _isRefilling = false;
        }

        public void IncreaseDamage(float increasePercent)
        {
            _damage *= 1 + increasePercent / 100;
        }
    }
}