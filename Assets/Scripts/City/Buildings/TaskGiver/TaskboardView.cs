using City.Buildings.Abstractions;
using City.TrainCamp;
using System.Collections.Generic;
using UIController;
using UIController.Buttons;
using UIController.Observers;
using UnityEngine;

namespace City.Buildings.TaskGiver
{
    public class TaskboardView : BaseBuildingView
    {
        public RectTransform Content;
        public TaskController Prefab;
        public ButtonWithObserverResource BuySimpleTaskButton;
        public ButtonWithObserverResource BuySpecialTaskButton;
        public ButtonWithObserverResource BuyReplacementButton;
    }
}
