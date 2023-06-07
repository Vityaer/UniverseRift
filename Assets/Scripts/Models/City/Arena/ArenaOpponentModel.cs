using Models.Fights.Campaign;

namespace Models.City.Arena
{
    [System.Serializable]
    public class ArenaOpponentModel
    {
        public string Name;
        public string Avatar;
        public int Level;
        public int Score;
        public int WinCount;
        public int LoseCount;
        public MissionModel Mission;
    }
}