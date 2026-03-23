using DG.Tweening;
using LocalizationSystems;
using Models.Common;
using System;
using Plugins.VContainerUi.Runtime.Abstraction;
using UniRx;
using UnityEngine;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace UIController.LoadingUI
{
    public class StartLoadingController : UiController<StartLoadingView>, IInitializable, IDisposable
    {
        private readonly CompositeDisposable m_disposable = new();
        private readonly CommonGameData m_commonGameData;
        private readonly ILocalizationSystem m_localizationSystem;

        private bool m_isOpen;
        private Tween m_tween;
        private Tween m_rotateTween;

        public ReactiveCommand OnLoaded { get; } = new();

        public StartLoadingController(CommonGameData commonGameData, ILocalizationSystem localizationSystem)
        {
            m_commonGameData = commonGameData;
            m_localizationSystem = localizationSystem;
        }

        public void Initialize()
        {
            View.LoadingSlider.value = 0f;
            m_commonGameData.OnStartLoadData.Subscribe(_ => OnStartDownload()).AddTo(m_disposable);
            m_commonGameData.OnFinishLoadData.Subscribe(_ => OnFinishDownLoad()).AddTo(m_disposable);
        }

        private void OnStartDownload()
        {
            if (m_isOpen)
                return;

            m_isOpen = true;
            Show();
        }

        protected override void Show()
        {
            base.Show();
            View.LoadingText.StringReference = m_localizationSystem.GetLocalizedContainer("LoadingAccountDataInProgressText");
            m_tween.Kill();
            m_tween = View.LoadingSlider.DOValue(1f, View.AnimationTime);

            m_rotateTween.Kill();
            m_rotateTween = DOTween.Sequence()
                .Append
                (
                    View.RotateImageRect.DORotate
                    (
                        new Vector3(0, 0, -360),
                        View.RotateAnimationTime,
                        RotateMode.FastBeyond360
                    )
                    .SetEase(Ease.Linear)
                )
                .SetLoops(-1);
        }

        private void OnFinishDownLoad()
        {
            Hide();
        }

        protected override void Hide()
        {
            View.LoadingText.StringReference = m_localizationSystem.GetLocalizedContainer("LoadingAccountDataCompleteText");
            m_tween.Kill();
            var time = (1f - View.LoadingSlider.value) * View.AnimationFinishTime;
            m_tween  = DOTween.Sequence()
                .Append(View.LoadingSlider.DOValue(1f, time).OnComplete(() => OnFinishHide()));
        }

        private void OnFinishHide()
        {
            OnLoaded.Execute();
            base.Hide();
        }

        public void Dispose()
        {
            m_rotateTween.Kill();
            m_tween.Kill();
            m_disposable?.Dispose();
        }

    }
}