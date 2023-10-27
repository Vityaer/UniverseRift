using City.Buildings.CityButtons.DailyReward;
using City.Buildings.CityButtons.EventAgent;
using City.Buildings.Market;
using City.Panels.BatllepasPanels;
using City.Panels.DailyRewards;
using ClientServices;
using Common.Rewards;
using Db.CommonDictionaries;
using Models;
using Models.Common;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UiExtensions.Scroll.Interfaces;
using UnityEngine;
using VContainer;

namespace City.Buildings.CityButtons
{
    public class DailyRewardPanelController : UiPanelController<DailyRewardPanelView>
    {
        private const string DAILY_REWARDS_CONTAIER_NAME = "DailyRewards";

        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] protected readonly ClientRewardService _clientRewardService;

        public List<DailyRewardUI> _rewardControllers = new List<DailyRewardUI>();

        protected override void OnLoadGame()
        {
            var index = 0;
            foreach (var rewardModel in _commonDictionaries.RewardContainerModels[DAILY_REWARDS_CONTAIER_NAME].Rewards)
            {
                var rewardViewPrefab = UnityEngine.Object.Instantiate(View.RewardPrefab, View.Content);

                var data = new GameReward(rewardModel);
                rewardViewPrefab.SetData(data, View.Scroll);
                var status = (index <= CommonGameData.BattlepasData.CurrentDailyBattlepasStage)
                    ?
                    ScrollableViewStatus.Completed
                    :
                    ScrollableViewStatus.Open;

                rewardViewPrefab.SetStatus(status);
                _rewardControllers.Add(rewardViewPrefab);

            }
        }
    }
}