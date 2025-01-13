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
using UiExtensions.Misc;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Buildings.Guild.AvailableGuildsPanels
{
    public class AvailableGuildsPanelController : UiPanelController<AvailableGuildsPanelView>, IDisposable
    {
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly IUiMessagesPublisherService _uiMessagesPublisher;
        [Inject] private readonly NewGuildPanelController _newGuildPanelController;
        [Inject] private readonly IUiMessagesPublisherService _messagesPublisher;
        [Inject] private readonly ILocalizationSystem _localizationSystem;
        
        private List<GuildData> _guilds = new();
        private DynamicUiList<AvailableGuildView, GuildData> _dynamicUiList;
        private int _index = -1;

        public override void Start()
        {
            View.OpenPanelCreateGuildButton.OnClickAsObservable().Subscribe(_ => OpenCreateNewGuildPanel()).AddTo(Disposables);
            _dynamicUiList = new DynamicUiList<AvailableGuildView, GuildData>(View.Prefab, View.Content, View.Scroll, OnSelectGuild);
            base.Start();
            _newGuildPanelController.OnCreateGuild.Subscribe(_ => OnCreateGuild()).AddTo(Disposables);
        }

        private void OnCreateGuild()
        {
            _messagesPublisher.MessageCloseWindowPublisher.CloseWindow<AvailableGuildsPanelController>();
        }

        private void OnSelectGuild(AvailableGuildView view)
        {
            CreateGuildEnterRequest(view).Forget();
        }

        public override void OnShow()
        {
            if (_guilds.Count == 0 && CommonGameData.PlayerInfoData.GuildId < 0)
                StartLoadGuilds();

            base.OnShow();
        }

        private void StartLoadGuilds()
        {
            LoadAvailableGuilds().Forget();
        }

        private void OpenCreateNewGuildPanel()
        {
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<NewGuildPanelController>(openType: OpenType.Exclusive);
        }

        private async UniTaskVoid LoadAvailableGuilds()
        {
            _index += 1;
            var message = new GetAvailableGuildsMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                PagginationIndex = _index
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                _guilds = _jsonConverter.Deserialize<List<GuildData>>(result);
                if (_guilds.Count > 0)
                {
                    View.NotFoundGuildsMessageText.SetActive(false);
                    _dynamicUiList.ShowDatas(_guilds);
                }
                else
                {
                    View.NotFoundGuildsMessageText.SetActive(true);
                    _dynamicUiList.ClearList();
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
            _dynamicUiList?.Dispose();
            base.Dispose();
        }
    }
}
