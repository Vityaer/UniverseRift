using System;
using System.Collections.Generic;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Inventories.Resourses;
using Cysharp.Threading.Tasks;
using Misc.Json;
using Models.City.Mines;
using Models.Common;
using Models.Data.Inventories;
using Network.DataServer;
using Network.DataServer.Messages.City.Mines;
using UiExtensions.Panels;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Buildings.Mines.Panels.CreateMines
{
    public class CreateMinePanelController : UiPanelController<CreateMinePanelView>, IStartable
    {
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly CommonGameData m_commonGameData;
        [Inject] private readonly IUiMessagesPublisherService m_uiMessagesPublisher;
        [Inject] private readonly ResourceStorageController m_resourceStorageController;
        [Inject] private readonly IJsonConverter m_jsonConverter;

        private readonly ReactiveCommand<MineData> m_onMineCreate = new();
        private readonly List<GameResource> m_cost = new();
        
        private PlaceForMine m_place;
        private MineData m_data;
        private MineCard m_currentMineCard;

        public IObservable<MineData> OnMineCreate => m_onMineCreate;

        public override void Start()
        {
            foreach (MineCard card in View.MineCards)
            {
                card.OnSelect.Subscribe(MineCardClick).AddTo(Disposables);
            }
            View.CreateButton.OnClickAsObservable().Subscribe(_ => CreateNewMine(m_place, m_currentMineCard.Model).Forget()).AddTo(Disposables);
            base.Start();
        }

        private void MineCardClick(MineCard mineCard)
        {
            if (!mineCard.GetCanCreateFromCount) return;
            if (m_currentMineCard != null)
                m_currentMineCard.Diselect();

            m_currentMineCard = mineCard;
            m_currentMineCard.Select();
            CheckCreate();
        }

        private void CheckCreate()
        {
            m_cost.Clear();
            List<ResourceData> costData = m_currentMineCard.Model.CreateCost;
            foreach (ResourceData cost in costData)
                m_cost.Add(new GameResource(cost));

            View.CostController.ShowCosts(m_cost);

            bool enoughResource = m_resourceStorageController.CheckResource(m_cost);
            View.CreateButton.interactable = enoughResource;
        }

        protected override void OnLoadGame()
        {
            Dictionary<string, MineRestrictionModel> restrictionModels = m_commonDictionaries.MineRestrictions;
            List<MineData> mineDatas = m_commonGameData.City.IndustrySave.Mines;

            int index = 0;
            foreach (MineRestrictionModel model in restrictionModels.Values)
            {
                if (!m_commonDictionaries.Mines.TryGetValue(model.MineId, out MineModel mineModel))
                {
                    Debug.LogError($"Not found Mine from {model.MineId}");
                    continue;
                }

                int currentCount = mineDatas.FindAll(data => data.MineId == model.MineId).Count;
                GameMineRestriction restriction = new GameMineRestriction(model, currentCount);
                View.MineCards[index].SetData(mineModel, restriction);
                index++;
            }
        }

        public void RefreshData()
        {
            OnLoadGame();
        }

        public void Open(PlaceForMine placeMine)
        {
            m_place = placeMine;

            foreach (MineCard card in View.MineCards)
            {

                if (card.Model == null)
                {
                    card.HidePanel();
                    continue;
                }

                if (placeMine.Types.Contains(card.Model.Type))
                {
                    card.ShowPanel();
                }
                else
                {
                    card.HidePanel();
                }
            }

            MineCard startSelectCard = View.MineCards.Find(x => x.IsOpen && x.GetCanCreateFromCount);
            MineCardClick(startSelectCard);

            m_uiMessagesPublisher.OpenWindowPublisher.OpenWindow<CreateMinePanelController>(openType: OpenType.Additive);
        }

        private async UniTaskVoid CreateNewMine(PlaceForMine place, MineModel model)
        {
            CreateNewMineMessage message = new CreateNewMineMessage { PlayerId = CommonGameData.PlayerInfoData.Id, MineModelId = model.Id, PlaceId = place.Id };
            string result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                var mineData = m_jsonConverter.Deserialize<MineData>(result);
                m_onMineCreate.Execute(mineData);
                m_resourceStorageController.SubtractResource(m_cost);
                Close();
            }
        }
    }
}