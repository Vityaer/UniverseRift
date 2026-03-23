using Common.Db.CommonDictionaries;
using DG.Tweening;
using Models.Data.Players;
using Models.Misc;
using TMPro;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.DataCheckUtils;

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

        private Tween m_tweenSended;
        private Tween m_tweenReceived;
        private int m_myId;
        private PlayerData m_playerData;
        private bool m_increased;

        public float AnimationTime = 0.5f;

        public override void SetData(FriendshipData data, ScrollRect scrollRect, CommonDictionaries dictionaries)
        {
            Data = data;
            Scroll = scrollRect.ThrowIfArgumentNull();
            CommonDataDictionaries = dictionaries.ThrowIfArgumentNull();
        }

        public void SetData(int id, PlayerData playerData)
        {
            m_myId = id;
            m_playerData = playerData;

            if (Data.FirstPlayerId == m_myId)
            {
                if (Data.PresentForSecondPlayer)
                    _sendedImage.DOFade(1f, 0f);
            }
            else
            {
                if (Data.PresentForFirstPlayer)
                    _sendedImage.DOFade(1f, 0f);
            }

            if (Data.FirstPlayerId == m_myId)
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
            if (Data.FirstPlayerId == m_myId)
            {
                Data.PresentForSecondPlayer = true;
            }
            else
            {
                Data.PresentForFirstPlayer = true;
            }

            m_tweenSended = _sendedImage.DOFade(1f, AnimationTime);
        }

        public void ShowReceivedHeart()
        {
            if (Data.FirstPlayerId == m_myId)
            {
                Data.FirstPlayerRecieved = true;
            }
            else
            {
                Data.SecondPlayerRecieved = true;
            }

            m_tweenReceived = _receivedImage.DOFade(1f, AnimationTime);
        }

        private void UpdateUi()
        {
            _playerName.text = $"{m_playerData.Name}";
            _playerLevel.text = $"{m_playerData.Level}";
            _playerAvatar.sprite = SpriteUtils.LoadSprite(m_playerData.AvatarPath);
            _friendshipLevel.text = $"{Data.Level} / 30";
        }

        protected override void OnDestroy()
        {
            m_tweenSended.Kill();
            m_tweenReceived.Kill();
            base.OnDestroy();
        }
    }
}
