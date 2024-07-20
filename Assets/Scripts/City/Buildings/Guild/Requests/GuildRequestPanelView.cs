using Ui.Misc.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Guild.Requests
{
    public class GuildRequestPanelView : BasePanel
    {
        public Button RefreshListButton;
        public ScrollRect ScrollRect;
        public GuildRequestView GuildRequestViewPrefab;
        public RectTransform Content;
    }
}
