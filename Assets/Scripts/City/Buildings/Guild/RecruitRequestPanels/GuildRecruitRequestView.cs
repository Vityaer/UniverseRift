using City.Buildings.PlayerPanels.PlayerMiniPanels;
using Common.Players;
using Models.Data.Players;
using Network.DataServer.Models.Guilds;
using System;
using Common.Db.CommonDictionaries;
using TMPro;
using UiExtensions.Misc;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace City.Buildings.Guild.RecruitRequestPanels
{
    public class GuildRecruitRequestView : ScrollableUiView<GuildPlayerRequest>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _declineButton;
        [SerializeField] private PlayerAvatarView _playerAvatarView;

        protected ReactiveCommand<GuildRecruitRequestView> _onAccept = new();
        protected ReactiveCommand<GuildRecruitRequestView> _onDecline = new();

        private PlayersStorage m_playersStorage;
        private PlayerData m_playerData;

        public IObservable<GuildRecruitRequestView> OnAccept => _onAccept;
        public IObservable<GuildRecruitRequestView> OnDecline => _onDecline;

        [Inject]
        private void Construct(PlayersStorage playersStorage)
        {
            m_playersStorage = playersStorage;
        }

        protected override void Start()
        {
            _acceptButton.OnClickAsObservable().Subscribe(_ => _onAccept.Execute(this)).AddTo(Disposable);
            _declineButton.OnClickAsObservable().Subscribe(_ => _onDecline.Execute(this)).AddTo(Disposable);
        }

        public override void SetData(GuildPlayerRequest data, ScrollRect scrollRect, CommonDictionaries dictionaries)
        {
            base.SetData(data, scrollRect, dictionaries);
            m_playerData = m_playersStorage.GetPlayerData(data.PlayerId);
            UpdateUi();
        }

        private void UpdateUi()
        {
            if (Data == null)
                return;

            _playerAvatarView.SetData(m_playerData);
            _name.text = $"{m_playerData.Name}";
        }
    }
}
