using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UIController
{
    public class TimeSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Image fillImage;
        [SerializeField] private Color _lowValue;
        [SerializeField] private Color _fillValue;

        public float MaxValue = 1f;
        private Tween _sequenceChangeValue;
        private Action _observerMaxFill;

        public void SetData(float curValue, float maxValue)
        {
            SetMaxValue(maxValue);
            ChangeValue(curValue);
        }

        public void ChangeValue(float value)
        {
            if (MaxValue > 0)
            {
                var t = value / MaxValue;
                _sequenceChangeValue.Kill();
                _sequenceChangeValue = DOTween.Sequence()
                .Append(slider.DOValue(t, 0.5f))
                .Join(fillImage.DOColor(Color.Lerp(_lowValue, _fillValue, t), 0.5f).OnComplete(() => CheckMaxFill(value)));
            }
        }

        private void CheckMaxFill(float value)
        {
            if (value >= MaxValue)
            {
                OnFillMaxSlider();
            }
        }

        public void SetMaxValue(float maxValue)
        {
            if (gameObject.activeSelf == false) gameObject.SetActive(true);
            this.MaxValue = maxValue;
            ChangeValue(this.MaxValue);
        }

        public void Death()
        {
            _sequenceChangeValue.Kill();
            _sequenceChangeValue = DOTween.Sequence()
                    .Append(slider.DOValue(0f, 0.5f))
                    .Join(fillImage.DOColor(_lowValue, 0.5f).OnComplete(OffSlider));
        }

        public void OffSlider()
        {
            gameObject.SetActive(false);
        }

        public void RegisterOnFillSliderInMax(Action d) { _observerMaxFill += d; }
        public void UnregisterOnFillSliderInMax(Action d) { _observerMaxFill -= d; }
        private void OnFillMaxSlider() { if (_observerMaxFill != null) _observerMaxFill(); }

        private void OnDestroy()
        {
            _sequenceChangeValue.Kill();
        }
    }
}