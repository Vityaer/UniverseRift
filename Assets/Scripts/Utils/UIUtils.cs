using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public static class UIUtils
    {
        public static void Reset(this RectTransform rect)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        }

        public static void Reset(this List<RectTransform> rects)
        {
            foreach (var rect in rects)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            }
        }
    }
}