using UnityEngine;

namespace Network.DataServer.Messages.City
{
    public class BuyProductMessage : INetworkMessage
    {
        public int PlayerId;
        public string ProductId;
        public int Count;

        public string Route => "Market/BuyProduct";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("ProductId", ProductId);
                form.AddField("Count", Count);
                return form;
            }
        }
    }
}
