using DG.Tweening;
using System;
using UniRx;
using UnityEngine;
using VContainerUi.Abstraction;

namespace UIController.FadeInOutPanels
{
    public class FadeInOutPanelView : UiView
    {
        [SerializeField] private float _animatinoTime;
        [SerializeField] private Transform _panel;
        [SerializeField] private Transform _firstPoint;
        [SerializeField] private Transform _openPoint;
        [SerializeField] private Transform _secondPoint;

        private ReactiveCommand _onShowAction = new();
        private ReactiveCommand _onHideAction = new();

        public IObservable<Unit> OnShowAction => _onShowAction;
        public ReactiveCommand<Unit> OnHideAction => _onHideAction;

        protected override void OnShow()
        {
            _panel.position = _firstPoint.position;
            TweenSequence.Kill();
            TweenSequence = DOTween.Sequence()
                                .Append(_panel.DOMove(_openPoint.position, _animatinoTime)
                                .OnComplete(() => { base.OnShow(); _onShowAction.Execute(); }));
        }

        protected override void OnHide()
        {
            _panel.position = _openPoint.position;
            TweenSequence.Kill();
            TweenSequence = DOTween.Sequence()
                .Append(_panel.DOMove(_secondPoint.position, _animatinoTime))
                .OnComplete(() => { base.OnHide(); _onHideAction.Execute(); });
        }
    }
}
