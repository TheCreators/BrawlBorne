using Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Hud
{
    [RequireComponent(typeof(Health))]
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _hpBar;

        private Health _health;

        private void Start()
        {
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            _hpBar.fillAmount = 100;
        }
    }
}
