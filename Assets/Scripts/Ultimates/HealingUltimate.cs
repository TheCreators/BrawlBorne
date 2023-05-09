using Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ultimates
{
    [RequireComponent(typeof(Health))]
    public class HealingUltimate : Ultimate
    {
        [SerializeField, Range(0, 100)] private float _healingPointsPercent = 50f;
        
        private Health _health;
        void Start()
        {
            _health = GetComponent<Health>();
        }

        public override void Use()
        {
            _health.Heal(_healingPointsPercent);
        }
    }
}
