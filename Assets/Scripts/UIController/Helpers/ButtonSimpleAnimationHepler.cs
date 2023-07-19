using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ui.Helpers
{
    public class ButtonSimpleAnimationHepler : ButtonWithAnimationHelper
    {
        [SerializeField] private Vector3 SqueezeScale = Vector3.one;

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (CanWork())
            {
                TweenSequence.Kill();
                TweenSequence = DOTween.Sequence().Append(UiRect.DOScale(SqueezeScale, AnimTime));
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            TweenSequence.Kill();
            TweenSequence = DOTween.Sequence().Append(UiRect.DOScale(Vector3.one, AnimTime));
        }
    }
}