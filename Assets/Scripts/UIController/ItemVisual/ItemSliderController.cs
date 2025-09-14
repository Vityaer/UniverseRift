using Common.Resourses;
using DG.Tweening;
using Models.Common.BigDigits;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.ItemVisual
{
    public class ItemSliderController : MonoBehaviour
    {
        public Slider slider;
        public TextMeshProUGUI textSlider;

        [SerializeField] private float _fillSliderTime;
        [SerializeField] private float _doneScaleTime;
        [SerializeField] private float _doneScaleValue;
        [SerializeField] private RectTransform _rectTransform;
        
        private Tween _sliderTween;
        private Tween _doneTween;
        
        void Awake()
        {
            if (slider == null) GetComponents();
        }

        public void SetAmount(int currentAmount, int maxAmount)
        {
            if (slider == null) GetComponents();
            slider.maxValue = maxAmount;
            _sliderTween.Kill();
            _sliderTween = slider.DOValue(currentAmount, _fillSliderTime).SetEase(Ease.Linear);
            textSlider.text = FunctionHelp.AmountFromRequireCount(currentAmount, maxAmount);
            Show();
        }

        public void SetAmount(BigDigit currentAmount, BigDigit maxAmount)
        {
            if (slider == null) GetComponents();
            slider.maxValue = 1f;
            var targetValue = (currentAmount / maxAmount).ToFloat();
            _sliderTween = slider.DOValue(targetValue, _fillSliderTime).SetEase(Ease.Linear);
            textSlider.text = FunctionHelp.AmountFromRequireCount(currentAmount, maxAmount);
            Show();
        }

        public void SetAmount(GameResource currentResource, GameResource maxResource)
        {
            SetAmount(currentResource.Amount, maxResource.Amount);
        }

        void GetComponents()
        {
            slider = GetComponent<Slider>();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        void Show()
        {
            gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _sliderTween.Kill();
            _doneTween.Kill();
        }

        public void ShowDone()
        {
            _doneTween.Kill();
            _doneTween = _rectTransform.DOScale(_doneScaleValue, _doneScaleTime)
                .SetLoops(-1, LoopType.Yoyo);
        }
        
        public void HideDone()
        {
            _doneTween.Kill();
        }
    }
}