using City.Panels.SubjectPanels.Common;
using Common.Rewards;
using UiExtensions.Scroll.Interfaces;
using VContainer;

namespace City.Panels.BoxRewards
{
    public class BoxRewardsPanelController : UiPanelController<BoxRewardsPanelView>
    {
        [Inject]
        private void Construct(SubjectDetailController SubjectDetailController)
        {
            View.RewardController?.SetDetailsController(SubjectDetailController);
        }

        public void ShowAll(GameReward reward)
        {
            //if (reward != null)
            //View.RewardController.ShowAllReward(reward);
        }
    }
}