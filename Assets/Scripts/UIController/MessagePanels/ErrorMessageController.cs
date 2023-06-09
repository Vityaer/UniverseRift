using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UIController.MessagePanels
{
    public class ErrorMessageController : MonoBehaviour
    {
        public TextMeshProUGUI textComponent;
        public RectTransform panel;
        public CanvasGroup canvas;
        Tween sequenceAnimation;
        Vector2 startPos, finishPos;
        public Vector2 delta = new Vector3(0, 3);
        public float timeShow = 0.2f, timeMove = 1f, timeHide = 0.3f;

        void Start()
        {
            panel.gameObject.SetActive(false);
            startPos = panel.anchoredPosition;
            finishPos = panel.anchoredPosition + delta;
        }

        public void ShowMessage(string errorText)
        {
            //prepare for animatiom
            panel.anchoredPosition = startPos;
            canvas.alpha = 0f;
            textComponent.text = errorText;
            if (sequenceAnimation != null) sequenceAnimation.Kill();
            panel.gameObject.SetActive(true);

            //animation
            sequenceAnimation = DOTween.Sequence()
                        .Append(panel.DOAnchorPos(finishPos, timeMove))
                        .Insert(0f, canvas.DOFade(1f, timeShow))
                        .Insert(timeMove - timeHide, canvas.DOFade(0f, timeHide).OnComplete(() => panel.gameObject.SetActive(false)));
        }

        [ContextMenu("Finish position")]
        public void SetFinishPosition()
        {
            panel.anchoredPosition = panel.anchoredPosition + delta;
        }

        [ContextMenu("Start position")]
        public void SetStartPosition()
        {
            panel.anchoredPosition = panel.anchoredPosition - delta;
        }
    }
}