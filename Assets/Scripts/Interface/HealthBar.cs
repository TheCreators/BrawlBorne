using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Combat;

namespace Interface
{
    [RequireComponent(typeof(Health))]
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _hpBar;

        private Health _health;

        void Start()
        {
            _health = GetComponent<Health>();
        }

        void Update()
        {
            _hpBar.fillAmount = _health.GetHealth / 100;
        }
    }
}
