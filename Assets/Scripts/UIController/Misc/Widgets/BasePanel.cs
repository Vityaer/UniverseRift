using City.Buildings.Abstractions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace Ui.Misc.Widgets
{
    public class BasePanel : BasePopupView
    {
        private const float STRETCH_TIME = 0.1f;
        private const float STRETCH_DEFAULT_TIME = 0.1f;
        private const float SQUEEZE_TIME = 0.1f;
        private const float STRETCH_P0WER = 1.1f;

        public Button CloseButton;
        public Button DimedButton;

        protected override void OnShow()
        {
            transform.SetAsLastSibling();
            TweenSequence.Kill();
            TweenSequence = DOTween.Sequence()
                                .Append(transform.DOScale(Vector3.one * STRETCH_P0WER, STRETCH_TIME))
                                .Append(transform.DOScale(Vector3.one, STRETCH_DEFAULT_TIME));
        }

        protected override void OnHide()
        {
            TweenSequence.Kill();
            TweenSequence = DOTween.Sequence()
                .Append(transform.DOScale(Vector3.one * STRETCH_P0WER, STRETCH_TIME))
                .Append(transform.DOScale(Vector3.zero, SQUEEZE_TIME)).OnComplete(() => base.OnHide());
        }
    }
}