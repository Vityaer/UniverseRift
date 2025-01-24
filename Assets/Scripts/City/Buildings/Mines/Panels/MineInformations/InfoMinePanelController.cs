using City.Buildings.Mines.Panels;
using ClientServices;
using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using LocalizationSystems;
using Misc.Json;
using Network.DataServer;
using Network.DataServer.Messages.City.Mines;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UIController.Rewards;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine;
using Utils;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Buildings.Mines
{
    public class InfoMinePanelController : UiPanelController<InfoMinePanelView>, IStartable, IDisposable
    {
        private const int INCOME_MAX_SECONDS = 36000;
        private const int INCOME_MIN_SECONDS = 10;
        private const string MAIN_BUILDING_MINE_ID = "MainMineBuilding";

        [Inject] private readonly ILocalizationSystem _localizationSystem;
        [Inject] private readonly IUiMessagesPublisherService _uiMessagesPublisher;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly ClientRewardService _clientRewardService;
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        private PlaceForMine _place;
        private List<GameResource> _cost;
        private MineData _mainMineData;
        private ReactiveCommand<PlaceForMine> _onMineDestroy = new();
        private CancellationTokenSource _cancellationTokenSource;

        public IObservable<PlaceForMine> OnMineDestroy => _onMineDestroy;

        public override void Start()
        {
            View.LevelUpButton.OnClickAsObservable().Subscribe(_ => LevelUp().Forget()).AddTo(Disposables);
            View.GetResourceButton.OnClickAsObservable().Subscribe(_ => GetResources().Forget()).AddTo(Disposables);

            View.OpenPanelDestroyMineButton.OnClickAsObservable().Subscribe(_ => OpenDestroyMessagePanel()).AddTo(Disposables);
            View.DestroyMineAgreeButton.OnClickAsObservable().Subscribe(_ => DestroyMine().Forget()).AddTo(Disposables);
            View.DestroyMineDisagreeButton.OnClickAsObservable().Subscribe(_ => CloseDestroyMessagePanel()).AddTo(Disposables);
            base.Start();
        }

        public void Open(PlaceForMine place, MineData mainMineData)
        {
            _mainMineData = mainMineData;
            _place = place;

            var canDestroasble = !place.MineModel.Id.Equals(MAIN_BUILDING_MINE_ID);
            View.OpenPanelDestroyMineButton.gameObject.SetActive(canDestroasble);

            UpdateUI();
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<InfoMinePanelController>(openType: OpenType.Additive);
        }

        public override void OnHide()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

            base.OnHide();
        }

        private async UniTaskVoid LevelUp()
        {
            var message = new MineLevelUpMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                MineId = _place.MineData.Id
            };

            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                _place.LevelUp();
                _resourceStorageController.SubtractResource(_cost);
                UpdateUI();
            }
        }

        private async UniTaskVoid GetResources()
        {
            Close();
            var message = new MineTakeResourceMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                MineId = _place.MineData.Id
            };

            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                var rewardModel = _jsonConverter.Deserialize<RewardModel>(result);
                var calculatedReward = new GameReward(rewardModel, _commonDictionaries);
                _clientRewardService.ShowReward(calculatedReward);
                _place.MineData.LastDateTimeGetIncome = DateTime.UtcNow.ToString();
                UpdateUI();

                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;

                _cancellationTokenSource = new();
                WaitCanCollect(INCOME_MIN_SECONDS, _cancellationTokenSource.Token).Forget();
            }
        }

        private void UpdateUI()
        {
            View.NameMineText.StringReference = _localizationSystem
                .GetLocalizedContainer($"{_place.MineModel.Id}Name");

            View.LevelMineText.text = $"{_place.MineData.Level}";

            var income = _place.MineModel.IncomesContainer.GetCostForLevelUp(_place.MineData.Level)[0];
            var levelUpCost = _place.MineModel.CostLevelUpContainer.GetCostForLevelUp(_place.MineData.Level + 1);

            View.MainImage.sprite = _place.MineModel.SpritePath.LoadSpriteFromResources();

            View.IncomeText.text = income.GetTextAmount();

            var startDateTime = DateTime.ParseExact(
                    _place.MineData.LastDateTimeGetIncome,
                    Constants.Common.DateTimeFormat,
                    CultureInfo.InvariantCulture
                    );

            var currentTimeSpan = DateTime.UtcNow - startDateTime;

            if (currentTimeSpan.TotalSeconds > INCOME_MAX_SECONDS)
                currentTimeSpan = new TimeSpan(0, 0, INCOME_MAX_SECONDS);

            if (currentTimeSpan.TotalSeconds < INCOME_MIN_SECONDS)
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;

                _cancellationTokenSource = new();
                WaitCanCollect(INCOME_MIN_SECONDS - currentTimeSpan.TotalSeconds, _cancellationTokenSource.Token).Forget();
            }
            else
            {
                View.GetResourceButton.interactable = true;
            }

            var timeFactor = (float)(currentTimeSpan.TotalSeconds / INCOME_MAX_SECONDS);
            var currentAmount = income.Amount * timeFactor;
            var currentResource = new GameResource(income.Type, currentAmount);

            View.SliderAmount.SetAmount(currentResource.Amount, income.Amount);
            View.CostController.ShowCosts(levelUpCost);
            _cost = levelUpCost;

            var requireLevel = (_mainMineData.Level * 2) >= (_place.MineData.Level + 1);
            if (!requireLevel)
            {
                View.RequireLevelText.text = $"Require main building level {_mainMineData.Level + 1}";
                View.LevelUpButton.interactable = false;
                View.PanelController.SetActive(false);
            }
            else
            {
                View.RequireLevelText.text = string.Empty;
                View.LevelUpButton.interactable = _resourceStorageController.CheckResource(levelUpCost);
                View.PanelController.SetActive(true);
            }
        }

        private async UniTaskVoid WaitCanCollect(double leftSecond, CancellationToken token)
        {
            View.GetResourceButton.interactable = false;
            var leftTime = Mathf.CeilToInt((float)leftSecond * 1000);

            await UniTask.Delay(leftTime, cancellationToken: token);
            UpdateUI();
        }

        private void OpenDestroyMessagePanel()
        {
            View.DestroyMinePanel.SetActive(true);
        }

        private async UniTaskVoid DestroyMine()
        {
            var message = new MineDestroyMessage { PlayerId = CommonGameData.PlayerInfoData.Id, MineId = _place.MineData.Id };
            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                _onMineDestroy.Execute(_place);
                View.DestroyMinePanel.SetActive(false);
                Close();
            }
        }

        private void CloseDestroyMessagePanel()
        {
            View.DestroyMinePanel.SetActive(false);
        }

        public override void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}