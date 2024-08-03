using DG.Tweening;
using Models.Data.Players;
using Models.Misc;
using System;
using TMPro;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace City.Buildings.Friends.FriendViews
{
    public class FriendView : ScrollableUiView<FriendshipData>
    {
        [SerializeField] private Image _playerAvatar;
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private TMP_Text _playerLevel;
        [SerializeField] private TMP_Text _playerLastGameEnter;
        [SerializeField] private TMP_Text _friendshipLevel;
        [SerializeField] private Image _sendedImage;
        [SerializeField] private Image _receivedImage;

        private Tween _tweenSended;
        private Tween _tweenReceived;
        private int _myId;
        private PlayerData _playerData;
        private bool _increased;

        public float AnimationTime = 0.5f;

        public override void SetData(FriendshipData data, ScrollRect scrollRect)
        {
            Data = data;
            Scroll = scrollRect;
        }

        public void SetData(int id, PlayerData playerData)
        {
            _myId = id;
            _playerData = playerData;

            if (Data.FirstPlayerId == _myId)
            {
                if (Data.PresentForSecondPlayer)
                    _sendedImage.DOFade(1f, 0f);
            }
            else
            {
                if (Data.PresentForFirstPlayer)
                    _sendedImage.DOFade(1f, 0f);
            }

            if (Data.FirstPlayerId == _myId)
            {
                if (Data.FirstPlayerRecieved)
                    _receivedImage.DOFade(1f, 0f);
            }
            else
            {
                if (Data.SecondPlayerRecieved)
                    _receivedImage.DOFade(1f, 0f);
            }

            UpdateUi();
        }

        public void ShowSendedHeart()
        {
            if (Data.FirstPlayerId == _myId)
            {
                Data.PresentForSecondPlayer = true;
            }
            else
            {
                Data.PresentForFirstPlayer = true;
            }

            _tweenSended = _sendedImage.DOFade(1f, AnimationTime);
        }

        public void ShowReceivedHeart()
        {
            if (Data.FirstPlayerId == _myId)
            {
                Data.FirstPlayerRecieved = true;
            }
            else
            {
                Data.SecondPlayerRecieved = true;
            }

            _tweenReceived = _receivedImage.DOFade(1f, AnimationTime);
        }

        private void UpdateUi()
        {
            _playerName.text = $"{_playerData.Name}";
            _playerLevel.text = $"{_playerData.Level}";
            _playerAvatar.sprite = SpriteUtils.LoadSprite(_playerData.AvatarPath);
            _friendshipLevel.text = $"{Data.Level} / 30";
        }

        protected override void OnDestroy()
        {
            _tweenSended.Kill();
            _tweenReceived.Kill();
            base.OnDestroy();
        }
    }
}
