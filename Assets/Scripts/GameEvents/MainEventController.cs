using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class MainEventController : MonoBehaviour
    {
        public List<EventPanel> ListSimpleEvent = new List<EventPanel>();
        public StageCycleEvent StageCycleEvent;

        public void Open(DateTime startTime, TimeSpan requireTime)
        {
            gameObject.SetActive(true);
            foreach (EventPanel panel in ListSimpleEvent)
            {
                panel.Show(startTime, requireTime);
            }
        }

        [ContextMenu("Close")]
        private void Close()
        {
            foreach (EventPanel panel in ListSimpleEvent)
            {
                panel.Hide();
            }
            gameObject.SetActive(false);
        }
    }
}