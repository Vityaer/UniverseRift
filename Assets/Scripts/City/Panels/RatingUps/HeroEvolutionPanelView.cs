using City.Panels.SelectHeroes;
using City.TrainCamp;
using Ui.Misc.Widgets;
using UIController;
using UIController.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace City.Panels.RatingUps
{
    public class HeroEvolutionPanelView : BasePanel
    {
        public Button ButtonLevelUP;
        public CostUIList CostController;

        [Header("UI")]
        public ListRequirementHeroesUI ListRequirementHeroes;

        public GameObject SelectHeroesPanel;
        public HeroCardsContainerController CardsContainer;
        public RequireCard RequireCardInfo;
        public Button DimmedSelectedPanelButton;
    }
}
