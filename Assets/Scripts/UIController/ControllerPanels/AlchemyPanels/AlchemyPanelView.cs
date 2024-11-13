using City.Buildings.Abstractions;
using Common.Resourses;
using Models.Data.Inventories;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.ControllerPanels.AlchemyPanels
{
    public class AlchemyPanelView : BaseBuildingView
    {
        [field: SerializeField] public Button AlchemyButton { get; private set; }
        public SliderTime SliderTimeAlchemy;
    }
}
