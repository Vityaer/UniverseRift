using Assets.Scripts.City.Buildings.General;
using Assets.Scripts.City.Buildings.Market;
using City.Buildings.CityButtons.DailyReward;
using City.Buildings.Market;
using Common;
using Common.Resourses;
using Models;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UIController;
using UIController.GameSystems;
using UIController.Inventory;
using UIController.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.CityButtons
{
    public class DailyController : Building
    {
        private const char SYMBOL_1 = '1';
        private const string SUM_RECEIVIED_REWARD = "SumReward";
        private const string ID_CURRENT_REWARD = "IDCurReward";
        private const string DATA_LAST_CHECK = "DataLastCheck";

        private List<int> idReceivedReward = new List<int>();

        private SimpleBuildingModel dailyReward = null;
        [OdinSerialize] private List<BaseMarketProduct> listRewards = new List<BaseMarketProduct>();
        public List<DailyRewardUI> dailyRewardUI = new List<DailyRewardUI>();
        public SliderTime sliderTime;
        public ParentScroll scrollRectController;
        private int sumReward = 0, IDCurReward = 0;
        private DateTime dateLastCheck;
        private TimeSpan timeForNextOpenReward = new TimeSpan(1, 0, 0, 0);
        private bool isFillData = false;

        void Awake()
        {
            instance = this;
        }

        protected override void OnLoadGame()
        {
            dailyReward = GameController.GetCitySave.dailyReward;
            sumReward = dailyReward.GetRecordInt(SUM_RECEIVIED_REWARD, 0);
            IDCurReward = dailyReward.GetRecordInt(ID_CURRENT_REWARD, 1);
            dateLastCheck = dailyReward.GetRecordDate(DATA_LAST_CHECK);
            RepackReceivedReward(sumReward);
            TimeControllerSystem.Instance.RegisterOnNewCycle(OpenNextReward, CycleRecover.Day);
            sliderTime.SetData(dateLastCheck, timeForNextOpenReward);
            SetAllRewardsAvailable();
        }

        private void SetAllRewardsAvailable()
        {
            for (int i = 0; i < IDCurReward; i++)
            {
                dailyRewardUI[i].SetStatus(EventAgentRewardStatus.Open);
            }

            for (int i = 0; i < idReceivedReward.Count; i++)
            {
                dailyRewardUI[idReceivedReward[i]].SetStatus(EventAgentRewardStatus.Received);
            }
        }

        public void OpenNextReward()
        {
            IDCurReward += 1;
            dateLastCheck = TimeControllerSystem.Instance.GetDayCycle;
            dailyReward.SetRecordInt(ID_CURRENT_REWARD, IDCurReward);
            dailyReward.SetRecordDate(DATA_LAST_CHECK, dateLastCheck);
            SaveGame();
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
            dailyReward.SetRecordInt(SUM_RECEIVIED_REWARD, sumReward);
            SaveGame();
        }

        protected override void OpenPage()
        {
            if (isFillData == false)
            {
                for (int i = 0; i < listRewards.Count; i++)
                {
                    dailyRewardUI[i].SetData(listRewards[i]);
                }
                isFillData = true;
            }
        }

        [Button] public void AddResource() { listRewards.Add(new MarketProduct<Resource>()); }
        [Button] public void AddSplinter() { listRewards.Add(new MarketProduct<SplinterModel>()); }
        [Button] public void AddItem() { listRewards.Add(new MarketProduct<Item>()); }

        //Save
        private static DailyController instance;
        public static DailyController Instance { get => instance; }
    }
}