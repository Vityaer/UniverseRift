using MainPages.Events.Cycles;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MainPages.Events
{
    [Serializable]
    public class EventContainer
    {
        public Image Main;
        public Image PreviousArrow;
        public Image NextArrow;
        public BaseCycleContainerController Container;

        public void Active()
        {
            Main.color = Color.white;
            PreviousArrow.color = Color.white;
            NextArrow.color = Color.white;
        }
    }
}
