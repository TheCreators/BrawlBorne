using TMPro;
using UnityEngine;

namespace CharacterSelection
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ButtonCharacterName : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshPro;

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        public void SetText(Component component, object data)
        {
            _textMeshPro.text = (string) data;
        }
    }
}