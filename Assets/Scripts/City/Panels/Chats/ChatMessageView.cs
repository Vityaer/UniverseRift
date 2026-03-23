using DG.Tweening;
using Models.Misc;
using TMPro;
using UiExtensions.Misc;
using UnityEngine.UI;
using UnityEngine;
using City.Buildings.PlayerPanels.PlayerMiniPanels;
using Common.Db.CommonDictionaries;
using Models.Data.Players;
using Utils.DataCheckUtils;

namespace City.Panels.Chats
{
    public class ChatMessageView : ScrollableUiView<ChatMessageData>
    {
        [SerializeField] private PlayerAvatarView _playerAvatar;
        [SerializeField] private TMP_Text _playerWritterName;
        [SerializeField] private TMP_Text _message;
        [SerializeField] private TMP_Text _senderDate;

        private Tween m_tween;

        public void SetPlayerData(PlayerData playerData)
        {
            _playerAvatar.SetData(playerData);
            _playerWritterName.text = playerData.Name;
        }

        public override void SetData(ChatMessageData data, ScrollRect scrollRect, CommonDictionaries dictionaries)
        {
            Data = data;
            Scroll = scrollRect.ThrowIfArgumentNull();
            CommonDataDictionaries = dictionaries.ThrowIfArgumentNull();
            UpdateUi();
        }

        private void UpdateUi()
        {
            _senderDate.text = Data.CreateDateTime;
            _message.text = Data.Message;
            
        }

        protected override void OnDestroy()
        {
            m_tween.Kill();
            base.OnDestroy();
        }
    }
}
