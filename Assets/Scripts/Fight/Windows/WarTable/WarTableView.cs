using City.Buildings.Abstractions;
using City.Panels.SelectHeroes;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace Fight.WarTable
{
    public class WarTableView : BaseBuildingView
    {
        public HeroCardsContainerController ListCardPanel;
        public List<WarriorPlace> LeftTeam = new List<WarriorPlace>();
        public List<WarriorPlace> RightTeam = new List<WarriorPlace>();

        public Button StartFightButton;
        public TextMeshProUGUI StrengthLeftTeam;
        public TextMeshProUGUI StrengthRightTeam;
    }
}
