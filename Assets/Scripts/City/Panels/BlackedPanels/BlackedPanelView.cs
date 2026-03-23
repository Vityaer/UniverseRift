using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Ui.Misc.Widgets;
using UnityEngine;

namespace City.Panels.BlackedPanels
{
    public class BlackedPanelView : BasePanel
    {
        public Vector2 DefaultViewOffset;
        [SerializeField] private Dictionary<BlackImageSide, DimmedImage> m_dimmedImages = new Dictionary<BlackImageSide, DimmedImage>();
        
        private Vector3[] m_mainCorners = new Vector3[4];
        private Vector2 m_mainSize = new Vector2();
        
        protected override void Awake()
        {
            (transform as RectTransform)!.GetWorldCorners(m_mainCorners);
            m_mainSize = new Vector2(m_mainCorners[3].x - m_mainCorners[0].x,
                m_mainCorners[1].y - m_mainCorners[0].y);
        }

        public async UniTask SelectArea(Vector2 bottomLeftPoint, Vector2 topRightPoint)
        {
            gameObject.SetActive(true);
            float topAnchor = (topRightPoint.y - m_mainCorners[0].y) / m_mainSize.y;
            float rightAnchor = (topRightPoint.x - m_mainCorners[0].x) / m_mainSize.x;
            float bottomAnchor = (bottomLeftPoint.y - m_mainCorners[0].y) / m_mainSize.y;
            float leftAnchor = (bottomLeftPoint.x - m_mainCorners[0].x) / m_mainSize.x;

            UniTask[] tasks = new UniTask[]
            {
                m_dimmedImages[BlackImageSide.Top].SetAnchors(new Vector2(0f, topAnchor), Vector2.one),
                m_dimmedImages[BlackImageSide.Right].SetAnchors(new Vector2(rightAnchor, bottomAnchor), new Vector2(1f, topAnchor)),
                m_dimmedImages[BlackImageSide.Bottom].SetAnchors(Vector2.zero, new Vector2(1f, bottomAnchor)),
                m_dimmedImages[BlackImageSide.Left].SetAnchors(new Vector2(0f, bottomAnchor), new Vector2(leftAnchor, topAnchor)),
            };

            await UniTask.WhenAll(tasks);
        }
        
        private enum BlackImageSide
        {
            LeftTop = 0,
            Top = 1,
            RightTop = 2,
            Right = 3,
            RightBottom = 4,
            Bottom = 5,
            LeftBottom = 6,
            Left = 7,
        }
    }
}