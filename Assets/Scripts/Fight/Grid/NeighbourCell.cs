using System.Collections.Generic;
using UnityEngine;
using Models.Heroes;
using Fight.HeroControllers.Generals;
#if UNITY_EDITOR_WIN
using UnityEditor;
#endif

namespace Fight.Grid
{
    [System.Serializable]
    public class NeighbourCell
    {
        public CellDirectionType direction;
        [SerializeField] private HexagonCell cell;

        public HexagonCell Cell { get => cell; }
        public bool achievableMove { get => cell.achievableMove; }
        public bool available { get => cell.available; }
        public HeroController GetHero { get => cell.Hero; }
        public void CheckMove(int step) { cell.CheckMove(step); }
        public NeighbourCell(HexagonCell mainCell, HexagonCell neighbourCell)
        {
            cell = neighbourCell;
            direction = GetDirection(mainCell, neighbourCell);
#if UNITY_EDITOR_WIN
            Undo.RecordObject(cell, "fill neighbourCell");
#endif
        }
        public void FindWay(HexagonCell previousCell, HexagonCell target, TypeMovement typeMovement = TypeMovement.Ground, int step = 1) { cell.FindWay(previousCell, target, typeMovement, step); }
        public static CellDirectionType GetDirection(HexagonCell mainCell, HexagonCell neighbourCell)
        {
            CellDirectionType direction = CellDirectionType.UpLeft;
            float deltaX = neighbourCell.Position.x - mainCell.Position.x;
            float deltaZ = neighbourCell.Position.z - mainCell.Position.z;
            if (deltaX > 0)
            {
                if (deltaZ > 0.1f)
                {
                    direction = CellDirectionType.UpRight;
                }
                else if (deltaZ < -0.1f)
                {
                    direction = CellDirectionType.BottomRight;
                }
                else
                {
                    direction = CellDirectionType.Right;
                }
            }
            else
            {
                if (deltaZ > 0.1f)
                {
                    direction = CellDirectionType.UpLeft;
                }
                else if (deltaZ < -0.1f)
                {
                    direction = CellDirectionType.BottomLeft;
                }
                else
                {
                    direction = CellDirectionType.Left;
                }
            }
            return direction;
        }
        private static List<CellDirectionType> sampleDirections = new List<CellDirectionType>()
        {
            CellDirectionType.UpRight,
            CellDirectionType.Right,
            CellDirectionType.BottomRight,
            CellDirectionType.BottomLeft,
            CellDirectionType.Left,
            CellDirectionType.UpLeft
        };

        private static int currentNumDirection = 0, leftNumDirection = 0, rightNumDirection = 0;
        private static NeighbourCell currentNeighbourCell = null;
        public static void GetNeighboursOnLevelNear(List<NeighbourCell> neighbours, CellDirectionType direction, int levelNear, List<NeighbourCell> result)
        {
            result.Clear();
            currentNumDirection = sampleDirections.IndexOf(direction);
            leftNumDirection = currentNumDirection - levelNear;
            if (leftNumDirection < 0) leftNumDirection = 6 + leftNumDirection;
            rightNumDirection = currentNumDirection + levelNear;
            if (rightNumDirection >= 6) rightNumDirection = rightNumDirection - 6;
            currentNeighbourCell = neighbours.Find(x => x.direction == sampleDirections[leftNumDirection]);
            if (currentNeighbourCell != null) result.Add(currentNeighbourCell);
            if (rightNumDirection != leftNumDirection)
            {
                currentNeighbourCell = neighbours.Find(x => x.direction == sampleDirections[rightNumDirection]);
                if (currentNeighbourCell != null) result.Add(currentNeighbourCell);
            }
        }
    }

}