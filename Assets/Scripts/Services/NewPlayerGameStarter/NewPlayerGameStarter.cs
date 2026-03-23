using System.Threading;
using City.Panels.Registrations;
using Common.Db.CommonDictionaries;
using Cysharp.Threading.Tasks;
using Services.Tutorials;
using VContainer;

namespace Services.NewPlayerGameStarter
{
    public class NewPlayerGameStarter
    {
        [Inject] private readonly RegistrationPanelController m_registrationPanelController;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly TutorialDirector m_tutorialDirector;
        
        private CancellationTokenSource m_cancellationTokenSource;
        
        public async UniTask StartGame()
        {
            m_cancellationTokenSource = new();
            await m_registrationPanelController.RegistrationAsync();
            await m_tutorialDirector.RunTutorial("StartTutorial", m_cancellationTokenSource.Token);
        }
    }
}