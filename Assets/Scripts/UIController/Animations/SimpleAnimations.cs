using DG.Tweening;
using UnityEngine;

namespace UIController.Animations
{
    public class SimpleAnimations : MonoBehaviour
    {
        public RectTransform rect;
        public float time = 0.1f;

        private readonly Vector2 m_startSize = new Vector2(1f, 1f);
        private const float MinSize = 0.95f;
        private readonly float m_maxSize = 1.05f;
        private Tween m_sequenceScalePulse;

        void Start()
        {
            if (rect == null)
                rect = GetComponent<RectTransform>();
        }

        public void ScalePulse()
        {
            m_sequenceScalePulse.Kill();
            m_sequenceScalePulse = DOTween.Sequence()
                            .Append(rect.DOScale(m_startSize * MinSize, time))
                            .Append(rect.DOScale(m_startSize, time));
        }
        public void Squezze()
        {
            m_sequenceScalePulse.Kill();
            m_sequenceScalePulse = DOTween.Sequence().Append(rect.DOScale(m_startSize * MinSize, time));

        }
        public void ToDefaultSize()
        {
            m_sequenceScalePulse.Kill();
            m_sequenceScalePulse = DOTween.Sequence().Append(rect.DOScale(m_startSize, time));
        }
        public void Expansion()
        {
            m_sequenceScalePulse.Kill();
            m_sequenceScalePulse = DOTween.Sequence().Append(rect.DOScale(m_startSize * m_maxSize, time));
        }

        private void OnDestroy()
        {
            m_sequenceScalePulse.Kill();
        }
    }
}