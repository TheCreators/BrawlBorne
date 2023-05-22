using System;
using NaughtyAttributes;
using UnityEngine;

namespace Models
{
    [Serializable]
    public struct ColoredMinMaxRange
    {
        [SerializeField] [MinMaxSlider(0, 200)]
        private Vector2 _range;

        [SerializeField]
        private Color _minColor;

        [SerializeField]
        private Color _maxColor;

        public float Min => _range.x;
        public float Max => _range.y;
        public Color MinColor => _minColor;
        public Color MaxColor => _maxColor;
    }
}