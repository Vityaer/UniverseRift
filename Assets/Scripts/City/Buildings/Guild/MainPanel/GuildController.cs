using City.Buildings.Abstractions;
using City.Buildings.Guild.AvailableGuildsPanels;
using City.Buildings.Guild.BossRaid;
using City.Buildings.Guild.GuildAdministrations;
using City.Buildings.Guild.GuildMarket;
using City.Buildings.Guild.GuildTaskboardPanels;
using City.Buildings.Guild.Requests;
using Cysharp.Threading.Tasks;
using Models.Common;
using Network.DataServer.Models.Guilds;
using UniRx;
using UnityEngine;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Buildings.Guild
{
    public class GuildController : BaseBuilding<GuildView>
    {
        private const int BOSS_FREE_RAID_COUNT = 2;
        private const int BOSS_RAID_REFRESH_HOURS = 16;
        private const int BOSS_RAID_COST_STEP = 50;

        [Inject] private readonly IUiMessagesPublisherService _uiMessagesPublisher;


        private GameObject _currentPanel;
        public ReactiveCommand OnLoadGuild = new();

        protected override void OnStart()
        {
            View.GuildTaskboardButton.OnClickAsObservable().Subscribe(_ => OpenGuildTaskboardPanel()).AddTo(Disposables);
            View.AdministrationButton.OnClickAsObservable().Subscribe(_ => OpenGuildAdministration()).AddTo(Disposables);
            View.MarketButton.OnClickAsObservable().Subscribe(_ => OpenGuildMarket()).AddTo(Disposables);
            View.BossRaidButton.OnClickAsObservable().Subscribe(_ => OpenBossRaidPanel()).AddTo(Disposables);
            base.OnStart();
        }

        private void OpenBossRaidPanel()
        {
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<GuildBossRaidPanelController>(openType: OpenType.Exclusive);
        }

        private void OpenGuildMarket()
        {
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<GuildMarketController>(openType: OpenType.Exclusive);
        }

        private void OpenGuildAdministration()
        {
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<GuildAdministrationPanelController>(openType: OpenType.Exclusive);
        }

        protected override void OnLoadGame()
        {
            if (CommonGameData.PlayerInfoData.GuildId > 0)
            {
                OpenPanel(View.InnerGuildPanel);
            }
        }

        public void CreateGuild(GuildPlayerSaveContainer guildPlayerSaveContainer)
        {
            CommonGameData.City.GuildPlayerSaveContainer = guildPlayerSaveContainer;
            OnLoadGuild.Execute();
        }

        private void OpenRequestsPanel()
        {
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<GuildRequestPanelController>(openType: OpenType.Additive);
        }

        private void OpenAvailableGuildsPanel()
        {
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<AvailableGuildsPanelController>(openType: OpenType.Exclusive);
        }

        private void OpenGuildTaskboardPanel()
        {
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<GuildTaskboardPanelController>(openType: OpenType.Exclusive);
        }

        private void OpenPanel(GameObject panel)
        {
            _currentPanel?.SetActive(false);
            panel.SetActive(true);
            _currentPanel = panel;
        }

        protected override void OpenPage()
        {
            if (CommonGameData.PlayerInfoData.GuildId >= 0)
            {
                Debug.Log("OpenGuildData");
                OpenGuildData();
            }
            else
            {
                Debug.Log("OpenAvailableGuildsPanel");
                OpenAvailableGuildsPanel();
            }

            base.OpenPage();
        }

        private void OpenGuildData()
        {
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<GuildController>(openType: OpenType.Exclusive);
            OpenPanel(View.InnerGuildPanel);
        }
    }
}