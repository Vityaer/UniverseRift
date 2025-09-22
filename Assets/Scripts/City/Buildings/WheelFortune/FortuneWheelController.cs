using City.Buildings.Abstractions;
using ClientServices;
using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Misc.Json;
using Models.City.FortuneRewards;
using Models.Common.BigDigits;
using Models.Data.Buildings.FortuneWheels;
using Network.DataServer;
using Network.DataServer.Messages;
using Network.DataServer.Messages.City.FortuneWheels;
using Network.DataServer.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using City.Panels.ScreenBlockers;
using Common.Db.CommonDictionaries;
using Common.Inventories.Resourses;
using UIController.Inventory;
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

        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly ClientRewardService m_clientRewardService;
        [Inject] private readonly ResourceStorageController m_resourceStorageController;
        [Inject] private readonly ScreenBlockerController m_screenBlockerController;
        
        private readonly GameResource m_oneRotate = new GameResource(ResourceType.CoinFortune, 1, 0);
        private readonly GameResource m_tenRotate = new GameResource(ResourceType.CoinFortune, 8, 0);
        private readonly GameResource m_refreshCost = new GameResource(ResourceType.Diamond, 0, 0);

        private FortuneWheelData m_data;
        private readonly ReactiveCommand<BigDigit> m_onSimpleRotate = new();
        private Sequence m_sequence;
        private float m_currentTilt;
        private GameReward m_reward;

        private bool m_shakingFlag;
        private FortuneRewardContainer m_fortuneRewardContainer;
        
        private IDisposable m_onGetRewardDisposable;
        
        public IObservable<BigDigit> OnSimpleRotate => m_onSimpleRotate;

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
            View.OneRotateButton.SetCost(m_oneRotate);
            View.ManyRotateButton.SetCost(m_tenRotate);
            View.RefreshWheelButton.SetCost(m_refreshCost);

            base.OnStart();
        }

        public override void OnShow()
        {
            PlayEternalRotate();
            base.OnShow();
        }

        public override void OnHide()
        {
            m_sequence.Kill();
            base.OnHide();
        }

        private void PlayEternalRotate()
        {
            m_sequence.Kill();
            m_sequence = DOTween.Sequence()
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
            m_data = CommonGameData.City.FortuneWheelData;
            FillWheelFortune(rewards, false).Forget();
            base.OnLoadGame();
        }

        private void PlayOneSimpleRoulette()
        {
            m_shakingFlag = false;
            PlaySimpleRoulette(m_oneRotate, ONE_TIME).Forget();
        }
        
        private void PlayTenSimpleRoulette()
        {
            m_shakingFlag = true;
            PlaySimpleRoulette(m_tenRotate, MANY_TIME).Forget();
        }
        
        public async UniTaskVoid PlaySimpleRoulette(GameResource cost, int count = 1)
        {
            m_screenBlockerController.Open();
            var message = new FortuneWheelRotate { PlayerId = CommonGameData.PlayerInfoData.Id, Count = count };
            var result = await DataServer.PostData(message);
            m_screenBlockerController.Close();

            if (!string.IsNullOrEmpty(result))
            {
                m_fortuneRewardContainer = m_jsonConverter.Deserialize<FortuneRewardContainer>(result);
                m_reward = new GameReward(m_fortuneRewardContainer.Reward, m_commonDictionaries);

                m_resourceStorageController.SubtractResource(cost);

                StartRotateArrow(m_fortuneRewardContainer.ResultItemIndex);
                m_onSimpleRotate.Execute(new BigDigit(count));
            }
            
        }

        private async UniTaskVoid RefreshRewards()
        {
            var message = new FortuneWheelRefresh { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                var cost = m_commonDictionaries.CostContainers["FortuneWheelRefresh"]
                    .GetCostForLevelUp(m_data.RefreshCount);

                m_resourceStorageController.SubtractResource(cost);
                m_data = m_jsonConverter.Deserialize<FortuneWheelData>(result);
                FillWheelFortune(m_data.Rewards, true).Forget();
            }
        }

        private async UniTask FillWheelFortune(List<FortuneRewardData> rewards, bool blockScreen)
        {
            if(blockScreen)
                m_screenBlockerController.Open();
            using CancellationTokenSource tokenSource = new CancellationTokenSource();
            
            View.RefreshWheelButton.Button.interactable = false;
            for (var i = 0; i < rewards.Count; i++)
            {
                var rewardModel = m_commonDictionaries.FortuneRewardModels[rewards[i].RewardModelId];

                switch (rewardModel)
                {
                    case FortuneResourseRewardModel resourceProductModel:
                        var resource = new GameResource(resourceProductModel.Subject);
                        View.RewardCells[i].SetData(resource);
                        break;
                    case FortuneItemRewardModel itemProductModel:
                        var itemModel = m_commonDictionaries.Items[itemProductModel.Subject.Id];
                        var item = new GameItem(itemModel, itemProductModel.Subject.Amount);
                        View.RewardCells[i].SetData(item);
                        break;
                }

                await UniTask.Delay(Mathf.FloorToInt(View.RefreshCellDelay * 1000), cancellationToken: tokenSource.Token);
            }

            var cost = m_commonDictionaries.CostContainers["FortuneWheelRefresh"]
                .GetCostForLevelUp(m_data.RefreshCount);

            View.RefreshWheelButton.SetCost(cost[0]);
            if(blockScreen)
                m_screenBlockerController.Close();
        }
        private void StartRotateArrow(int rewardIndex)
        {
            m_screenBlockerController.Open();
            float targetTilt = -rewardIndex * 45;
            m_sequence?.Kill();

            m_currentTilt = NormalizeAngle(View.Arrow.localRotation.eulerAngles.z);

            float diff = targetTilt - m_currentTilt;
            diff = (diff + 180) % 360 - 180;

            if (diff > 0)
                targetTilt -= 360;  // rotate counterclockwise long way
            else
                targetTilt += 360;  // rotate clockwise long way

            int fullRotations = 3;
            float fullRotationDegrees = 360 * fullRotations * (diff > 0 ? -1 : 1);
            float finalRotation = targetTilt + fullRotationDegrees;

            m_sequence = DOTween.Sequence().Append(View.Arrow.DOLocalRotate(
                    new Vector3(0, 0, finalRotation),
                    View.ArrowSpeed * (fullRotations + 1),  // increase duration proportional to rotations
                    View.RotateModes[0]))
                .SetEase(View.EaseMode)  // example easing tweak, smooth start and end
                .SetSpeedBased(false)     // use duration instead of speed-based for better control
                .OnComplete(ShowReward);
            
            if (m_shakingFlag)
            {
                m_sequence.Insert(0f, 
                    View.FortuneWheel.DOShakePosition(View.ShakeDuration,
                    View.ShakeStrength,
                    View.ShakeVibrato,
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
            m_screenBlockerController.Close();
            m_sequence.Kill();
            var rewardIndex = m_fortuneRewardContainer.ResultItemIndex;
            m_sequence = DOTween.Sequence()
                .Append(View.RewardCells[rewardIndex].transform
                    .DOShakeScale(View.ScaleRewardDuration, View.ScaleRewardStrength));
            
            m_onGetRewardDisposable = m_clientRewardService.OnGetReward.Subscribe(_ => OnGetReward());
            m_clientRewardService.ShowReward(m_reward);
        }

        private void OnGetReward()
        {
            PlayEternalRotate();
            m_onGetRewardDisposable?.Dispose();
            m_onGetRewardDisposable = null;
            m_fortuneRewardContainer = null;
        }

        public new void Dispose()
        {
            m_onGetRewardDisposable?.Dispose();
            m_sequence.Kill();
            base.Dispose();
        }
    }
}