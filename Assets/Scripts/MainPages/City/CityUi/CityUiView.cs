using MainPages.City.CityUi;
using UnityEngine;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace MainPages.MenuHud
{
    public class CityUiView : UiView
    {
        [field: SerializeField] public ButtonWithNewsView FriendlistButton { get; private set; }
        [field: SerializeField] public ButtonWithNewsView MailButton { get; private set; }
        [field: SerializeField] public ButtonWithNewsView InventoryButton { get; private set; }
        [field: SerializeField] public ButtonWithNewsView DailyRewardButton { get; private set; }
        [field: SerializeField] public ButtonWithNewsView DailyTaskButton { get; private set; }
        [field: SerializeField] public ButtonWithNewsView AchievmentsButton { get; private set; }
        [field: SerializeField] public ButtonWithNewsView ServerChatButton { get; private set; }
    }
}
