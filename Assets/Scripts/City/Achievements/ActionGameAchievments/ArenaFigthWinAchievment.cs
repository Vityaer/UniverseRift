using City.Panels.Arenas.SimpleArenas;
using Models.Common.BigDigits;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments
{
    public class ArenaFigthWinAchievment : GameAchievment
    {
        [Inject] private readonly SimpleArenaPanelController _simpleArenaPanelController;

        protected override void Subscribe()
        {
            _simpleArenaPanelController.OnCompleteMission.Subscribe(x => AddProgress(new BigDigit(x))).AddTo(Disposables);
        }
    }
}
