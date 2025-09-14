using System.Collections.Generic;
using City.Buildings.TaskGiver.Abstracts;
using UIController.Buttons;
using UnityEngine.UI;

namespace City.Buildings.TaskGiver
{
    public class TaskboardView : BaseTaskboardView
    {
        public Button NoFilterTasksButton;
        public Button LowRatingFilterTasksButton;
        public Button HighRatingFilterTasksButton;
        public ButtonWithObserverResource BuySimpleTaskButton;
        public ButtonWithObserverResource BuySpecialTaskButton;
        public ButtonWithObserverResource BuyReplacementButton;
    }
}