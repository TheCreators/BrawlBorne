using TMPro;
using UnityEngine;

namespace Hud
{
    public class EnemiesLeftText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void SetEnemiesLeft(Component component, object data)
        {
            int enemiesLeft = (int) data;
            _text.text = $"{enemiesLeft - 1}";
        }
    }
}