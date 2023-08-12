using City.Achievements;
using City.Buildings.Mines;
using Cysharp.Threading.Tasks;
using Misc.Json;
using Models.City.Markets;
using Models.City.Mines;
using Models.Data;
using Models.Data.Heroes;
using Models.Data.Inventories;
using Models.Data.Players;
using Network.DataServer;
using Network.DataServer.Messages.Common;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Models.Common
{
    [System.Serializable]
    public class CommonGameData : BaseDataModel
    {
        private const string PLAYER_ID_KEY = "PlayerId";

        [Inject] private readonly IJsonConverter _jsonConverter;

        public CityData City = new CityData();
        public PlayerData PlayerInfoData = new PlayerData();
        public List<TaskData> ListTasks = new();
        public AchievmentStorageData Requirements = new();
        public CycleEventsData CycleEventsData = new CycleEventsData();
        public HeroesStorage HeroesStorage = new HeroesStorage();
        public List<ResourceData> Resources = new();
        public InventoryData InventoryData = new();

        public bool IsInited { get; private set; } = false;

        public async UniTaskVoid Init(int playerId)
        {
            var message = new GetPlayerSaveMessage { PlayerId = playerId };
            var result = await DataServer.PostData(message);
            var data = _jsonConverter.FromJson<CommonGameData>(result);

            City = data.City;
            PlayerInfoData = data.PlayerInfoData;
            ListTasks = data.ListTasks;
            Requirements = data.Requirements;
            CycleEventsData = data.CycleEventsData;
            HeroesStorage = data.HeroesStorage;
            InventoryData = data.InventoryData;
            Resources = data.Resources;

            if (!PlayerPrefs.HasKey(PLAYER_ID_KEY))
            {
                PlayerPrefs.SetInt(PLAYER_ID_KEY, PlayerInfoData.Id);
            }

            IsInited = true;
            Debug.Log("data loaded");
        }
    }
}