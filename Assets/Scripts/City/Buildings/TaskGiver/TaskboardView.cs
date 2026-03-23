using System.Collections.Generic;
using City.Buildings.TaskGiver.Abstracts;
using UIController;
using UIController.Buttons;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
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
        public SliderTime SliderEventLeftTime;
        public LocalizeStringEvent LeftTasksCountText;
        
        public void SetTaskCount(int arg1, int arg2)
        {
            if(LeftTasksCountText.StringReference.TryGetValue("Count", out var variable))
            {
                var stringVariable = variable as IntVariable;
                stringVariable.Value = arg1;
            }
            
            if(LeftTasksCountText.StringReference.TryGetValue("MaxCount", out var maxVariable))
            {
                var stringVariable = maxVariable as IntVariable;
                stringVariable.Value = arg2;
            }
            
            LeftTasksCountText.RefreshString();
        }
    }
}