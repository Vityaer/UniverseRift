using DG.Tweening;
using Models.Misc;
using TMPro;
using UiExtensions.Misc;
using UnityEngine.UI;
using UnityEngine;
using City.Buildings.PlayerPanels.PlayerMiniPanels;
using Models.Data.Players;

namespace City.Panels.Chats
{
    public class ChatMessageView : ScrollableUiView<ChatMessageData>
    {
        [SerializeField] private PlayerAvatarView _playerAvatar;
        [SerializeField] private TMP_Text _playerWritterName;
        [SerializeField] private TMP_Text _message;
        [SerializeField] private TMP_Text _senderDate;

        private Tween _tween;
        private PlayerData _playerData;

        public void SetPlayerData(PlayerData playerData)
        {
            _playerData = playerData;
            _playerAvatar.SetData(playerData);
            _playerWritterName.text = playerData.Name;
        }

        public override void SetData(ChatMessageData data, ScrollRect scrollRect)
        {
            Data = data;
            Scroll = scrollRect;
            UpdateUi();
        }

        private void UpdateUi()
        {
            _senderDate.text = Data.CreateDateTime;
            _message.text = Data.Message;
            
        }

        protected override void OnDestroy()
        {
            _tween.Kill();
            base.OnDestroy();
        }
    }
}
