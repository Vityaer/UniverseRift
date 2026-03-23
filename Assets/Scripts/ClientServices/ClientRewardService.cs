using System;
using System.Collections.Generic;
using System.Linq;
using City.Panels.Inventories;
using City.Panels.Rewards;
using Common.Db.CommonDictionaries;
using Common.Inventories.Resourses;
using Common.Rewards;
using UIController.Inventory;
using UniRx;
using VContainer;

namespace ClientServices
{
    public class ClientRewardService
    {
        [Inject] private readonly GameInventory m_gameInventory;
        [Inject] private readonly ResourceStorageController m_resourceStorageController;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly RewardPanelController m_rewardPanelController;


        private IDisposable m_disposable;
        public readonly ReactiveCommand OnGetReward = new ReactiveCommand();

        public void ShowReward(GameReward reward, RewardType rewardType = RewardType.Simple, bool fast = true)
        {
            if (reward == null) throw new ArgumentNullException(nameof(reward));

            if (rewardType == RewardType.Simple && reward.Objects.Count == 0)
                return;

            m_disposable = m_rewardPanelController.OnClose.Subscribe(_ => GetReward(reward));
            m_rewardPanelController.Open(reward, rewardType, fast);
        }

        public void GetReward(GameReward reward)
        {
            m_disposable?.Dispose();

            List<GameResource> resources = reward.Objects
                .OfType<GameResource>()
                .ToList();

            m_resourceStorageController.AddResource(resources);

            List<GameItem> items = reward.Objects
                .OfType<GameItem>()
                .ToList();

            foreach (GameItem item in items) item.Model = m_commonDictionaries.Items[item.Id];

            foreach (GameItem item in items) m_gameInventory.Add(item);
            OnGetReward.Execute();
        }
    }
}