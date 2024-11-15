﻿using City.Panels.Units;
using System.Collections.Generic;
using TMPro;
using Ui.Misc.Widgets;
using UIController;
using UnityEngine.UI;

namespace City.Buildings.Voyage.Panels
{
    public class VoyageMissionPanelView : BasePanel
    {
        public TextMeshProUGUI textNameMission;
        public RewardUIController rewardController;
        public TMP_Text StatusMissionText;
        public Button MainButton;

        public List<UnitCell> UnitCells;
    }
}
