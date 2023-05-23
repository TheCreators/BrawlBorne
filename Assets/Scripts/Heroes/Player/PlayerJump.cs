using Events;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Heroes.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(GroundChecker))]
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Min(0)]
        private float _jumpForce = 8f;

        [SerializeField] [BoxGroup(Group.Settings)] [Min(0)]
        private float _jumpCooldown = 0.5f;

        [SerializeField] [BoxGroup(Group.Events)]
        private GameEvent _onJump;

        private bool _readyToJump = true;
        private bool _jumpHeld;

        private Rigidbody _rigidbody;
        private GroundChecker _groundChecker;

        private void OnValidate()
        {
            this.CheckIfNull(_onJump);
        }

        private void Awake()
        {
            _rigidbody = this.GetComponentWithNullCheck<Rigidbody>();
            _groundChecker = this.GetComponentWithNullCheck<GroundChecker>();
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