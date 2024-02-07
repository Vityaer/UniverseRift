using VContainerUi.Model;
using VContainerUi.Messages;
using UniRx;
using City.Panels.Events.FortuneCycles;

namespace MainPages.Events.Cycles
{
    public class FortuneCycleContainerController : BaseCycleContainerController
    {
        private void Start()
        {
            MainPanelButton.OnClickAsObservable().Subscribe(_ => OpenMainPanel()).AddTo(Disposables);
        }

        private void OpenMainPanel()
        {
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<FortuneCycleMainPanelController>(openType: OpenType.Exclusive);
        }
    }
}
