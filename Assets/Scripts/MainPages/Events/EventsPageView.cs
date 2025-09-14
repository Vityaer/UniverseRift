using City.General;
using City.Panels.Events.TavernCycleMainPanels;
using Models.Events;
using TMPro;
using UIController;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Utils;

namespace MainPages.Events
{
    public class EventsPageView : MainPage
    {
        public GameObject BasePanel;
        public SliderTime SliderEventLeftTime;
        public Button ArenaButton;
        public Button TravelButton;
        public Button TableTasksButton;
        public Button EvolutionButton;

        public RectTransform ContentAction;
        public LocalizeStringEvent EventCycleName;

        public SerializableDictionary<GameEventType, EventContainer> EventContainers = new();

        public Button HeroSpaceMarketButton;
    }
}
