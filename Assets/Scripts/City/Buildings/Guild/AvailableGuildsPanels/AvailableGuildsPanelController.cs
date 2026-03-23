using City.Buildings.Guild.NewGuildPanels;
using Cysharp.Threading.Tasks;
using LocalizationSystems;
using Misc.Json;
using Network.DataServer;
using Network.DataServer.Messages.Guilds;
using Network.DataServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Common.Db.CommonDictionaries;
using UiExtensions.Misc;
using UiExtensions.Panels;
using UniRx;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Buildings.Guild.AvailableGuildsPanels
{
    public class AvailableGuildsPanelController : UiPanelController<AvailableGuildsPanelView>, IDisposable
    {
        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly IUiMessagesPublisherService m_uiMessagesPublisher;
        [Inject] private readonly NewGuildPanelController m_newGuildPanelController;
        [Inject] private readonly IUiMessagesPublisherService m_messagesPublisher;
        [Inject] private readonly ILocalizationSystem m_localizationSystem;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        
        private List<GuildData> m_guilds = new();
        private DynamicUiList<AvailableGuildView, GuildData> m_dynamicUiList;
        private int m_index = -1;

        public override void Start()
        {
            View.OpenPanelCreateGuildButton.OnClickAsObservable().Subscribe(_ => OpenCreateNewGuildPanel()).AddTo(Disposables);
            m_dynamicUiList = new DynamicUiList<AvailableGuildView, GuildData>(View.Prefab, View.Content, View.Scroll, m_commonDictionaries, OnSelectGuild);
            base.Start();
            m_newGuildPanelController.OnCreateGuild.Subscribe(_ => OnCreateGuild()).AddTo(Disposables);
        }

        private void OnCreateGuild()
        {
            m_messagesPublisher.MessageCloseWindowPublisher.CloseWindow<AvailableGuildsPanelController>();
        }

        private void OnSelectGuild(AvailableGuildView view)
        {
            CreateGuildEnterRequest(view).Forget();
        }

        public override void OnShow()
        {
            if (m_guilds.Count == 0 && CommonGameData.PlayerInfoData.GuildId < 0)
                StartLoadGuilds();

            base.OnShow();
        }

        private void StartLoadGuilds()
        {
            LoadAvailableGuilds().Forget();
        }

        private void OpenCreateNewGuildPanel()
        {
            m_uiMessagesPublisher.OpenWindowPublisher.OpenWindow<NewGuildPanelController>(openType: OpenType.Additive);
        }

        private async UniTaskVoid LoadAvailableGuilds()
        {
            m_index += 1;
            var message = new GetAvailableGuildsMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                PagginationIndex = m_index
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                m_guilds = m_jsonConverter.Deserialize<List<GuildData>>(result);
                if (m_guilds.Count > 0)
                {
                    View.NotFoundGuildsMessageText.SetActive(false);
                    m_dynamicUiList.ShowDatas(m_guilds);
                }
                else
                {
                    View.NotFoundGuildsMessageText.SetActive(true);
                    m_dynamicUiList.ClearList();
                }
            }
            else
            {
                View.NotFoundGuildsMessageText.SetActive(true);
            }
        }

        private async UniTaskVoid CreateGuildEnterRequest(AvailableGuildView availableGuildView)
        {
            var message = new CreatePlayerRequestMessage { PlayerId = CommonGameData.PlayerInfoData.Id, GuildId = availableGuildView.GetData.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                availableGuildView.SetUsed();
            }
        }

        public override void Dispose()
        {
            m_dynamicUiList?.Dispose();
            base.Dispose();
        }
    }
}
