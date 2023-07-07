using City.Buildings.Abstractions;
using City.Panels.Messages;
using Common;
using Common.Inventories.Splinters;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Misc.Json;
using Misc.Json.Impl;
using Models.Common.BigDigits;
using Network.DataServer;
using Network.DataServer.Messages;
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
    public class WheelFortuneController : BaseBuilding<WheelFortuneView>, IInitializable
    {
        private const int ONE_TIME = 1; 
        private const int MANY_TIME = 10;
        private const float ARROW_SPEED = 3f;

        [Inject] private IJsonConverter _jsonConverter;

        [Header("List reward")]
        private List<FortuneRewardModel> rewards = new List<FortuneRewardModel>();

        public List<FortuneReward<GameResource>> resources = new List<FortuneReward<GameResource>>();
        public List<FortuneReward<GameItem>> items = new List<FortuneReward<GameItem>>();
        public List<FortuneReward<GameSplinter>> splinters = new List<FortuneReward<GameSplinter>>();

        [Header("Arrow")]
        private float previousTilt = 0f;

        private float generalProbability = 0f;
        [Header("Test")]
        public float testRandom = 0;
        private List<int> numbersReward = new List<int>();
        private float deltaMiss = 15f;
        private ReactiveCommand<BigDigit> _onSimpleRotate = new ReactiveCommand<BigDigit>();
        private Sequence _sequence;
        public IObservable<BigDigit> OnSimpleRotate => _onSimpleRotate;

        private void FillRewards()
        {
            for (int i = 0; i < resources.Count; i++) rewards.Add(resources[i]);
            for (int i = 0; i < items.Count; i++) rewards.Add(items[i]);
            for (int i = 0; i < splinters.Count; i++) rewards.Add(splinters[i]);
        }
        public void Initialize()
        {
            View.OneRotateButton.OnClick.Subscribe(_ => PlaySimpleRoulette(ONE_TIME)).AddTo(Disposables);
            View.OneRotateButton.OnClick.Subscribe(_ => PlaySimpleRoulette(MANY_TIME)).AddTo(Disposables);
        }

        protected override void OpenPage()
        {
            FillWheelFortune();
            CalculateProbability();
        }

        public void PlaySimpleRoulette(int coin = 1)
        {
            View.OneRotateButton.Disable();
            View.ManyRotateButton.Disable();
            StartRotateArrow(GetRandom(coin));
            _onSimpleRotate.Execute(new BigDigit(coin));
        }

        private async UniTaskVoid RefreshRewards()
        {
            var message = new FortuneWheelRewards { PlayerId = CommonGameData.Player.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);
            rewards = _jsonConverter.FromJson<FortuneRewardModel[]>(result).ToList();
        }

        private float GetRandom(int countSpin)
        {
            float result = 0f, rand = 0f;
            numbersReward.Clear();
            int k = 0;
            for (int i = 0; i < countSpin; i++)
            {
                rand = UnityEngine.Random.Range(0f, generalProbability);
                k = 0;
                for (int j = 0; j < rewards.Count; j++)
                {
                    result += rewards[j].Probability;
                    if (result < rand) { k++; } else { break; }
                }
                numbersReward.Add(k);
            }
            return k * 45f + UnityEngine.Random.Range(-deltaMiss, deltaMiss);
        }
        private void CalculateProbability()
        {
            generalProbability = 0f;
            foreach (FortuneRewardModel reward in rewards)
                generalProbability += reward.Probability;
        }
        private void FillWheelFortune()
        {
            for (int i = 0; i < rewards.Count; i++)
            {
                switch (rewards[i])
                {
                    case FortuneReward<GameResource> product:
                        View.RewardCells[i].SetData(product.subject);
                        break;
                    case FortuneReward<GameItem> product:
                        View.RewardCells[i].SetData(product.subject);
                        break;
                    case FortuneReward<GameSplinter> product:
                        View.RewardCells[i].SetData(product.subject);
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
                .Append(View.Arrow.DOLocalRotate(new Vector3(0, 0, targetTilt), 1 / ARROW_SPEED, RotateMode.FastBeyond360).OnComplete(GetReward));
        }

        private void GetReward()
        {
            RewardData reward = new RewardData();
            for (int i = 0; i < numbersReward.Count; i++)
            {
                switch (rewards[numbersReward[i]])
                {
                    case FortuneReward<GameResource> res:
                        GameResource rewardRes = (rewards[numbersReward[i]] as FortuneReward<GameResource>).subject as GameResource;
                        //reward.Add(rewardRes);
                        break;
                    case FortuneReward<GameItem> item:
                        GameItem rewardItem = (rewards[numbersReward[i]] as FortuneReward<GameItem>).subject as GameItem;
                        //reward.Add(rewardItem);
                        break;
                    case FortuneReward<GameSplinter> splinter:
                        break;
                }
            }
            //MessageController.Instance.OpenSimpleRewardPanel(reward);

            View.OneRotateButton.Enable();
            View.ManyRotateButton.Enable();
        }
    }
}