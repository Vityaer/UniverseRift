using Common;
using Common.Resourses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.ItemVisual
{
    public class ItemSliderController : MonoBehaviour
    {

        public Slider slider;
        public TextMeshProUGUI textSlider;

        void Awake()
        {
            if (slider == null) GetComponents();
        }

        public void SetAmount(int currentAmount, int maxAmount)
        {
            if (slider == null) GetComponents();
            slider.maxValue = maxAmount;
            slider.value = currentAmount;
            textSlider.text = FunctionHelp.AmountFromRequireCount(currentAmount, maxAmount);
            Show();
        }

        public void SetAmount(BigDigit currentAmount, BigDigit maxAmount)
        {
            if (slider == null) GetComponents();
            slider.maxValue = 1f;
            slider.value = (currentAmount / maxAmount).ToFloat();
            textSlider.text = FunctionHelp.AmountFromRequireCount(currentAmount, maxAmount);
            Show();
        }

        public void SetAmount(Resource currentResource, Resource maxResource)
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
    }
}