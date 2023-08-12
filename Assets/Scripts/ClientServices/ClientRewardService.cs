using Assets.Scripts.ClientServices;
using Common.Resourses;
using Common.Rewards;
using Db.CommonDictionaries;
using System.Linq;
using UIController.Inventory;
using VContainer;

namespace ClientServices
{
    public class ClientRewardService
    {
        [Inject] private readonly InventoryController _inventoryController;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        public void AddReward(GameReward reward)
        {
            var resources = reward.Objects.Where(obj => obj is GameResource).Select(obj => (GameResource)obj).ToList();
            _resourceStorageController.AddResource(resources);

            var items = reward.Objects.Where(obj => obj is GameItem).Select(obj => (GameItem)obj).ToList();

            foreach(var item in items)
            {
                item.Model = _commonDictionaries.Items[item.Id];
            }

            _inventoryController.Add(items);
        }
    }
}
