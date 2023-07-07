using City.Buildings.Abstractions;
using UIController;
using UnityEngine;

namespace City.Buildings.TaskGiver
{
    public class TaskboardView : BaseBuildingView
    {
        public RectTransform Content;
        public TaskController Prefab;
        public ButtonCostController BuySimpleTaskButton;
        public ButtonCostController BuySpecialTaskButton;
        public ButtonCostController BuyReplacementButton;
    }
}
