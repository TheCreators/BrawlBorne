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
        
        private void OnValidate()
        {
            this.CheckIfNull(_bar);
            this.CheckIfNull(_text);
        }

        public void SetHealthBar(Component component, object data) 
        {
            if (component.TryGetComponent(out PlayerMovement _))
            {
                HealthAmount healthAmount = (HealthAmount) data;
                _bar.fillAmount = healthAmount.CurrentHealth / healthAmount.MaxHealth;
                if (_text != null) _text.text = $"{Mathf.RoundToInt(healthAmount.CurrentHealth)} / {Mathf.RoundToInt(healthAmount.MaxHealth)}";
            }
        }
    }
}
