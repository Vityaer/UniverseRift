using UnityEngine;

namespace Network.DataServer.Messages.Guilds
{
    public class DenyRequestMessage : INetworkMessage
    {
        public int PlayerId;
        public int GuildId;
        public int RequestId;
        public string Route => "Guild/DenyRequest";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("GuildId", GuildId);
                form.AddField("RequestId", RequestId);
                return form;
            }
        }
    }
}
