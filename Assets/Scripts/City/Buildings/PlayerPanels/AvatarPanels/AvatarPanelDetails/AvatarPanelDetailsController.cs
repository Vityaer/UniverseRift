using Cysharp.Threading.Tasks;
using Mirror.Examples.MultipleMatch;
using Models.Common;
using Models.Data.Players;
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

        private ReactiveCommand<AvatarModel> _onSelectNewAvatar = new();
        private PlayerData _playerInfo;

        public IObservable<AvatarModel> OnSelectNewAvatar => _onSelectNewAvatar;


        public override void Start()
        {
            View.SetNewAvatarButton.OnClickAsObservable().Subscribe(_ => SetNewAvatar().Forget()).AddTo(Disposables);
            base.Start();
        }

        protected override void OnLoadGame()
        {
            _playerInfo = _commonGameData.PlayerInfoData;
        }

        public void ShowAvatarDetail(AvatarIconView avatarView)
        {
            View.SetNewAvatarButton.interactable = !_playerInfo.AvatarPath.Equals(avatarView.GetData.Path);
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
