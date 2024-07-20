using City.Buildings.Guild.RecruitViews;
using TMPro;
using Ui.Misc.Widgets;
using UIController;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Guild.BossRaid
{
    public class GuildBossRaidPanelView : BasePanel
    {
        public TMP_Text BossLevel;
        public Image BossImage;
        public Slider BossHealthSlider;

        public TMP_Text TimeForRefreshRaid;
        public ButtonCostController BossRaidButton;
        public GameObject TimerRaidPanel;

        public ScrollRect Scroll;
        public RecruitProgressView RecruitProgressViewPrefab;
        public RectTransform Content;
    }
}
