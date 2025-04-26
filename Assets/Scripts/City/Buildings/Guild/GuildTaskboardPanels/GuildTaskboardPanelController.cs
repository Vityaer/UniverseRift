using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainerUi.Model;
using VContainerUi.Messages;
using City.Buildings.Guild.GuildDonatePanels;
using City.Buildings.TaskGiver.Abstracts;
using Models.Data.Buildings.Taskboards;
using VContainer;

namespace City.Buildings.Guild.GuildTaskboardPanels
{
    public class GuildTaskboardPanelController : BaseTaskboardController<GuildTaskboardPanelView>
    {
        [Inject] private readonly GuildController m_guildController;
        
        private TaskBoardData _taskBoardData;

        protected override void OnStart()
        {
            m_guildController.OnLoadGuild.Subscribe(_ => OnLoadGame()).AddTo(Disposables);
            View.OpenDonatePanelButton.OnClickAsObservable().Subscribe(_ => OpenGuildDonatePanel()).AddTo(Disposables);
        }
        
        protected override void OnLoadGame()
        {
            _taskBoardData = CommonGameData.City.GuildPlayerSaveContainer.TasksData;

            if (_taskBoardData != null)
            {
                RecreateTaskControllers(_taskBoardData.ListTasks);
            }
        }

        private void OpenGuildDonatePanel()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<GuildDonatePanelController>(openType: OpenType.Exclusive);
        }
    }
}
