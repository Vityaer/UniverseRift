using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace Ui.LoadingScreen.ProgressBar
{
    public class ProgressBarView : UiView
    {
        public TextMeshProUGUI Percent;
        public Slider ProgressBar;
        public RectTransform SliderFill;
    }
}