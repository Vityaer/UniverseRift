using Common.Db.CommonDictionaries;
using Cysharp.Threading.Tasks;
using Models.Common;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace Common
{
    public class GameController : IStartable
    {
        [Inject] private readonly CommonGameData _сommonGameData;
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        public ReactiveCommand OnLoadedGameData = new ReactiveCommand();
        public ReactiveCommand OnGameSave = new ReactiveCommand();

        public void Start()
        {
            WaitGameDataInit().Forget();
        }

        private async UniTaskVoid WaitGameDataInit()
        {
            await UniTask.WaitUntil(() => _сommonGameData.IsInited && _commonDictionaries.Inited);
            OnloadGame();
        }

        private void OnloadGame()
        {
            OnLoadedGameData.Execute();
        }
    }
}