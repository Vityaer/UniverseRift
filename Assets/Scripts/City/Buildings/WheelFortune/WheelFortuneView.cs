using City.Buildings.Abstractions;
using System.Collections.Generic;
using UIController;
using UIController.ItemVisual;
using UnityEngine;

namespace City.Buildings.WheelFortune
{
    public class WheelFortuneView : BaseBuildingView
    {
        [field: SerializeField] public ButtonCostController OneRotateButton { get; private set; }
        [field: SerializeField] public ButtonCostController ManyRotateButton { get; private set; }
        [field: SerializeField] public RectTransform Arrow { get; private set; }

        [SerializeField] private List<SubjectCell> _rewardCells;

        public IReadOnlyList<SubjectCell> RewardCells => _rewardCells;

    }
}
