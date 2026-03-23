using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace City.Panels.BlackedPanels
{
    public class DimmedImage : MonoBehaviour
    {
        [SerializeField] private RectTransform m_rectTransform;
        [SerializeField] private  float m_animationTime;

        private Tween m_tween;

        private CancellationToken m_cancellationToken;
        
        public async UniTask SetAnchors(Vector2 min, Vector2 max, CancellationToken token = default)
        {
            bool done = false;
            m_tween.Kill();
            m_tween =  DOTween.Sequence()
                .Append(m_rectTransform.DOAnchorMin(min, m_animationTime))
                .Join(m_rectTransform.DOAnchorMax(max, m_animationTime))
                .OnComplete(() => done = true);
            
            await UniTask.WaitUntil(() => done, cancellationToken: token);
        }
    }
}