using City.Buildings.Abstractions;
using City.Buildings.Arena;
using TMPro;
using UController.Other;
using UIController;
using UnityEngine;
using UnityEngine.UI;

namespace City.Panels.Arenas.SimpleArenas
{
    public class SimpleArenaPanelView : BaseBuildingView
    {
        public SliderTime LeftTime;
        public AvatarView PlayerAvatar;
        public TMP_Text PlayerScore;
        public RectTransform Content;
        public ScrollRect ScrollRect;
        public ArenaOpponentView OpponentPrefab;
        public Button DefendersButton;
    }
}
