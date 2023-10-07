using City.TrainCamp;
using System.Collections.Generic;
using Ui.Misc.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Mines.Panels.CreateMines
{
    public class CreateMinePanelView : BasePanel
    {
        public GameObject Panel;
        public GameObject PanelController;
        public Button CreateButton;
        public List<MineCard> MineCards = new List<MineCard>();
        public CostUIList CostController;
    }
}
