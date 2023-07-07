using UnityEngine;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace MainPages.MenuHud
{
    public class CityUiView : UiView
    {
        [field: SerializeField] public Button FriendlistButton { get; private set; }
        [field: SerializeField] public Button MailButton { get; private set; }
        [field: SerializeField] public Button InventoryButton { get; private set; }
        [field: SerializeField] public Button DailyRewardButton { get; private set; }
        [field: SerializeField] public Button DailyTaskButton { get; private set; }
        [field: SerializeField] public Button AchievmentsButton { get; private set; }
    }
}
