using UnityEngine;

namespace Network.DataServer.Messages.Chats
{
    public class LoadChatMessage : INetworkMessage
    {
        public int PlayerId;

        public string Route => "Chat/LoadChat";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                return form;
            }
        }
    }
}
