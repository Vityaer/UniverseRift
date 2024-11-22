using DG.Tweening;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UiExtensions.CustomElements
{
    public class AnimateCustomToggle : MonoBehaviour
    {
        [SerializeField] private bool _isOn;
        [SerializeField] private Button _button;

        [SerializeField] private Image _background;
        [SerializeField] private RectTransform _circleRect;

        [Header("Animation")]
        [SerializeField] private RectTransform _selectCirclePoint;
        [SerializeField] private RectTransform _deselectCirclePoint;
        [SerializeField] private Color _selectColor;
        [SerializeField] private Color _deselectColor;
        [SerializeField] private float _animationTime;
        [SerializeField] private Ease _ease;

        private bool _inAnimation = false;
        private bool _canChange = true;
        private Tween _tween;
        private ReactiveCommand<bool> _onChange = new();
        private IDisposable _disposable;

        public bool IsOn => _isOn;
        public IObservable<bool> OnChange => _onChange;

        private void Awake()
        {
            _disposable = _button.OnClickAsObservable().Subscribe(_ => ChangeToggle());
            StartAnimation();
        }

        private void ChangeToggle()
        {
            if (!_canChange)
                return;

            if (_inAnimation)
                return;

            _isOn = !_isOn;
            _onChange.Execute(_isOn);
            StartAnimation();
        }

        private void StartAnimation()
        {
            _tween.Kill();
            _inAnimation = true;

            if (_isOn)
            {
                _tween = DOTween.Sequence()
                    .Append(_background.DOColor(_selectColor, _animationTime))
                    .Join(_circleRect.DOAnchorPos(_selectCirclePoint.position, _animationTime))
                    .SetEase(_ease)
                    .OnComplete(() => _inAnimation = false);
            }
            else
            {
                _tween = DOTween.Sequence()
                    .Append(_background.DOColor(_deselectColor, _animationTime))
                    .Join(_circleRect.DOAnchorPos(_deselectCirclePoint.position, _animationTime))
                    .SetEase(_ease)
                    .OnComplete(() => _inAnimation = false);
            }
        }

        private void OnDestroy()
        {
            _tween.Kill();
            _disposable?.Dispose();
        }
    }
}
