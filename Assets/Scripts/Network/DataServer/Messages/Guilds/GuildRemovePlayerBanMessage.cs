using UnityEngine;

namespace Network.DataServer.Messages.Guilds
{
    public class GuildRemovePlayerBanMessage : INetworkMessage
    {
        public int PlayerId;
        public int GuildId;
        public int BanId;

        public string Route => "Guild/RemovePlayerBan";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("GuildId", GuildId);
                form.AddField("BanId", BanId);
                return form;
            }
        }
    }
}
