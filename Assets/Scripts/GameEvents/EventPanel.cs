using System;
using UIController;
using UIController.ControllerPanels;
using UnityEngine;

namespace GameEvents
{
    public class EventPanel : MonoBehaviour
    {
        public BasePanelScript PanelEvent;
        public SliderTime TimerToEnd;

        public void Open()
        {
            PanelEvent.Open();
        }

        public void Show(DateTime startTime, TimeSpan requireTime)
        {
            gameObject.SetActive(true);
            TimerToEnd.SetData(startTime, requireTime);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}