using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interface
{
    public class Crosshair : MonoBehaviour
    {
        [Header("Requirements")]
        [SerializeField] private Texture2D _crosshair;

        private void OnGUI() 
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - 10, Screen.height / 2, 20, 20), _crosshair);
        }
    }
}
