using Events;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Misc
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Required]
        private Transform _groundCheck;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(0, 3)]
        private float _checkRadius = 0.4f;

        [SerializeField] [BoxGroup(Group.Settings)]
        private LayerMask _groundMask;

        [SerializeField] [BoxGroup(Group.Events)] [Required]
        private GameEvent _onLand;

        [SerializeField] [BoxGroup(Group.Events)] [Required]
        private GameEvent _onAirborne;

        public bool IsGrounded { get; private set; }
        private bool _wasGrounded;

        private void OnValidate()
        {
            this.CheckIfNull(_groundCheck);
            this.CheckIfNull(_groundMask);
            this.CheckIfNull(_onLand, _onAirborne);
        }

        private void Update()
        {
            IsGrounded = Physics.CheckBox(_groundCheck.position, Vector3.one * _checkRadius, Quaternion.identity, _groundMask);

            if (IsGrounded && !_wasGrounded)
            {
                _onLand.Raise(this, null);
            }

            if (!IsGrounded && _wasGrounded)
            {
                _onAirborne.Raise(this, null);
            }

            _wasGrounded = IsGrounded;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(_groundCheck.position, Vector3.one * _checkRadius);
        }
    }
}