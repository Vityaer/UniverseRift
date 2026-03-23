using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Inventories.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using LocalizationSystems;
using Misc.Json;
using Network.DataServer;
using Network.DataServer.Messages.City.Mines;
using UIController.Rewards;
using UiExtensions.Panels;
using UniRx;
using UnityEngine;
using Utils;
using Utils.AsyncUtils;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Buildings.Mines.Panels.MineInformations
{
    public class InfoMinePanelController : UiPanelController<InfoMinePanelView>
    {
        private const int INCOME_MAX_SECONDS = 36000;
        private const int INCOME_MIN_SECONDS = 10;
        private const string MAIN_BUILDING_MINE_ID = "MainMineBuilding";

        [Inject] private readonly ILocalizationSystem m_localizationSystem;
        [Inject] private readonly IUiMessagesPublisherService m_uiMessagesPublisher;
        [Inject] private readonly ResourceStorageController m_resourceStorageController;
        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly ClientRewardService m_clientRewardService;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;

        private PlaceForMine m_place;
        private List<GameResource> m_cost;
        private MineData m_mainMineData;
        private readonly ReactiveCommand<PlaceForMine> m_onMineDestroy = new();
        private CancellationTokenSource m_cancellationTokenSource;

        public IObservable<PlaceForMine> OnMineDestroy => m_onMineDestroy;

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
            m_mainMineData = mainMineData;
            m_place = place;

            var canDestroasble = !place.MineModel.Id.Equals(MAIN_BUILDING_MINE_ID);
            View.OpenPanelDestroyMineButton.gameObject.SetActive(canDestroasble);
            View.InfoDataContainer.SetActive(canDestroasble);

            UpdateUI();
            m_uiMessagesPublisher.OpenWindowPublisher.OpenWindow<InfoMinePanelController>(openType: OpenType.Additive);
        }

        public override void OnHide()
        {
            m_cancellationTokenSource?.TryCancel();
            m_cancellationTokenSource = null;

            base.OnHide();
        }

        private async UniTaskVoid LevelUp()
        {
            var message = new MineLevelUpMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                MineId = m_place.MineData.Id
            };

            string result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                m_place.LevelUp();
                m_resourceStorageController.SubtractResource(m_cost);
                UpdateUI();
            }
        }

        private async UniTaskVoid GetResources()
        {
            Close();
            MineTakeResourceMessage message = new MineTakeResourceMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                MineId = m_place.MineData.Id
            };

            string result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                var rewardModel = m_jsonConverter.Deserialize<RewardModel>(result);
                var calculatedReward = new GameReward(rewardModel, m_commonDictionaries);
                m_clientRewardService.ShowReward(calculatedReward);
                m_place.MineData.LastDateTimeGetIncome = DateTime.UtcNow.ToString();
                UpdateUI();

                m_cancellationTokenSource?.TryCancel();
                m_cancellationTokenSource = new();
                WaitCanCollect(INCOME_MIN_SECONDS, m_cancellationTokenSource.Token).Forget();
            }
        }

        private void UpdateUI()
        {
            View.NameMineText.StringReference = m_localizationSystem
                .GetLocalizedContainer($"{m_place.MineModel.Id}Name");

            View.LevelMineText.text = $"{m_place.MineData.Level}";

            var income = m_place.MineModel.IncomesContainer.GetCostForLevelUp(m_place.MineData.Level)[0];
            var levelUpCost = m_place.MineModel.CostLevelUpContainer.GetCostForLevelUp(m_place.MineData.Level + 1);

            View.MainImage.sprite = m_place.MineModel.SpritePath.LoadSpriteFromResources();

            View.IncomeText.text = income.GetTextAmount();

            var startDateTime = DateTime.ParseExact(
                    m_place.MineData.LastDateTimeGetIncome,
                    Constants.Common.DateTimeFormat,
                    CultureInfo.InvariantCulture
                    );

            var currentTimeSpan = DateTime.UtcNow - startDateTime;

            if (currentTimeSpan.TotalSeconds > INCOME_MAX_SECONDS)
                currentTimeSpan = new TimeSpan(0, 0, INCOME_MAX_SECONDS);

            if (currentTimeSpan.TotalSeconds < INCOME_MIN_SECONDS)
            {
                m_cancellationTokenSource?.Cancel();
                m_cancellationTokenSource?.Dispose();
                m_cancellationTokenSource = null;

                m_cancellationTokenSource = new();
                WaitCanCollect(INCOME_MIN_SECONDS - currentTimeSpan.TotalSeconds, m_cancellationTokenSource.Token).Forget();
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
            m_cost = levelUpCost;

            var requireLevel = (m_mainMineData.Level * 2) >= (m_place.MineData.Level + 1);
            if (!requireLevel)
            {
                View.RequireLevelText.text = $"Require main building level {m_mainMineData.Level + 1}";
                View.LevelUpButton.interactable = false;
                View.PanelController.SetActive(false);
            }
            else
            {
                View.RequireLevelText.text = string.Empty;
                View.LevelUpButton.interactable = m_resourceStorageController.CheckResource(levelUpCost);
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
            var message = new MineDestroyMessage { PlayerId = CommonGameData.PlayerInfoData.Id, MineId = m_place.MineData.Id };
            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                m_onMineDestroy.Execute(m_place);
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
            m_cancellationTokenSource.TryCancel();
        }
    }
}