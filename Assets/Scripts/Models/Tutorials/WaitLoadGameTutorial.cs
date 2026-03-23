using System.Threading;
using City.Panels.DialogPanels;
using Common.Db.CommonDictionaries;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UIController.LoadingUI;
using UniRx;
using VContainer;

namespace Models.Tutorials
{
    public class WaitLoadGameTutorial : ITutorialStage
    {
        [LabelWidth(200)]
        public string StartDialogId;

        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly StartLoadingController m_loadingController;
        [Inject] private DialogPanelController m_dialogPanelController;

        public async UniTask PlayAsync(CancellationToken token)
        {
            await m_loadingController.OnLoaded.First().ToUniTask(cancellationToken: token);
            await m_dialogPanelController.ShowDialog(StartDialogId, token);
        }
    }
}