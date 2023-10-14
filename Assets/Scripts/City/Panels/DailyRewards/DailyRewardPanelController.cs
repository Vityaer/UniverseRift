using City.Buildings.CityButtons.DailyReward;
using City.Buildings.CityButtons.EventAgent;
using City.Buildings.Market;
using City.Panels.DailyRewards;
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
        [Inject] private readonly CommonGameData _commonGameData;

        private const char SYMBOL_1 = '1';
        private const string SUM_RECEIVIED_REWARD = "SumReward";
        private const string ID_CURRENT_REWARD = "IDCurReward";
        private const string DATA_LAST_CHECK = "DataLastCheck";

        private List<int> idReceivedReward = new List<int>();

        private SimpleBuildingData dailyReward = null;
        public List<DailyRewardUI> _rewardControllers = new List<DailyRewardUI>();
        private int sumReward = 0, IDCurReward = 0;
        private DateTime dateLastCheck;
        private TimeSpan timeForNextOpenReward = new TimeSpan(1, 0, 0, 0);
        private bool isFillData = false;

        protected override void OnLoadGame()
        {
            //dailyReward = _commonGameData.City.DailyReward;
            //sumReward = dailyReward.IntRecords.GetRecord(SUM_RECEIVIED_REWARD, 0);
            //IDCurReward = dailyReward.IntRecords.GetRecord(ID_CURRENT_REWARD, 1);
            //dateLastCheck = dailyReward.DateRecords.GetRecord(DATA_LAST_CHECK);
            //RepackReceivedReward(sumReward);
            //sliderTime.SetData(dateLastCheck, timeForNextOpenReward);
            //SetAllRewardsAvailable();
        }

        private void SetAllRewardsAvailable()
        {
            for (int i = 0; i < IDCurReward; i++)
            {
                _rewardControllers[i].SetStatus(DailyTaskRewardStatus.Open);
            }

            for (int i = 0; i < idReceivedReward.Count; i++)
            {
                _rewardControllers[idReceivedReward[i]].SetStatus(DailyTaskRewardStatus.Received);
            }
        }

        public void OpenNextReward()
        {
            IDCurReward += 1;
            //dateLastCheck = TimeControllerSystem.Instance.GetDayCycle;
            dailyReward.IntRecords.SetRecord(ID_CURRENT_REWARD, IDCurReward);
            dailyReward.DateRecords.SetRecord(DATA_LAST_CHECK, dateLastCheck);
            //Utils.TextUtils.Save(_ñommonGameData);
        }


        private void RepackReceivedReward(int sum)
        {
            string binaryCode = Convert.ToString(sum, 2);

            for (int i = 0; i < binaryCode.Length; i++)
                if (binaryCode[i].Equals(SYMBOL_1))
                    idReceivedReward.Add(i);
        }

        //API
        public void OnGetReward(int ID)
        {
            Debug.Log("Id: " + ID.ToString());
            idReceivedReward.Add(ID);
            sumReward += (int)Math.Pow(2, ID);
            dailyReward.IntRecords.SetRecord(SUM_RECEIVIED_REWARD, sumReward);
            //SaveGame();
        }
    }
}