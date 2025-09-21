using City.Buildings.Abstractions;
using ClientServices;
using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Misc.Json;
using Models.City.FortuneRewards;
using Models.Common;
using Models.Common.BigDigits;
using Models.Data.Buildings.FortuneWheels;
using Network.DataServer;
using Network.DataServer.Messages;
using Network.DataServer.Messages.City.FortuneWheels;
using Network.DataServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using City.Panels.ScreenBlockers;
using Common.Db.CommonDictionaries;
using Common.Inventories.Resourses;
using UIController.Inventory;
using UIController.Rewards;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace City.Buildings.WheelFortune
{
    public class FortuneWheelController : BaseBuilding<FortuneWheelView>, IInitializable, IDisposable
    {
        private const int ONE_TIME = 1;
        private const int MANY_TIME = 10;

        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly ClientRewardService _clientRewardService;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly ScreenBlockerController _screenBlockerController;
        
        private GameResource _oneRotate = new GameResource(ResourceType.CoinFortune, 1, 0);
        private GameResource _tenRotate = new GameResource(ResourceType.CoinFortune, 8, 0);
        private GameResource _refreshCost = new GameResource(ResourceType.Diamond, 0, 0);

        private FortuneWheelData _data;
        private ReactiveCommand<BigDigit> _onSimpleRotate = new();
        private Sequence _sequence;
        private float _currentTilt;
        private GameReward _reward;

        private bool _shakingFlag;
        private FortuneRewardContainer _fortuneRewardContainer;
        
        private IDisposable _onGetRewardDisposable;
        
        public IObservable<BigDigit> OnSimpleRotate => _onSimpleRotate;

        public void Initialize()
        {
            View.OneRotateButton.OnClick.Subscribe(_ => PlayOneSimpleRoulette()).AddTo(Disposables);
            View.ManyRotateButton.OnClick.Subscribe(_ => PlayTenSimpleRoulette()).AddTo(Disposables);
            View.RefreshWheelButton.OnClick.Subscribe(_ => RefreshRewards().Forget()).AddTo(Disposables);

            foreach (var rewardCell in View.RewardCells)
                rewardCell.OnSelect.Subscribe(_ => SubjectDetailController.ShowData(rewardCell.Subject)).AddTo(Disposables);
        }


        protected override void OnStart()
        {
            View.OneRotateButton.SetCost(_oneRotate);
            View.ManyRotateButton.SetCost(_tenRotate);
            View.RefreshWheelButton.SetCost(_refreshCost);

            base.OnStart();
        }

        public override void OnShow()
        {
            PlayEternalRotate();
            base.OnShow();
        }

        public override void OnHide()
        {
            _sequence.Kill();
            base.OnHide();
        }

        private void PlayEternalRotate()
        {
            _sequence.Kill();
            _sequence = DOTween.Sequence()
                .AppendInterval(View.EternalRotateDelay)
                .Append(View.Arrow
                .DOLocalRotate(new Vector3(0, 0, 360), View.EternalRotateSpeed, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetSpeedBased(true))
                .SetLoops(-1);
        }

        protected override void OnLoadGame()
        {
            var rewards = CommonGameData.City.FortuneWheelData.Rewards;
            _data = CommonGameData.City.FortuneWheelData;
            FillWheelFortune(rewards, false).Forget();
            base.OnLoadGame();
        }

        private void PlayOneSimpleRoulette()
        {
            _shakingFlag = false;
            PlaySimpleRoulette(_oneRotate, ONE_TIME).Forget();
        }
        
        private void PlayTenSimpleRoulette()
        {
            _shakingFlag = true;
            PlaySimpleRoulette(_tenRotate, MANY_TIME).Forget();
        }
        
        public async UniTaskVoid PlaySimpleRoulette(GameResource cost, int count = 1)
        {
            _screenBlockerController.Open();
            var message = new FortuneWheelRotate { PlayerId = CommonGameData.PlayerInfoData.Id, Count = count };
            var result = await DataServer.PostData(message);
            _screenBlockerController.Close();

            if (!string.IsNullOrEmpty(result))
            {
                _fortuneRewardContainer = _jsonConverter.Deserialize<FortuneRewardContainer>(result);
                _reward = new GameReward(_fortuneRewardContainer.Reward, _commonDictionaries);

                _resourceStorageController.SubtractResource(cost);

                StartRotateArrow(_fortuneRewardContainer.ResultItemIndex);
                _onSimpleRotate.Execute(new BigDigit(count));
            }
            
        }

        private async UniTaskVoid RefreshRewards()
        {
            var message = new FortuneWheelRefresh { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                var cost = _commonDictionaries.CostContainers["FortuneWheelRefresh"]
                    .GetCostForLevelUp(_data.RefreshCount);

                _resourceStorageController.SubtractResource(cost);
                _data = _jsonConverter.Deserialize<FortuneWheelData>(result);
                FillWheelFortune(_data.Rewards, true).Forget();
            }
        }

        private async UniTask FillWheelFortune(List<FortuneRewardData> rewards, bool blockScreen)
        {
            if(blockScreen)
                _screenBlockerController.Open();
            using CancellationTokenSource tokenSource = new CancellationTokenSource();
            
            View.RefreshWheelButton.Button.interactable = false;
            for (var i = 0; i < rewards.Count; i++)
            {
                var rewardModel = _commonDictionaries.FortuneRewardModels[rewards[i].RewardModelId];

                switch (rewardModel)
                {
                    case FortuneResourseRewardModel resourceProductModel:
                        var resource = new GameResource(resourceProductModel.Subject);
                        View.RewardCells[i].SetData(resource);
                        break;
                    case FortuneItemRewardModel itemProductModel:
                        var itemModel = _commonDictionaries.Items[itemProductModel.Subject.Id];
                        var item = new GameItem(itemModel, itemProductModel.Subject.Amount);
                        View.RewardCells[i].SetData(item);
                        break;
                }

                await UniTask.Delay(Mathf.FloorToInt(View.RefreshCellDelay * 1000), cancellationToken: tokenSource.Token);
            }

            var cost = _commonDictionaries.CostContainers["FortuneWheelRefresh"]
                .GetCostForLevelUp(_data.RefreshCount);

            View.RefreshWheelButton.SetCost(cost[0]);
            if(blockScreen)
                _screenBlockerController.Close();
        }
        private void StartRotateArrow(int rewardIndex)
        {
            _screenBlockerController.Open();
            float targetTilt = -rewardIndex * 45;
            _sequence?.Kill();

            _currentTilt = NormalizeAngle(View.Arrow.localRotation.eulerAngles.z);

            float diff = targetTilt - _currentTilt;
            diff = (diff + 180) % 360 - 180;

            if (diff > 0)
                targetTilt -= 360;  // rotate counterclockwise long way
            else
                targetTilt += 360;  // rotate clockwise long way

            int fullRotations = 3;
            float fullRotationDegrees = 360 * fullRotations * (diff > 0 ? -1 : 1);
            float finalRotation = targetTilt + fullRotationDegrees;

            _sequence = DOTween.Sequence().Append(View.Arrow.DOLocalRotate(
                    new Vector3(0, 0, finalRotation),
                    View.ArrowSpeed * (fullRotations + 1),  // increase duration proportional to rotations
                    View.RotateModes[0]))
                .SetEase(View.EaseMode)  // example easing tweak, smooth start and end
                .SetSpeedBased(false)     // use duration instead of speed-based for better control
                .OnComplete(ShowReward);
            
            if (_shakingFlag)
            {
                _sequence.Insert(0f, View.FortuneWheel.DOShakePosition(View.ShakeDuration, View.ShakeStrength, View.ShakeVibrato,
                        View.ShakeRandomness));
            }
        }
        
        private float NormalizeAngle(float angle)
        {
            angle %= 360f;
            return angle < 0 ? angle + 360f : angle;
        }

        private void ShowReward()
        {
            _screenBlockerController.Close();
            _sequence.Kill();
            var rewardIndex = _fortuneRewardContainer.ResultItemIndex;
            _sequence = DOTween.Sequence()
                .Append(View.RewardCells[rewardIndex].transform
                    .DOShakeScale(View.ScaleRewardDuration, View.ScaleRewardStrength));
            
            _onGetRewardDisposable = _clientRewardService.OnGetReward.Subscribe(_ => OnGetReward());
            _clientRewardService.ShowReward(_reward);
        }

        private void OnGetReward()
        {
            PlayEternalRotate();
            _onGetRewardDisposable?.Dispose();
            _onGetRewardDisposable = null;
            _fortuneRewardContainer = null;
        }

        public new void Dispose()
        {
            _onGetRewardDisposable?.Dispose();
            _sequence.Kill();
            base.Dispose();
        }
    }
}