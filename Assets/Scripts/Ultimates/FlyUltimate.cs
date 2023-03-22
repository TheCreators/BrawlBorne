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

        private CharacterController _controller;
        private GroundChecker _groundChecker;

        private const float Gravity = -9.81f;
        private const float AscendCoefficient = 0.113f;
        private const float FlyCoefficient = 0.0945f;

        private float _startSpeed;
        private float _flyingSpeed;

        private bool _isFlying = false;
        private bool _isAscending = false;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _groundChecker = GetComponent<GroundChecker>();
        }

        private void Update()
        {
            if (_isAscending || _isFlying)
            {
                ApplyGravity();
                _controller.Move(Vector3.up * _flyingSpeed * Time.deltaTime);
            }
        }

        public void OnUltimate(InputAction.CallbackContext context)
        {
            if (context.performed is false || _groundChecker.IsGrounded is false)
            {
                return;
            }
            _isAscending = true;
            _startSpeed = Mathf.Sqrt(_ascendPower * -2.0f * Gravity);
            _flyingSpeed = 0.05f * _startSpeed;
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
                _flyingSpeed += AscendCoefficient * _startSpeed * -Gravity * Time.deltaTime;
            }
            if (_isFlying)
            {
                _flyingSpeed += FlyCoefficient * _startSpeed * -Gravity * Time.deltaTime;
            }
        }
    }
}