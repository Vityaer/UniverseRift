using City.Buildings.General;
using Common;
using Fight;
using Models;
using Models.City.Arena;

namespace City.Buildings.Arena
{
    public class ArenaController : BuildingWithFight
    {
        ArenaBuildingModel arenaBuildingSave;

        public static ArenaController Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        protected override void OnLoadGame()
        {
            arenaBuildingSave = GameController.GetCitySave.arenaBuilding;
        }

        public void FightWithOpponentUseAI(ArenaOpponentModel opponent)
        {
            OpenMission(opponent.Mission);
        }

        protected override void OnResultFight(FightResultType result)
        {
            if (result == FightResultType.Win)
            {
                OnWinFight(1);
                SaveGame();
            }
            OnTryFight();
        }

    }
}