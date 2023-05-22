using System.Collections;
using Combat;
using Heroes.Player;
using Misc;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hud
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] [Required] [ShowAssetPreview]
        private Image _bar;

        [SerializeField] [Required] [ShowAssetPreview]
        private TextMeshProUGUI _text;

        [SerializeField] [Required] [ShowAssetPreview]
        private GameObject _objectToRotate;

        [SerializeField]
        private bool _isHud;

        private Camera _camera;
        private float _currentHealth;
        private float _maxHealth;

        private void OnValidate()
        {
            this.CheckIfNull(_bar);
            this.CheckIfNull(_text);
            this.CheckIfNull(_objectToRotate);
        }

        private void Start()
        {
            StartCoroutine(TryGetCamera());
        }

        private void Update()
        {
            if (_isHud is false && _camera != null)
            {
                _objectToRotate.transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
            }
        }

        public void UpdateCurrentHealth(Component component, object data)
        {
            if (ShouldUpdate(component) is false) return;

            _currentHealth = (float) data;
            UpdateHealthBard();
        }

        public void UpdateMaxHealth(Component component, object data)
        {
            if (ShouldUpdate(component) is false) return;

            _maxHealth = (float) data;
            UpdateHealthBard();
        }

        private bool ShouldUpdate(Component component)
        {
            return (_isHud && component.TryGetComponent(out Player _)) || component == GetComponentInParent<Health>();
        }

        private void UpdateHealthBard()
        {
            _bar.fillAmount = _currentHealth / _maxHealth;

            if (_text != null)
            {
                _text.text = $"{Mathf.RoundToInt(_currentHealth)} / {Mathf.RoundToInt(_maxHealth)}";
            }
        }

        private IEnumerator TryGetCamera()
        {
            while (_camera == null)
            {
                _camera = Camera.main;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}