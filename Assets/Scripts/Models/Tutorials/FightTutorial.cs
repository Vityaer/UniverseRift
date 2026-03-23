using System.Threading;
using Cysharp.Threading.Tasks;
using Fight.Common;
using UniRx;
using VContainer;

namespace Models.Tutorials
{
    public class FightTutorial : ITutorialStage
    {
        [Inject] private readonly FightController m_fightController;

        public async UniTask PlayAsync(CancellationToken token)
        {
            await m_fightController.OnStartFight.First().ToUniTask(cancellationToken: token);
        }
    }
}