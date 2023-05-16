using System.Collections;
using Events;
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

        protected bool CanBeUsed = true;
        private bool _isRefilling = false;

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
    }
}