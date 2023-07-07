using TMPro;
using UIController.Inventory;
using UIController.Misc.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace City.Panels.SubjectPanels.Splinters
{
    public class SplinterPanelView : SubjectInfoPanel
    {
        [field: SerializeField] public TextMeshProUGUI MainLabel { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ItemType { get; private set; }
        [field: SerializeField] public TextMeshProUGUI GeneralInfo { get; private set; }
        [field: SerializeField] public GameObject PosibilityButton { get; private set; }
        [field: SerializeField] public GameObject HelpButton { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ActionButtonText { get; private set; }
    }
}
