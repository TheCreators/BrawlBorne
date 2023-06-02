using System;
using NaughtyAttributes;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class MinMaxRange
    {
        [SerializeField] [MinMaxSlider(0, 200)]
        private Vector2 _range = new(50, 100);

        public float Min => _range.x;
        public float Max => _range.y;
    }
}