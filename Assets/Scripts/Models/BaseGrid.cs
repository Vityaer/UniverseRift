using Fight.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace Models.Grid
{
    public class BaseGrid : MonoBehaviour
    {
        private List<HexagonCell> _cells = new List<HexagonCell>();

        public List<HexagonCell> StartCellsLeftTeam;
        public List<HexagonCell> StartCellsRightTeam;

        public List<HexagonCell> Cells => _cells;

        public void FindAllCell()
        {
            _cells.Clear();
            var components = GetComponentsInChildren<HexagonCell>();
            for (int i = 0; i < components.Length; i++)
                _cells.Add(components[i]);
        }

        [ContextMenu("FindNeighbours")]
        private void FindNeighbours()
        {
            FindAllCell();
            for (int i = 0; i < _cells.Count; i++)
                _cells[i].ClearNeighbours();

            for (int i = 0; i < _cells.Count - 1; i++)
            {
                for (int j = i + 1; j < _cells.Count; j++)
                {
                    _cells[i].CheckOnNeighbour(_cells[j]);
                    _cells[j].CheckOnNeighbour(_cells[i]);
                }
            }
        }

        public void OpenGrid()
        {
            Debug.Log($"OpenGrid: {gameObject.name}");
            gameObject.SetActive(true);
        }

        public void CloseGrid()
        {
            foreach (var cell in _cells)
            {
                cell.ClearCanMove();
            }

            Debug.Log($"close grid: {gameObject.name}");
            gameObject.SetActive(false);
        }

        public void ClearWays()
        {
            foreach (var cell in _cells)
            {
                if (cell.Hero == null)
                {
                    cell.ClearCanMove();
                }
            }
        }
    }
}