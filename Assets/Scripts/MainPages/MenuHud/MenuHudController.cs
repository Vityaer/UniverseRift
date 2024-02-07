using City.Buildings.PlayerPanels;
using City.Buildings.PlayerPanels.AvatarPanels.AvatarPanelDetails;
using Common;
using Models.Common;
using Models.Misc.Avatars;
using System;
using UniRx;
using UnityEngine;
using Utils;
using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace MainPages.MenuHud
{
    public class MenuHudController : UiController<MenuHudView>, IInitializable, IDisposable
    {
        [Inject] private readonly IObjectResolver _resolver;
        [Inject] private readonly IUiMessagesPublisherService _messagesPublisher;
        [Inject] private readonly AvatarPanelDetailsController _avatarPanelDetailsController;
        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] protected readonly GameController _gameController;

        private CompositeDisposable _disposables = new CompositeDisposable();

        public void Initialize()
        {
            _resolver.Inject(View.GoldObserverResource);
            _resolver.Inject(View.DiamondObserverResource);
            View.PlayerPanelButton.OnClickAsObservable().Subscribe(_ => OpenPlayerPanel()).AddTo(_disposables);
            _avatarPanelDetailsController.OnSelectNewAvatar.Subscribe(ChangeAvatar).AddTo(_disposables);
            _gameController.OnLoadedGameData.Subscribe(_ => OnLoadGame()).AddTo(_disposables);
        }

        protected override void OnLoadGame()
        {
            View.Avatar.sprite = SpriteUtils.LoadSprite(_commonGameData.PlayerInfoData.AvatarPath);
            base.OnLoadGame();
        }

        private void OpenPlayerPanel()
        {
            _messagesPublisher.OpenWindowPublisher.OpenWindow<PlayerPanelController>(openType: OpenType.Additive);
        }

        private void ChangeAvatar(AvatarModel avatar)
        {
            View.Avatar.sprite = SpriteUtils.LoadSprite(avatar.Path);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
