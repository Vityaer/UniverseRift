using UnityEngine;

namespace Network.DataServer.Messages.Guilds
{
    public class GuildCreatePlayerBanMessage : INetworkMessage
    {
        public int PlayerId;
        public int GuildId;
        public int OtherPlayerId;
        public int Reason;

        public string Route => "Guild/CreatePlayerBan";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("GuildId", GuildId);
                form.AddField("OtherPlayerId", OtherPlayerId);
                form.AddField("Reason", Reason);
                return form;
            }
        }
    }
}
