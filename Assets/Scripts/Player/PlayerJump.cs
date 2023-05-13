using System.Collections;
using Events;
using Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(GroundChecker))]
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _jumpForce = 8f;
        [SerializeField, Min(0)] private float _jumpCooldown = 0.5f;
        [SerializeField] private GameEvent _onJump;
        
        private bool _readyToJump = true;
        private bool _jumpHeld = false;

        private Rigidbody _rigidbody;
        private GroundChecker _groundChecker;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _groundChecker = GetComponent<GroundChecker>();
        }

        private void FixedUpdate()
        {
            if (_jumpHeld && _readyToJump && _groundChecker.IsGrounded)
            {
                _onJump.Raise(this, null);
                _readyToJump = false;
                Jump();
                Invoke(nameof(ResetJump), _jumpCooldown);
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _jumpHeld = true;
            }

            if (context.canceled)
            {
                _jumpHeld = false;
            }
        }

        public bool IsJumping => !_readyToJump;

        private void Jump()
        {
            // reset y velocity
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);

            _rigidbody.AddRelativeForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

        private void ResetJump()
        {
            _readyToJump = true;
        }
    }
}