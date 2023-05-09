using System.Collections;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ultimates
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(GroundChecker))]
    public class FlyingUltimate : Ultimate
    {
        [SerializeField, Range(5, 50)] private float _ascendSpeed = 10f;
        [SerializeField, Range(0, 5)] private float _ascendDuration = 1f;
        [SerializeField, Range(0, 15)] private float _flyDuration = 5f;

        private Rigidbody _rigidbody;
        private GroundChecker _groundChecker;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _groundChecker = GetComponent<GroundChecker>();
        }
        
        public override void Use()
        {
            if (_groundChecker.IsGrounded is true)
            {
                StartCoroutine(FlyingRoutine());
            }
        }

        public void OnUltimate(InputAction.CallbackContext context)
        {
            if (context.performed is false || _groundChecker.IsGrounded is false)
            {
                return;
            }
            StartCoroutine(FlyingRoutine());
        }

        private IEnumerator FlyingRoutine()
        {
            ChangeAscendingSpeed(_ascendSpeed);
            yield return new WaitForSeconds(_ascendDuration);
            _rigidbody.useGravity = false;
            ChangeAscendingSpeed(0f);
            yield return new WaitForSeconds(_flyDuration);
            _rigidbody.useGravity = true;
        }

        private void ChangeAscendingSpeed(float speed)
        {
            var velocity = _rigidbody.velocity;
            velocity = new Vector3(velocity.x, speed, velocity.z);
            _rigidbody.velocity = velocity;
        }
    }
}
