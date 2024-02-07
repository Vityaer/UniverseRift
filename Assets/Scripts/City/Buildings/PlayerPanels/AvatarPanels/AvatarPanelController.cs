using City.Buildings.PlayerPanels.AvatarPanels.AvatarPanelDetails;
using Db.CommonDictionaries;
using Models.Misc.Avatars;
using System.Linq;
using UiExtensions.Misc;
using UiExtensions.Scroll.Interfaces;
using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace City.Buildings.PlayerPanels.AvatarPanels
{
    public class AvatarPanelController : UiPanelController<AvatarPanelView>
    {
        [Inject] private readonly AvatarPanelDetailsController _avatarPanelDetailsController;
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        private bool _isCreate = false;
        private DynamicUiList<AvatarIconView, AvatarModel> _avatarsWrapper;

        protected override void Show()
        {
            if (!_isCreate)
                CreateAvatars();

            base.Show();
        }

        private void CreateAvatars()
        {
            _isCreate = true;
            _avatarsWrapper = new(View.AvatarIconPrefab, View.Content, View.Scroll, OnSelectAvatar);
            var avatars = _commonDictionaries.AvatarModels.Values.ToList();
            _avatarsWrapper.ShowDatas(avatars);
        }

        private void OnSelectAvatar(AvatarIconView view)
        {
            _avatarPanelDetailsController.ShowAvatarDetail(view);
        }
    }
}
