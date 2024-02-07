using UnityEngine;

namespace Network.DataServer.Messages.Guilds
{
    public class GetPlayerRequestsMessage : INetworkMessage
    {
        public int PlayerId;
        public int GuildId;

        public string Route => "Guild/GetPlayerRequests";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("GuildId", GuildId);
                return form;
            }
        }
    }
}
