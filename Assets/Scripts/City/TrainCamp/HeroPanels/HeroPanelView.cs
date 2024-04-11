using City.TrainCamp;
using System.Collections.Generic;
using TMPro;
using Ui.Misc.Widgets;
using UIController;
using UIController.ItemVisual;
using UnityEngine;
using UnityEngine.UI;

namespace City.TrainCamp.HeroPanels
{
    public class HeroPanelView : BasePanel
    {
        public Button ToLeftButton;
        public Button ToRightButton;
        public Button LevelUpButton;
        public Button OpenHeroDetailsButton;
        public Button EvolitionPanelButton;
        public Image imageHero;
        public TextMeshProUGUI textLevel;
        public TextMeshProUGUI textNameHero;
        public TextMeshProUGUI textHP;
        public TextMeshProUGUI textAttack;
        public TextMeshProUGUI textArmor;
        public TextMeshProUGUI textInitiative;
        public TextMeshProUGUI textStrengthHero;
        public CostUIList CostController;
        public RatingHero RatingHeroController;

        [Header("Items")]
        public List<HeroItemCellController> CellsForItem = new List<HeroItemCellController>();

        [Header("Skills")]
        public SkillUIController skillController;
    }
}
