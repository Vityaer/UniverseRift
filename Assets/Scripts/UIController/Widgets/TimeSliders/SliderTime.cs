using Cysharp.Threading.Tasks;
using LocalizationSystems;
using Services.TimeLocalizeServices;
using System;
using System.Collections;
using System.Threading;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils.AsyncUtils;
using VContainer;
using VContainerUi.Abstraction;

namespace UIController
{
    public class SliderTime : UiView
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Image fillImage;

        private DateTime finishTime;
        private DateTime startTime;
        public TextMeshProUGUI textTime;
        public TypeSliderTime typeSlider = TypeSliderTime.Remainder;
        public Color lowValue, fillValue;
        private int waitSeconds = 0;
        private DateTime deltaTime;
        private TimeSpan interval;
        private TimeSpan generalInterval;
        private float secondsInterval = 1f;
        private Coroutine coroutineTimer;
        private bool isSetData = false;
        private bool _isFinish = false;

        private TimeLocalizeService _timeLocalizeService;
        private ILocalizationSystem _localizationSystem;
        private CancellationTokenSource _cancellationTokenSource;

        private ReactiveCommand _onTimerFinish = new();
        
        public ReactiveCommand OnTimerFinish => _onTimerFinish;

        [Inject]
        private void Construct(ILocalizationSystem localizationSystem, TimeLocalizeService timeLocalizeService)
        {
            _localizationSystem = localizationSystem;
            _timeLocalizeService = timeLocalizeService;
            _onTimerFinish ??= new ReactiveCommand();
        }

        public void Init(ILocalizationSystem localizationSystem, TimeLocalizeService timeLocalizeService)
        {
            _localizationSystem = localizationSystem;
            _timeLocalizeService = timeLocalizeService;
            _onTimerFinish ??= new ReactiveCommand();
        }

        public void ChangeValue()
        {
            var t = 0f;
            switch (typeSlider)
            {
                case TypeSliderTime.Remainder:
                    interval = generalInterval - (DateTime.UtcNow - startTime);
                    waitSeconds = (int)interval.TotalSeconds;
                    t = waitSeconds / secondsInterval;
                    if (waitSeconds <= 0)
                        SetFinish();
                    break;

                case TypeSliderTime.Accumulation:
                    interval = DateTime.UtcNow - startTime;
                    waitSeconds = (int)interval.TotalSeconds;
                    t = waitSeconds / secondsInterval;
                    if (t >= 1)
                    {
                        interval = generalInterval;
                        SetFinish();
                    }
                    else
                    {
                        _isFinish = false;
                    }
                    break;
            }

            if (!_isFinish)
            {
                fillImage.color = Color.Lerp(lowValue, fillValue, t);
                slider.value = t;
                textTime.text = _timeLocalizeService.TimeSpanConvertToSmallString(interval);
            }
        }

        public void SetMaxValue(TimeSpan requireTime)
        {
            textTime.text = _timeLocalizeService.TimeSpanConvertToSmallString(requireTime);
        }

        public void SetData(DateTime startTime, TimeSpan requireTime)
        {
            this.startTime = startTime;
            finishTime = startTime + requireTime;
            generalInterval = requireTime;
            secondsInterval = (float)requireTime.TotalSeconds;
            ChangeValue();
            isSetData = true;
            _isFinish = false;
            if (_cancellationTokenSource != null)
            {
                StopTimer();
            }
            if (gameObject.activeInHierarchy)
                StartTimer();
        }

        private void StartTimer()
        {
            if (!_isFinish)
            {
                StopTimer();
                _cancellationTokenSource = new();
                CoroutineTimer(_cancellationTokenSource.Token).Forget();
            }
        }
        public void StopTimer()
        {
            _cancellationTokenSource.TryCancel();
            _cancellationTokenSource = null;
        }
        public void SetInfo(string str)
        {
            textTime.text = str;
        }

        public void SetFinish()
        {
            if (!_isFinish)
            {
                _isFinish = true;
                StopTimer();
                textTime.text = _localizationSystem.GetString("DoneLabel");
                OnTimerFinish.Execute();
            }
        }

        private async UniTaskVoid CoroutineTimer(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                ChangeValue();
                var delay = GetSecondForUpdateTimer(interval);
                await UniTask.Delay(Mathf.RoundToInt(delay * 1000), cancellationToken: token);
            }
        }

        void OnEnable()
        {
            if (isSetData && gameObject.activeInHierarchy)
                StartTimer();
        }

        void OnDisable()
        {
            StopTimer();
        }

        private void OnDestroy()
        {
            StopTimer();
        }

        private float GetSecondForUpdateTimer(TimeSpan interval)
        {
            float result = 1f;
            switch (typeSlider)
            {
                //TODO: можно использовать TotalSeconds
                case TypeSliderTime.Remainder:
                    if (interval.Days > 0)
                    {
                        result = interval.Hours * 3600 + interval.Minutes * 60 + interval.Seconds;
                    }
                    else if (interval.Hours > 0)
                    {
                        result = interval.Minutes * 60 + interval.Seconds;
                    }
                    else if (interval.Minutes > 0)
                    {
                        result = interval.Seconds;
                    }
                    else
                    {
                        result = 1f;
                    }
                    break;
                case TypeSliderTime.Accumulation:
                    if (interval.Days > 0)
                    {
                        result = 3600 - (interval.Minutes * 60 + interval.Seconds);
                    }
                    else if (interval.Hours > 0)
                    {
                        result = 60 - interval.Seconds;
                    }
                    else
                    {
                        result = 1f;
                    }
                    break;
            }
            return result;
        }
    }
}