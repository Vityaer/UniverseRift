using DG.Tweening;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.ButtonsInCity
{
    public class MainMenuButton : MonoBehaviour, IDisposable
    {
        private const float STRETCH_P0WER = 1.1f;
        private const float ANIMATION_TIME = 0.25f;

        [SerializeField] private Image _background;
        [SerializeField] private RectTransform _self;
        [SerializeField] private Vector2 _startSize;
        [SerializeField] private Color _colorSelected;
        [SerializeField] private Color colorUnSelected;
        [SerializeField] private Button _button;

        private IDisposable _disposable;
        private Sequence _sequence;

        public ReactiveCommand OnClick = new ReactiveCommand();

        void Awake()
        {
            //_disposable = _button.OnClickAsObservable().Subscribe(_ => OnClick.Execute());
            _startSize = _self.localScale;
        }

        public void Select()
        {
            _sequence.Kill();
            _sequence = DOTween.Sequence()
                            .Append(_self.DOScale(_startSize * STRETCH_P0WER, ANIMATION_TIME))
                            .Insert(0f, _background.DOColor(_colorSelected, ANIMATION_TIME));
        }

        public void UnSelect()
        {
            _sequence.Kill();
            _sequence = DOTween.Sequence()
                .Append(_self.DOScale(_startSize, ANIMATION_TIME))
                .Insert(0f, _background.DOColor(colorUnSelected, ANIMATION_TIME));
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}