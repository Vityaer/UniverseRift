using System.Collections.Generic;
using City.Buildings.CityButtons.EventAgent;
using City.Panels.SubjectPanels.Common;
using Common.Db.CommonDictionaries;
using DG.Tweening;
using TMPro;
using UIController;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace City.Panels.BatllepasPanels
{
    public class BattlepasRewardView : ScrollableUiView<GameBattlepasReward>
    {
        [SerializeField] private GameObject _donePanel;
        [SerializeField] private RewardUIController _rewardController;
        [SerializeField] private TMP_Text NumberText;
        [SerializeField] private RectTransform _rect;
        
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Dictionary<ScrollableViewStatus, float> _alphas = new();
        [SerializeField] private float _animationFadeTime;
        
        [Header("Open animation")]
        [SerializeField] private float _openScale;
        [SerializeField] private float _animationFirstStageOpenTime;
        [SerializeField] private float _animationSecondStageOpenTime;
        [SerializeField] private Ease _animationOpenEase;

        [Inject] private CommonDictionaries _commonDictionaries;

        private Tween _tweenFade;
        private Tween _tweenOpen;

        [Inject]
        private void Construct(SubjectDetailController subjectDetailController)
        {
            _rewardController.SetDetailsController(subjectDetailController);
        }

        public override void SetData(GameBattlepasReward data, ScrollRect scrollRect)
        {
            Data = data;
            Scroll = scrollRect;
            _rewardController.ShowReward(data.RewardModel, _commonDictionaries);
            NumberText.text = $"{transform.GetSiblingIndex()}";
        }

        public void OpenWithAnimation()
        {
            _tweenOpen.Kill();
            _tweenOpen = DOTween.Sequence()
                .Append(_rect.DOScale(_openScale, _animationFirstStageOpenTime))
                .Append(_rect.DOScale(1, _animationSecondStageOpenTime))
                .SetEase(_animationOpenEase);

            SetStatus(ScrollableViewStatus.Open);
        }

        public override void SetStatus(ScrollableViewStatus status)
        {
            if (_alphas.TryGetValue(status, out float alpha))
            {
                _tweenFade.Kill();
                _tweenFade = _canvasGroup.DOFade(alpha, _animationFadeTime);
            }

            switch (status)
            {
                case ScrollableViewStatus.Completed:
                case ScrollableViewStatus.Close:
                    _donePanel.SetActive(true);
                    break;
                case ScrollableViewStatus.Open:
                    _donePanel.SetActive(false);
                    break;
            }
        }

        public override void Dispose()
        {
            _tweenOpen.Kill();
            _tweenFade.Kill();
            base.Dispose();
        }
    }
}
