using UniRx;
using VContainerUi.Model;
using VContainerUi.Messages;
using MainPages.Events.Cycles.TavernCycles.Panels;

namespace MainPages.Events.Cycles.TavernCycles
{
    public class TavernCycleContainerController : BaseCycleContainerController
    {
        private void Start()
        {
            MainPanelButton.OnClickAsObservable().Subscribe(_ => OpenMainPanel()).AddTo(Disposables);
        }

        private void OpenMainPanel()
        {
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<TavernCycleMainPanelController>(openType: OpenType.Exclusive);
        }
    }
}
