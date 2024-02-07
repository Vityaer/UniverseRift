using Cysharp.Threading.Tasks;
using Models.Data.Players;
using Models.Misc;
using System;
using TMPro;
using UiExtensions.Misc;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace City.Buildings.Friends.Panels.FriendRequests
{
    public class RequestFriendView : ScrollableUiView<FriendshipRequest>
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private TMP_Text _nickname;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private Button _agreeButton;
        [SerializeField] private Button _deniedButton;

        private ReactiveCommand<RequestFriendView> _onAgreeFriendship = new();
        private ReactiveCommand<RequestFriendView> _onDeniedFriendship = new();
        private PlayerData _playerData;

        public IObservable<RequestFriendView> OnAgreeFriendship => _onAgreeFriendship;
        public IObservable<RequestFriendView> OnDeniedFriendship => _onDeniedFriendship;

        protected override void Start()
        {
            _agreeButton.OnClickAsObservable().Subscribe(_ => _onAgreeFriendship.Execute(this)).AddTo(Disposable);
            _deniedButton.OnClickAsObservable().Subscribe(_ => _onDeniedFriendship.Execute(this)).AddTo(Disposable);
            base.Start();
        }

        public override void SetData(FriendshipRequest data, ScrollRect scrollRect)
        {
            Data = data;
            Scroll = scrollRect;
        }

        public void SetData(PlayerData playerData)
        {
            _playerData = playerData;
            UpdateUi();
        }

        private void UpdateUi()
        {
            _avatar.sprite = SpriteUtils.LoadSprite(_playerData.AvatarPath);
            _nickname.text = $"{_playerData.Name}";
            _level.text = $"{_playerData.Level}";
        }
    }
}
