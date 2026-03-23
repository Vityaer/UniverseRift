using System;
using Fight.Common.Misc;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace Fight.AI
{
    public class BotProvider : IInitializable, IDisposable
    {
        [Inject] private readonly Common.FightController m_fightController;
        [Inject] private readonly BotFactory m_botFactory;
        
        private readonly CompositeDisposable m_disposables = new(); 
        
        private BaseBot m_bot;

        public void Initialize()
        {
            m_fightController.OnStartFight.Subscribe(_ => OnStartFight()).AddTo(m_disposables);
            m_fightController.OnFinishFight.Subscribe(_ => OnFinishFight()).AddTo(m_disposables);
        }

        private void OnStartFight()
        {
            m_bot = m_botFactory.Create<BaseBot>();
            m_bot.Initialize();
            m_bot.SetSideAI(m_fightController.IsFastFight ? Side.All : Side.Right);
        }

        private void OnFinishFight()
        {
            m_bot = null;
        }

        public bool CheckMeOnSubmission(Side side)
        {
            return m_bot.CheckMeOnSubmission(side);
        }

        void IDisposable.Dispose()
        {
            m_disposables.Dispose();
        }
    }
}
