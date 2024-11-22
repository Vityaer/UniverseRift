using Ui.Misc.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Guild.AvailableGuildsPanels
{
    public class AvailableGuildsPanelView : BasePanel
    {
        public ScrollRect Scroll;
        public RectTransform Content;
        public AvailableGuildView Prefab;
        public GameObject NotFoundGuildsMessageText;
        public Button OpenPanelCreateGuildButton;
    }
}
