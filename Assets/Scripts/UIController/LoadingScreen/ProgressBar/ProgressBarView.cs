using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace Ui.LoadingScreen.ProgressBar
{
    public class ProgressBarView : UiView
    {
        public GameObject Panel;
        public TextMeshProUGUI Percent;
        public Slider ProgressBar;
        public RectTransform SliderFill;
    }
}