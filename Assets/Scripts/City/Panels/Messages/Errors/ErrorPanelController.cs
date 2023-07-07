using DG.Tweening;
using UnityEngine;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace City.Panels.Messages.Errors
{
    public class ErrorPanelController : UiController<ErrorPanelView>, IInitializable
    {
        public Vector2 delta = new Vector3(0, 3);
        public float timeShow = 0.2f;
        public float timeMove = 1f;
        public float timeHide = 0.3f;

        private Tween sequenceAnimation;
        private Vector2 startPos, finishPos;

        public void Initialize()
        {
            View.PanelRect.gameObject.SetActive(false);
            startPos = View.PanelRect.anchoredPosition;
            finishPos = View.PanelRect.anchoredPosition + delta;
        }

        public void ShowMessage(string errorText)
        {
            //prepare for animatiom
            View.PanelRect.anchoredPosition = startPos;
            View.Canvas.alpha = 0f;
            View.Text.text = errorText;
            View.PanelRect.gameObject.SetActive(true);

            sequenceAnimation.Kill();
            sequenceAnimation = DOTween.Sequence()
                        .Append(View.PanelRect.DOAnchorPos(finishPos, timeMove))
                        .Insert(0f, View.Canvas.DOFade(1f, timeShow))
                        .Insert(timeMove - timeHide, View.Canvas.DOFade(0f, timeHide).OnComplete(() => View.PanelRect.gameObject.SetActive(false)));
        }

        [ContextMenu("Finish position")]
        public void SetFinishPosition()
        {
            View.PanelRect.anchoredPosition = View.PanelRect.anchoredPosition + delta;
        }

        [ContextMenu("Start position")]
        public void SetStartPosition()
        {
            View.PanelRect.anchoredPosition = View.PanelRect.anchoredPosition - delta;
        }
    }
}