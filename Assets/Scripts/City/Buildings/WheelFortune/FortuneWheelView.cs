using City.Buildings.Abstractions;
using System.Collections.Generic;
using DG.Tweening;
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
        public List<RotateMode> RotateModes;
        public Ease EaseMode;
        
        public float EternalRotateSpeed;
        public float RefreshCellDelay;
        
        [SerializeField] private List<SubjectCell> _rewardCells;
        public List<SubjectCell> RewardCells => _rewardCells;


        public float EternalRotateDelay;

        public float ArrowSpeed;


        [Header("Shaking")]
        public RectTransform FortuneWheel;

        public float ShakeDuration;
        public float ShakeStrength;
        public int ShakeVibrato;
        public float ShakeRandomness;
        
        [Header("Scale reward")]
        public float ScaleRewardDuration;
        public float ScaleRewardStrength;
    }
}
