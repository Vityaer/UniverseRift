using City.TrainCamp;
using TMPro;
using Ui.Misc.Widgets;
using UIController.ItemVisual;
using UnityEngine.UI;

namespace City.Buildings.PlayerPanels
{
    public class PlayerPanelView : BasePanel
    {
        public ItemSliderController SliderExp;
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Level;
        public TextMeshProUGUI GuildId;
        public TextMeshProUGUI PlayerId;
        public CostLevelUpContainer PlayerLevelList;
        public CostLevelUpContainer RewardForLevelUp;
        public Image Avatar;
        public Image OutlineAvatar;
    }
}
