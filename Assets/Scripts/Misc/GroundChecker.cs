using UnityEngine;
using UnityEngine.Events;

namespace Misc
{
    public class GroundChecker : MonoBehaviour
    {
        [Header("Requirements")]
        [SerializeField] private Transform _groundCheck;

        [Header("Settings")]
        [SerializeField, Range(0, 5)] private float _checkRadius = 0.4f;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private UnityEvent _onLand;
        [SerializeField] private UnityEvent _onAirborne;

        public bool IsGrounded { get; private set; }
        private bool _wasGrounded = false;

        private void Update()
        {
            IsGrounded = Physics.CheckBox(_groundCheck.position, Vector3.one * _checkRadius, Quaternion.identity, _groundMask);

            if (IsGrounded && !_wasGrounded)
            {
                _onLand.Invoke();
            }

            if (!IsGrounded && _wasGrounded)
            {
                _onAirborne.Invoke();
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