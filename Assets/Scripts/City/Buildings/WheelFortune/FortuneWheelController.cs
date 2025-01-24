using City.Buildings.Abstractions;
using ClientServices;
using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
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
        private const float ARROW_SPEED = 3f;

        [Inject] private IJsonConverter _jsonConverter;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly ClientRewardService _clientRewardService;
        [Inject] private readonly ResourceStorageController _resourceStorageController;

        private GameResource _oneRotate = new GameResource(ResourceType.CoinFortune, 1, 0);
        private GameResource _tenRotate = new GameResource(ResourceType.CoinFortune, 8, 0);
        private GameResource _refreshCost = new GameResource(ResourceType.Diamond, 0, 0);

        private FortuneWheelData _data;
        private ReactiveCommand<BigDigit> _onSimpleRotate = new();
        private Sequence _sequence;
        private float _currentTilt;
        private GameReward _reward;

        public IObservable<BigDigit> OnSimpleRotate => _onSimpleRotate;

        public void Initialize()
        {
            View.OneRotateButton.OnClick.Subscribe(_ => PlaySimpleRoulette(_oneRotate, ONE_TIME).Forget()).AddTo(Disposables);
            View.ManyRotateButton.OnClick.Subscribe(_ => PlaySimpleRoulette(_tenRotate, MANY_TIME).Forget()).AddTo(Disposables);
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


        protected override void OnLoadGame()
        {
            var rewards = CommonGameData.City.FortuneWheelData.Rewards;
            _data = CommonGameData.City.FortuneWheelData;
            FillWheelFortune(rewards);
            base.OnLoadGame();
        }

        public async UniTaskVoid PlaySimpleRoulette(GameResource cost, int count = 1)
        {
            var message = new FortuneWheelRotate { PlayerId = CommonGameData.PlayerInfoData.Id, Count = count };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var rewardContainer = _jsonConverter.Deserialize<FortuneRewardContainer>(result);
                _reward = new GameReward(rewardContainer.Reward, _commonDictionaries);

                _resourceStorageController.SubtractResource(cost);

                StartRotateArrow(-rewardContainer.ResultItemIndex * 45);
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
                FillWheelFortune(_data.Rewards);
            }
        }

        private void FillWheelFortune(List<FortuneRewardData> rewards)
        {
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
            }

            var cost = _commonDictionaries.CostContainers["FortuneWheelRefresh"]
                .GetCostForLevelUp(_data.RefreshCount);

            Debug.Log($"_data.RefreshCount: {_data.RefreshCount}");
            View.RefreshWheelButton.SetCost(cost[0]);
        }
        private void StartRotateArrow(float targetTilt)
        {
            _sequence = DOTween.Sequence()
                .Append(View.Arrow.DOLocalRotate(new Vector3(0, 0, 360 - _currentTilt), 1 / ARROW_SPEED, RotateMode.FastBeyond360))
                .Append(View.Arrow.DOLocalRotate(new Vector3(0, 0, 360), 1 / ARROW_SPEED, RotateMode.FastBeyond360))
                .Append(View.Arrow.DOLocalRotate(new Vector3(0, 0, targetTilt), 1 / ARROW_SPEED, RotateMode.FastBeyond360)
                .OnComplete(() => _clientRewardService.ShowReward(_reward)));

            _currentTilt = targetTilt;
        }

        public new void Dispose()
        {
            _sequence.Kill();
            base.Dispose();
        }
    }
}