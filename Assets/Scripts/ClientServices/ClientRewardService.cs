using Assets.Scripts.ClientServices;
using Common;
using Common.Resourses;
using Common.Rewards;
using System.Linq;
using VContainer;

namespace ClientServices
{
    public class ClientRewardService
    {
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly GameController _gameController;

        public void AddReward(GameReward reward)
        {
            var resources = reward.Objects.Where(obj => obj is GameResource).Select(obj => (GameResource)obj).ToList();
            _resourceStorageController.AddResource(resources);
            _gameController.SaveGame();
        }
    }
}
