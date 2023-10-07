using City.Buildings.Abstractions;
using City.Panels.SelectHeroes;
using Common.Resourses;
using UnityEngine.UI;
using Utils;

namespace City.Buildings.Sanctuary
{
    public class SanctuaryView : BaseBuildingView
    {
        public Button SaveButton;
        public Button ReplacementButton;
        public SerializableDictionary<int, GameResource> Costs;
        public HeroCardsContainerController CardsContainer;
    }
}
