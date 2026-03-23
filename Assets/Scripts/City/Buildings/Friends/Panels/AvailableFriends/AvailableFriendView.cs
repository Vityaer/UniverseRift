using Models.Misc;
using Common.Db.CommonDictionaries;
using TMPro;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.DataCheckUtils;

namespace City.Buildings.Friends.Panels.AvailableFriends
{
    public class AvailableFriendView : ScrollableUiView<AvailableFriendData>
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private TMP_Text _nickname;
        [SerializeField] private TMP_Text _level;

        public override void SetData(AvailableFriendData data, ScrollRect scrollRect, CommonDictionaries dictionaries)
        {
            Data = data;
            Scroll = scrollRect.ThrowIfArgumentNull();
            CommonDataDictionaries = dictionaries.ThrowIfArgumentNull();
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
