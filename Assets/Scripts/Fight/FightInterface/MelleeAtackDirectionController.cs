using Fight.Grid;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fight.FightInterface
{
    public class MelleeAtackDirectionController : MonoBehaviour
    {
        public GameObject PanelDirectionAttack;
        public List<MelleeAttackUI> ListDirections = new List<MelleeAttackUI>();
        private Action<CellDirectionType> _actionOnSelectDirection;
        private List<NeighbourCell> _currentNeighbours = new List<NeighbourCell>();

        public void AttackDirectionSelect(int numDirection)
        {
            CellDirectionType direction = (CellDirectionType)numDirection;
            Debug.Log(direction.ToString());
            Debug.Log(_currentNeighbours.Find(x => x.direction == direction)?.Cell.gameObject.name);
            if (_actionOnSelectDirection != null)
            {
                _actionOnSelectDirection((CellDirectionType)numDirection);
                _actionOnSelectDirection = null;
            }
            Close();
        }

        private void Open(HexagonCell cell, List<NeighbourCell> neighbours)
        {
            PanelDirectionAttack.SetActive(true);
            _currentNeighbours = neighbours;
            PanelDirectionAttack.transform.position = cell.Position;
            foreach (NeighbourCell neighbour in neighbours)
            {
                if (neighbour.achievableMove && neighbour.Cell.available)
                {
                    ListDirections.Find(x => x.direction == neighbour.direction).Open();
                }
            }
        }

        private void Close()
        {
            foreach (MelleeAttackUI directionUI in ListDirections)
                directionUI.Hide();
            PanelDirectionAttack.SetActive(false);
        }

        public void RegisterOnSelectDirection(Action<CellDirectionType> d, HexagonCell cell, List<NeighbourCell> neighbours)
        {
            _actionOnSelectDirection += d;
            Open(cell, neighbours);
        }

        public void UnregisterOnSelectDirection(Action<CellDirectionType> d)
        {
            _actionOnSelectDirection -= d; Close();
        }
    }
}