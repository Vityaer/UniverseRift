using City.Panels.SelectHeroes;
using City.TrainCamp;
using TMPro;
using Ui.Misc.Widgets;
using UIController;
using UnityEngine;
using UnityEngine.Localization.Components;
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
        public LocalizeStringEvent NameText;

        [Header("Old Hero")]
        public RatingHero OldRatingHeroController;
        public TMP_Text OldLimitLevelText;
        public TMP_Text OldStrengthText;
        public RawImage OldHeroRawImage;
        
        [Header("New Hero")]
        public RatingHero NewRatingHeroController;
        public TMP_Text NewLimitLevelText;
        public TMP_Text NewStrengthText;
        public RawImage NewHeroRawImage;
    }
}
