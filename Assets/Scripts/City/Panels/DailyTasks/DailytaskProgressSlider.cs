using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.CityButtons.EventAgent
{
    public class DailytaskProgressSlider : MonoBehaviour
    {
        private const int START_DELAY = 1000;
        private const float ANIMATION_SPEED = 1f;

        [SerializeField] private Slider _slider;

        private int _startValue;
        private int _targetValue;
        private int _maxValue;
        private ReactiveCommand<int> _onFillReward = new ReactiveCommand<int>();
        private CancellationTokenSource _cancellationTokenSource;

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
            await UniTask.Delay(START_DELAY, cancellationToken: cancellationToken);
            var t = 0f;

            while (t <= 1f)
            {
                t += Time.deltaTime * ANIMATION_SPEED;
                var value = Mathf.Lerp(_startValue, targetValue, t) / _maxValue;
                ChangeSliderValue(value);
                await UniTask.Yield(cancellationToken: cancellationToken);
            }

            _startValue = targetValue;
        }

        private void ChangeSliderValue(float value)
        {
            _slider.value = value;
        }

        private void OnDisable()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }

            _startValue = _targetValue;
            ChangeSliderValue(_startValue * 1f / _maxValue);
        }
    }
}