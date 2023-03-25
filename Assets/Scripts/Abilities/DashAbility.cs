using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Abilities
{
    [RequireComponent(typeof(Rigidbody))]
    public class DashAbility : MonoBehaviour
    {
        [SerializeField][Range(0, 25)] private float _dashSpeed = 7f;
        [SerializeField][Range(0, 10)] private float _dashDuration = 0.5f;

        private Rigidbody _rigidbody;

        private bool _isDashing;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Update()
        {
            if (_isDashing)
            {
                Dash();
            }
        }

        public void OnAbility(InputAction.CallbackContext context)
        {
            if (context.performed is false)
            {
                return;
            }

            _isDashing = true;
            StartCoroutine(StopDashingAfter(_dashDuration));
        }

        private IEnumerator StopDashingAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _isDashing = false;
        }

        private void Dash()
        {
            _rigidbody.AddRelativeForce(Vector3.forward * _dashSpeed, ForceMode.Impulse);
        }
    }
}
