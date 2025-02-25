using Campaign.GoldHeaps;
using City.Panels.AutoFights;
using ClientServices;
using Common;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using DG.Tweening;
using LocalizationSystems;
using Misc.Json;
using Models.Common;
using Models.Common.BigDigits;
using Models.Data.Rewards;
using Network.DataServer;
using Network.DataServer.Messages.Campaigns;
using Network.GameServer;
using Services.TimeLocalizeServices;
using System;
using System.Threading;
using UIController.Rewards;
using UniRx;
using UnityEngine;
using Utils;
using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace Campaign
{
    public class GoldHeapController : UiController<GoldHeapView>, IInitializable, IDisposable
    {
        private const string NAME_RECORD_AUTOFIGHT_PREVIOUS_DATETIME = "AutoFight";
        private const float ANIMATION_TIME = 0.25f;
        private const int TACT_MIN_COUNT = 2;
        private TimeSpan maxTime = new TimeSpan(12, 0, 0);

        [Inject] private readonly AutoFightRewardPanelController _autoFightRewardPanelController;
        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] private readonly GameController _gameController;
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly ILocalizationSystem _localizationSystem;
        [Inject] private readonly TimeLocalizeService _timeLocalizeService;

        private DateTime _previousDateTime;
        private bool _isHavePreviousDateTime;

        private AutoRewardData _autoReward;
        private GameReward _calculatedReward;
        private Tween _tween;
        private ReactiveCommand<BigDigit> _observerGetHour = new();
        private CancellationTokenSource _cancellationTokenSource = new();
        private CompositeDisposable _disposables = new();
        private IDisposable _disposable;
        private int _missionIndex;

        public IObservable<BigDigit> OnRewardGetHour => _observerGetHour;

        public void Initialize()
        {
            _gameController.OnLoadedGameData.Subscribe(_ => OnLoadGame()).AddTo(_disposables);
            View.HeapButton.OnClickAsObservable().Subscribe(_ => OnClickHeap()).AddTo(_disposables);
            OffGoldHeap();
            View.SliderAccumulation.gameObject.SetActive(false);
        }

        protected override void OnLoadGame()
        {
            if (!string.IsNullOrEmpty(_commonGameData.City.MainCampaignSave.LastGetAutoFightReward))
            {
                _previousDateTime = TimeUtils.ParseTime(_commonGameData.City.MainCampaignSave.LastGetAutoFightReward);
                _isHavePreviousDateTime = true;
            }
        }

        public void SetNewReward(AutoRewardData newAutoReward, int missionIndex)
        {
            if (newAutoReward != null)
            {
                if (!_isHavePreviousDateTime)
                {
                    if (!string.IsNullOrEmpty(_commonGameData.City.MainCampaignSave.LastGetAutoFightReward))
                    {
                        _previousDateTime = TimeUtils.ParseTime(_commonGameData.City.MainCampaignSave.LastGetAutoFightReward);
                    }
                    else
                    {
                        _previousDateTime = DateTime.UtcNow;
                    }

                    _isHavePreviousDateTime = true;
                }

                _autoReward = newAutoReward;
                _missionIndex = missionIndex;
                View.SliderAccumulation.gameObject.SetActive(true);
                View.SliderAccumulation.Init(_localizationSystem, _timeLocalizeService);
                View.SliderAccumulation.SetData(_previousDateTime, maxTime);
            }
            CheckSprite();
        }

        private void OnClickHeap()
        {
            if (_autoReward != null)
            {
                CalculateReward().Forget();
            }
        }

        public void GetReward()
        {
            var delta = DateTime.UtcNow - _previousDateTime;
            _observerGetHour.Execute(new BigDigit((float) delta.TotalHours));
            _disposable.Dispose();
            _previousDateTime = DateTime.UtcNow;
            _commonGameData.City.MainCampaignSave.LastGetAutoFightReward = _previousDateTime.ToString(Constants.Common.DateTimeFormat);
            View.SliderAccumulation.SetData(_previousDateTime, maxTime);
            OffGoldHeap();
        }

        private async UniTaskVoid CalculateReward()
        {
            var message = new GetAutoFightRewardMessage { PlayerId = _commonGameData.PlayerInfoData.Id, NumMission = _missionIndex };
            var result = await DataServer.PostData(message);

            var rewardModel = _jsonConverter.Deserialize<RewardModel>(result);
            _calculatedReward = new GameReward(rewardModel, _commonDictionaries);

            _disposable = _autoFightRewardPanelController.OnClose.Subscribe(_ => GetReward());
            _autoFightRewardPanelController.Open(_autoReward, _calculatedReward, _previousDateTime);
            OffGoldHeap();
        }

        public int CalculateCountTact(DateTime previousDateTime, int MaxCount = 8640, int lenthTact = 5)
        {
            var interval = FunctionHelp.CalculateTimeHasPassed(previousDateTime);
            var tact = (int)(interval.TotalSeconds) / lenthTact;
            tact = Math.Min(tact, MaxCount);
            return tact;
        }

        public override void OnShow()
        {
            if (_autoReward != null)
            {
                CheckSprite();
            }
        }

        public void OnCloseSheet()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void CheckSprite()
        {
            if (_autoReward == null)
            {
                View.Image.enabled = false;
            }

            var tact = CalculateCountTact(_previousDateTime);
            if (tact >= TACT_MIN_COUNT && View.Image.enabled == false)
            {
                View.Image.enabled = true;
                _tween.Kill();
                _tween = View.Rect.DOScale(Vector2.one, ANIMATION_TIME);
            }
            View.Image.sprite = View.ListSprite.GetSprite(tact);

            TryCancelToken();
            _cancellationTokenSource = new CancellationTokenSource();
            WaitForCheck(_cancellationTokenSource.Token).Forget();
        }

        private void TryCancelToken()
        {
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }

        private async UniTaskVoid WaitForCheck(CancellationToken cancellationToken)
        {
            await UniTask.Delay(Constants.Game.TACT_TIME * 1000, cancellationToken: cancellationToken);
            CheckSprite();
        }

        private void OffGoldHeap()
        {
            TryCancelToken();
            _tween.Kill();
            _tween = View.Rect.DOScale(Vector2.zero, 0.25f).OnComplete(OffSprite);
        }

        private void OffSprite()
        {
            View.Image.enabled = false;
        }

        public void Dispose()
        {
            _tween.Kill();
            _disposables.Dispose();
        }
    }
}