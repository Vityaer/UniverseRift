using System;
using System.Linq;
using City.Buildings.Abstractions;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Heroes;
using Common.Inventories.Resourses;
using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Hero;
using Misc.Json;
using Network.DataServer;
using Network.DataServer.Messages.Hires;
using UIController.Rewards;
using UniRx;
using VContainer;

namespace City.Buildings.MagicCircle
{
    public class MagicCircleController : BaseBuilding<MagicCircleView>
    {
        [Inject] private readonly HeroesStorageController m_heroesStorageController;
        [Inject] private readonly ResourceStorageController m_resourceStorageController;
        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly ClientRewardService m_clientRewardService;

        private string m_selectedRace;
        private readonly GameResource m_raceHireCost = new GameResource(ResourceType.RaceHireCard, 1, 0);
        private readonly ReactiveCommand<int> m_onRaceHire = new();

        private Tween m_selectHireTween;

        public IObservable<int> OnRaceHire => m_onRaceHire;

        protected override void OnStart()
        {
            foreach (var button in View.RaceSelectButtons)
                button.Value.OnClickAsObservable().Subscribe(_ => ChangeHireRace(button.Key))
                    .AddTo(Disposables);

            View.OneHire.ChangeCost(m_raceHireCost, () => MagicHire(1).Forget());
            View.ManyHire.ChangeCost(m_raceHireCost * 10, () => MagicHire(10).Forget());

            m_selectedRace = View.RaceSelectButtons.ElementAt(0).Key;
            ChangeHireRace(m_selectedRace);
            base.OnStart();
        }

        private void ChangeHireRace(string stringRace)
        {
            if (!string.IsNullOrEmpty(m_selectedRace))
                View.RaceSelectButtons[m_selectedRace].interactable = true;

            m_selectedRace = stringRace;
            View.RaceSelectButtons[m_selectedRace].interactable = false;


            m_selectHireTween.Kill();
            m_selectHireTween = DOTween.Sequence()
                .Append(View.BackgroundImage.DOColor(View.HireColors[stringRace], View.BackgroundChangeColorTime));
        }

        private async UniTaskVoid MagicHire(int count = 1)
        {
            var message = new MagicCircleHire
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                Count = count,
                RaceName = m_selectedRace
            };
            
            string result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                var reward = m_jsonConverter.Deserialize<RewardModel>(result);
                var gameReward = new GameReward(reward, m_commonDictionaries);
                m_clientRewardService.ShowReward(gameReward, fast: false);
                m_resourceStorageController.SubtractResource(m_raceHireCost * count);

                m_onRaceHire.Execute(count);
            }
        }
    }
}