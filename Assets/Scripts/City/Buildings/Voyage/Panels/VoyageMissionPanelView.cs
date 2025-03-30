using City.Panels.Units;
using System.Collections.Generic;
using TMPro;
using Ui.Misc.Widgets;
using UIController;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace City.Buildings.Voyage.Panels
{
    public class VoyageMissionPanelView : BasePanel
    {
        public LocalizeStringEvent NameMission;
        public RewardUIController rewardController;
        public LocalizeStringEvent StatusMission;
        public Button MainButton;

        public List<UnitCell> UnitCells;
    }
}
