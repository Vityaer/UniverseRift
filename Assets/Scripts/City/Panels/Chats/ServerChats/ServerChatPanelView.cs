using TMPro;
using Ui.Misc.Widgets;
using UnityEngine.UI;

namespace City.Panels.Chats.ServerChats
{
    public class ServerChatPanelView : BasePanel
    {
        public ScrollRect ChatScrollRect;
        public ChatMessageView ChatMessagePrefab;
        public TMP_InputField InputFieldMessage;
        public Button SendMessageButton;

    }
}
