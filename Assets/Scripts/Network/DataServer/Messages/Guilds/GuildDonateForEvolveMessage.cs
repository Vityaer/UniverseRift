using UnityEngine;

namespace Network.DataServer.Messages.Guilds
{
    public class GuildDonateForEvolveMessage : INetworkMessage
    {
        public int PlayerId;
        public int GuildId;
        public string Donate;

        public string Route => "Guild/DonateForEvolve";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("GuildId", GuildId);
                form.AddField("Donate", Donate);
                return form;
            }
        }
    }
}
