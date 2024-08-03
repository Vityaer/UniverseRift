using City.Buildings.Abstractions;
using City.Buildings.Guild.RecruitViews;
using TMPro;
using UIController;
using UIController.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Guild
{
    public class GuildView : BaseBuildingView
    {
        public GameObject InnerGuildPanel;
        public TMP_Text GuildId;
        public TMP_Text GuildName;
        public TMP_Text GuildLevel;

        public Button GuildTaskboardButton;
        public Button AdministrationButton; 
        public Button BossRaidButton;
        public Button MarketButton;
    }
}
