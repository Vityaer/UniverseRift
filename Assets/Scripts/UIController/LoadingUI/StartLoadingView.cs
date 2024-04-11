using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace UIController.LoadingUI
{
    public class StartLoadingView : UiView
    {
        public TMP_Text LoadingText;
        public Slider LoadingSlider;
        public float AnimationTime;
        public float AnimationFinishTime;
        public CanvasGroup CanvasGroup;
        public float FadeTime;

        private Tween _tween;

        protected override void OnHide()
        {
            _tween.Kill();
            _tween = CanvasGroup.DOFade(0f, FadeTime).SetEase(Ease.Linear)
                .OnComplete(() => base.OnHide());
        }

        protected override void OnDestroy()
        {
            _tween.Kill();
        }
    }
}
