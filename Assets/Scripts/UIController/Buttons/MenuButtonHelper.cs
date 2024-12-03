using DG.Tweening;
using System;
using System.Linq;
using Ui.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.Buttons
{
    public class MenuButtonHelper : ButtonWithAnimationHelper
    {
        [SerializeField] private float BUTTON_SCALE_MULTIPLE = 1.1f;
        [SerializeField] private float BUTTON_ICON_SCALE_MULTIPLE = 1.2f;
        [SerializeField] private float BUTTON_ICON_DEFAULT_HEIGHT = 14f;
        [SerializeField] private float BUTTON_ICON_FINAL_HEIGHT = 70f;

        [SerializeField] private LayoutElement _layoutElement;
        [SerializeField] private RectTransform _iconTransform;

        public void Select()
        {
            TweenSequence.Kill();
            TweenSequence = DOTween.Sequence()
                                .Append(_iconTransform.DOScale(Vector3.one * BUTTON_ICON_SCALE_MULTIPLE, AnimTime))
                                .Insert(0, _iconTransform.transform.DOLocalMoveY(BUTTON_ICON_FINAL_HEIGHT, AnimTime));

            _layoutElement.minWidth = UiRect.rect.width * BUTTON_SCALE_MULTIPLE;
        }

        public void Diselect()
        {

            TweenSequence.Kill();
            TweenSequence = DOTween.Sequence()
                                .Append(_iconTransform.DOScale(Vector3.one, AnimTime))
                                .Insert(0, _iconTransform.transform.DOLocalMoveY(BUTTON_ICON_DEFAULT_HEIGHT, AnimTime));
            _layoutElement.minWidth = 0;
        }

        protected override void GetComponents()
        {
            ButtonComponent = GetComponent<Button>();
            _layoutElement = GetComponent<LayoutElement>();
            base.GetComponents();
        }
    }
}
