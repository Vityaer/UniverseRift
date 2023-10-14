using UnityEngine;

namespace Network.DataServer.Messages.City.Mines
{
    public class CreateNewMineMessage : INetworkMessage
    {
        public int PlayerId;
        public string MineModelId;
        public string PlaceId;
        public string Route => "Mines/Create";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("MineModelId", MineModelId);
                form.AddField("placeId", PlaceId);
                return form;
            }
        }
    }
}
