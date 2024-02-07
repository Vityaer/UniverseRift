using City.Buildings.Abstractions;
using City.Buildings.PageCycleEvent.MonthlyEvents.ArenaTasksPanel;
using City.Buildings.PageCycleEvent.MonthlyEvents.EvolutionTasksPanel;
using City.Buildings.PageCycleEvent.MonthlyEvents.TaskboardTasksPanel;
using City.Buildings.PageCycleEvent.MonthlyEvents.TravelTasksPanel;
using Models;
using Models.Achievments;
using Models.Common;
using Models.Data;
using System.Collections.Generic;
using Utils;
using VContainer;
using VContainerUi.Interfaces;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Buildings.PageCycleEvent.MonthlyEvents
{
    public class MonthlyEventsController : BaseBuilding<MonthlyEventView>
    {
        [Inject] private readonly CommonGameData _ñommonGameData;
        [Inject] private readonly IUiMessagesPublisherService _messagesPublisher;
        [Inject] private readonly ArenaTaskPanelController _arenaTaskPanelController;
        [Inject] private readonly EvolutionTasksPanelController _evolutionTasksPanelController;
        [Inject] private readonly TaskboardTasksPanelController _taskboardTasksPanelController;
        [Inject] private readonly TravelTasksPanelController _travelTasksPanelController;

        private GameMonthlyTasks _currentPage;

        MonthlyRequirementsModel monthlyRequirements = null;

        protected override void OnStart()
        {
            //View.ArenaOpenButton.OnClickAsObservable().Subscribe(_ => OpenPageEvents<ArenaTaskPanelController>(arenaTasks)).AddTo(Disposables);
            //View.TravelOpenButton.OnClickAsObservable().Subscribe(_ => OpenPageEvents<EvolutionTasksPanelController>(travelTasks)).AddTo(Disposables);
            //View.EvolutionOpenButton.OnClickAsObservable().Subscribe(_ => OpenPageEvents<TaskboardTasksPanelController>(evolutionTasks)).AddTo(Disposables);
            //View.TaskBoardsOpenButton.OnClickAsObservable().Subscribe(_ => OpenPageEvents<TravelTasksPanelController>(taskBoardsTasks)).AddTo(Disposables);
        }

        protected override void OnLoadGame()
        {
            //monthlyRequirements = _ñommonGameData.City.CycleEvents.monthlyRequirements;
            LoadTasks();
        }

        private void LoadTasks()
        {
            //arenaTasks.LoadData(monthlyRequirements.GetTasks(TypeMonthlyTasks.Arena));
            //travelTasks.LoadData(monthlyRequirements.GetTasks(TypeMonthlyTasks.Travel));
            //evolutionTasks.LoadData(monthlyRequirements.GetTasks(TypeMonthlyTasks.Evolution));
            //taskBoardsTasks.LoadData(monthlyRequirements.GetTasks(TypeMonthlyTasks.TaskBoard));
        }

        public void SaveData(TypeMonthlyTasks type, List<AchievmentModel> tasks)
        {
            List<AchievmentData> RequirementSaves = monthlyRequirements.GetTasks(type);
            //GameController.GetPlayerGame.GeneralSaveAchievments(RequirementSaves, tasks);
            TextUtils.Save(_ñommonGameData);
        }

        private void OpenPageEvents<T>(GameMonthlyTasks currentPage) where T : IPopUp
        {
            _messagesPublisher.OpenWindowPublisher.OpenWindow<T>(openType: OpenType.Additive);
            _currentPage = currentPage;
            //_currentPage.MainPanel.SetActive(true);
        }
    }
    public enum TypeMonthlyTasks
    {
        Arena = 0,
        Travel = 1,
        Evolution = 2,
        TaskBoard = 3
    }
}