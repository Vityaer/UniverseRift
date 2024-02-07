using Models.Misc;
using System;
using TMPro;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace City.Buildings.Friends.Panels.AvailableFriends
{
    public class AvailableFriendView : ScrollableUiView<AvailableFriendData>
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private TMP_Text _nickname;
        [SerializeField] private TMP_Text _level;

        public override void SetData(AvailableFriendData data, ScrollRect scrollRect)
        {
            Data = data;
            Scroll = scrollRect;
            UpdateUi();
        }

        public void SetDone()
        {
            Button.interactable = false;
        }

        private void UpdateUi()
        {
            _nickname.text = Data.Name;
            _level.text = $"{Data.Level}";
            _avatar.sprite = SpriteUtils.LoadSprite(Data.IconPath);
        }
    }
}
