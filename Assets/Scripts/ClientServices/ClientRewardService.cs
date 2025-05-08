using System;
using System.Linq;
using City.Panels.Rewards;
using Common.Resourses;
using Common.Rewards;
using Db.CommonDictionaries;
using UIController.Inventory;
using UniRx;
using VContainer;

namespace ClientServices
{
    public class ClientRewardService
    {
        [Inject] private readonly GameInventory _gameInventory;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly RewardPanelController _rewardPanelController;

        private IDisposable _disposable;
        public ReactiveCommand OnGetReward = new ReactiveCommand();

        public void ShowReward(GameReward reward, RewardType rewardType = RewardType.Simple, bool fast = true)
        {
            if (rewardType == RewardType.Simple && reward.Objects.Count == 0)
                return;

            _disposable = _rewardPanelController.OnClose.Subscribe(_ => GetReward(reward));
            _rewardPanelController.Open(reward, rewardType, fast);
        }

        public void GetReward(GameReward reward)
        {
            _disposable?.Dispose();

            var resources = reward.Objects.Where(obj => obj is GameResource)
                .Select(obj => (GameResource)obj)
                .ToList();
            _resourceStorageController.AddResource(resources);

            var items = reward.Objects.Where(obj => obj is GameItem)
                .Select(obj => (GameItem)obj)
                .ToList();

            foreach (var item in items) item.Model = _commonDictionaries.Items[item.Id];

            foreach (var item in items) _gameInventory.Add(item);
            OnGetReward.Execute();
        }
    }
}