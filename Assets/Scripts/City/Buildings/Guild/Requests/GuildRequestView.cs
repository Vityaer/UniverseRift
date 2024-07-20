using Network.DataServer.Models.Guilds;
using System;
using TMPro;
using UiExtensions.Misc;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Guild.Requests
{
    public class GuildRequestView : PlayerScrollableUiView<GuildPlayerRequest>
    {
        [SerializeField] private Button _applyButton;
        [SerializeField] private Button _denyButton;
        [SerializeField] private Image _playerAvatar;
        [SerializeField] private TMP_Text _playerLevel;
        [SerializeField] private TMP_Text _playerName;

        private ReactiveCommand<GuildRequestView> _onApplyRequest = new();
        private ReactiveCommand<GuildRequestView> _onDenyRequest = new();

        public IObservable<GuildRequestView> OnApplyRequest => _onApplyRequest;
        public IObservable<GuildRequestView> OnDenyRequest => _onDenyRequest;

        protected override void Start()
        {
            _applyButton.OnClickAsObservable().Subscribe(_ => ApplyRequest()).AddTo(Disposable);
            _denyButton.OnClickAsObservable().Subscribe(_ => DenyRequest()).AddTo(Disposable);
            base.Start();
        }

        private void DenyRequest()
        {
            _onDenyRequest.Execute(this);
        }

        private void ApplyRequest()
        {
            _onApplyRequest.Execute(this);
        }

        public override void SetData(GuildPlayerRequest data, ScrollRect scrollRect)
        {
            base.SetData(data, scrollRect, data.PlayerId);
        }
    }
}
