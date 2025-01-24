using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UIController.FightUI.SmoothSliders;
using UnityEngine;
using UnityEngine.UI;

namespace UIController
{
    public class SmoothSlider : SerializedMonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _fillImage;
        [SerializeField] private List<ColorByValueContainer> _colors = new();
        [SerializeField] private float _animationSpeed;

        private float _maxValue;
        private Tween _sequenceChangeValue;

        public float MaxValue => _maxValue;

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
                var colorMinIndex = -1;
                for (var i = 0; i < _colors.Count; i++)
                {
                    if (_colors.ElementAt(i).Value < t)
                    {
                        colorMinIndex = i;
                    }
                }
                var colorMaxIndex = Math.Clamp(colorMinIndex + 1, 0, _colors.Count - 1);

                var lowValue = Color.red;
                var fillValue = Color.green;

                if (colorMinIndex >= 0)
                {
                    lowValue = _colors[colorMinIndex].Color;
                    fillValue = _colors[colorMaxIndex].Color;
                }

                _sequenceChangeValue.Kill();
                _sequenceChangeValue = DOTween.Sequence()
                    .Append(_slider.DOValue(t, _animationSpeed))
                    .Join(_fillImage.DOColor(Color.Lerp(lowValue, fillValue, t), _animationSpeed))
                    .SetSpeedBased(true);
            }
        }

        public void SetMaxValue(float maxValue)
        {
            if (gameObject.activeSelf == false) gameObject.SetActive(true);
            _maxValue = maxValue;
            ChangeValue(_maxValue);
        }

        public void Death()
        {
            _sequenceChangeValue.Kill();
            _sequenceChangeValue = DOTween.Sequence()
                    .Append(_slider.DOValue(0f, _animationSpeed))
                    .Join(_fillImage.DOColor(_colors[0].Color, _animationSpeed))
                    .SetSpeedBased(true)
                    .OnComplete(OffSlider);
        }

        private void OffSlider()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _sequenceChangeValue.Kill();
        }
    }
}