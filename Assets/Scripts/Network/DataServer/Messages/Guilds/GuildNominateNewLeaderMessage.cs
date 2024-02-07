using UnityEngine;

namespace Network.DataServer.Messages.Guilds
{
    public class GuildNominateNewLeaderMessage : INetworkMessage
    {
        public int PlayerId;
        public int GuildId;
        public int NewLeaderPlayerId;

        public string Route => "Guild/NominateNewLeader";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("GuildId", GuildId);
                form.AddField("NewLeaderPlayerId", NewLeaderPlayerId);
                return form;
            }
        }
    }
}
