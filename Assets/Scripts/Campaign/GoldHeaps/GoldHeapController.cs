using System;
using System.Threading;
using City.Panels.AutoFights;
using Common;
using Common.Db.CommonDictionaries;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using LocalizationSystems;
using Misc.Json;
using Models.Common;
using Models.Common.BigDigits;
using Models.Data.Rewards;
using Network.DataServer;
using Network.DataServer.Messages.Campaigns;
using Plugins.VContainerUi.Runtime.Abstraction;
using Services.TimeLocalizeServices;
using UIController.Rewards;
using UniRx;
using UnityEngine;
using Utils;
using Utils.AsyncUtils;
using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace Campaign.GoldHeaps
{
    public class GoldHeapController : UiController<GoldHeapView>, IInitializable, IDisposable
    {
        private const float ANIMATION_TIME = 0.25f;
        private const int TACT_MIN_COUNT = 2;
        private readonly TimeSpan m_maxTime = new TimeSpan(12, 0, 0);

        [Inject] private readonly AutoFightRewardPanelController m_autoFightRewardPanelController;
        [Inject] private readonly CommonGameData m_commonGameData;
        [Inject] private readonly GameController m_gameController;
        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly ILocalizationSystem m_localizationSystem;
        [Inject] private readonly TimeLocalizeService m_timeLocalizeService;

        private DateTime m_previousDateTime;
        private bool m_isHavePreviousDateTime;

        private AutoRewardData m_autoReward;
        private GameReward m_calculatedReward;
        private Tween m_tween;
        private readonly ReactiveCommand<BigDigit> m_observerGetHour = new();
        private CancellationTokenSource m_cancellationTokenSource = new();
        private readonly CompositeDisposable m_disposables = new();
        private IDisposable m_disposable;
        private int m_missionIndex;

        public IObservable<BigDigit> OnRewardGetHour => m_observerGetHour;

        public void Initialize()
        {
            m_gameController.OnLoadedGameData.Subscribe(_ => OnLoadGame()).AddTo(m_disposables);
            View.HeapButton.OnClickAsObservable().Subscribe(_ => OnClickHeap()).AddTo(m_disposables);
            OffGoldHeap();
            View.SliderAccumulation.gameObject.SetActive(false);
        }

        protected override void OnLoadGame()
        {
            if (!string.IsNullOrEmpty(m_commonGameData.City.MainCampaignSave.LastGetAutoFightReward))
            {
                m_previousDateTime = TimeUtils.ParseTime(m_commonGameData.City.MainCampaignSave.LastGetAutoFightReward);
                m_isHavePreviousDateTime = true;
            }
        }

        public void SetNewReward(AutoRewardData newAutoReward, int missionIndex)
        {
            if (newAutoReward == null)
            {
                CheckSprite();
                return;
            }

            if (!m_isHavePreviousDateTime)
            {
                m_previousDateTime = !string.IsNullOrEmpty(m_commonGameData.City.MainCampaignSave.LastGetAutoFightReward)
                    ? TimeUtils.ParseTime(m_commonGameData.City.MainCampaignSave.LastGetAutoFightReward)
                    : DateTime.UtcNow;

                m_isHavePreviousDateTime = true;
            }

            m_autoReward = newAutoReward;
            m_missionIndex = missionIndex;
            View.SliderAccumulation.gameObject.SetActive(true);
            View.SliderAccumulation.Init(m_localizationSystem, m_timeLocalizeService);
            View.SliderAccumulation.SetData(m_previousDateTime, m_maxTime);
            CheckSprite();
        }

        private void OnClickHeap()
        {
            if (m_autoReward != null)
            {
                CalculateReward().Forget();
            }
        }

        private void GetReward()
        {
            var delta = DateTime.UtcNow - m_previousDateTime;
            m_observerGetHour.Execute(new BigDigit((float) delta.TotalHours));
            m_disposable.Dispose();
            m_previousDateTime = DateTime.UtcNow;
            m_commonGameData.City.MainCampaignSave.LastGetAutoFightReward = m_previousDateTime.ToString(Constants.Common.DateTimeFormat);
            View.SliderAccumulation.SetData(m_previousDateTime, m_maxTime);
            OffGoldHeap();
        }

        private async UniTaskVoid CalculateReward()
        {
            var message = new GetAutoFightRewardMessage { PlayerId = m_commonGameData.PlayerInfoData.Id, NumMission = m_missionIndex };
            var result = await DataServer.PostData(message);

            var rewardModel = m_jsonConverter.Deserialize<RewardModel>(result);
            m_calculatedReward = new GameReward(rewardModel, m_commonDictionaries);

            m_disposable = m_autoFightRewardPanelController.OnClose.Subscribe(_ => GetReward());
            m_autoFightRewardPanelController.Open(m_autoReward, m_calculatedReward, m_previousDateTime);
            OffGoldHeap();
        }

        private int CalculateCountTact(DateTime previousDateTime, int maxCount = 8640, int lenthTact = 5)
        {
            TimeSpan interval = FunctionHelp.CalculateTimeHasPassed(previousDateTime);
            int tact = (int)(interval.TotalSeconds) / lenthTact;
            tact = Math.Min(tact, maxCount);
            return tact;
        }

        public override void OnShow()
        {
            if (m_autoReward != null)
            {
                CheckSprite();
            }
        }

        public void OnCloseSheet()
        {
            m_cancellationTokenSource.Cancel();
            m_cancellationTokenSource.Dispose();
        }

        private void CheckSprite()
        {
            if (m_autoReward == null)
            {
                View.Image.enabled = false;
            }

            var tact = CalculateCountTact(m_previousDateTime);
            if (tact >= TACT_MIN_COUNT && View.Image.enabled == false)
            {
                View.Image.enabled = true;
                m_tween.Kill();
                m_tween = View.Rect.DOScale(Vector2.one, ANIMATION_TIME);
            }
            View.Image.sprite = View.ListSprite.GetSprite(tact);

            TryCancelToken();
            m_cancellationTokenSource = new CancellationTokenSource();
            WaitForCheck(m_cancellationTokenSource.Token).Forget();
        }

        private void TryCancelToken()
        {
            m_cancellationTokenSource?.TryCancel();
            m_cancellationTokenSource = null;
        }

        private async UniTaskVoid WaitForCheck(CancellationToken cancellationToken)
        {
            await UniTask.Delay(Constants.Game.TACT_TIME * 1000, cancellationToken: cancellationToken);
            CheckSprite();
        }

        private void OffGoldHeap()
        {
            TryCancelToken();
            m_tween?.Kill();
            m_tween = View.Rect.DOScale(Vector2.zero, 0.25f).OnComplete(OffSprite);
        }

        private void OffSprite()
        {
            View.Image.enabled = false;
        }

        public void Dispose()
        {
            m_tween.Kill();
            m_disposables.Dispose();
        }
    }
}