using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Models.Common;
using System;
using System.Linq;
using UIController.Rewards;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace Common
{
    public class GameController : IStartable, IDisposable
    {
        [Inject] private readonly CommonGameData _сommonGameData;

        public ReactiveCommand OnLoadedGameData = new ReactiveCommand();
        public ReactiveCommand OnGameSave = new ReactiveCommand();

        public void Start()
        {
            WaitGameDataInit().Forget();
        }

        private async UniTaskVoid WaitGameDataInit()
        {
            _сommonGameData.Init();
            await UniTask.WaitUntil(() => _сommonGameData.IsInited);
            OnloadGame();
        }

        private void OnloadGame()
        {
            OnLoadedGameData.Execute();
        }

        public void SaveGame()
        {
            OnGameSave.Execute();
            _сommonGameData.Save();
        }

        void OnApplicationPause(bool pauseStatus)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
		// SaveGame();
#endif
        }

        void OnApplicationFocus(bool hasFocus)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        SaveGame();
#endif
        }

        public void Dispose()
        {
            SaveGame();
        }
    }
}