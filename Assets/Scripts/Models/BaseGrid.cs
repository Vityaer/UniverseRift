using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Models.Grid
{
    public class BaseGrid : MonoBehaviour
    {
        private List<HexagonCellScript> _cells = new List<HexagonCellScript>(); 
        public List<HexagonCellScript> Cells => _cells;

        public List<HexagonCellScript> StartCellsLeftTeam, StartCellsRightTeam;

        private void FindAllCell()
        {
            _cells.Clear();
		    var components = GetComponentsInChildren<HexagonCellScript>();
		    for(int i = 0; i < components.Length; i++)
			    _cells.Add(components[i]);
	    }

        [ContextMenu("FindNeighbours")]
        public void FindNeighbours()
        {
            FindAllCell();
            for(int i = 0; i < _cells.Count; i++)
                _cells[i].ClearNeighbours();
                
            for(int i = 0; i < _cells.Count - 1; i++)
            {
                for(int j = i + 1; j < _cells.Count; j++)
                {
                    _cells[i].CheckOnNeighbour(_cells[j]);
                    _cells[j].CheckOnNeighbour(_cells[i]);
                }
            }
        }

        public void OpenGrid()
        {
            gameObject.SetActive(true);
        }

        public void CloseGrid()
        {
            gameObject.SetActive(false);
        }
    }
}