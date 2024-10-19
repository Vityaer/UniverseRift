using UnityEngine;

namespace Network.DataServer.Messages.Chats
{
    public class CreateChatMessage : INetworkMessage
    {
        public int PlayerId;
        public string Message;

        public string Route => "Chat/CreateChatMessage";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("message", Message);
                return form;
            }
        }
    }
}
