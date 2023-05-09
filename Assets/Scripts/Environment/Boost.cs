using Combat;
using UnityEngine;

namespace Environment
{
    public class Boost : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _healthIncreasePrecent = 30f;
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Health health))
            {
                health.IncreaseMaxHealth(_healthIncreasePrecent);
                Destroy(gameObject);
            }
        }
    }
}
