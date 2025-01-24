using City.Buildings.Abstractions;
using City.Panels.SelectHeroes;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace Fight.WarTable
{
    public class WarTableView : BaseBuildingView
    {
        public HeroCardsContainerController ListCardPanel;
        public List<WarriorPlace> LeftTeam = new();
        public List<WarriorPlace> RightTeam = new();

        public Button StartFightButton;
        public TextMeshProUGUI StrengthLeftTeam;
        public TextMeshProUGUI StrengthRightTeam;

        [Header("Drag")]
        public RectTransform DragableItem;
        public Image DragableItemImage;
        public float DragSpeed;
    }
}
