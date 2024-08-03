using City.Buildings.Guild.RecruitViews;
using Ui.Misc.Widgets;
using UnityEngine.UI;
using UnityEngine;

namespace City.Buildings.Guild.RecruitRequestPanels
{
    public class RecruitRequestPanelView : BasePanel
    {
        public ScrollRect Scroll;
        public GuildRecruitRequestView GuildRecruitRequestViewPrefab;
        public RectTransform Content;
    }
}
