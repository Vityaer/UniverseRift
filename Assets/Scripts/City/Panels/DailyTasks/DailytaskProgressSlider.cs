using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils.AsyncUtils;

namespace City.Buildings.CityButtons.EventAgent
{
    public class DailytaskProgressSlider : MonoBehaviour
    {
        [SerializeField] private int startDelaySeconds = 1000;
        [SerializeField] private float _animationSpeed = 1f;
        [SerializeField] private Slider _slider;
        [SerializeField] private Ease _ease = Ease.OutBounce;

        private int _startValue;
        private int _targetValue;
        private int _maxValue = 3000;
        private ReactiveCommand<int> _onFillReward = new ReactiveCommand<int>();
        private CancellationTokenSource _cancellationTokenSource;
        private Tween _tween;
        
        public IObservable<int> OnFillReward => _onFillReward;

        public void SetValue(int start, int maxValue)
        {
            _startValue = start;
            _maxValue = maxValue;
            _slider.value = _startValue * 1f / maxValue;
        }

        public void SetNewValueWithAnim(int targetValue)
        {
            _targetValue = targetValue;
            _cancellationTokenSource = new CancellationTokenSource();

            FillMainSliderAmount(targetValue, _cancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid FillMainSliderAmount(int targetValue, CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(startDelaySeconds), cancellationToken: cancellationToken);

            _tween.Kill();
            float valFloat = _startValue;
            var nextTransition = GetNextTransion(valFloat);
            _tween = DOTween.To(() => valFloat, x =>
                {
                    valFloat = x;
                    ChangeSliderValue(valFloat / _maxValue);
                    if (valFloat >= nextTransition)
                    {
                        _onFillReward.Execute(nextTransition / 100);
                        nextTransition = GetNextTransion(valFloat);
                    }
                },
                targetValue,
                _animationSpeed)
            .SetEase(_ease)
            .SetSpeedBased(true)
            .OnComplete(() => _startValue = targetValue)
            .OnKill(() => _startValue = targetValue);
        }

        private int GetNextTransion(float valFloat)
        {
            return (int)valFloat - ((int)valFloat) % 100 + 100;
        }

        private void ChangeSliderValue(float value)
        {
            _slider.value = value;
        }

        private void OnDisable()
        {
            _cancellationTokenSource.TryCancel();
            _tween.Kill();

            _startValue = _targetValue;
            ChangeSliderValue(_startValue * 1f / _maxValue);
        }
    }
}