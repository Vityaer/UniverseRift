using System.Collections.Generic;
using City.Buildings.Abstractions;
using City.Panels.SelectHeroes;
using UIController.Buttons;
using UnityEngine.UI;

namespace City.Buildings.Sanctuary
{
    public class SanctuaryView : BaseBuildingView
    {
        public ButtonWithObserverResource ReplacementButton;
        public Dictionary<string, Button> RaceButtons = new();

        public HeroCardsContainerController CardsContainer;
    }
}
