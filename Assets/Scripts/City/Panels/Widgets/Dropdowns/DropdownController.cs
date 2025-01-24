using City.Panels.Widgets.Dropdowns;
using DG.Tweening;
using System;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace City.Panels.Widgets
{
    public class DropdownController : MonoBehaviour
    {
        [SerializeField] private CustomDropdown _dropdown;
        [SerializeField] private RectTransform _rightArrowTransform;
        [SerializeField] private RectTransform _leftArrowTransform;
        [SerializeField] private Vector3 _arrowLeftOpenRotation;
        [SerializeField] private Vector3 _arrowLeftCloseRotation;
        [SerializeField] private Vector3 _arrowRightOpenRotation;
        [SerializeField] private Vector3 _arrowRightCloseRotation;
        [SerializeField] private float _arrowRotationTime;
        [SerializeField] private Button _leftArrowButton;
        [SerializeField] private Button _rightArrowButton;
        [SerializeField] private Ease _rotationEase;
        [SerializeField] private RotateMode _leftArrowRotateOpen;
        [SerializeField] private RotateMode _leftArrowRotateClose;

        private ReactiveCommand<int> _onChangeValue = new();
        private CompositeDisposable _disposable = new();
        private bool _isOpen;
        private Tween _tweenRotate;
        private Tween _tweenView;
        private int _currentIndex;

        public IObservable<int> OnChangeValue => _onChangeValue;
        public int Value => _dropdown.value;
        public TMP_Dropdown Dropdown => _dropdown;
        public Button LeftArrowButton => _leftArrowButton;
        public Button RightArrowButton => _rightArrowButton;

        private void Start()
        {
            _dropdown.onValueChanged.AddListener(ChangeValue);
            _dropdown.OnOpenDropdown += DropdownOnClick;
            _dropdown.OnCloseDropdown += DropdownOnClick;
            _leftArrowButton.OnClickAsObservable().Subscribe(_ => OnLeftArrowButtonClick()).AddTo(_disposable);
            _rightArrowButton.OnClickAsObservable().Subscribe(_ => OnRightArrowButtonClick()).AddTo(_disposable);
            _currentIndex = _dropdown.value;
        }

        public void OnRightArrowButtonClick()
        {
            var newCurrentValue = _dropdown.value;

            newCurrentValue = ChangeIndex(newCurrentValue, -1);

            if (_currentIndex != newCurrentValue)
            {
                _currentIndex = newCurrentValue;
                _dropdown.value = newCurrentValue;
            }
        }

        public void OnLeftArrowButtonClick()
        {
            var newCurrentValue = _dropdown.value;

            newCurrentValue = ChangeIndex(newCurrentValue, 1);

            if (_currentIndex != newCurrentValue)
            {
                _currentIndex = newCurrentValue;
                _dropdown.value = newCurrentValue;
            }
        }

        private int ChangeIndex(int value, int delta)
        {
            value += delta;
            if (value == _dropdown.options.Count)
            {
                value = 0;
            }
            else if (value < 0)
            {
                value = _dropdown.options.Count - 1;
            }
            return value;
        }

        public void SetData(int value)
        {
            _dropdown.onValueChanged.RemoveListener(ChangeValue);
            _dropdown.value = value;
            _dropdown.onValueChanged.AddListener(ChangeValue);
            _currentIndex = value;
        }

        private void DropdownOnClick()
        {
            _isOpen = !_isOpen;
            _tweenRotate.Kill();
            if (_isOpen)
            {
                _tweenRotate = DOTween.Sequence()
                    .Append(_rightArrowTransform.DORotate(_arrowRightOpenRotation, _arrowRotationTime).SetEase(_rotationEase))
                    .Join(_leftArrowTransform.DORotate(_arrowLeftOpenRotation, _arrowRotationTime, _leftArrowRotateOpen).SetEase(_rotationEase))
                    .SetUpdate(true);
            }
            else
            {
                _tweenRotate = DOTween.Sequence()
                    .Append(_rightArrowTransform.DORotate(_arrowRightCloseRotation, _arrowRotationTime).SetEase(_rotationEase))
                    .Join(_leftArrowTransform.DORotate(_arrowLeftCloseRotation, _arrowRotationTime, _leftArrowRotateClose).SetEase(_rotationEase))
                    .SetUpdate(true);
            }
        }

        private void ChangeValue(int value)
        {
            _onChangeValue.Execute(value);
        }

        private void OnDestroy()
        {
            _tweenView.Kill();
            _tweenRotate.Kill();
            _disposable?.Dispose();
        }
    }
}
