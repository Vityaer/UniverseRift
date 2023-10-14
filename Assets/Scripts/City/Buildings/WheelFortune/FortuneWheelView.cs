using City.Buildings.Abstractions;
using System.Collections.Generic;
using UIController;
using UIController.ItemVisual;
using UnityEngine;

namespace City.Buildings.WheelFortune
{
    public class FortuneWheelView : BaseBuildingView
    {
        [field: SerializeField] public ButtonCostController OneRotateButton { get; private set; }
        [field: SerializeField] public ButtonCostController ManyRotateButton { get; private set; }
        [field: SerializeField] public ButtonCostController RefreshWheelButton { get; private set; }
        [field: SerializeField] public RectTransform Arrow { get; private set; }

        [SerializeField] private List<SubjectCell> _rewardCells;

        public List<SubjectCell> RewardCells => _rewardCells;
    }
}
