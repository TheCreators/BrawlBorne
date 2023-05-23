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
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Min(0)]
        private float _walkSpeed = 10f;

        [SerializeField] [BoxGroup(Group.Settings)] [Min(0)]
        private float _sneakSpeed = 7f;

        [SerializeField] [BoxGroup(Group.Settings)] [Min(0)]
        private float _groundDrag = 5f;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(0, 1)]
        private float _airSpeedMultiplier = 0.5f;

        [SerializeField] [BoxGroup(Group.Events)] [Required]
        private GameEvent _onMove;

        [SerializeField] [BoxGroup(Group.Events)] [Required]
        private GameEvent _onStopMoving;

        [SerializeField] [BoxGroup(Group.Events)] [Required]
        private GameEvent _onSneak;

        [SerializeField] [BoxGroup(Group.Events)] [Required]
        private GameEvent _onStopSneaking;

        private Rigidbody _rigidbody;
        private GroundChecker _groundChecker;

        private float _moveSpeed;
        private Vector2 _inputMoveDirection;
        private bool _sneakHeld;
        private const int SpeedMultiplier = 10;

        public float WalkSpeed
        {
            get => _walkSpeed;
            set => _walkSpeed = value;
        }

        private void OnValidate()
        {
            this.CheckIfNull(_onMove, _onStopMoving, _onSneak, _onStopSneaking);
        }

        private void Awake()
        {
            _rigidbody = this.GetComponentWithNullCheck<Rigidbody>();
            _groundChecker = this.GetComponentWithNullCheck<GroundChecker>();
        }

        private void Update()
        {
            UpdateRigidbodyDrag();
            UpdateSpeed();
            LimitSpeed();
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _inputMoveDirection = context.ReadValue<Vector2>();

            if (context.performed)
            {
                _onMove.Raise(this, null);
            }
            else if (context.canceled)
            {
                _onStopMoving.Raise(this, null);
            }
        }

        public void OnSneak(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _sneakHeld = true;
                _onSneak.Raise(this, null);
            }
            else if (context.canceled)
            {
                _sneakHeld = false;
                _onStopSneaking.Raise(this, null);
            }
        }

        public Vector2 GetNormalizedRelativeVelocity()
        {
            if (_rigidbody is null)
            {
                return Vector2.zero;
            }

            var forwardVelocity = Vector3.Dot(_rigidbody.velocity, transform.forward);
            var rightVelocity = Vector3.Dot(_rigidbody.velocity, transform.right);
            return new Vector2(forwardVelocity, rightVelocity) / _walkSpeed;
        }

        public bool IsMoving => _inputMoveDirection != Vector2.zero;

        private void UpdateRigidbodyDrag()
        {
            _rigidbody.drag = _groundChecker.IsGrounded ? _groundDrag : 0f;
        }

        private void Move()
        {
            var moveDirection = new Vector3(_inputMoveDirection.x, 0f, _inputMoveDirection.y).normalized;

            _rigidbody.AddRelativeForce(moveDirection * _moveSpeed * SpeedMultiplier * (_groundChecker.IsGrounded ? 1f : _airSpeedMultiplier),
                ForceMode.Force);
        }

        private void LimitSpeed()
        {
            var flatVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

            if (flatVelocity.magnitude > _moveSpeed)
            {
                var limitedVelocity = flatVelocity.normalized * _moveSpeed;
                _rigidbody.velocity = new Vector3(limitedVelocity.x, _rigidbody.velocity.y, limitedVelocity.z);
            }
        }

        private void UpdateSpeed()
        {
            if (_groundChecker.IsGrounded)
            {
                _moveSpeed = _sneakHeld ? _sneakSpeed : _walkSpeed;
            }
        }
    }
}