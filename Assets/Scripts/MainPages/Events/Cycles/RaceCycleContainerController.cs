using City.Panels.Events.RaceCircleCycles;
using UniRx;
using VContainerUi.Model;
using VContainerUi.Messages;

namespace MainPages.Events.Cycles
{
    public class RaceCycleContainerController : BaseCycleContainerController
    {
        private void Start()
        {
            MainPanelButton.OnClickAsObservable().Subscribe(_ => OpenMainPanel()).AddTo(Disposables);
        }

        private void OpenMainPanel()
        {
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<RaceCicrleCycleMainPanelController>(openType: OpenType.Exclusive);
        }
    }
}
