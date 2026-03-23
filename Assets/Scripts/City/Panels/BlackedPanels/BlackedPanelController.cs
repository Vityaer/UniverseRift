using City.Panels.ScreenBlockers;
using Cysharp.Threading.Tasks;
using UiExtensions.Panels;
using UnityEngine;
using VContainer;

namespace City.Panels.BlackedPanels
{
    public class BlackedPanelController : UiPanelController<BlackedPanelView>
    {
        [Inject] private readonly ScreenBlockerController m_screenBlockerController;

        public async UniTask  SelectView(RectTransform view)
        {
            await SelectView(view, View.DefaultViewOffset);
        }
        
        public async UniTask SelectView(RectTransform view, Vector2 offset)
        {
            Vector3[] viewCorners = new Vector3[4];
            view.GetWorldCorners(viewCorners);
            
            Vector2 bottomLeftPoint = new Vector2(viewCorners[0].x, viewCorners[0].y) - offset;
            Vector2 topRightPoint = new Vector2(viewCorners[2].x, viewCorners[2].y) + offset;
            
            await InternalSelectArea(bottomLeftPoint, topRightPoint);
        }
        


        public async UniTask  SelectAreaAroundPoint(Vector2 point, Vector2 size)
        {
            Vector2 bottomLeftPoint = point - size / 2;
            Vector2 topRightPoint = point + size / 2;
            
            await InternalSelectArea(bottomLeftPoint, topRightPoint);
        }

        public void HideSelect()
        {
            View.gameObject.SetActive(false);
        }
        
        private async UniTask InternalSelectArea(Vector2 bottomLeftPoint, Vector2 topRightPoint)
        {
            m_screenBlockerController.Open();
            await View.SelectArea(bottomLeftPoint, topRightPoint);
            m_screenBlockerController.Close();
        }
    }
}