using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Misc.Json;
using Models.Misc;
using Network.DataServer;
using Network.DataServer.Messages.Chats;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UiExtensions.Misc;
using UiExtensions.Scroll.Interfaces;
using UniRx;

namespace City.Panels.Chats.ServerChats
{
    public class ServerChatPanelController : UiPanelController<ServerChatPanelView>
    {
        private readonly IJsonConverter _jsonConverter;
        private readonly CommonDictionaries _commonDictionaries;

        private List<ChatMessageData> _chatMesagges = new();
        private DynamicUiList<ChatMessageView, ChatMessageData> _chatWrapper;

        public ServerChatPanelController(IJsonConverter jsonConverter, CommonDictionaries commonDictionaries)
        {
            _jsonConverter = jsonConverter;
        }

        protected override void Show()
        {
            _chatWrapper = new(View.ChatMessagePrefab, View.ChatScrollRect.content, View.ChatScrollRect, null, null);
            LoadChat().Forget();
            View.SendMessageButton.OnClickAsObservable().Subscribe(_ => SendChatMessage().Forget()).AddTo(Disposables);
            View.InputFieldMessage.onValueChanged.AddListener(OnChangeMessageInputField);
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
                var messages = _jsonConverter.Deserialize<List<ChatMessageData>>(result);
                ShowChats(messages);
            }
        }

        private void ShowChats(List<ChatMessageData> messages)
        {
            _chatMesagges = messages;
            UnityEngine.Debug.Log($"_chatMesagges: {_chatMesagges.Count}");
            _chatWrapper.ShowDatas(messages);
            foreach(var chatMessage in _chatWrapper.Views)
            {
                if(CommonGameData.CommunicationData.PlayersData.TryGetValue(chatMessage.GetData.PlayerWritterId, out var playerData))
                {
                    chatMessage.SetPlayerData(playerData);
                }
            }
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
