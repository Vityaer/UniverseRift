using Models.City.Mines;

namespace City.Buildings.Mines
{
    public class GameMineRestriction
    {
        private readonly MineRestrictionModel m_model;
        private readonly int m_currentCount;

        public int CurrentCount => m_currentCount;
        public int MaxCount => m_model.MaxCount;

        public GameMineRestriction(MineRestrictionModel model, int currentCount)
        {
            m_model = model;
            m_currentCount = currentCount;
        }
    }
}
