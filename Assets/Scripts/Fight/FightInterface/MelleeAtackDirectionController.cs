using Fight.Grid;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fight.FightInterface
{
    public class MelleeAtackDirectionController : MonoBehaviour
    {
        public GameObject PanelDirectionAttack;
        public List<MelleeAttackUI> ListDirections = new();

        private List<NeighbourCell> _currentNeighbours = new();
        private event Action<CellDirectionType> _actionOnSelectDirection;

        public void RegisterOnSelectDirection(Action<CellDirectionType> callback, HexagonCell cell, List<NeighbourCell> neighbours)
        {
            _actionOnSelectDirection += callback;
            Open(cell, neighbours);
        }

        public void AttackDirectionSelect(int numDirection)
        {
            CellDirectionType direction = (CellDirectionType)numDirection;
            if (_actionOnSelectDirection != null)
            {
                _actionOnSelectDirection?.Invoke((CellDirectionType)numDirection);
                _actionOnSelectDirection = null;
            }
            Close();
        }

        public void UnregisterOnSelectDirection(Action<CellDirectionType> callback)
        {
            _actionOnSelectDirection -= callback;
            Close();
        }

        private void Open(HexagonCell cell, List<NeighbourCell> neighbours)
        {
            PanelDirectionAttack.SetActive(true);
            _currentNeighbours = neighbours;
            PanelDirectionAttack.transform.position = cell.Position;
            foreach (var neighbour in neighbours)
            {
                if (neighbour.achievableMove && neighbour.Cell.available)
                {
                    ListDirections.Find(x => x.direction == neighbour.direction).Open();
                }
            }
        }

        private void Close()
        {
            foreach (var directionUI in ListDirections)
                directionUI.Hide();

            PanelDirectionAttack.SetActive(false);
        }
    }
}