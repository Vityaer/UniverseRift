﻿using Assets.Scripts.ClientServices;
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

        private List<int> numbersReward = new List<int>();
        private ReactiveCommand<BigDigit> _onSimpleRotate = new ReactiveCommand<BigDigit>();
        private Sequence _sequence;

        public IObservable<BigDigit> OnSimpleRotate => _onSimpleRotate;

        public void Initialize()
        {
            View.OneRotateButton.SetCost(_oneRotate);
            View.ManyRotateButton.SetCost(_tenRotate);
            View.RefreshWheelButton.SetCost(_refreshCost);

            View.OneRotateButton.OnClick.Subscribe(_ => PlaySimpleRoulette(_oneRotate, ONE_TIME).Forget()).AddTo(Disposables);
            View.ManyRotateButton.OnClick.Subscribe(_ => PlaySimpleRoulette(_tenRotate, MANY_TIME).Forget()).AddTo(Disposables);
            View.RefreshWheelButton.OnClick.Subscribe(_ => RefreshRewards().Forget()).AddTo(Disposables);
        }

        protected override void OnLoadGame()
        {
            var rewards = CommonGameData.City.FortuneWheelData.Rewards;
            FillWheelFortune(rewards);
            base.OnLoadGame();
        }

        public async UniTaskVoid PlaySimpleRoulette(GameResource cost, int count = 1)
        {
            View.OneRotateButton.Disable();
            View.ManyRotateButton.Disable();

            var message = new FortuneWheelRotate { PlayerId = CommonGameData.PlayerInfoData.Id, Count = count };
            var result = await DataServer.PostData(message);

            var rewardModel = _jsonConverter.FromJson<RewardModel>(result);
            var rewards = new GameReward(rewardModel);
            _clientRewardService.AddReward(rewards);

            _resourceStorageController.SubtractResource(cost);
            StartRotateArrow(UnityEngine.Random.Range(120, 345));
            _onSimpleRotate.Execute(new BigDigit(count));
        }

        private async UniTaskVoid RefreshRewards()
        {
            var message = new FortuneWheelRefresh { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);
            var rewards = _jsonConverter.FromJson<List<FortuneRewardData>>(result);
            FillWheelFortune(rewards);
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
        }
        private void StartRotateArrow(float targetTilt)
        {
            var currentRotation = View.Arrow.localRotation.eulerAngles;
            _sequence = DOTween.Sequence()
                .Append(View.Arrow.DOLocalRotate(currentRotation + new Vector3(0, 0, 360), 1 / ARROW_SPEED, RotateMode.FastBeyond360))
                .Append(View.Arrow.DOLocalRotate(new Vector3(0, 0, 360), 1 / ARROW_SPEED, RotateMode.FastBeyond360))
                .Append(View.Arrow.DOLocalRotate(new Vector3(0, 0, targetTilt), 1 / ARROW_SPEED, RotateMode.FastBeyond360));
        }

        public new void Dispose()
        {
            _sequence.Kill();
            base.Dispose();
        }
    }
}