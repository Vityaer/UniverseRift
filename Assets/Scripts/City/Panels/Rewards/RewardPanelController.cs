using City.Panels.AutoFights;
using Common.Rewards;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Panels.Rewards
{
    public class RewardPanelController : UiPanelController<RewardPanelView>
    {
        [Inject] protected readonly IUiMessagesPublisherService UiMessagesPublisher;

        public ReactiveCommand OnClose = new ReactiveCommand();

        public void Open(GameReward reward)
        {
            View.RewardUIController.ShowReward(reward);
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<RewardPanelController>(openType: OpenType.Exclusive);
        }

        protected override void Close()
        {
            OnClose.Execute();
            base.Close();
        }
    }
}
