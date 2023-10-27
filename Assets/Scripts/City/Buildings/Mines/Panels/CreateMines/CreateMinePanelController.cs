using City.Buildings.Mines.Panels.CreateMines;
using ClientServices;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Misc.Json;
using Models.City.Mines;
using Models.Common;
using Network.DataServer;
using Network.DataServer.Messages.City.Mines;
using System;
using System.Collections.Generic;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Buildings.Mines
{
    public class CreateMinePanelController : UiPanelController<CreateMinePanelView>, IStartable
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly CommonGameData _ñommonGameData;
        [Inject] private readonly IUiMessagesPublisherService _uiMessagesPublisher;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly IJsonConverter _jsonConverter;

        private PlaceForMine _place;
        private MineData _data;
        private MineCard _currentMineCard;
        private ReactiveCommand<MineData> _onMineCreate = new();
        private List<GameResource> _cost = new();

        public IObservable<MineData> OnMineCreate => _onMineCreate;

        public override void Start()
        {
            foreach (var card in View.MineCards)
            {
                card.OnSelect.Subscribe(MineCardClick).AddTo(Disposables);
            }
            View.CreateButton.OnClickAsObservable().Subscribe(_ => CreateNewMine(_place, _currentMineCard.Model).Forget()).AddTo(Disposables);
            base.Start();
        }

        private void MineCardClick(MineCard mineCard)
        {
            if (mineCard.GetCanCreateFromCount)
            {
                if (_currentMineCard != null)
                    _currentMineCard.Diselect();

                _currentMineCard = mineCard;
                _currentMineCard.Select();
                CheckCreate();
            }
        }

        private void CheckCreate()
        {
            _cost.Clear();
            var costData = _currentMineCard.Model.CreateCost;
            foreach (var cost in costData)
                _cost.Add(new GameResource(cost));

            View.CostController.ShowCosts(_cost);

            var enoughResource = _resourceStorageController.CheckResource(_cost);
            View.CreateButton.interactable = enoughResource;
        }

        protected override void OnLoadGame()
        {
            var restrictionModels = _commonDictionaries.MineRestrictions;
            var mineDatas = _ñommonGameData.City.IndustrySave.Mines;

            var index = 0;
            foreach (var model in restrictionModels.Values)
            {
                var mineModel = _commonDictionaries.Mines[model.MineId];
                var currentCount = mineDatas.FindAll(data => data.MineId == model.MineId).Count;
                var restriction = new GameMineRestriction(model, currentCount);
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
            _place = placeMine;

            for (int i = placeMine.Types.Count; i < View.MineCards.Count; i++)
            {
                View.MineCards[i].Hide();
            }

            var startSelectCard = View.MineCards.Find(x => x.GetCanCreateFromCount == true);
            MineCardClick(startSelectCard);

            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<CreateMinePanelController>(openType: OpenType.Additive);
        }

        public async UniTaskVoid CreateNewMine(PlaceForMine place, MineModel model)
        {
            var message = new CreateNewMineMessage { PlayerId = CommonGameData.PlayerInfoData.Id, MineModelId = model.Id, PlaceId = place.Id };
            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                var mineData = _jsonConverter.FromJson<MineData>(result);
                _onMineCreate.Execute(mineData);
                _resourceStorageController.SubtractResource(_cost);
                Close();
            }
        }
    }
}