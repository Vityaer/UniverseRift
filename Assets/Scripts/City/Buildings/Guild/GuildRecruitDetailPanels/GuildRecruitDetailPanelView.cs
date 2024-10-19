using City.Buildings.PlayerPanels.PlayerMiniPanels;
using TMPro;
using Ui.Misc.Widgets;
using UnityEngine.UI;

namespace City.Buildings.Guild.GuildRecruitDetailPanels
{
    public class GuildRecruitDetailPanelView : BasePanel
    {
        public PlayerAvatarView PlayerAvatarView;
        public Button RecruitBanButton;
        public Button LeaveGuildButton;
        public TMP_Text PlayerName;
        public TMP_Text DonateValue;
        public TMP_Text LastEnterValue;
    }
}
