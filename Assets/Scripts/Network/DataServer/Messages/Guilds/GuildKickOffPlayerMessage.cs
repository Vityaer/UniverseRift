using UnityEngine;

namespace Network.DataServer.Messages.Guilds
{
    public class GuildKickOffPlayerMessage : INetworkMessage
    {
        public int PlayerId;
        public int GuildId;
        public int OtherPlayerId;

        public string Route => "Guild/KickOffPlayer";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("GuildId", GuildId);
                form.AddField("OtherPlayerId", OtherPlayerId);
                return form;
            }
        }
    }
}
