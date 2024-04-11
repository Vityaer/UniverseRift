using UnityEngine;
using UnityEngine.UI;

namespace UIController
{
    public class GridCellsController : MonoBehaviour
    {
        public GridLayoutGroup GridLayoutGroup;

        public void Start()
        {
            CalculateCellSize();

        }

        private void CalculateCellSize()
        {
            var screenWidth = Screen.width;
            var targetColumn = GridLayoutGroup.constraintCount;
            var workWidth = screenWidth - GridLayoutGroup.padding.horizontal - ((targetColumn - 1) * GridLayoutGroup.spacing.x);
            var targetCellWidth = workWidth / targetColumn;
            var ratio = targetCellWidth / GridLayoutGroup.cellSize.x;
            var newSize = GridLayoutGroup.cellSize * ratio;
            GridLayoutGroup.cellSize = newSize;
        }
    }
}
