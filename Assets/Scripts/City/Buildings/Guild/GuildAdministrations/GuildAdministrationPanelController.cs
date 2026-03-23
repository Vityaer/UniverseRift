using City.Buildings.Guild.GuildRecruitDetailPanels;
using City.Buildings.Guild.RecruitRequestPanels;
using City.Buildings.Guild.RecruitViews;
using City.Buildings.Guild.Utils;
using Misc.Json;
using Network.DataServer.Models;
using Network.DataServer.Models.Guilds;
using System.Collections.Generic;
using Common.Db.CommonDictionaries;
using UiExtensions.Misc;
using UiExtensions.Panels;
using UniRx;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Buildings.Guild.GuildAdministrations
{
    public class GuildAdministrationPanelController : UiPanelController<GuildAdministrationPanelView>
    {
        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly IObjectResolver m_diContainer;
        [Inject] private readonly IUiMessagesPublisherService m_uiMessagesPublisher;
        [Inject] private readonly GuildRecruitDetailPanelController m_guildRecruitDetailPanelController;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        
        private readonly Dictionary<int, GuildRecruitView> m_recruitsView = new();

        private GuildData GuildData => CommonGameData.City.GuildPlayerSaveContainer.GuildData;

        public override void Start()
        {
            View.RequestPanelOpenButton.OnClickAsObservable().Subscribe(_ => OpenRequestsPanel()).AddTo(Disposables);
            base.Start();
        }

        protected override void OnLoadGame()
        {
            base.OnLoadGame();
            UpdateUi();
        }

        private void OpenRequestsPanel()
        {
            m_uiMessagesPublisher.OpenWindowPublisher.OpenWindow<RecruitRequestPanelController>(openType: OpenType.Additive);
        }

        protected override void Show()
        {
            UpdateUi();
            base.Show();
        }

        private void UpdateUi()
        {
            if (GuildData == null)
                return;

            View.GuildName.text = GuildData.Name;
            View.GuildLevel.text = $"Level {GuildData.Level}";
            View.GuildId.text = $"ID: {GuildData.Id}";

            var recruits = CommonGameData.City.GuildPlayerSaveContainer.GuildRecruits;
            recruits.Sort(new RecruitDamageComparer());

            var index = 0;
            View.HaveRequest.SetActive(CommonGameData.City.GuildPlayerSaveContainer.Requests.Count > 0);

            foreach (var recruit in recruits)
            {
                GuildRecruitView prefab = null;
                if (m_recruitsView.TryGetValue(recruit.PlayerId, out prefab))
                {
                    prefab.SetData(recruit, View.Scroll, m_commonDictionaries);
                }
                else
                {
                    prefab = UnityEngine.GameObject.Instantiate(View.GuildRecruitViewPrefab, View.Content);
                    m_recruitsView.Add(recruit.PlayerId, prefab);
                    m_diContainer.Inject(prefab);
                    prefab.SetData(recruit, View.Scroll, m_commonDictionaries);
                    prefab.OnSelect.Subscribe(OpenRecruitDetails).AddTo(Disposables);
                }

                if (prefab != null)
                {
                    prefab.transform.SetSiblingIndex(index);
                    index++;
                }
            }
        }

        private void OpenRecruitDetails(ScrollableUiView<RecruitData> view)
        {
            m_guildRecruitDetailPanelController.ShowRecruitDetail(view.GetData);
        }
    }
}
