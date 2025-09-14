using City.Panels.Events.HeroSpaceMarkets;
using City.Panels.MonthTasks.Arena;
using City.Panels.MonthTasks.Evolutions;
using City.Panels.MonthTasks.Taskboards;
using City.Panels.MonthTasks.Travels;
using LocalizationSystems;
using Models.Common;
using Services.TimeLocalizeServices;
using UiExtensions.MainPages;
using UniRx;
using UnityEngine;
using Utils;
using VContainer;
using VContainer.Unity;
using VContainerUi.Interfaces;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace MainPages.Events
{
    public class EventsPageController : UiMainPageController<EventsPageView>, IStartable
    {
        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] private readonly IObjectResolver _resolver;
        [Inject] private readonly ILocalizationSystem _localizationSystem;
        [Inject] private readonly TimeLocalizeService _timeLocalizeService;

        public void Start()
        {
            View.ArenaButton.OnClickAsObservable().Subscribe(_ => OpenMonthPage<MonthArenaPanelController>()).AddTo(Disposables);
            View.EvolutionButton.OnClickAsObservable().Subscribe(_ => OpenMonthPage<MonthEvolutionPanelController>()).AddTo(Disposables);
            View.TableTasksButton.OnClickAsObservable().Subscribe(_ => OpenMonthPage<MonthTaskboardPanelController>()).AddTo(Disposables);
            View.TravelButton.OnClickAsObservable().Subscribe(_ => OpenMonthPage<MonthTravelPanelController>()).AddTo(Disposables);
            View.HeroSpaceMarketButton.OnClickAsObservable().Subscribe(_ => OpenHeroMarket()).AddTo(Disposables);
        }

        private void OpenHeroMarket()
        {
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<HeroMarketController>(openType: OpenType.Additive);

        }

        private void OpenMonthPage<TWindow>() where TWindow : IPopUp
        {
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<TWindow>(openType: OpenType.Additive);
        }

        protected override void OnLoadGame()
        {
            View.SliderEventLeftTime.Init(_localizationSystem, _timeLocalizeService);
            ShowCurrentGameCycle();
            base.OnLoadGame();
        }

        private void ShowCurrentGameCycle()
        {
            var eventContainer = View.EventContainers[_commonGameData.CycleEventsData.CurrentEventType];
            if (eventContainer == null)
            {
                Debug.LogError($"Event {_commonGameData.CycleEventsData.CurrentEventType} not found.");
                return;
            }

            var containerController = UnityEngine.Object.Instantiate(eventContainer.Container, View.ContentAction);
            _resolver.Inject(containerController);
            eventContainer.Active();
            View.EventCycleName.StringReference = _localizationSystem
                .GetLocalizedContainer($"Cycle{_commonGameData.CycleEventsData.CurrentEventType}Name");

            var startDateTime = TimeUtils.ParseTime(_commonGameData.CycleEventsData.StartGameCycleDateTime) - Constants.Game.GameCycleTime;
            View.SliderEventLeftTime.SetData(startDateTime, Constants.Game.GameCycleTime);
            containerController.SetData(startDateTime, Constants.Game.GameCycleTime);
        }
    }
}
