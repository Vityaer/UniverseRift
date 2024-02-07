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

namespace UIController.ControllerPanels.AlchemyPanels
{
    public class AlchemyPanelController : BaseBuilding<AlchemyPanelView>
    {
        private TimeSpan ALCHEMY_TIMESPAN = new TimeSpan(8, 0, 0);

        [Inject] private readonly CommonGameData _сommonGameData;
        [Inject] private readonly ResourceStorageController _storageController;

        private DateTime _lastGetAlchemyDateTime;

        public ReactiveCommand OnGetAchemyGold = new();

        protected override void OnStart()
        {
            View.AlchemyButton.OnCancelAsObservable().Subscribe(_ => AlchemyGold()).AddTo(Disposables);
            View.SliderTimeAlchemy.RegisterOnFinish(FinishAlchemySlider);
        }

        protected override void OnLoadGame()
        {
            try
            {
                _lastGetAlchemyDateTime = DateTime.ParseExact(
                _сommonGameData.CycleEventsData.LastGetAlchemyDateTime,
                Constants.Common.DateTimeFormat,
                CultureInfo.InvariantCulture
                );
            }
            catch
            {
                _lastGetAlchemyDateTime = DateTime.Parse(_сommonGameData.CycleEventsData.LastGetAlchemyDateTime);
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

        private void AlchemyGold()
        {
            GetAlchemyGold().Forget();
        }

        private async UniTaskVoid GetAlchemyGold()
        {
            View.AlchemyButton.interactable = false;
            var message = new GetAlchemyMessage
            {
                PlayerId = _сommonGameData.PlayerInfoData.Id
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var resource = new GameResource(View.AlchemyBonus);
                _storageController.AddResource(resource);
                _lastGetAlchemyDateTime = DateTime.UtcNow;
                OnGetAchemyGold.Execute();
            }

            CheckAlchemyButton();
        }
    }
}
