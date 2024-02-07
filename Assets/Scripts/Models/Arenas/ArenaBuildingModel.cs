using Models.Arenas;
using System.Collections.Generic;

namespace Models
{
    [System.Serializable]
    public class ArenaBuildingModel
    {
        public ArenaPlayerData MyData;
        public List<ArenaPlayerData> Opponents = new();
    }
}
