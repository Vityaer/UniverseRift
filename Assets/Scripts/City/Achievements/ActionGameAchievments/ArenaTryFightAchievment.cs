using City.Panels.Arenas.SimpleArenas;
using Models.Common.BigDigits;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments
{
    public class ArenaTryFightAchievment : GameAchievment
    {
        [Inject] private readonly SimpleArenaPanelController _simpleArenaPanelController;

        protected override void Subscribe()
        {
            _simpleArenaPanelController.OnTryFight.Subscribe(x => AddProgress(new BigDigit(1))).AddTo(Disposables);
        }
    }
}
