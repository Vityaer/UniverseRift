using UnityEngine;

namespace Network.DataServer.Messages.City.Mines
{
    public class MineDestroyMessage : INetworkMessage
    {
        public int PlayerId;
        public int MineId;

        public string Route => "Mines/DestroyMine";

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
