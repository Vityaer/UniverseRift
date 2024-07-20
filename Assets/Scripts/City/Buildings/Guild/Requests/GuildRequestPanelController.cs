using Cysharp.Threading.Tasks;
using Network.DataServer;
using Network.DataServer.Messages.Guilds;
using System.Collections.Generic;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;

namespace City.Buildings.Guild.Requests
{
    public class GuildRequestPanelController : UiPanelController<GuildRequestPanelView>
    {
        [Inject] private readonly GuildController _guildController;

        private Dictionary<int, GuildRequestView> _views = new();
        private bool _needRefreshData = false;

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
                    var prefab = UnityEngine.Object.Instantiate(View.GuildRequestViewPrefab, View.Content);
                    prefab.SetData(request, View.ScrollRect);
                    prefab.OnApplyRequest.Subscribe(x => OnApplyRequest(x).Forget()).AddTo(Disposables);
                    prefab.OnDenyRequest.Subscribe(x => OnDenyRequest(x).Forget()).AddTo(Disposables);
                }
            }
        }

        private async UniTaskVoid OnDenyRequest(GuildRequestView view)
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
                _needRefreshData = true;
            }

        }

        private async UniTaskVoid OnApplyRequest(GuildRequestView view)
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
                _needRefreshData = true;
            }
        }
    }
}
