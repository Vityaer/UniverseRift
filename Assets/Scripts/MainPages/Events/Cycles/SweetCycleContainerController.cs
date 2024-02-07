using City.Panels.Events.SweetCycles;
using UniRx;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace MainPages.Events.Cycles
{
    public class SweetCycleContainerController : BaseCycleContainerController
    {
        private void Start()
        {
            MainPanelButton.OnClickAsObservable().Subscribe(_ => OpenMainPanel()).AddTo(Disposables);
        }

        private void OpenMainPanel()
        {
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<SweetCycleMainPanelController>(openType: OpenType.Exclusive);
        }
    }
}
