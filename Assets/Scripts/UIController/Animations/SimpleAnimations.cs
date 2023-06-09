using DG.Tweening;
using UnityEngine;

namespace UIController.Animations
{
    public class SimpleAnimations : MonoBehaviour
    {
        public RectTransform rect;
        public float time = 0.1f;

        private Vector2 startSize = new Vector2(1f, 1f);
        private float minSize = 0.95f, maxSize = 1.05f;
        private Tween sequenceScalePulse;

        void Start()
        {
            if (rect == null)
                rect = GetComponent<RectTransform>();
        }

        public void ScalePulse()
        {
            if (sequenceScalePulse != null) sequenceScalePulse.Kill();
            sequenceScalePulse = DOTween.Sequence()
                            .Append(rect.DOScale(startSize * minSize, time))
                            .Append(rect.DOScale(startSize, time));
        }
        public void Squezze()
        {
            if (sequenceScalePulse != null) sequenceScalePulse.Kill();
            sequenceScalePulse = DOTween.Sequence().Append(rect.DOScale(startSize * minSize, time));

        }
        public void SquezzeToDefault()
        {
            if (sequenceScalePulse != null) sequenceScalePulse.Kill();
            sequenceScalePulse = DOTween.Sequence().Append(rect.DOScale(startSize, time));
        }
        public void Expansion()
        {
            if (sequenceScalePulse != null) sequenceScalePulse.Kill();
            sequenceScalePulse = DOTween.Sequence().Append(rect.DOScale(startSize * maxSize, time));
        }
        public void ExpansionToDefault()
        {
            if (sequenceScalePulse != null) sequenceScalePulse.Kill();
            sequenceScalePulse = DOTween.Sequence().Append(rect.DOScale(startSize, time));
        }
    }
}