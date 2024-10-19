using City.Buildings.PlayerPanels.PlayerMiniPanels;
using TMPro;
using Ui.Misc.Widgets;
using UnityEngine.UI;

namespace City.Panels.PlayerInfoPanels
{
    public class PlayerMiniInfoPanelView : BasePanel
    {
        public PlayerAvatarView PlayerAvatarView;
        public TMP_Text PlayerName;
        public Button SendMessageButton;
    }
}
