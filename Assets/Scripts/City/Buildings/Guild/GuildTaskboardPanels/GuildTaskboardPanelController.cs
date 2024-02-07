using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainerUi.Model;
using VContainerUi.Messages;
using City.Buildings.Guild.GuildDonatePanels;

namespace City.Buildings.Guild.GuildTaskboardPanels
{
    public class GuildTaskboardPanelController : UiPanelController<GuildTaskboardPanelView>
    {
        public override void Start()
        {
            View.OpenDonatePanelButton.OnClickAsObservable().Subscribe(_ => OpenGuildDonatePanel()).AddTo(Disposables);
            base.Start();
        }

        private void OpenGuildDonatePanel()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<GuildDonatePanelController>(openType: OpenType.Exclusive);
        }
    }
}
