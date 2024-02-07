using Models.Misc.Avatars;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace City.Buildings.PlayerPanels.AvatarPanels
{
    public class AvatarIconView : ScrollableUiView<AvatarModel>
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private Image _outline;

        public Image Avatar => _avatar;

        public override void SetData(AvatarModel data, ScrollRect scrollRect)
        {
            Data = data;
            Scroll = scrollRect;

            UpdateUi();
        }

        private void UpdateUi()
        {
            _avatar.sprite = SpriteUtils.LoadSprite(Data.Path);
        }
    }
}
