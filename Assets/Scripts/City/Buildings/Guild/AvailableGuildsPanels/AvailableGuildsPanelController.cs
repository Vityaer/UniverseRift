using Cysharp.Threading.Tasks;
using Misc.Json;
using Network.DataServer;
using Network.DataServer.Messages.Guilds;
using Network.DataServer.Models;
using System;
using System.Collections.Generic;
using UiExtensions.Misc;
using UiExtensions.Scroll.Interfaces;
using VContainer;

namespace City.Buildings.Guild.AvailableGuildsPanels
{
    public class AvailableGuildsPanelController : UiPanelController<AvailableGuildsPanelView>, IDisposable
    {
        [Inject] private readonly IJsonConverter _jsonConverter;

        private List<GuildData> _guilds = new();
        private DynamicUiList<AvailableGuildView, GuildData> _dynamicUiList;
        private int _index = -1;

        public override void Start()
        {
            _dynamicUiList = new DynamicUiList<AvailableGuildView, GuildData>(View.Prefab, View.Content, View.Scroll, OnSelectGuild);
            base.Start();
        }

        private void OnSelectGuild(AvailableGuildView view)
        {
            CreateGuildEnterRequest(view).Forget();
        }

        public override void OnShow()
        {
            if (_guilds.Count == 0)
                StartLoadGuilds();

            base.OnShow();
        }

        private void StartLoadGuilds()
        {
            LoadAvailableGuilds().Forget();
        }

        private async UniTaskVoid LoadAvailableGuilds()
        {
            _index += 1;
            var message = new GetAvailableGuildsMessage { PlayerId = CommonGameData.PlayerInfoData.Id, PagginationIndex = _index };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                _guilds = _jsonConverter.FromJson<List<GuildData>>(result);
                if (_guilds.Count > 0)
                {
                    View.MessageText.text = string.Empty;
                    _dynamicUiList.ShowDatas(_guilds);
                }
                else
                {
                    _dynamicUiList.ClearList();
                    View.MessageText.text = "Not found available guilds";
                }
            }
            else
            {
                View.MessageText.text = "Not found available guilds";
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
            _dynamicUiList.Dispose();
            base.Dispose();
        }
    }
}
