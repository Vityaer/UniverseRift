using City.Panels.MonthTasks.Arena;
using City.Panels.MonthTasks.Evolutions;
using City.Panels.MonthTasks.Taskboards;
using City.Panels.MonthTasks.Travels;
using Models.Common;
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

        public void Start()
        {
            View.ArenaButton.OnClickAsObservable().Subscribe(_ => OpenMonthPage<MonthArenaPanelController>()).AddTo(Disposables);
            View.EvolutionButton.OnClickAsObservable().Subscribe(_ => OpenMonthPage<MonthEvolutionPanelController>()).AddTo(Disposables);
            View.TableTasksButton.OnClickAsObservable().Subscribe(_ => OpenMonthPage<MonthTaskboardPanelController>()).AddTo(Disposables);
            View.TravelButton.OnClickAsObservable().Subscribe(_ => OpenMonthPage<MonthTravelPanelController>()).AddTo(Disposables);
        }

        private void OpenMonthPage<TWindow>() where TWindow : IPopUp
        {
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<TWindow>(openType: OpenType.Exclusive);
        }

        protected override void OnLoadGame()
        {
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
            View.EventCycleName.text = $"{_commonGameData.CycleEventsData.CurrentEventType}";
            Debug.Log(_commonGameData.CycleEventsData.StartGameCycleDateTime);
            var startDateTime = TimeUtils.ParseTime(_commonGameData.CycleEventsData.StartGameCycleDateTime) - Constants.Game.GameCycleTime;
            View.SliderEventLeftTime.SetData(startDateTime, Constants.Game.GameCycleTime);
            containerController.SetData(startDateTime, Constants.Game.GameCycleTime);
        }
    }
}
