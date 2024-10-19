using Cysharp.Threading.Tasks;
using Misc.Json;
using Network.DataServer;
using Network.DataServer.Messages.Guilds;
using Network.DataServer.Models;
using Network.DataServer.Models.Guilds;
using System.Collections.Generic;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;

namespace City.Buildings.Guild.RecruitRequestPanels
{
    public class RecruitRequestPanelController : UiPanelController<RecruitRequestPanelView>
    {
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly IObjectResolver _diContainer;

        private Dictionary<int, GuildRecruitRequestView> _views = new();

        private GuildData _guildData => CommonGameData.City.GuildPlayerSaveContainer.GuildData;

        public override void Start()
        {
            base.Start();
        }

        protected override void OnLoadGame()
        {
            base.OnLoadGame();
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (CommonGameData.City.GuildPlayerSaveContainer.GuildData == null)
                return;

            foreach (var request in CommonGameData.City.GuildPlayerSaveContainer.Requests)
            {
                if (!_views.ContainsKey(request.Id))
                {
                    var prefab = UnityEngine.Object.Instantiate(View.GuildRecruitRequestViewPrefab, View.Content);
                    _diContainer.Inject(prefab);
                    prefab.SetData(request, View.Scroll);
                    prefab.OnAccept.Subscribe(x => OnApplyRequest(x).Forget()).AddTo(Disposables);
                    prefab.OnDecline.Subscribe(x => OnDenyRequest(x).Forget()).AddTo(Disposables);
                    _views.Add(request.Id, prefab);
                }
            }
        }

        private async UniTaskVoid OnDenyRequest(GuildRecruitRequestView view)
        {
            var message = new DenyRequestMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                GuildId = view.GetData.GuildId,
                RequestId = view.GetData.Id
            };

            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                DeleteView(view);
            }
        }

        private async UniTaskVoid OnApplyRequest(GuildRecruitRequestView view)
        {
            var message = new ApplyRequestMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                GuildId = view.GetData.GuildId,
                RequestId = view.GetData.Id
            };

            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                DeleteView(view);
                var guildPlayerSaveContainer = _jsonConverter.Deserialize<GuildPlayerSaveContainer>(result);
                CommonGameData.PlayerInfoData.GuildId = guildPlayerSaveContainer.GuildData.Id;
                CommonGameData.City.GuildPlayerSaveContainer = guildPlayerSaveContainer;
            }
        }

        private void DeleteView(GuildRecruitRequestView view)
        {
            CommonGameData.City.GuildPlayerSaveContainer.Requests.Remove(view.GetData);
            _views.Remove(view.GetData.Id);
            UnityEngine.GameObject.Destroy(view.gameObject);
            UpdateUI();
        }
    }
}
