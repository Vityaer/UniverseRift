using Models.City.Mines;

namespace City.Buildings.Mines
{
    public class GameMineRestriction
    {
        private MineRestrictionModel _model;
        private int _currentCount;

        public int CurrentCount => _currentCount;
        public int MaxCount => _model.MaxCount;

        public GameMineRestriction(MineRestrictionModel model, int currentCount)
        {
            _model = model;
            _currentCount = currentCount;
        }
    }
}
