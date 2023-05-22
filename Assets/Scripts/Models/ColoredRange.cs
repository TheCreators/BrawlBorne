using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public struct ColoredRange
    {
        [SerializeField] [Min(0)]
        private float _range;

        [SerializeField]
        private Color _color;

        public float Value => _range;
        public Color Color => _color;
    }
}