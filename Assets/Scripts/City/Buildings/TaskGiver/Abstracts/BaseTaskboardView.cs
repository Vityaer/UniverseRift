using City.Buildings.Abstractions;
using UIController.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.TaskGiver.Abstracts
{
    public class BaseTaskboardView : BaseBuildingView
    {
        public ScrollRect Scroll;
        public RectTransform Content;
        public TaskController Prefab;
    }
}