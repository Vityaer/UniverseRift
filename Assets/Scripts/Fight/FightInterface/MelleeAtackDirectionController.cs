using DG.Tweening;
using Fight.Grid;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fight.FightInterface
{
    public class MelleeAtackDirectionController : MonoBehaviour
    {
        public RectTransform RectContainer;
        
        public RectTransform PanelDirectionAttack;
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
            var direction = (CellDirectionType)numDirection;
            if (_actionOnSelectDirection != null)
            {
                _actionOnSelectDirection?.Invoke(direction);
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
            PanelDirectionAttack.gameObject.SetActive(true);
            _currentNeighbours = neighbours;
            var world = Camera.main.WorldToScreenPoint(cell.Position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                RectContainer,
                world,
                null,
                out var uiPositionResult
                );

            PanelDirectionAttack.anchoredPosition = uiPositionResult;

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

            PanelDirectionAttack.gameObject.SetActive(false);
        }
    }
}