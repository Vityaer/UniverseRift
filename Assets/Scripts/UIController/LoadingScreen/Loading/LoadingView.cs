using DG.Tweening;
using UnityEngine;
using VContainerUi.Abstraction;

namespace Ui.LoadingScreen.Loading
{
    public class LoadingView : UiView
    {
        public CanvasGroup CanvasGroup;
        public float FadeTime;
        public GameObject LoadingRotateObject;

        private Tween _tween;

        protected override void OnHide()
        {
            _tween.Kill();
            LoadingRotateObject.SetActive(false);
            _tween = DOTween.Sequence()
                .Append(CanvasGroup.DOFade(0f, FadeTime).SetEase(Ease.Linear).OnComplete(() => base.OnHide()));
        }

        protected override void OnDestroy()
        {
            _tween.Kill();
        }
    }
}