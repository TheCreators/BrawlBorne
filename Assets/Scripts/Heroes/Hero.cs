﻿using Misc;
using NaughtyAttributes;
using UnityEngine;

namespace Heroes
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] [Required]
        private Transform _shootAt;

        [SerializeField]
        private string _name;

        public Vector3 ShootAt => _shootAt.position;

        public string Name => _name;

        private void OnValidate()
        {
            this.CheckIfNull(_shootAt);
            this.CheckIfNull(_name);
        }
    }
}