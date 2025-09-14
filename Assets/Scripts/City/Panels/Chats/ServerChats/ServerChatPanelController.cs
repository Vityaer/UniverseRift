using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Misc.Json;
using Models.Misc;
using Network.DataServer;
using Network.DataServer.Messages.Chats;
using UiExtensions.Misc;
using UiExtensions.Scroll.Interfaces;
using UniRx;

namespace City.Panels.Chats.ServerChats
{
    public class ServerChatPanelController : UiPanelController<ServerChatPanelView>
    {
        private readonly IJsonConverter m_jsonConverter;
        private readonly CommonDictionaries m_commonDictionaries;

        private List<ChatMessageData> m_chatMesagges = new();
        private DynamicUiList<ChatMessageView, ChatMessageData> m_chatWrapper;

        public ServerChatPanelController(IJsonConverter jsonConverter, CommonDictionaries commonDictionaries)
        {
            m_jsonConverter = jsonConverter;
        }

        protected override void OnLoadGame()
        {
            m_chatWrapper = new(View.ChatMessagePrefab,
                View.ChatScrollRect.content,
                View.ChatScrollRect);
            
            View.SendMessageButton.OnClickAsObservable().Subscribe(_ => SendChatMessage().Forget())
                .AddTo(Disposables);
            View.InputFieldMessage.onValueChanged.AddListener(OnChangeMessageInputField);
            base.OnLoadGame();
        }

        protected override void Show()
        {
            LoadChat().Forget();
            base.Show();
        }

        private async UniTaskVoid LoadChat()
        {
            var message = new LoadChatMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
            };

            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                var messages = m_jsonConverter.Deserialize<List<ChatMessageData>>(result);
                ShowChats(messages);
            }
        }

        private void ShowChats(List<ChatMessageData> messages)
        {
            m_chatMesagges = messages;
            m_chatWrapper.ShowDatas(messages);
            foreach (var chatMessage in m_chatWrapper.Views)
                if (CommonGameData.CommunicationData.PlayersData
                    .TryGetValue(chatMessage.GetData.PlayerWritterId, out var playerData))
                    chatMessage.SetPlayerData(playerData);
        }

        private void OnChangeMessageInputField(string text)
        {
            View.SendMessageButton.gameObject.SetActive(text.Length > 0);
        }

        private async UniTaskVoid SendChatMessage()
        {
            View.SendMessageButton.interactable = false;
            var message = new CreateChatMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                Message = View.InputFieldMessage.text
            };

            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                View.InputFieldMessage.text = string.Empty;
                View.SendMessageButton.gameObject.SetActive(false);
            }

            View.SendMessageButton.interactable = true;
            LoadChat().Forget();
        }
    }
}