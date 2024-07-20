using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UIController.Misc.CustomComponents
{
    public class GridOverrider : MonoBehaviour
    {
        private float MIN_HORIZONTAL_SPACING = 0f;
        [SerializeField] private List<GridLayoutGroup> _grids = new();

        private void Start()
        {
            RecalculateGridSize();
        }

        //[Button("RecalculateGridSpacing")]
        public void RecalculateGridSpacing()
        {
            Debug.Log($"Screen.width: {Screen.width}");
            foreach (var grid in _grids)
            {
                var calcScreenWidth = Screen.width / transform.root.localScale.x;
                var rectComponent = grid.GetComponent<RectTransform>();

                calcScreenWidth = Mathf.Abs(rectComponent.rect.width);
                var cellSize = grid.cellSize;
                var cellCount = grid.constraintCount;
                var allSpacingWidth = calcScreenWidth - cellCount * cellSize.x - grid.padding.horizontal;
                var spacing = grid.spacing;
                var calcSpacingX = allSpacingWidth / (cellCount - 1);
                if (calcSpacingX >= MIN_HORIZONTAL_SPACING)
                {
                    calcSpacingX = Mathf.Clamp(calcSpacingX, MIN_HORIZONTAL_SPACING, grid.spacing.x);
                    spacing.x = calcSpacingX;
                }
                else
                {
                    spacing.x = MIN_HORIZONTAL_SPACING;
                    var generalLack = MIN_HORIZONTAL_SPACING * (cellCount - 1) - allSpacingWidth;
                    var scale = (cellSize.x - (generalLack / cellCount)) / cellSize.x;
                    cellSize *= scale;
                    grid.cellSize = cellSize;
                }

                grid.spacing = spacing;
                rectComponent.Reset();
            }
        }

        [Button("RecalculateGridSize")]
        public void RecalculateGridSize()
        {
            foreach (var grid in _grids)
            {
                var rectComponent = grid.GetComponent<RectTransform>();
                var calcScreenWidth = Mathf.Abs(rectComponent.rect.width);

                var cellSize = grid.cellSize;
                var cellCount = grid.constraintCount;
                var spacing = grid.spacing;
                var allWorkWidth = calcScreenWidth - grid.padding.horizontal - (cellCount - 1) * spacing.x;

                var newCellSizeWidth = allWorkWidth / cellCount;
                var scale =  newCellSizeWidth / cellSize.x;

                cellSize *= scale;
                grid.cellSize = cellSize;
                rectComponent.Reset();
            }
        }
    }
}
