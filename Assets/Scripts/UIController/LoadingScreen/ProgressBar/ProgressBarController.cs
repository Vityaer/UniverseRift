using Network.Misc;
using System;
using UnityEngine;
using Utils;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace Ui.LoadingScreen.ProgressBar
{
    public class ProgressBarController : UiController<ProgressBarView>
    {
        public void Open()
        {
            View.Panel.SetActive(true);
            Reset();
        }

        public void Reset()
        {
            View.ProgressBar.value = 0;
            View.Percent.text = "0%";
        }

        public void ChangeProgress(float newProgress)
        {
            View.ProgressBar.value = newProgress;
            View.Percent.text = $"{(newProgress * 100) : 0}%";
            View.SliderFill.Reset();
        }

        public void Close()
        {
            View.Panel.SetActive(false);
        }
    }
}