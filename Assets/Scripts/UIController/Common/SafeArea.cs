using UnityEngine;

namespace UIController.Common
{
    public class SafeArea : MonoBehaviour
    {
        public RectTransform RootPermanentWindows;
        public RectTransform RootTemporallyWindows;

        private void Awake()
        {
            var rect = GetComponent<RectTransform>();
            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = anchorMin + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rect.anchorMax = anchorMax;
            //TODO: У айфонов есть UnSafe Area и снизу, но это не критично для нашего UI, поэтому снизу оставляем как есть. Если включить, то все нижные кнопки поднимаются вверх и смотриться плохо на айфонах
            //rect.anchorMin = anchorMin;
        }
    }
}
