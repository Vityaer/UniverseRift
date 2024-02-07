using UnityEngine;

namespace Network.DataServer.Messages.Guilds
{
    public class RaidBossMessage : INetworkMessage
    {
        public int PlayerId;

        public string Route => "Guild/RaidBoss";

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
