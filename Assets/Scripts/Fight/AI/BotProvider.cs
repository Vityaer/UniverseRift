using System;
using Fight.Common.Misc;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace Fight.Common.AI
{
    public class BotProvider : IInitializable, IDisposable
    {
        [Inject] private readonly Common.FightController _fightController;
        [Inject] private readonly BotFactory _botFactory;
        
        private BaseBot _bot;
        private CompositeDisposable _disposables = new CompositeDisposable(); 

        public void Initialize()
        {
            _fightController.OnStartFight.Subscribe(_ => OnStartFight()).AddTo(_disposables);
            _fightController.OnFinishFight.Subscribe(_ => OnFinishFight()).AddTo(_disposables);
        }

        private void OnStartFight()
        {
            _bot = _botFactory.Create<BaseBot>();
            _bot.Initialize();
            _bot.SetSideAI(_fightController.IsFastFight ? Side.All : Side.Right);
        }

        private void OnFinishFight()
        {
            _bot = null;
        }

        public bool CheckMeOnSubmission(Side side)
        {
            return _bot.CheckMeOnSubmission(side);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
