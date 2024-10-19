using Models.Data.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace City.Buildings.PlayerPanels.PlayerMiniPanels
{
    public class PlayerAvatarView : MonoBehaviour
    {
        [SerializeField] private Image _outlineAvatar;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _level;

        private PlayerData _playerData;

        public void SetData(PlayerData playerData)
        {
            _playerData = playerData;
            UpdateUi();
        }

        public void UpdateUi()
        {
            if (_playerData == null)
            {
                Debug.LogError("PlayerData is null.");
                return;
            }

            _image.sprite = SpriteUtils.LoadSprite(_playerData.AvatarPath);
            _level.text = $"{_playerData.Level}";
        }
    }
}
