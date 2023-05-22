using System.Collections;
using Events;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Combat.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] [BoxGroup(Group.Cells)] [ProgressBar("Cells", nameof(_maxCells), EColor.Orange)]
        private int _cellsBar = 3;

        [SerializeField] [BoxGroup(Group.Cells)] [Min(0)]
        [OnValueChanged(nameof(OnMaxCellsChangedUsingInspector))]
        private int _maxCells = 3;

        [SerializeField] [BoxGroup(Group.Cells)] [Min(0)]
        [OnValueChanged(nameof(OnCurrentCellsChangedUsingInspector))]
        private int _currentCells = 3;

        [SerializeField] [BoxGroup(Group.Cells)] [Min(0)]
        private float _refillTime = 2f;

        [SerializeField] [BoxGroup(Group.Events)] [Required]
        protected GameEvent _onUse;

        [SerializeField] [BoxGroup(Group.Events)] [Required]
        protected GameEvent _onCellsAmountChanged;

        [SerializeField] [Min(0)] [BoxGroup(Group.Hit)]
        protected float _damage = 5f;

        [SerializeField] [BoxGroup(Group.Hit)]
        protected LayerMask _hitLayers;

        protected bool CanBeUsed = true;
        private bool _isRefilling;

        private int CurrentCells
        {
            get => _currentCells;
            set
            {
                if (value < 0)
                {
                    _currentCells = 0;
                }
                else if (value > _maxCells)
                {
                    _currentCells = _maxCells;
                }
                else
                {
                    _currentCells = value;
                }

                _cellsBar = CurrentCells;
                _onCellsAmountChanged.Raise(this, CurrentCells);
            }
        }

        private int MaxCells
        {
            get => _maxCells;
            set
            {
                if (value < 0)
                {
                    _maxCells = 0;
                }
                else
                {
                    _maxCells = value;
                }

                if (CurrentCells > MaxCells)
                {
                    CurrentCells = MaxCells;
                }
            }
        }

        protected virtual void OnValidate()
        {
            this.CheckIfNull(_onUse, _onCellsAmountChanged);
            this.CheckIfNull(_hitLayers);
        }

        protected virtual void Start()
        {
            CurrentCells = _currentCells;
            MaxCells = _maxCells;
        }

        protected abstract void Use();

        public void TryUse()
        {
            if (CanBeUsed is false || CurrentCells == 0)
            {
                return;
            }

            CurrentCells--;
            _onCellsAmountChanged.Raise(this, CurrentCells);
            if (_isRefilling is false)
            {
                StartCoroutine(Refill());
            }

            _onUse.Raise(this, null);
            Use();
        }

        public void IncreaseDamage(float increasePercent)
        {
            _damage *= 1 + increasePercent / 100;
        }

        private IEnumerator Refill()
        {
            _isRefilling = true;

            while (CurrentCells != MaxCells)
            {
                yield return new WaitForSeconds(_refillTime);
                CurrentCells++;
                _onCellsAmountChanged.Raise(this, CurrentCells);
            }

            _isRefilling = false;
        }

        private void OnCurrentCellsChangedUsingInspector() => CurrentCells = _currentCells;

        private void OnMaxCellsChangedUsingInspector() => MaxCells = _maxCells;
    }
}