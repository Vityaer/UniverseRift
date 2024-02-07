using UnityEngine;

namespace Network.DataServer.Messages.Guilds
{
    public class RemovePlayerRequestMessage : INetworkMessage
    {
        public int PlayerId;
        public int RequestId;

        public string Route => "Guild/RemovePlayerRequest";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("RequestId", RequestId);
                return form;
            }
        }
    }
}
