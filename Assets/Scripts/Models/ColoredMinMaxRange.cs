using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class ColoredMinMaxRange : MinMaxRange
    {
        [SerializeField]
        private Color _minColor = Color.black;

        [SerializeField]
        private Color _maxColor = Color.black;
        
        public Color MinColor => _minColor;
        public Color MaxColor => _maxColor;
    }
}