using Cysharp.Threading.Tasks;
using Models.Data.Players;
using Models.Misc;
using System;
using Common.Db.CommonDictionaries;
using TMPro;
using UiExtensions.Misc;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.DataCheckUtils;

namespace City.Buildings.Friends.Panels.FriendRequests
{
    public class RequestFriendView : ScrollableUiView<FriendshipRequest>
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private TMP_Text _nickname;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private Button _agreeButton;
        [SerializeField] private Button _deniedButton;

        private ReactiveCommand<RequestFriendView> m_onAgreeFriendship = new();
        private ReactiveCommand<RequestFriendView> m_onDeniedFriendship = new();
        private PlayerData m_playerData;

        public IObservable<RequestFriendView> OnAgreeFriendship => m_onAgreeFriendship;
        public IObservable<RequestFriendView> OnDeniedFriendship => m_onDeniedFriendship;

        protected override void Start()
        {
            _agreeButton.OnClickAsObservable().Subscribe(_ => m_onAgreeFriendship.Execute(this)).AddTo(Disposable);
            _deniedButton.OnClickAsObservable().Subscribe(_ => m_onDeniedFriendship.Execute(this)).AddTo(Disposable);
            base.Start();
        }

        public override void SetData(FriendshipRequest data, ScrollRect scrollRect, CommonDictionaries commonDictionaries)
        {
            Data = data;
            Scroll = scrollRect.ThrowIfArgumentNull();
            CommonDataDictionaries = commonDictionaries.ThrowIfArgumentNull();
        }

        public void SetData(PlayerData playerData)
        {
            m_playerData = playerData;
            UpdateUi();
        }

        private void UpdateUi()
        {
            _avatar.sprite = SpriteUtils.LoadSprite(m_playerData.AvatarPath);
            _nickname.text = $"{m_playerData.Name}";
            _level.text = $"{m_playerData.Level}";
        }
    }
}
