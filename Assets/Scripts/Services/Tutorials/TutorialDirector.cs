using System.Threading;
using Common.Db.CommonDictionaries;
using Cysharp.Threading.Tasks;
using Models.Tutorials;
using VContainer;

namespace Services.Tutorials
{
    public class TutorialDirector
    {
        [Inject] private readonly IObjectResolver m_objectResolver;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        
        public async UniTask RunTutorial(string tutorialId, CancellationToken cancellationToken)
        {
            if (!m_commonDictionaries.Tutorials.TryGetValue(tutorialId, out TutorialModel tutorial))
            {
                return;
            }

            foreach (ITutorialStage stage in tutorial.Stages)
            {
                m_objectResolver.Inject(stage);
                await stage.PlayAsync(cancellationToken);
            }
        }
    }
}