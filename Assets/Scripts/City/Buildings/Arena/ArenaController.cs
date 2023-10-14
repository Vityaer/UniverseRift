using Assets.Scripts.City.Panels.Arenas;
using City.Buildings.Abstractions;
using City.Panels.Arenas;
using City.Panels.Arenas.SimpleArenas;
using City.Panels.NewLevels;
using Fight;
using Models;
using Models.City.Arena;
using Models.Common;
using UniRx;
using Utils;
using VContainer;
using VContainer.Unity;
using VContainerUi.Model;
using VContainerUi.Messages;

namespace City.Buildings.Arena
{
    public class ArenaController : BuildingWithFight<ArenaView>, IInitializable
    {
        [Inject] private readonly CommonGameData _сommonGameData;

        private ArenaBuildingModel _arenaBuildingSave;

        public void Initialize()
        {
            View.SimpleArenaButton.OnClickAsObservable().Subscribe(_ => OpenSimpleAreana()).AddTo(Disposables);
            View.RatingArenaButton.OnClickAsObservable().Subscribe(_ => OpenRatingAreana()).AddTo(Disposables);
            View.TournamentButton.OnClickAsObservable().Subscribe(_ => OpenTournament()).AddTo(Disposables);
        }

        private void OpenTournament()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<TournamentPanelController>(openType: OpenType.Exclusive);
        }

        private void OpenRatingAreana()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<RatingArenaPanelController>(openType: OpenType.Exclusive);

        }

        private void OpenSimpleAreana()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<SimpleArenaPanelController>(openType: OpenType.Exclusive);
        }

        protected override void OnLoadGame()
        {
            _arenaBuildingSave = _сommonGameData.City.ArenaSave;
        }

        public void FightWithOpponentUseAI(ArenaOpponentModel opponent)
        {
            OpenMission(opponent.Mission);
        }

        protected override void OnResultFight(FightResultType result)
        {
            if (result == FightResultType.Win)
            {
                _onWinFight.Execute(1);
                TextUtils.Save(_сommonGameData);
            }
            _onTryFight.Execute(1);
        }
    }
}