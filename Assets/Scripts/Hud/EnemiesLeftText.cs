﻿using Misc;
using TMPro;
using UnityEngine;

namespace Hud
{
    public class EnemiesLeftText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        private void OnValidate()
        {
            this.CheckIfNull(_text);
        }

        public void SetEnemiesLeft(Component component, object data)
        {
            int enemiesLeft = (int) data;
            _text.text = $"{enemiesLeft - 1}";
        }
    }
}