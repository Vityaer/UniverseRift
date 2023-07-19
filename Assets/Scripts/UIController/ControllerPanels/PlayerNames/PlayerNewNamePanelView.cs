using City.Buildings.PlayerPanels;
using TMPro;
using Ui.Misc.Widgets;
using UnityEngine;

namespace UIController.ControllerPanels.PlayerNames
{
    public class PlayerNewNamePanelView : BasePanel
    {
        [field: SerializeField] public TMP_InputField NewNamePlayerInputField { get; private set; }
        [field: SerializeField] public ButtonCostController PayNewNameButton { get; private set; }
        [field: SerializeField] public PlayerPanelController PlayerPanelController { get; private set; }
    }
}
