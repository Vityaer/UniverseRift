using City.Buildings.Abstractions;
using City.Buildings.Mines.Panels.Travels;
using City.TrainCamp.HeroPanels.HeroDetails;
using ClientServices;
using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Misc.Json;
using Models.City.Mines;
using Network.DataServer;
using Network.DataServer.Messages.City.Mines;
using System;
using System.Collections.Generic;
using Common.Db.CommonDictionaries;
using UIController.Rewards;
using UniRx;
using UnityEngine;
using VContainer;
using VContainerUi.Model;
using VContainerUi.Messages;

namespace City.Buildings.Mines
{
    public class MinesPageController : BaseBuilding<MinesView>
    {
        private const string MAIN_MINE_NAME = "MainMineBuilding";

        [Inject] private readonly InfoMinePanelController _panelMineInfo;
        [Inject] private readonly CreateMinePanelController _panelNewMineCreate;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly ClientRewardService _clientRewardService;

        private List<MineData> _mineDatas = new();

        public ReactiveCommand OnGetResource = new();

        protected override void OnStart()
        {
            base.OnStart();
            foreach (var place in View.MinePlaces)
            {
                place.OnClick.Subscribe(ClickMinePlace).AddTo(Disposables);
            }
            _panelNewMineCreate.OnMineCreate.Subscribe(OnCreateNewMine).AddTo(Disposables);
            _panelMineInfo.OnMineDestroy.Subscribe(OnMineDestroy).AddTo(Disposables);

            View.MineTavelOpenButton.OnClickAsObservable().Subscribe(_ => OpenMineTravelPage()).AddTo(Disposables);
            View.CollectAllButton.OnClickAsObservable().Subscribe(_ => GetResources().Forget()).AddTo(Disposables);
        }

        private void OnMineDestroy(PlaceForMine place)
        {
            CommonGameData.City.IndustrySave.Mines.Remove(place.MineData);
            place.Clear();
            _panelNewMineCreate.RefreshData();
        }

        private void OnCreateNewMine(MineData data)
        {
            CommonGameData.City.IndustrySave.Mines.Add(data);

            var placeForMineCreate = View.MinePlaces.Find(place => place.Id == data.PlaceId);

            var model = _commonDictionaries.Mines[data.MineId];
            placeForMineCreate.CreateMine(model, data);
            _panelNewMineCreate.RefreshData();

            View.CollectAllButton.interactable = (_mineDatas.Count > 1);
        }

        private void ClickMinePlace(PlaceForMine place)
        {
            var mainMineData = CommonGameData.City.IndustrySave.Mines.Find(data => data.MineId == MAIN_MINE_NAME);
            if (place.MineData == null)
            {
                _panelNewMineCreate.Open(place);
            }
            else
            {
                _panelMineInfo.Open(place, mainMineData);
            }
        }

        protected override void OnLoadGame()
        {
            _mineDatas = CommonGameData.City.IndustrySave.Mines;

            foreach (var mineData in _mineDatas)
            {
                var place = View.MinePlaces.Find(x => x.Id == mineData.PlaceId);
                if (place == null)
                {
                    Debug.LogError($"place: {mineData.PlaceId} with ID = {mineData.Id} not found");
                    continue;
                }

                var mineModel = _commonDictionaries.Mines[mineData.MineId];
                place.SetData(mineModel, mineData);
            }

            View.CollectAllButton.interactable = (_mineDatas.Count > 1);

        }

        private void OpenMineTravelPage()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<MineTravelPanelController>(openType: OpenType.Exclusive);
        }

        private async UniTaskVoid GetResources()
        {
            var message = new MinesTakeAllResourceMessage { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                var rewardModel = _jsonConverter.Deserialize<RewardModel>(result);
                var calculatedReward = new GameReward(rewardModel, _commonDictionaries);
                _clientRewardService.ShowReward(calculatedReward);

                foreach (var mineData in CommonGameData.City.IndustrySave.Mines)
                {
                    mineData.LastDateTimeGetIncome = DateTime.UtcNow.ToString();
                }

                OnLoadGame();

                if(CommonGameData.City.IndustrySave.Mines.Count > 0)
                    OnGetResource.Execute();
            }
        }
    }
}