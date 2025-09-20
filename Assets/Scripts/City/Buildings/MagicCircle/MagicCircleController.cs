using City.Buildings.Abstractions;
using City.Panels.HeroesHireResultPanels;
using ClientServices;
using Common.Heroes;
using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Hero;
using Misc.Json;
using Models;
using Models.Heroes;
using Network.DataServer;
using Network.DataServer.Messages;
using Network.DataServer.Messages.Hires;
using System;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using DG.Tweening;
using UIController.Rewards;
using UniRx;
using UnityEngine;
using VContainer;

namespace City.Buildings.MagicCircle
{
    public class MagicCircleController : BaseBuilding<MagicCircleView>
    {
        [Inject] private readonly HeroesStorageController _heroesStorageController;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly ClientRewardService _clientRewardService;

        private string _selectedRace;
        private GameResource _raceHireCost = new GameResource(ResourceType.RaceHireCard, 1, 0);
        private ReactiveCommand<int> _onRaceHire = new();

        private Tween _selectHireTween;
        
        public IObservable<int> OnRaceHire => _onRaceHire;

        protected override void OnStart()
        {
            foreach (var button in View.RaceSelectButtons)
            {
                button.Value.OnClickAsObservable().Subscribe(_ => ChangeHireRace(button.Key)).AddTo(Disposables);
            }

            View.OneHire.ChangeCost(_raceHireCost, () => MagicHire(1).Forget());
            View.ManyHire.ChangeCost(_raceHireCost * 10, () => MagicHire(10).Forget());

            _selectedRace = View.RaceSelectButtons.ElementAt(0).Key;
            ChangeHireRace(_selectedRace);
            base.OnStart();
        }

        private void ChangeHireRace(string stringRace)
        {
            if(!string.IsNullOrEmpty(_selectedRace))
                View.RaceSelectButtons[_selectedRace].interactable = true;

            _selectedRace = stringRace;
            View.RaceSelectButtons[_selectedRace].interactable = false;
            
            
            _selectHireTween.Kill();
            _selectHireTween = DOTween.Sequence()
                .Append(View.BackgroundImage.DOColor(View.HireColors[stringRace], View.BackgroundChangeColorTime));
        }

        private async UniTaskVoid MagicHire(int count = 1)
        {
            var message = new MagicCircleHire
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                Count = count,
                RaceName = _selectedRace
            };
            var result = await DataServer.PostData(message);
            if(!string.IsNullOrEmpty(result))
            {
                var reward = _jsonConverter.Deserialize<RewardModel>(result);
                var gameReward = new GameReward(reward, _commonDictionaries);
                _clientRewardService.ShowReward(gameReward, fast: false);
                _resourceStorageController.SubtractResource(_raceHireCost * count);

                _onRaceHire.Execute(count);
            }
        }

        private void AddNewHero(GameHero hero)
        {
            _heroesStorageController.AddHero(hero);
        }
    }
}