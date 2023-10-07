using Network.DataServer;
using UnityEngine;

namespace Network.DataServer.Messages.City.Mines
{
    public class MineTakeResourceMessage : INetworkMessage
    {
        public int PlayerId;
        public int MineId;

        public string Route => "Mines/TakeMineResources";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("MineId", MineId);
                return form;
            }
        }
    }
}
