using City.Panels.SubjectPanels.Items;
using TMPro;
using UIController.Misc.Widgets;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace City.Panels.SubjectPanels
{
    public class ItemPanelView : SubjectInfoPanel
    {
        public LocalizeStringEvent Name;
        public LocalizeStringEvent ItemType;
        public TextMeshProUGUI GeneralInfo;
        public TextMeshProUGUI AddactiveInfo;
        public LocalizeStringEvent ActionButtonText;
        public Button SwapButton;

        [Header("Bonuses")]
        public ScrollRect BonusesScroll;
        public ItemBonusView itemBonusViewPrefab;
        
        public GameObject SetBonusesPanel;
        public ScrollRect SetBonusesScroll;

    }
}
