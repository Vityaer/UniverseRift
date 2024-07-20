using TMPro;
using UIController.ItemVisual;
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
        public TextMeshProUGUI ActionButtonText;
    }
}
