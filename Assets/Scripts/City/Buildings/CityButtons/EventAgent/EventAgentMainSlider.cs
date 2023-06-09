using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.CityButtons.EventAgent
{
    public class EventAgentMainSlider : MonoBehaviour
    {
        public Slider slider;
        private float startValue = 0f;
        public void SetValue(float value)
        {
            startValue = value;
            slider.value = value;
        }
        float targetValue = 0f;
        public void SetNewValueWithAnim(float targetValue)
        {
            this.targetValue = targetValue;
            coroutineFillMainSliderAmount = StartCoroutine(IFillMainSliderAmount(startValue, targetValue));
        }
        void OnDisable()
        {
            if (coroutineFillMainSliderAmount != null)
            {
                StopCoroutine(coroutineFillMainSliderAmount);
                coroutineFillMainSliderAmount = null;
            }
            startValue = targetValue;
            slider.value = targetValue;
        }
        Coroutine coroutineFillMainSliderAmount = null;
        IEnumerator IFillMainSliderAmount(float startValue, float targetValue)
        {
            yield return new WaitForSeconds(1f);
            float t = 0;
            while (t <= 1)
            {
                slider.value = Mathf.Lerp(startValue, targetValue, t);
                t += Time.deltaTime;
                yield return null;
            }
            this.startValue = targetValue;
        }
    }
}