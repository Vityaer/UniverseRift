using City.Buildings.CityButtons.EventAgent;
using City.Buildings.CityButtons;
using City.Buildings.Friends;
using City.Buildings.Mails;
using UnityEngine.UI;
using VContainer.Unity;
using VContainerUi.Abstraction;
using VContainerUi.Messages;
using VContainerUi.Interfaces;
using UniRx;
using VContainerUi.Model;
using VContainer;
using VContainerUi.Services;
using UIController.Inventory;
using City.Buildings.Requirement;

namespace MainPages.MenuHud
{
    public class CityUiController : UiController<CityUiView>, IInitializable
    {
        [Inject] protected readonly IUiMessagesPublisherService UiMessagesPublisher;

        private CompositeDisposable Disposables = new CompositeDisposable();

        public void Initialize()
        {
            OpenPanelOnClick<FriendsController>(View.FriendlistButton);
            OpenPanelOnClick<MailController>(View.MailButton);
            OpenPanelOnClick<InventoryController>(View.InventoryButton);
            OpenPanelOnClick<DailyRewardPanelController>(View.DailyRewardButton);
            OpenPanelOnClick<DailyTaskPanelController>(View.DailyTaskButton);
            OpenPanelOnClick<AchievmentsPageController>(View.AchievmentsButton);
        }

        private void OpenPanelOnClick<T>(Button button) where T : IPopUp
        {
            button.OnClickAsObservable().Subscribe(_ => OpenBuilding<T>()).AddTo(Disposables);
        }

        private void OpenBuilding<T>() where T : IPopUp
        {
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<T>(openType: OpenType.Additive);
        }
    }

}
