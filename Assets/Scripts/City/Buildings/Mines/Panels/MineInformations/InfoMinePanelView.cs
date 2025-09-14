using City.TrainCamp;
using TMPro;
using Ui.Misc.Widgets;
using UIController.ItemVisual;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace City.Buildings.Mines.Panels
{
    public class InfoMinePanelView : BasePanel
    {
        public GameObject Panel;
        public GameObject PanelController;
        public Image MainImage;
        public LocalizeStringEvent NameMineText;
        public TMP_Text LevelMineText;

        public GameObject InfoDataContainer;
        public TextMeshProUGUI IncomeText;
        public ItemSliderController SliderAmount;
        // public SubjectCellControllerScript product;
        public CostUIList CostController;
        public Button LevelUpButton;
        public Button GetResourceButton;
        public TMP_Text RequireLevelText;

        [Header("Destroy")]
        public Button OpenPanelDestroyMineButton;
        public Button DestroyMineAgreeButton;
        public Button DestroyMineDisagreeButton;
        public GameObject DestroyMinePanel;
    }
}
