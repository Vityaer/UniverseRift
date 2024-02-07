using Cysharp.Threading.Tasks;
using Models.Common;
using Models.Misc.Avatars;
using Network.DataServer;
using Network.DataServer.Messages.Players;
using System;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Buildings.PlayerPanels.AvatarPanels.AvatarPanelDetails
{
    public class AvatarPanelDetailsController : UiPanelController<AvatarPanelDetailsView>, IInitializable
    {
        [Inject] protected readonly CommonGameData _commonGameData;

        private AvatarIconView _avatarView;
        private CompositeDisposable _disposables;

        private ReactiveCommand<AvatarModel> _onSelectNewAvatar = new();

        public IObservable<AvatarModel> OnSelectNewAvatar => _onSelectNewAvatar;

        public override void OnShow()
        {
            View.SetNewAvatarButton.OnClickAsObservable().Subscribe(_ => SetNewAvatar().Forget()).AddTo(_disposables);
            base.OnShow();
        }

        public void ShowAvatarDetail(AvatarIconView avatarView)
        {
            View.SetNewAvatarButton.interactable = true;
            _avatarView = avatarView;
            View.MainImage.sprite = _avatarView.Avatar.sprite;
            MessagesPublisher.OpenWindowPublisher.OpenWindow<AvatarPanelDetailsController>(openType: OpenType.Exclusive);
        }

        private async UniTaskVoid SetNewAvatar()
        {
            View.SetNewAvatarButton.interactable = false;
            var message = new PlayerChangeAvatarMessage
            {
                PlayerId = _commonGameData.PlayerInfoData.Id,
                AvatarPath = _avatarView.GetData.Path
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                _onSelectNewAvatar.Execute(_avatarView.GetData);
            }

        }
    }
}
