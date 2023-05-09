using System.Collections;
using UnityEngine;

namespace Combat.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [Header("Cells")] 
        [SerializeField, Min(1)] private int _maxCells = 3;
        [SerializeField, Min(0)] private int _currentCells = 3;
        [SerializeField, Min(0)] private float _refillTime = 2f;

        protected bool CanBeUsed = true;
        private bool _isRefilling = false;
        
        protected abstract void Use();

        public void TryUse()
        {
            if (CanBeUsed is false || _currentCells <= 0)
            {
                return;
            }

            _currentCells--;
            if (_isRefilling is false)
            {
                StartCoroutine(Refill());
            }

            Use();
        }
        
        private IEnumerator Refill()
        {
            _isRefilling = true;

            while (_currentCells < _maxCells)
            {
                yield return new WaitForSeconds(_refillTime);
                _currentCells++;
            }

            _isRefilling = false;
        }
    }
}