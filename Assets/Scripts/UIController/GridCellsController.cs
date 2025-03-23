using UnityEngine;
using UnityEngine.UI;

namespace UIController
{
    public class GridCellsController : MonoBehaviour
    {
        [SerializeField] private float m_koeficient = 1f;
        
        public GridLayoutGroup GridLayoutGroup;

        public void Start()
        {
            CalculateCellSize();
        }

        [ContextMenu("Calculate Cell Size")]
        private void CalculateCellSize()
        {
            var screenWidth = Screen.width;
            var targetColumn = GridLayoutGroup.constraintCount;
            var workWidth = screenWidth - GridLayoutGroup.padding.horizontal -
                            ((targetColumn - 1) * GridLayoutGroup.spacing.x);
            var targetCellWidth = workWidth / targetColumn;
            var ratio = targetCellWidth / GridLayoutGroup.cellSize.x;
            var newSize = GridLayoutGroup.cellSize * ratio * m_koeficient;
            GridLayoutGroup.cellSize = newSize;
        }
    }
}