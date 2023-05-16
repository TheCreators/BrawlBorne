﻿using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Hud
{
    public class UltimateBar : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _objectToToggle;

        public void Start()
        {
            TurnOff();
        }

        public void MakeVisible(Component component, object data)
        {
            if (component.TryGetComponent(out PlayerCombat _))
            {
                TurnOn();
            }
        }
        
        public void MakeInvisible(Component component, object data)
        {
            if (component.TryGetComponent(out PlayerCombat _))
            {
                TurnOff();
            }
        }
        
        private void TurnOff()
        {
            foreach (GameObject objectToToggle in _objectToToggle)
            {
                objectToToggle.SetActive(false);
            }
        }
        
        private void TurnOn()
        {
            foreach (GameObject objectToToggle in _objectToToggle)
            {
                objectToToggle.SetActive(true);
            }
        }
    }
}