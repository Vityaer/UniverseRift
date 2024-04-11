using DG.Tweening;
using Models.Common;
using System;
using UniRx;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace UIController.LoadingUI
{
    public class StartLoadingController : UiController<StartLoadingView>, IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposable = new();
        private readonly CommonGameData _commonGameData;

        private bool _isOpen;
        private Tween _tween;

        public StartLoadingController(CommonGameData commonGameData)
        {
            _commonGameData = commonGameData;
        }

        public void Initialize()
        {
            View.LoadingSlider.value = 0f;
            _commonGameData.OnStartLoadData.Subscribe(_ => OnStartDownload()).AddTo(_disposable);
            _commonGameData.OnFinishLoadData.Subscribe(_ => OnFinishDownLoad()).AddTo(_disposable);
        }

        public void OnStartDownload()
        {
            if (_isOpen)
                return;

            _isOpen = true;
            Show();
        }

        protected override void Show()
        {
            base.Show();
            View.LoadingText.text = "Loading account data...";
            _tween.Kill();
            _tween = View.LoadingSlider.DOValue(1f, View.AnimationTime);
        }

        private void OnFinishDownLoad()
        {
            Hide();
        }

        protected override void Hide()
        {
            View.LoadingText.text = "Loading complete!";
            _tween.Kill();
            var time = (1f - View.LoadingSlider.value) * View.AnimationFinishTime;
            _tween  = DOTween.Sequence()
                .Append(View.LoadingSlider.DOValue(1f, time).OnComplete(() => base.Hide()));
        }

        public void Dispose()
        {
            _tween.Kill();
            _disposable?.Dispose();
        }

    }
}