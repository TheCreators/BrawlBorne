using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Hud
{
    [RequireComponent(typeof(Image))]
    public class HealthBar : MonoBehaviour
    {
        private Image _hpBar;
        
        private void Awake()
        {
            _hpBar = GetComponent<Image>();
        }

        public void SetHealthBar(Component component, object data) 
        {
            Debug.Log("SetHealthBar");
            if (component.TryGetComponent(out PlayerMovement playerMovement))
            {
                var hpPercentage = (float) data;
                _hpBar.fillAmount = hpPercentage / 100f;
            }
        }
    }
}
