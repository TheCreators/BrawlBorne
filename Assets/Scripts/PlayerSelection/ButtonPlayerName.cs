using Misc;
using TMPro;
using UnityEngine;

namespace PlayerSelection
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ButtonPlayerName : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshPro;

        private void Awake()
        {
            _textMeshPro = this.GetComponentWithNullCheck<TextMeshProUGUI>();
        }

        public void SetText(Component component, object data)
        {
            _textMeshPro.text = (string) data;
        }
    }
}