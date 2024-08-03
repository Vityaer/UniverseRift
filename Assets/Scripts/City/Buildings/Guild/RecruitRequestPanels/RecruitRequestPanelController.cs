using Misc.Json;
using Network.DataServer.Models;
using System.Collections.Generic;
using UiExtensions.Scroll.Interfaces;
using VContainer;

namespace City.Buildings.Guild.RecruitRequestPanels
{
    public class RecruitRequestPanelController : UiPanelController<RecruitRequestPanelView>
    {
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly IObjectResolver _diContainer;

        private Dictionary<int, GuildRecruitRequestView> _recruitRequestsView = new();

        private GuildData _guildData => CommonGameData.City.GuildPlayerSaveContainer.GuildData;

        public override void Start()
        {
            base.Start();
        }

        protected override void OnLoadGame()
        {
            base.OnLoadGame();
            UpdateUi();
        }

        private void OpenRequestsPanel()
        {

        }

        private void UpdateUi()
        {
            var requests = CommonGameData.City.GuildPlayerSaveContainer.Requests;
            foreach (var request in requests)
            {
                GuildRecruitRequestView prefab = null;
                if (!_recruitRequestsView.ContainsKey(request.PlayerId))
                {
                    prefab = UnityEngine.GameObject.Instantiate(View.GuildRecruitRequestViewPrefab, View.Content);
                    _recruitRequestsView.Add(request.PlayerId, prefab);
                    _diContainer.Inject(prefab);
                    prefab.SetData(request, View.Scroll);
                }
            }
        }
    }
}
