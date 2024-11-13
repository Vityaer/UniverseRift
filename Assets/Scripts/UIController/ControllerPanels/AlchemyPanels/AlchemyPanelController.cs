using City.Buildings.Abstractions;
using ClientServices;
using Cysharp.Threading.Tasks;
using Models.Common;
using Network.DataServer;
using Network.DataServer.Messages.City.Alchemies;
using System;
using System.Globalization;
using UniRx.Triggers;
using VContainer;
using UniRx;
using Common.Resourses;
using System.Diagnostics;
using Db.CommonDictionaries;
using Common.Rewards;
using City.Panels.Rewards;

namespace UIController.ControllerPanels.AlchemyPanels
{
    public class AlchemyPanelController : BaseBuilding<AlchemyPanelView>
    {
        private TimeSpan ALCHEMY_TIMESPAN = new TimeSpan(8, 0, 0);

        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly ClientRewardService _clientRewardService;

        private DateTime _lastGetAlchemyDateTime;

        public ReactiveCommand OnGetAchemyGold = new();

        protected override void OnStart()
        {
            View.AlchemyButton.OnClickAsObservable().Subscribe(_ => GetAlchemyGold().Forget()).AddTo(Disposables);
            View.SliderTimeAlchemy.RegisterOnFinish(FinishAlchemySlider);
        }

        protected override void OnLoadGame()
        {
            try
            {
                _lastGetAlchemyDateTime = DateTime.ParseExact(
                CommonGameData.CycleEventsData.LastGetAlchemyDateTime,
                Constants.Common.DateTimeFormat,
                CultureInfo.InvariantCulture
                );
            }
            catch
            {
                _lastGetAlchemyDateTime = DateTime.Parse(CommonGameData.CycleEventsData.LastGetAlchemyDateTime);
            }

            CheckAlchemyButton();
        }

        private void CheckAlchemyButton()
        {
            var delta = DateTime.UtcNow - _lastGetAlchemyDateTime;
            View.SliderTimeAlchemy.SetData(_lastGetAlchemyDateTime, ALCHEMY_TIMESPAN);
            View.AlchemyButton.interactable = delta >= ALCHEMY_TIMESPAN;
        }

        private void FinishAlchemySlider()
        {
            View.AlchemyButton.interactable = true;
        }

        private async UniTaskVoid GetAlchemyGold()
        {
            View.AlchemyButton.interactable = false;
            var message = new GetAlchemyMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var reward = new GameReward(_commonDictionaries.Rewards["Alchemy"], _commonDictionaries);
                _clientRewardService.ShowReward(reward);
                _lastGetAlchemyDateTime = DateTime.UtcNow;
                OnGetAchemyGold.Execute();
            }

            CheckAlchemyButton();
        }
    }
}
