using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ui.Helpers
{
    public abstract class ButtonWithAnimationHelper : BaseButtonHelper, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] protected float AnimTime = 0.1f;
        [SerializeField] protected RectTransform UiRect;

        protected Sequence TweenSequence;

        public virtual void OnPointerDown(PointerEventData eventData)
        {
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
        }

        protected override void GetComponents()
        {
            UiRect = GetComponent<RectTransform>();
            base.GetComponents();
        }

        private void OnDestroy()
        {
            TweenSequence.Kill();
        }
    }
}