using Plugins.VContainerUi.Runtime.Abstraction;
using UnityEngine;
using VContainerUi.Abstraction;

namespace City.Panels.TutorialTouchPanels
{
    public class TutorialTouchController : UiController<TutorialTouchView>
    {
        public void ShowTouch(RectTransform rectTransform, Vector3 offset = default(Vector3))
        {
            Vector3[] viewCorners = new Vector3[4];
            rectTransform.GetWorldCorners(viewCorners);
            
            Vector2 bottomLeftPoint = new Vector2(viewCorners[0].x, viewCorners[0].y);
            Vector2 topRightPoint = new Vector2(viewCorners[2].x, viewCorners[2].y);

            Vector3 point = (bottomLeftPoint + topRightPoint) / 2;
            ShowTouch(point, point);
        }
        
        public void ShowTouch(Vector3 startDragPoint, Vector3 finishDragPoint)
        {
            View.gameObject.SetActive(true);
            View.ShowDragObject(startDragPoint, finishDragPoint);
        }

        public void HideTouch()
        {
            View.StopTouch();
            View.gameObject.SetActive(false);
        }
    }
}