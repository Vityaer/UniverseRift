using UnityEngine;

namespace Network.DataServer.Messages.Guilds
{
    public class GetAvailableGuildsMessage : INetworkMessage
    {
        public int PlayerId;
        public int PagginationIndex;

        public string Route => "Guild/GetAvailableGuilds";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("PagginationIndex", PagginationIndex);
                return form;
            }
        }
    }
}
