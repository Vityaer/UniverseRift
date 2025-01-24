using City.TrainCamp;
using TMPro;
using Ui.Misc.Widgets;
using UIController.ItemVisual;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace City.Buildings.PlayerPanels
{
    public class PlayerPanelView : BasePanel
    {
        public ItemSliderController SliderExp;

        public TMP_Text Name;
        public TMP_Text Level;
        public TMP_Text GuildId;
        public TMP_Text PlayerId;

        public CostLevelUpContainer PlayerLevelList;
        public CostLevelUpContainer RewardForLevelUp;
        public Image Avatar;
        public Image OutlineAvatar;
        public Button AvatarButton;
        public Button SettingButton;
    }
}
