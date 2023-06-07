using City.Buildings.General;
using Common;
using Models;
using Models.City.Arena;

namespace City.Buildings.Arena
{
    public class ArenaScript : BuildingWithFight
    {
        ArenaBuildingModel arenaBuildingSave;

        public static ArenaScript Instance { get; private set; }

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

        protected override void OnResultFight(FightResult result)
        {
            if (result == FightResult.Win)
            {
                OnWinFight(1);
                SaveGame();
            }
            OnTryFight();
        }

    }
}