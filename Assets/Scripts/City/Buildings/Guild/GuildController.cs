using City.Buildings.Abstractions;
using City.Buildings.Guild.NewGuildPanels;
using Network.DataServer.Models;
using UniRx;
using VContainer;
using VContainerUi.Model;
using VContainerUi.Messages;
using VContainerUi.Services;
using UnityEngine;
using Network.DataServer.Messages.Guilds;
using Network.DataServer;
using Cysharp.Threading.Tasks;
using City.Buildings.Guild.AvailableGuildsPanels;
using City.Buildings.Guild.GuildTaskboardPanels;

namespace City.Buildings.Guild
{
    public class GuildController : BaseBuilding<GuildView>
    {
        [Inject] private readonly IUiMessagesPublisherService _uiMessagesPublisher;

        private GuildData _guildData;
        private GameObject _currentPanel;

        protected override void OnStart()
        {
            View.RaidBossButton.OnClickAsObservable().Subscribe(_ => StartRaidBoss().Forget()).AddTo(Disposables);
            View.OpenNewGuildPanelButton.OnClickAsObservable().Subscribe(_ => OpenCreateNewGuildPanel()).AddTo(Disposables);
            View.OpenAvailableGuildsPanelButton.OnClickAsObservable().Subscribe(_ => OpenAvailableGuildsPanel()).AddTo(Disposables);
            View.GuildTaskboardButton.OnClickAsObservable().Subscribe(_ => OpenGuildTaskboardPanel()).AddTo(Disposables);
        }

        protected override void OnLoadGame()
        {
            if (CommonGameData.PlayerInfoData.GuildId > 0)
            {
                OpenPanel(View.InnerGuildPanel);
                UpdateUi();
            }
            else
            {
                OpenPanel(View.OuterGuildPanel);
            }
        }

        public void LoadGuild(GuildData guildData)
        {
            _guildData = guildData;
            OpenPanel(View.InnerGuildPanel);
            UpdateUi();
        }

        private async UniTaskVoid StartRaidBoss()
        {
            var message = new RaidBossMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
            };

            var result = await DataServer.PostData(message);
        }

        private void OpenAvailableGuildsPanel()
        {
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<AvailableGuildsPanelController>(openType: OpenType.Exclusive);
        }

        private void OpenCreateNewGuildPanel()
        {
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<NewGuildPanelController>(openType: OpenType.Exclusive);
        }

        private void OpenGuildTaskboardPanel()
        {
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<GuildTaskboardPanelController>(openType: OpenType.Exclusive);
        }

        private void UpdateUi()
        {
            if (_guildData == null)
                return;

            View.GuildName.text = _guildData.Name;
            View.GuildLevel.text = $"Level {_guildData.Level}";
            View.GuildId.text = $"ID: {_guildData.Id}";
        }

        private void OpenPanel(GameObject panel)
        {
            _currentPanel?.SetActive(false);
            panel.SetActive(true);
            _currentPanel = panel;
        }
    }
}