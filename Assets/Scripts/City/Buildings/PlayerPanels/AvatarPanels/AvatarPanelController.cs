using System.Collections.Generic;
using System.Linq;
using City.Buildings.PlayerPanels.AvatarPanels.AvatarPanelDetails;
using Common.Db.CommonDictionaries;
using Models.Misc.Avatars;
using UiExtensions.Misc;
using UiExtensions.Panels;
using VContainer;

namespace City.Buildings.PlayerPanels.AvatarPanels
{
    public class AvatarPanelController : UiPanelController<AvatarPanelView>
    {
        [Inject] private readonly AvatarPanelDetailsController m_avatarPanelDetailsController;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;

        private bool m_isCreate;
        private DynamicUiList<AvatarIconView, AvatarModel> m_avatarsWrapper;

        protected override void Show()
        {
            if (!m_isCreate)
                CreateAvatars();

            base.Show();
        }

        private void CreateAvatars()
        {
            m_isCreate = true;
            m_avatarsWrapper = new(View.AvatarIconPrefab, View.Content, View.Scroll, m_commonDictionaries,
                OnSelectAvatar);
            List<AvatarModel> avatars = m_commonDictionaries.AvatarModels.Values.ToList();
            m_avatarsWrapper.ShowDatas(avatars);
        }

        private void OnSelectAvatar(AvatarIconView view)
        {
            m_avatarPanelDetailsController.ShowAvatarDetail(view);
        }
    }
}