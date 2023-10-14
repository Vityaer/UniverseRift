using UnityEngine;

namespace Network.DataServer.Messages.City.Mines
{
    public class MinesTakeAllResourceMessage : INetworkMessage
    {
        public int PlayerId;

        public string Route => "Mines/TakeAllResources";

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
