using City.Panels.CreatorMessagePanels;
using Common.Players;
using Models.Data.Players;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Panels.PlayerInfoPanels
{
    public class PlayerMiniInfoPanelController : UiPanelController<PlayerMiniInfoPanelView>
    {
        private readonly PlayersStorage _playersStorage;
        private readonly CreatorMessagePanelController _creatorMessagePanelController;

        private PlayerData _playerData;

        public PlayerMiniInfoPanelController
            (
            PlayersStorage playersStorage,
            CreatorMessagePanelController creatorMessagePanelController
            )
        {
            _playersStorage = playersStorage;
            _creatorMessagePanelController = creatorMessagePanelController;
        }

        public override void Start()
        {
            View.SendMessageButton.OnClickAsObservable().Subscribe(_ => OpenMessagePanel()).AddTo(Disposables);
            base.Start();
        }

        public void ShowPlayerData(PlayerData playerData)
        {
            _playerData = playerData;
            View.PlayerAvatarView.SetData(playerData);
            View.PlayerName.text = playerData.Name;
            MessagesPublisher.OpenWindowPublisher.OpenWindow<PlayerMiniInfoPanelController>(openType: OpenType.Exclusive);
        }

        private void OpenMessagePanel()
        {
            _creatorMessagePanelController.Open(_playerData);
        }
    }
}
