using City.Buildings.Abstractions;
using City.Panels.SelectHeroes;
using System.Collections.Generic;
using TMPro;
using Ui.Misc.Widgets.SwapToggle;
using UnityEngine;
using UnityEngine.UI;

namespace Fight.Common.WarTable
{
    public class WarTableView : BaseBuildingView
    {
        public HeroCardsContainerController ListCardPanel;
        public List<WarriorPlace> LeftTeam = new();
        public List<WarriorPlace> RightTeam = new();

        public Button StartFightButton;
        public TextMeshProUGUI StrengthLeftTeam;
        public TextMeshProUGUI StrengthRightTeam;

        public SwapSpriteToggle FastFightToggle;
        
        [Header("Drag")]
        public RectTransform DragableItem;
        public Image DragableItemImage;
        public float DragSpeed;
    }
}
