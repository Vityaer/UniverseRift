using City.Buildings.Guild.RecruitViews;
using TMPro;
using Ui.Misc.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Guild.GuildAdministrations
{
    public class GuildAdministrationPanelView : BasePanel
    {
        [Header("UI")]
        public TMP_Text GuildName;
        public TMP_Text GuildLevel;
        public TMP_Text GuildId;
        
        public Button RequestPanelOpenButton;
        public GameObject HaveRequest;

        public ScrollRect Scroll;
        public GuildRecruitView GuildRecruitViewPrefab;
        public RectTransform Content;
    }
}
