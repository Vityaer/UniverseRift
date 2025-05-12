using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using TMPro;

namespace Utils
{
    public static class TweenUtils
    {

        public static TweenerCore<int, int, NoOptions> DOCounter(
         this TMP_Text target, int fromValue, int endValue, float duration, IFormatProvider formatProvider = null
             )
        {
            int v = fromValue;
            TweenerCore<int, int, NoOptions> t = DOTween.To(() => v, x =>
            {
                v = x;
                target.text = formatProvider != null ? String.Format(formatProvider, "{0:BD}", v) : v.ToString();
            }, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        
        public static TweenerCore<float, float, FloatOptions> DOCounter(
         this TMP_Text target, float fromValue, float endValue, float duration)
        {
            float v = fromValue;

            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => v, x =>
            {
                v = x;
                target.text = v.ToString("N1");
            }, endValue, duration);

            t.SetTarget(target);
            return t;
        }
    }
}