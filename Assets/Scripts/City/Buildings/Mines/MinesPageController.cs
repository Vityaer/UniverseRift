using System;
using System.Collections.Generic;
using City.Buildings.Abstractions;
using City.Buildings.Mines.Panels.Travels;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Misc.Json;
using Network.DataServer;
using Network.DataServer.Messages.City.Mines;
using UIController.Rewards;
using UniRx;
using UnityEngine;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Buildings.Mines
{
    public class MinesPageController : BaseBuilding<MinesView>
    {
        private const string MAIN_MINE_NAME = "MainMineBuilding";

        [Inject] private readonly InfoMinePanelController m_panelMineInfo;
        [Inject] private readonly CreateMinePanelController m_panelNewMineCreate;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly ClientRewardService m_clientRewardService;

        private List<MineData> m_mineDatas = new();

        public readonly ReactiveCommand OnGetResource = new();

        protected override void OnStart()
        {
            base.OnStart();
            foreach (var place in View.MinePlaces) place.OnClick.Subscribe(ClickMinePlace).AddTo(Disposables);
            m_panelNewMineCreate.OnMineCreate.Subscribe(OnCreateNewMine).AddTo(Disposables);
            m_panelMineInfo.OnMineDestroy.Subscribe(OnMineDestroy).AddTo(Disposables);

            View.MineTavelOpenButton.OnClickAsObservable().Subscribe(_ => OpenMineTravelPage()).AddTo(Disposables);
            View.CollectAllButton.OnClickAsObservable().Subscribe(_ => GetResources().Forget()).AddTo(Disposables);
        }

        private void OnMineDestroy(PlaceForMine place)
        {
            CommonGameData.City.IndustrySave.Mines.Remove(place.MineData);
            place.Clear();
            m_panelNewMineCreate.RefreshData();
        }

        private void OnCreateNewMine(MineData data)
        {
            CommonGameData.City.IndustrySave.Mines.Add(data);

            var placeForMineCreate = View.MinePlaces.Find(place => place.Id == data.PlaceId);

            var model = m_commonDictionaries.Mines[data.MineId];
            placeForMineCreate.CreateMine(model, data);
            m_panelNewMineCreate.RefreshData();

            View.CollectAllButton.interactable = (m_mineDatas.Count > 1);
        }

        private void ClickMinePlace(PlaceForMine place)
        {
            var mainMineData = CommonGameData.City.IndustrySave.Mines.Find(data => data.MineId == MAIN_MINE_NAME);
            if (place.MineData == null)
                m_panelNewMineCreate.Open(place);
            else
                m_panelMineInfo.Open(place, mainMineData);
        }

        protected override void OnLoadGame()
        {
            m_mineDatas = CommonGameData.City.IndustrySave.Mines;

            foreach (var mineData in m_mineDatas)
            {
                var place = View.MinePlaces.Find(x => x.Id == mineData.PlaceId);
                if (place == null)
                {
                    Debug.LogError($"place: {mineData.PlaceId} with ID = {mineData.Id} not found");
                    continue;
                }

                var mineModel = m_commonDictionaries.Mines[mineData.MineId];
                place.SetData(mineModel, mineData);
            }

            View.CollectAllButton.interactable = (m_mineDatas.Count > 1);
        }

        private void OpenMineTravelPage()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<MineTravelPanelController>(openType: OpenType.Exclusive);
        }

        private async UniTaskVoid GetResources()
        {
            var message = new MinesTakeAllResourceMessage { PlayerId = CommonGameData.PlayerInfoData.Id };
            string result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                var rewardModel = m_jsonConverter.Deserialize<RewardModel>(result);
                var calculatedReward = new GameReward(rewardModel, m_commonDictionaries);
                m_clientRewardService.ShowReward(calculatedReward);

                foreach (var mineData in CommonGameData.City.IndustrySave.Mines)
                    mineData.LastDateTimeGetIncome = DateTime.UtcNow.ToString();

                OnLoadGame();

                if (CommonGameData.City.IndustrySave.Mines.Count > 0)
                    OnGetResource.Execute();
            }
        }
    }
}