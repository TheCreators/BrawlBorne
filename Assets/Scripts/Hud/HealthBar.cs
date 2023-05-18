using System;
using Combat;
using Misc;
using Models;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hud
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _bar;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private GameObject _objectToRotate;
        [SerializeField] private bool _isHud;
        
        private Camera _camera;
        
        private void OnValidate()
        {
            this.CheckIfNull(_bar);
            this.CheckIfNull(_text);
            this.CheckIfNull(_objectToRotate);
        }
        
        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (_isHud is false)
            {
                _objectToRotate.transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
            }
        }

        public void SetHealthBar(Component component, object data) 
        {
            if ((_isHud && component.TryGetComponent(out PlayerMovement _)) || component == GetComponentInParent<Health>())
            {
                HealthAmount healthAmount = (HealthAmount) data;
                _bar.fillAmount = healthAmount.CurrentHealth / healthAmount.MaxHealth;
                if (_text != null) _text.text = $"{Mathf.RoundToInt(healthAmount.CurrentHealth)} / {Mathf.RoundToInt(healthAmount.MaxHealth)}";
            }
        }
    }
}
