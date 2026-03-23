using System.Collections.Generic;
using Common.Db.CommonDictionaries;
using Cysharp.Threading.Tasks;
using Misc.Json;
using Network.DataServer;
using Network.DataServer.Messages.Guilds;
using Network.DataServer.Models.Guilds;
using UiExtensions.Panels;
using UniRx;
using VContainer;

namespace City.Buildings.Guild.RecruitRequestPanels
{
    public class RecruitRequestPanelController : UiPanelController<RecruitRequestPanelView>
    {
        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly IObjectResolver m_diContainer;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;

        private readonly Dictionary<int, GuildRecruitRequestView> m_views = new();

        protected override void OnLoadGame()
        {
            base.OnLoadGame();
            UpdateUI();
        }

        public override void OnShow()
        {
            View.RequestList.SetActive(CommonGameData.City.GuildPlayerSaveContainer.Requests.Count > 0);
            View.RequestListEmpty.SetActive(CommonGameData.City.GuildPlayerSaveContainer.Requests.Count == 0);
            base.OnShow();
        }

        private void UpdateUI()
        {
            if (CommonGameData.City.GuildPlayerSaveContainer.GuildData == null)
                return;

            View.RequestList.SetActive(CommonGameData.City.GuildPlayerSaveContainer.Requests.Count > 0);
            View.RequestListEmpty.SetActive(CommonGameData.City.GuildPlayerSaveContainer.Requests.Count == 0);

            foreach (GuildPlayerRequest request in CommonGameData.City.GuildPlayerSaveContainer.Requests)
                if (!m_views.ContainsKey(request.Id))
                {
                    GuildRecruitRequestView prefab =
                        UnityEngine.Object.Instantiate(View.GuildRecruitRequestViewPrefab, View.Content);
                    m_diContainer.Inject(prefab);
                    prefab.SetData(request, View.Scroll, m_commonDictionaries);
                    prefab.OnAccept.Subscribe(x => OnApplyRequest(x).Forget()).AddTo(Disposables);
                    prefab.OnDecline.Subscribe(x => OnDenyRequest(x).Forget()).AddTo(Disposables);
                    m_views.Add(request.Id, prefab);
                }
        }

        private async UniTaskVoid OnDenyRequest(GuildRecruitRequestView view)
        {
            DenyRequestMessage message = new DenyRequestMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                GuildId = view.GetData.GuildId,
                RequestId = view.GetData.Id
            };

            string result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result)) DeleteView(view);
        }

        private async UniTaskVoid OnApplyRequest(GuildRecruitRequestView view)
        {
            ApplyRequestMessage message = new ApplyRequestMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                GuildId = view.GetData.GuildId,
                RequestId = view.GetData.Id
            };

            string result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                DeleteView(view);
                GuildPlayerSaveContainer guildPlayerSaveContainer =
                    m_jsonConverter.Deserialize<GuildPlayerSaveContainer>(result);
                CommonGameData.PlayerInfoData.GuildId = guildPlayerSaveContainer.GuildData.Id;
                CommonGameData.City.GuildPlayerSaveContainer = guildPlayerSaveContainer;
            }
        }

        private void DeleteView(GuildRecruitRequestView view)
        {
            CommonGameData.City.GuildPlayerSaveContainer.Requests.Remove(view.GetData);
            m_views.Remove(view.GetData.Id);
            UnityEngine.Object.Destroy(view.gameObject);
            UpdateUI();
        }
    }
}