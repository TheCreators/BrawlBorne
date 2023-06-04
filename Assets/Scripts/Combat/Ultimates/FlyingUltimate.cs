using System.Collections;
using Misc;
using Models;
using NaughtyAttributes;
using UnityEngine;

namespace Ultimates
{
    public class FlyingUltimate : Ultimate
    {
        [SerializeField] [BoxGroup(Group.Settings)] [Range(5, 50)]
        private float _ascendSpeed = 10f;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(0, 5)]
        private float _ascendDuration = 1f;

        [SerializeField] [BoxGroup(Group.Settings)] [Range(0, 15)]
        private float _flyDuration = 5f;

        private Rigidbody _rigidbody;
        private GroundChecker _groundChecker;

        private void Awake()
        {
            _rigidbody = this.GetComponentInParentWithNullCheck<Rigidbody>();
            _groundChecker = this.GetComponentInParentWithNullCheck<GroundChecker>();
        }

        protected override void Use()
        {
            StartCoroutine(FlyingRoutine());
        }

        protected override bool CanBeUsedExtraCondition => _groundChecker.IsGrounded;

        private IEnumerator FlyingRoutine()
        {
            ChangeAscendingSpeed(_ascendSpeed);
            yield return new WaitForSeconds(_ascendDuration);

            _rigidbody.useGravity = false;
            ChangeAscendingSpeed(0f);
            yield return new WaitForSeconds(_flyDuration);
            _rigidbody.useGravity = true;

            Invoke(nameof(SetCanBeUsedToTrue), _cooldown);
        }

        private void ChangeAscendingSpeed(float speed)
        {
            var velocity = _rigidbody.velocity;
            velocity = new Vector3(velocity.x, speed, velocity.z);

            _rigidbody.velocity = velocity;
        }
    }
}