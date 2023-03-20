using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ultimates
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(GroundChecker))]
    public class FlyUltimate : MonoBehaviour
    {
        [SerializeField] [Range(0, 10)] private float _ascendDuration = 1.5f;
        [SerializeField] [Range(0, 15)] private float _flyDuration = 10f;

        [SerializeField] [Range(10, 50)] private float _ascendPower = 20f;
        [SerializeField] [Range(0, 0.2f)] private float _ascendCoefficient = 0.113f;
        [SerializeField] [Range(0, 0.2f)] private float _flyCoefficient = 0.0945f;

        private CharacterController _controller;
        private GroundChecker _groundChecker;

        private const float Gravity = -9.81f;

        private Vector3 _direction;
        private float _ascendSpeed;

        private bool _isFlying = false;
        private bool _isAscending = false;
        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _groundChecker = GetComponent<GroundChecker>();
        }
        void Update()
        {
            if (_isAscending || _isFlying)
            {
                ApplyGravity();
                _controller.Move(_direction * Time.deltaTime);
            }
        }

        public void OnUltimate(InputAction.CallbackContext context)
        {
            if (context.performed is false || _groundChecker.IsGrounded is false)
            {
                return;
            }
            _isAscending = true;
            _ascendSpeed = Mathf.Sqrt(_ascendPower * -2.0f * Gravity);
            _direction.y = 0.05f * _ascendSpeed;
            StartCoroutine(AscendingTime());
        }

        private IEnumerator AscendingTime()
        {
            yield return new WaitForSeconds(_ascendDuration);
            _isAscending = false;

            _isFlying = true;
            yield return new WaitForSeconds(_flyDuration);
            _isFlying = false;
        }

        private void ApplyGravity()
        {
            if (_isAscending)
            {
                _direction.y += _ascendCoefficient * _ascendSpeed * -Gravity * Time.deltaTime;
            }
            if (_isFlying)
            {
                _direction.y += _flyCoefficient * _ascendSpeed * -Gravity * Time.deltaTime;
            }
        }
    }
}