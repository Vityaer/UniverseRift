using UnityEngine;

namespace Network.DataServer.Messages.Guilds
{
    public class RaidBossMessage : INetworkMessage
    {
        public int PlayerId;
        public int Damage;

        public string Route => "Guild/RaidBoss";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("Damage", Damage);
                return form;
            }
        }
    }
}
