using Cysharp.Threading.Tasks;
using Misc.Json;
using Models.Battlepases;
using Models.Data;
using Models.Data.Heroes;
using Models.Data.Inventories;
using Models.Data.Players;
using Models.Misc;
using Models.Misc.Temp;
using Network.DataServer;
using Network.DataServer.Messages.Common;
using Sirenix.Utilities;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

namespace Models.Common
{
    [System.Serializable]
    public class CommonGameData : BaseDataModel
    {
        private const string PLAYER_ID_KEY = "PlayerId";

        [Inject] private readonly IJsonConverter _jsonConverter;

        public CityData City = new();
        public PlayerData PlayerInfoData = new();
        public List<TaskData> ListTasks = new();
        public AchievmentStorageData AchievmentStorage = new();
        public CycleEventsData CycleEventsData = new();
        public HeroesStorage HeroesStorage = new();
        public List<ResourceData> Resources = new();
        public InventoryData InventoryData = new();
        public BattlepasData BattlepasData = new();
        public CommunicationData CommunicationData = new();

        public TemporallyData TemporallyData = new();

        public ReactiveCommand OnStartLoadData = new();
        public ReactiveCommand OnFinishLoadData = new();
        public ReactiveCommand OnLoadedData = new();
        public bool IsInited { get; private set; } = false;

        public async UniTaskVoid Init(int playerId)
        {
            var message = new GetPlayerSaveMessage { PlayerId = playerId };
            OnStartLoadData.Execute();
            
            var result = await DataServer.PostData(message);
            if (result.IsNullOrWhitespace())
                return;

            OnFinishLoadData.Execute();
            var data = _jsonConverter.Deserialize<CommonGameData>(result);

            City = data.City;
            PlayerInfoData = data.PlayerInfoData;
            ListTasks = data.ListTasks;
            AchievmentStorage = data.AchievmentStorage;
            CycleEventsData = data.CycleEventsData;
            HeroesStorage = data.HeroesStorage;
            InventoryData = data.InventoryData;
            Resources = data.Resources;
            BattlepasData = data.BattlepasData;
            CommunicationData = data.CommunicationData;

            if (!PlayerPrefs.HasKey(PLAYER_ID_KEY))
            {
                PlayerPrefs.SetInt(PLAYER_ID_KEY, PlayerInfoData.Id);
            }

            IsInited = true;
            OnLoadedData.Execute();
            Debug.Log($"data loaded, PLAYER_ID: {PlayerInfoData.Id}");
        }
    }
}