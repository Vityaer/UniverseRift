using Common.Resourses;

namespace UIController.Rewards.PosibleRewards
{
    public class PosibleGameResource : PosibleGameObject
    {
        private readonly GameResource _resource;

        public PosibleGameResource(string id, GameResource gameResource, float percent = 1) : base(id, percent)
        {
            _resource = gameResource;
        }

        public GameResource GetResource(int tryCount)
        {
            GameResource result = null;
            var currentPosible = Posibility / 1000f;
            var countSuccess = 0;

            for (var i = 0; i < tryCount; i++)
            {
                if (UnityEngine.Random.Range(0f, 100f) < currentPosible)
                {
                    countSuccess += 1;
                }
            }

            if (countSuccess > 0)
            {
                result = _resource * countSuccess;
            }

            return result;
        }
    }
}
