using City.Buildings.CityButtons.DailyReward;
using City.Buildings.CityButtons.EventAgent;
using City.Buildings.Market;
using City.Panels.DailyRewards;
using Common;
using Common.Inventories.Splinters;
using Common.Resourses;
using Models;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UIController;
using UIController.GameSystems;
using UIController.Inventory;
using UiExtensions.Scroll.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.CityButtons
{
    public class DailyRewardPanelController : UiPanelController<DailyRewardPanelView>
    {
        private const char SYMBOL_1 = '1';
        private const string SUM_RECEIVIED_REWARD = "SumReward";
        private const string ID_CURRENT_REWARD = "IDCurReward";
        private const string DATA_LAST_CHECK = "DataLastCheck";

        private List<int> idReceivedReward = new List<int>();

        private SimpleBuildingData dailyReward = null;
        [OdinSerialize] private List<BaseMarketProduct> listRewards = new List<BaseMarketProduct>();
        public List<DailyRewardUI> dailyRewardUI = new List<DailyRewardUI>();
        public SliderTime sliderTime;
        private int sumReward = 0, IDCurReward = 0;
        private DateTime dateLastCheck;
        private TimeSpan timeForNextOpenReward = new TimeSpan(1, 0, 0, 0);
        private bool isFillData = false;

        protected override void OnLoadGame()
        {
            //dailyReward = GameController.GetCitySave.DailyReward;
            //sumReward = dailyReward.IntRecords.GetRecord(SUM_RECEIVIED_REWARD, 0);
            //IDCurReward = dailyReward.IntRecords.GetRecord(ID_CURRENT_REWARD, 1);
            //dateLastCheck = dailyReward.DateRecords.GetRecord(DATA_LAST_CHECK);
            //RepackReceivedReward(sumReward);
            //TimeControllerSystem.Instance.RegisterOnNewCycle(OpenNextReward, CycleRecover.Day);
            //sliderTime.SetData(dateLastCheck, timeForNextOpenReward);
            //SetAllRewardsAvailable();
        }

        private void SetAllRewardsAvailable()
        {
            for (int i = 0; i < IDCurReward; i++)
            {
                dailyRewardUI[i].SetStatus(DailyTaskRewardStatus.Open);
            }

            for (int i = 0; i < idReceivedReward.Count; i++)
            {
                dailyRewardUI[idReceivedReward[i]].SetStatus(DailyTaskRewardStatus.Received);
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

        //protected override void OpenPage()
        //{
        //    if (isFillData == false)
        //    {
        //        for (int i = 0; i < listRewards.Count; i++)
        //        {
        //            dailyRewardUI[i].SetData(listRewards[i]);
        //        }
        //        isFillData = true;
        //    }
        //}

        [Button] public void AddResource() { listRewards.Add(new MarketProduct<GameResource>()); }
        [Button] public void AddSplinter() { listRewards.Add(new MarketProduct<GameSplinter>()); }
        [Button] public void AddItem() { listRewards.Add(new MarketProduct<GameItem>()); }
    }
}