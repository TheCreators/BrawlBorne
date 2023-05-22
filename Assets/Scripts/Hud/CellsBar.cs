using System.Collections.Generic;
using Heroes.Player;
using UnityEngine;

namespace Hud
{
    public class CellsBar : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _cells;

        public void SetCellsBar(Component component, object data)
        {
            if (component.GetComponentInParent<Player>() == null) return;

            int cells = (int) data;
            for (int i = 0; i < _cells.Count; i++)
            {
                _cells[i].SetActive(i < cells);
            }
        }
    }
}