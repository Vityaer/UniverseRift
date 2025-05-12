using Models.Arenas;
using Models.Data.Players;
using System.Collections.Generic;

namespace Models
{
    [System.Serializable]
    public class ArenaData
    {
        public ArenaPlayerData MyData;
        public List<ArenaPlayerData> Opponents = new();
        public Dictionary<int, PlayerData> PlayersData = new();
        public ArenaGeneralData ArenaGeneralData;
    }
}
