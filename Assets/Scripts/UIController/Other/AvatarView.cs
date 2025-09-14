using Models.Data.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UController.Other
{
    public class AvatarView : MonoBehaviour
    {
        [SerializeField] private Image _mainImage;
        [SerializeField] private Image _borderImage;
        [SerializeField] private TMP_Text _level;

        public void SetData(PlayerData playerInfoData)
        {
            if (playerInfoData == null)
            {
                Debug.LogError("playerInfoData null");
                return;
            }

            _level.text = $"{playerInfoData.Level}";
            _mainImage.sprite = SpriteUtils.LoadSprite(playerInfoData.AvatarPath);
        }
    }
}