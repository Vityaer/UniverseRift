using Campaign.GoldHeaps;
using City.Panels.AutoFights;
using ClientServices;
using Common;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Models.Common;
using Models.Common.BigDigits;
using Models.Data.Rewards;
using Network.GameServer;
using System;
using System.Threading;
using UIController.Rewards;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace Campaign
{
    public class GoldHeapController : UiController<GoldHeapView>, IInitializable, IDisposable
    {
        private const float ANIMATION_TIME = 0.25f;
        private const int TACT_MIN_COUNT = 2;

        [Inject] private readonly AutoFightRewardPanelController _autoFightRewardPanelController;
        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] private readonly GameController _gameController;
        [Inject] private readonly ClientRewardService _clientRewardService;
        
        private DateTime _previousDateTime;
        private AutoRewardData _autoReward;
        private GameReward _calculatedReward;
        private Tween _tween;
        private ReactiveCommand<BigDigit> _observerGetHour = new ReactiveCommand<BigDigit>();
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private CompositeDisposable _disposables = new CompositeDisposable();
        private IDisposable _disposable;

        public IObservable<BigDigit> ObserverGetHour => _observerGetHour;

        public void Initialize()
        {
            _gameController.OnLoadedGameData.Subscribe(_ => OnLoadGame()).AddTo(_disposables);
            View.HeapButton.OnClickAsObservable().Subscribe(_ => OnClickHeap()).AddTo(_disposables);
        }

        protected override void OnLoadGame()
        {
            _previousDateTime = _commonGameData.City.MainCampaignSave.DateRecords.GetRecord(nameof(_previousDateTime), DateTime.Now);
        }

        public void SetNewReward(AutoRewardData newAutoReward)
        {
            if (newAutoReward != null)
            {
                _autoReward = newAutoReward;
            }
        }

        private void OnClickHeap()
        {
            CalculateReward();
            if (_autoReward != null)
            {
                _disposable = _autoFightRewardPanelController.OnClose.Subscribe(_ => GetReward());
                _autoFightRewardPanelController.Open(_autoReward, _calculatedReward, _previousDateTime);
            }
            OffGoldHeap();
        }

        public void GetReward()
        {
            _disposable.Dispose();
            CalculateReward();
            //_previousDateTime = Client.Instance.GetServerTime();
            _previousDateTime = DateTime.Now;
            _clientRewardService.AddReward(_calculatedReward);
            _commonGameData.City.MainCampaignSave.DateRecords.SetRecord(nameof(_previousDateTime), _previousDateTime);
            CheckSprite();
        }

        private void CalculateReward()
        {
            if (_autoReward != null)
            {
                int tact = CalculateCountTact(_previousDateTime);
                _calculatedReward = _autoReward.GetCaculateReward(tact);
                _observerGetHour.Execute(new BigDigit(tact / 720f));
            }
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

            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            WaitForCheck(_cancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid WaitForCheck(CancellationToken cancellationToken)
        {
            await UniTask.Delay(Constants.Game.TACT_TIME * 1000, cancellationToken: cancellationToken);
            CheckSprite();
        }

        private void OffGoldHeap()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _tween.Kill();
            _tween = View.Rect.DOScale(Vector2.zero, 0.25f).OnComplete(OffSprite);
        }

        private void OffSprite()
        {
            View.Image.enabled = false;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}