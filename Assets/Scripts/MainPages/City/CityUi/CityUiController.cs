using City.Buildings.CityButtons;
using City.Buildings.CityButtons.EventAgent;
using City.Buildings.Friends;
using City.Buildings.Mails;
using City.Buildings.Requirement;
using City.Panels.Achievments;
using City.Panels.Chats.ServerChats;
using City.Panels.DailyRewards;
using City.Panels.DailyTasks;
using City.Panels.Inventories;
using MainPages.MenuHud;
using UIController.Inventory;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;
using VContainerUi.Interfaces;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace MainPages.City.CityUi
{
    public class CityUiController : UiController<CityUiView>, IInitializable
    {
        [Inject] private readonly IUiMessagesPublisherService m_uiMessagesPublisher;
        [Inject] private readonly InventoryPanelController m_inventoryPanelController;
        [Inject] private IObjectResolver m_diContainer;

        private readonly CompositeDisposable m_disposables = new CompositeDisposable();

        public void Initialize()
        {
            OpenPanelOnClick<FriendsPanelController>(View.FriendlistButton);
            OpenPanelOnClick<MailPanelController>(View.MailButton);
            OpenPanelOnClick<DailyRewardPanelController>(View.DailyRewardButton);
            OpenPanelOnClick<DailyTaskPanelController>(View.DailyTaskButton);
            OpenPanelOnClick<AchievmentsPageController>(View.AchievmentsButton);
            OpenPanelOnClick<ServerChatPanelController>(View.ServerChatButton);

            View.InventoryButton.Button.OnClickAsObservable().Subscribe(_ => OpenAllInventory()).AddTo(m_disposables);
            m_inventoryPanelController.OnNewsStatusChange
                .Subscribe(flag => ChangeNewsStatus(View.InventoryButton, flag))
                .AddTo(m_disposables);
        }

        private void OpenPanelOnClick<T>(ButtonWithNewsView buttonWithNews) where T : IPopUp
        {
            buttonWithNews.Button.OnClickAsObservable().Subscribe(_ => OpenBuilding<T>()).AddTo(m_disposables);
            var uiController = m_diContainer.Resolve<T>();
            uiController.OnNewsStatusChange.Subscribe(flag => ChangeNewsStatus(buttonWithNews, flag))
                .AddTo(m_disposables);
        }

        private void ChangeNewsStatus(ButtonWithNewsView button, bool flag)
        {
            button.SetNewsEnabled(flag);
        }

        private void OpenBuilding<T>() where T : IPopUp
        {
            m_uiMessagesPublisher.OpenWindowPublisher.OpenWindow<T>(openType: OpenType.Additive);
        }

        private void OpenAllInventory()
        {
            m_inventoryPanelController.ShowAll();
        }
    }
}