using City.General;
using City.Panels.Events.TavernCycleMainPanels;
using Models.Events;
using TMPro;
using UIController;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace MainPages.Events
{
    public class EventsPageView : MainPage
    {
        public GameObject BasePanel;
        public Button ArenaButton;
        public Button TravelButton;
        public Button TableTasksButton;
        public Button EvolutionButton;

        public RectTransform ContentAction;
        public TMP_Text EventCycleName;

        public SerializableDictionary<GameEventType, EventContainer> EventContainers = new();
    }
}
