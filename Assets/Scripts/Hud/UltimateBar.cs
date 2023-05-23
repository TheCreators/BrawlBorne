using System.Collections.Generic;
using Heroes.Player;
using UnityEngine;

namespace Hud
{
    public class UltimateBar : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _objectToToggle;

        public void Start()
        {
            TurnOff();
        }

        public void MakeVisible(Component component, object data)
        {
            if (component.GetComponentInParent<Player>() != null)
            {
                TurnOn();
            }
        }

        public void MakeInvisible(Component component, object data)
        {
            if (component.GetComponentInParent<Player>() != null)
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