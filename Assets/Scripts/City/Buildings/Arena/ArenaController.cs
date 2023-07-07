using City.Buildings.Abstractions;
using Common;
using Fight;
using Models;
using Models.City.Arena;
using Models.Common;
using Models.Common.BigDigits;
using Utils;
using VContainer;

namespace City.Buildings.Arena
{
    public class ArenaController : BuildingWithFight<ArenaView>
    {
        private ArenaBuildingModel _arenaBuildingSave;
        [Inject] private readonly CommonGameData _сommonGameData;

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