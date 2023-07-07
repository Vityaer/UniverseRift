using Assets.Scripts.ClientServices;
using Common.Inventories.Splinters;
using Common.Resourses;
using System.Linq;
using UIController.Inventory;
using UIController.Rewards;
using VContainer;

namespace Services
{
    public class RewarderService
    {
        [Inject] private readonly InventoryController _inventoryController;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        public void AddReward(RewardData reward)
        {
            var resources = reward.Objects.Where(obj => obj is GameResource).Cast<GameResource>().ToList();
            var items = reward.Objects.Where(obj => obj is GameItem).Cast<GameItem>().ToList();
            var splinters = reward.Objects.Where(obj => obj is GameSplinter).Cast<GameSplinter>().ToList();

            _resourceStorageController.AddResource(resources);
            _inventoryController.Add(items);
            _inventoryController.Add(splinters);
        }
    }
}
