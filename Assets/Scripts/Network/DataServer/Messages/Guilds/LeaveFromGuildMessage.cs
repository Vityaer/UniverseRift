using UnityEngine;

namespace Network.DataServer.Messages.Guilds
{
    public class LeaveFromGuildMessage : INetworkMessage
    {
        public int PlayerId;
        public int GuildId;

        public string Route => "Guild/LeaveFromGuild";

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
