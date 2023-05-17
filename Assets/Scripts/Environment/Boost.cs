using Combat;
using Events;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;

namespace Environment
{
    public class Boost : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _healthIncreasePercent = 30f;
        [SerializeField] private GameEvent _onCollected;
        
        private void OnValidate()
        {
            this.CheckIfNull(_onCollected);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Health health))
            {
                health.IncreaseMaxHealth(_healthIncreasePercent);
                _onCollected.Raise(this, null);
            }
        }
    }
}
