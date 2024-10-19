using Cysharp.Threading.Tasks;
using Models.Data.Players;
using Network.DataServer;
using Network.DataServer.Messages.Mails;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Panels.CreatorMessagePanels
{
    public class CreatorMessagePanelController : UiPanelController<CreatorMessagePanelView>
    {
        private PlayerData _otherPlayer;

        public override void Start()
        {
            View.SendMessageButton.OnClickAsObservable().Subscribe(_ => SendMessage().Forget()).AddTo(Disposables);
            base.Start();
        }

        public void Open(PlayerData otherPlayer)
        {
            _otherPlayer = otherPlayer;
            MessagesPublisher.OpenWindowPublisher.OpenWindow<CreatorMessagePanelController>(openType: OpenType.Exclusive);
        }

        private async UniTaskVoid SendMessage()
        {
            View.SendMessageButton.interactable = false;
            var message = new CreateLetterMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                OtherPlayerId = _otherPlayer.Id,
                Message = View.InputFieldMessage.text
            };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                Close();
            }

            View.SendMessageButton.interactable = true;
        }
    }
}
