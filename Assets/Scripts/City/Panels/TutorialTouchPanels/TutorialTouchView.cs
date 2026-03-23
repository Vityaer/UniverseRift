using AssetKits.ParticleImage;
using DG.Tweening;
using UnityEngine;
using VContainerUi.Abstraction;

namespace City.Panels.TutorialTouchPanels
{
    public class TutorialTouchView : UiView
    {
        [SerializeField] private RectTransform m_fingerTransform;

        [SerializeField] private  float m_fingerDragTime = 0.6f;
        [SerializeField] private  float m_fingerTouchTime = 0.9f;
        [SerializeField] private  float m_fingerTouchInterval = 0.8f;

        [SerializeField] private  Vector3 m_rotateTouch = new Vector3(0, 0, -15);
        [SerializeField] private  Vector3 m_deltaPositionTouch = new Vector3(-100, 100, 0);

        [SerializeField] private ParticleImage m_particleImage;
        
        private Sequence m_tweenSequence;

        private void Awake()
        {
            m_fingerTransform.gameObject.SetActive(false);
        }

        public void ShowDragObject(Vector3 startDragPoint, Vector3 finishDragPoint)
        {
            m_fingerTransform.gameObject.SetActive(true);

            startDragPoint += m_deltaPositionTouch;
            finishDragPoint += m_deltaPositionTouch;

            m_fingerTransform.position = startDragPoint;
            m_fingerTransform.rotation = Quaternion.identity;

            m_tweenSequence.Kill();
            m_tweenSequence = DOTween.Sequence()
                .Append(m_fingerTransform.DOLocalRotate(m_rotateTouch, m_fingerTouchTime))
                .AppendInterval(m_fingerTouchInterval)
                .Append(m_fingerTransform.DOMove(finishDragPoint, m_fingerDragTime))
                .AppendInterval(m_fingerTouchInterval)
                .Append(m_fingerTransform.DOLocalRotate(Vector3.zero, m_fingerTouchTime))
                .Append(m_fingerTransform.DOMove(startDragPoint, m_fingerDragTime))
                .SetLoops(-1);
        }
        
        public void ShowTouchObject(Vector3 point)
        {
            m_fingerTransform.gameObject.SetActive(true);

            m_fingerTransform.position = point;
            m_fingerTransform.rotation = Quaternion.identity;

            m_tweenSequence.Kill();
            m_tweenSequence = DOTween.Sequence()
                .Append(m_fingerTransform.DOLocalRotate(m_rotateTouch, m_fingerTouchTime))
                .AppendInterval(m_fingerTouchInterval)
                .Append(m_fingerTransform.DOLocalRotate(Vector3.zero, m_fingerTouchTime).OnComplete(() => m_particleImage.Play()))
                .AppendInterval(m_fingerTouchTime)
                .OnComplete(() => m_particleImage.Stop())
                .SetLoops(-1);
        }

        public void HideDragObject()
        {
            m_tweenSequence.Kill();
            m_fingerTransform.gameObject.SetActive(false);
        }

        private new void OnDestroy()
        {
            m_tweenSequence.Kill();
        }

        public void StopTouch()
        {
            m_tweenSequence.Kill();
        }
    }
}