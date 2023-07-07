using Assets.Scripts.ClientServices;
using City.Panels.DailyTasks;
using City.Panels.Messages;
using Common;
using Common.Resourses;
using Models;
using Models.Common;
using System;
using System.Collections.Generic;
using UIController.Rewards;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace City.Buildings.CityButtons.EventAgent
{
    public class DailyTaskPanelController : UiPanelController<DailyTaskPanelView>, IInitializable
    {
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly CommonGameData _ñommonGameData;

        private const int REWARD_COUNT = 30;
        private const int REWARD_MONET_COST = 100;

        private static string SUM_RECEIVIED_REWARD = "SumReward";
        private const char SYMBOL_1 = '1';

        private int _currentProgress = 0;
        private int _sumReward = 0;
        private List<RewardData> listRewards;
        private List<int> idReceivedReward = new List<int>(REWARD_COUNT);
        private List<DailyTaskRewardView> _rewardsUi = new List<DailyTaskRewardView>(REWARD_COUNT);
        private SimpleBuildingData progressObjectSave;
        private bool isFillData = false;

        private int MaxCount => REWARD_COUNT * REWARD_MONET_COST;

        public void OnGetMonet(GameResource res)
        {
            _currentProgress = res.ConvertToInt();
            View.miniSliderAmount.SetAmount(OverMonet(), 100);
        }

        public void Initialize()
        {
            _resourceStorageController.Subscribe(ResourceType.EventAgentMonet, OnGetMonet).AddTo(Disposables);

            //for (var i = 0; i < REWARD_COUNT; i++)
            //{
            //    var rewardUi = UnityEngine.Object.Instantiate(View.PrefabRewardUi);
            //    _rewardsUi.Add(rewardUi);
            //}
        }

        protected override void OnLoadGame()
        {
            progressObjectSave = _ñommonGameData.Player.Requirements.EventAgentProgress;
            _sumReward = progressObjectSave.IntRecords.GetRecord(SUM_RECEIVIED_REWARD);
            RepackReceivedReward(_sumReward);
            var statusReward = DailyTaskRewardStatus.Close;
            var countOpenLevel = GetOpenLevel();

            for (int i = 0; i < listRewards.Count; i++)
            {
                if (i < countOpenLevel)
                {
                    if (idReceivedReward.Contains(i))
                    {
                        statusReward = DailyTaskRewardStatus.Received;
                    }
                    else
                    {
                        statusReward = DailyTaskRewardStatus.Open;
                    }
                }
                else
                {
                    statusReward = DailyTaskRewardStatus.Close;
                }

                _rewardsUi[i].SetStatus(statusReward);
            }

            OnGetMonet(_resourceStorageController.GetResource(ResourceType.EventAgentMonet));
            View.mainSliderController.SetValue(_currentProgress, MaxCount);
        }

        private void RepackReceivedReward(int sum)
        {
            string binaryCode = Convert.ToString(sum, 2);
            for (int i = 0; i < binaryCode.Length; i++)
                if (binaryCode[i].Equals(SYMBOL_1))
                    idReceivedReward.Add(i);
        }

        private int GetOpenLevel()
        {
            return _currentProgress / 100;
        }

        public void GetReward(DailyTaskRewardView dailyTaskRewardView)
        {
            switch (dailyTaskRewardView.Status)
            {
                case DailyTaskRewardStatus.Close:
                    //MessageController.Instance.AddMessage("Íàãðàäó åù¸ íóæíî çàñëóæèòü, ïðèõîäèòå ïîçæå");
                    break;
                case DailyTaskRewardStatus.Received:
                    //MessageController.Instance.AddMessage("Âû óæå ïîëó÷àëè ýòó íàãðàäó");
                    break;
                case DailyTaskRewardStatus.Open:
                    //GameController.Instance.AddReward(_reward);
                    var index = _rewardsUi.FindIndex(rewardUi => rewardUi == dailyTaskRewardView);
                    OnGetReward(index);
                    dailyTaskRewardView.SetStatus(DailyTaskRewardStatus.Received);
                    break;
            }
        }

        public override void OnShow()
        {
            //View.mainSliderController.SetNewValueWithAnim(_currentProgress);
            //if (isFillData == false)
            //{
            //    for (int i = 0; i < listRewards.Count; i++)
            //        _rewardsUi[i].SetData(listRewards[i], View.Scroll);
            //    isFillData = true;
            //}
        }

        public int GetMaxLevelReceivedReward()
        {
            int result = 0;
            for (int i = 0; i < idReceivedReward.Count; i++)
            {
                if (idReceivedReward[i] > result)
                {
                    result = idReceivedReward[i];
                }
            }
            return result;
        }

        public int OverMonet()
        {
            int maxLevel = GetMaxLevelReceivedReward();
            return _currentProgress - maxLevel * 100;
        }

        private void OnGetReward(int index)
        {
            idReceivedReward.Add(index);
            _sumReward += (int)Math.Pow(2, index);
            SaveData(_sumReward);
        }

        public void SaveData(int sum)
        {
            progressObjectSave.IntRecords.SetRecord(SUM_RECEIVIED_REWARD, sum);
            //GameController.Instance.SaveGame();
        }

    }
}