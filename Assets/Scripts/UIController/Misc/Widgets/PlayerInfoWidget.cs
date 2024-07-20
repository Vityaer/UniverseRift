using Models.Data.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using VContainerUi.Abstraction;

namespace UIController.Misc.Widgets
{
    public class PlayerInfoWidget : UiView
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;

        private PlayerData _data;

        public void SetData(PlayerData data)
        {
            _data = data;
            _avatar.sprite = SpriteUtils.LoadSprite(_data.AvatarPath);
            _name.text = _data.Name;
            _level.text = $"{_data.Level}";
        }
    }
}
