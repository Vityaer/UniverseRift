using System.Threading;
using Cysharp.Threading.Tasks;

namespace Models.Tutorials
{
    public interface ITutorialStage
    {
        UniTask PlayAsync(CancellationToken token);
    }
}