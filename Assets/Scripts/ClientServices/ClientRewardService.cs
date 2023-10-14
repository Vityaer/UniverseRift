using Assets.Scripts.ClientServices;
using City.Panels.Rewards;
using Common.Resourses;
using Common.Rewards;
using Db.CommonDictionaries;
using System;
using System.Linq;
using UIController.Inventory;
using UniRx;
using VContainer;

namespace ClientServices
{
    public class ClientRewardService
    {
        [Inject] private readonly InventoryController _inventoryController;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly RewardPanelController _rewardPanelController;


        private IDisposable _disposable;
        public ReactiveCommand OnGetReward = new ReactiveCommand();

        public void ShowReward(GameReward reward)
        {
            _disposable = _rewardPanelController.OnClose.Subscribe(_ => GetReward(reward));
            _rewardPanelController.Open(reward);
        }

        public void GetReward(GameReward reward)
        {
            _disposable?.Dispose();

            var resources = reward.Objects.Where(obj => obj is GameResource).Select(obj => (GameResource)obj).ToList();
            _resourceStorageController.AddResource(resources);

            var items = reward.Objects.Where(obj => obj is GameItem).Select(obj => (GameItem)obj).ToList();

            foreach (var item in items)
            {
                item.Model = _commonDictionaries.Items[item.Id];
            }

            _inventoryController.Add(items);
            OnGetReward.Execute();
        }
    }
}
