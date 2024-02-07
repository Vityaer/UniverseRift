using City.Buildings.Abstractions;
using City.Buildings.Guild.RecruitViews;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Guild
{
    public class GuildView : BaseBuildingView
    {
        [Header("Outer guild")]
        public GameObject OuterGuildPanel;
        public Button OpenNewGuildPanelButton;
        public Button OpenAvailableGuildsPanelButton;

        [Header("Inner guild")]
        public GameObject InnerGuildPanel;
        public TMP_Text GuildId;
        public TMP_Text GuildName;
        public TMP_Text GuildLevel;
        public TMP_Text BossLevel;
        public Image BossImage;
        public Button RaidBossButton;
        public Button GuildTaskboardButton;
        public ScrollRect Scroll;
        public RecruitProgressView RecruitProgressViewPrefab;
        public RectTransform Content;
    }
}
