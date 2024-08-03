using City.Buildings.PlayerPanels.PlayerMiniPanels;
using Common.Players;
using Models.Data.Players;
using Network.DataServer.Models.Guilds;
using System;
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

        private PlayersStorage _playersStorage;
        private PlayerData _playerData;

        public IObservable<GuildRecruitRequestView> OnAccept => _onAccept;
        public IObservable<GuildRecruitRequestView> OnDecline => _onDecline;

        [Inject]
        private void Construct(PlayersStorage playersStorage)
        {
            _playersStorage = playersStorage;
        }

        protected override void Start()
        {
            _acceptButton.OnClickAsObservable().Subscribe(_ => _onAccept.Execute(this)).AddTo(Disposable);
            _declineButton.OnClickAsObservable().Subscribe(_ => _onDecline.Execute(this)).AddTo(Disposable);
        }

        public override void SetData(GuildPlayerRequest data, ScrollRect scrollRect)
        {
            base.SetData(data, scrollRect);
            _playerData = _playersStorage.GetPlayerData(data.PlayerId);
            UpdateUi();
        }

        private void UpdateUi()
        {
            if (Data == null)
                return;

            _playerAvatarView.SetData(_playerData);
            _name.text = $"{_playerData.Name}";
        }
    }
}
