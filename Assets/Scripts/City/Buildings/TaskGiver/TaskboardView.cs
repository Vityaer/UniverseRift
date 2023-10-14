using City.Buildings.Abstractions;
using City.TrainCamp;
using System.Collections.Generic;
using UIController;
using UIController.Buttons;
using UIController.Observers;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.TaskGiver
{
    public class TaskboardView : BaseBuildingView
    {
        public ScrollRect Scroll;
        public RectTransform Content;
        public TaskController Prefab;
        public ButtonWithObserverResource BuySimpleTaskButton;
        public ButtonWithObserverResource BuySpecialTaskButton;
        public ButtonWithObserverResource BuyReplacementButton;
    }
}
