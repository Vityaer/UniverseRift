using UnityEngine;

namespace Network.DataServer.Messages.City.Alchemies
{
    public class GetAlchemyMessage : INetworkMessage
    {
        public int PlayerId;
        public string Route => "Market/GetAlchemyMessage";

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
