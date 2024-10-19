using City.Buildings.Guild.GuildRecruitDetailPanels;
using City.Buildings.Guild.RecruitRequestPanels;
using City.Buildings.Guild.RecruitViews;
using City.Buildings.Guild.Utils;
using Misc.Json;
using Network.DataServer.Models;
using Network.DataServer.Models.Guilds;
using System.Collections.Generic;
using UiExtensions.Misc;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Buildings.Guild.GuildAdministrations
{
    public class GuildAdministrationPanelController : UiPanelController<GuildAdministrationPanelView>
    {
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly IObjectResolver _diContainer;
        [Inject] private readonly IUiMessagesPublisherService _uiMessagesPublisher;
        [Inject] private readonly GuildRecruitDetailPanelController _guildRecruitDetailPanelController;

        private Dictionary<int, GuildRecruitView> _recruitsView = new();

        private GuildData _guildData => CommonGameData.City.GuildPlayerSaveContainer.GuildData;

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
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<RecruitRequestPanelController>(openType: OpenType.Exclusive);
        }

        protected override void Show()
        {
            UpdateUi();
            base.Show();
        }

        private void UpdateUi()
        {
            if (_guildData == null)
                return;

            View.GuildName.text = _guildData.Name;
            View.GuildLevel.text = $"Level {_guildData.Level}";
            View.GuildId.text = $"ID: {_guildData.Id}";

            var recruits = CommonGameData.City.GuildPlayerSaveContainer.GuildRecruits;
            recruits.Sort(new RecruitDamageComparer());

            var index = 0;
            View.HaveRequest.SetActive(CommonGameData.City.GuildPlayerSaveContainer.Requests.Count > 0);

            foreach (var recruit in recruits)
            {
                GuildRecruitView prefab = null;
                if (_recruitsView.TryGetValue(recruit.PlayerId, out prefab))
                {
                    prefab.SetData(recruit, View.Scroll);
                }
                else
                {
                    prefab = UnityEngine.GameObject.Instantiate(View.GuildRecruitViewPrefab, View.Content);
                    _recruitsView.Add(recruit.PlayerId, prefab);
                    _diContainer.Inject(prefab);
                    prefab.SetData(recruit, View.Scroll);
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
            _guildRecruitDetailPanelController.ShowRecruitDetail(view.GetData);
        }
    }
}
