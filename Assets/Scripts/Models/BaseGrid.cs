using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Models.Grid
{
    public class BaseGrid : MonoBehaviour
    {
        private List<HexagonCell> _cells = new List<HexagonCell>(); 
        public List<HexagonCell> Cells => _cells;

        public List<HexagonCell> StartCellsLeftTeam, StartCellsRightTeam;

        private void FindAllCell()
        {
            _cells.Clear();
		    var components = GetComponentsInChildren<HexagonCell>();
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
            foreach(var cell in _cells)
            {
                cell.ClearCanMove();
            }

            gameObject.SetActive(false);
        }
    }
}